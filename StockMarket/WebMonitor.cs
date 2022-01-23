using StockMarket;
using StockMarket.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PriceMonitor
{
    public class WebMonitor
    {
        #region Consts

        public const int MAX_CANDLES_IN_GRAPH = 100;

        #endregion

        #region Constructors

        public WebMonitor(string[] acoes, IGatewayService gatewayService, ISerializationService serializationService, IConfigsService configsService)
        {
            this._acoes = acoes;

            this._configsService = configsService;
            this._gatewayService = gatewayService;
            this._serializationService = serializationService;

            this._gatewayService.LoadBestGateway(acoes);
        }

        #endregion

        #region Private Fields

        private readonly string[] _acoes;
        private readonly Dictionary<string, decimal> _minValues = new Dictionary<string, decimal>();

        private IConfigsService _configsService;
        private IGatewayService _gatewayService;
        private ISerializationService _serializationService;

        #endregion

        #region Attributes and Properties

        private List<AcoesCollection> _acoesCollection = null;
        public List<AcoesCollection> AcoesCollections
        {
            get
            {
                if (this._acoesCollection == null)
                {
                    this._acoesCollection = this.LoadFromFiles();

                    if (!this._acoesCollection.Any())
                    {
                        foreach (var acao in this._acoes)
                        {
                            var collection = new AcoesCollection();
                            collection.Name = acao;

                            this._acoesCollection.Add(collection);
                        }
                    }

                    foreach (var acoes in this._acoesCollection)
                        this._minValues.Add(acoes.Name, acoes.Acoes.LastOrDefault()?.MinimunPrice ?? 0M);
                }

                return this._acoesCollection;
            }
        }

        #endregion

        #region Private Methods

        private List<AcoesCollection> LoadFromFiles()
        {
            var acoesMonitorList = new List<AcoesCollection>();

            foreach (var file in Directory.EnumerateFiles("DataFiles"))
            {
                if (!this._acoes.Any(x => x == Path.GetFileNameWithoutExtension(file)))
                    continue;

                var acoes = LoadFromFile(file);
                if (acoes != null)
                    acoesMonitorList.Add(acoes);
            }

            return acoesMonitorList;
        }

        private List<AcoesCollection> ProcessJsonResult(AcoesJsonReaderPriceCollection acoesJsonReader, List<string> zeroValue)
        {
            var validAcoesCollections = new List<AcoesCollection>();

            if (acoesJsonReader != null)
            {
                foreach (var jsonAcao in acoesJsonReader.Value)
                {
                    var name = jsonAcao.S;
                    var acao = new Acao(jsonAcao);

                    if (acao.OppeningPrice <= 0 || acao.Price <= 0)
                    {
                        zeroValue.Add(name);
                        continue;
                    }

                    SaveToFile(name, acao);

                    var acoesCollection = this.AcoesCollections.FirstOrDefault(x => x.Name == name);
                    if (acoesCollection != null)
                    {
                        acoesCollection.Acoes.Add(acao);
                        validAcoesCollections.Add(acoesCollection);
                    }
                    else
                        this.AcoesCollections.Add(new AcoesCollection(name, acao));
                }
            }

            // Invalidate the url
            if (validAcoesCollections.Count != this.AcoesCollections.Count)
                this._gatewayService.InvalidateGateway();

            return validAcoesCollections;
        }

        #endregion

        #region Public Methods

        public void Run(Action<List<AcoesCollection>> callback, Action callback_OpenedMarket, Action callback_ClosedMarket, Action<string> callback_Error, Action<int, string> callback_Notification)
        {
            // Verifica se o mercado esta aberto
            if (!this._configsService.IsMarketOpened)
            {
                callback_ClosedMarket();
                return;
            }

            if (string.IsNullOrEmpty(this._gatewayService.Url))
            {
                callback_Error("Nenhum gateway válido foi identificado.");

                //Retesta os gateways
                this._gatewayService.LoadBestGateway(this._acoes);

                return;
            }

            var currentGateway = new Uri(this._gatewayService.Url).Host;
            currentGateway = currentGateway.Substring(0, currentGateway.IndexOf("."));

            var json = this._gatewayService.RequestJson(this._gatewayService.Url);

            if (string.IsNullOrWhiteSpace(json))
            {
                callback_Error($"Falha ao comunicar com a internet ({currentGateway}).");

                this._gatewayService.InvalidateGateway();
                return;
            }

            var acoesJsonReader = this._serializationService.DeserializeJson(json);

            var errorMessage = string.Empty;

            if (acoesJsonReader.Value.Count == this._acoes.Length)
                callback_OpenedMarket();
            else
                errorMessage = $"A URL designada não retornou todos os dados solicitados ({currentGateway}).\r\n    Apenas {acoesJsonReader.Value.Count} de {this._acoes.Length}.";

            var zeroValue = new List<string>();
            var updateddAcoes = this.ProcessJsonResult(acoesJsonReader, zeroValue);

            if (zeroValue.Any())
            {
                if (!string.IsNullOrWhiteSpace(errorMessage))
                    errorMessage += Environment.NewLine + Environment.NewLine;

                errorMessage += $"As Seguintes acões possuem valor zerado ({currentGateway}):{Environment.NewLine}    {string.Join(Environment.NewLine + "    ", zeroValue)}";
            }

            if (!string.IsNullOrEmpty(errorMessage))
                callback_Error(errorMessage);


            if (this.AcoesCollections != null && this.AcoesCollections.Any())
                callback(this.AcoesCollections);

            var messageNotification = new StringBuilder();

            var acoesNewMinimun = updateddAcoes
                .Where(acoes => acoes.Acoes.LastOrDefault().MinimunPrice < this._minValues[acoes.Name] || acoes.Acoes.LastOrDefault().Price <= this._minValues[acoes.Name])
                .ToList();

            foreach (var acoes in acoesNewMinimun)
            {
                var last = acoes.Acoes.LastOrDefault();
                this._minValues[acoes.Name] = Math.Min(last.MinimunPrice, last.Price); //Esperasse que seja sempre o mesmo valor
                messageNotification.AppendLine($"{acoes.Name}: {this._minValues[acoes.Name]:#,##0.00}");
            }

            if (acoesNewMinimun.Any())
                callback_Notification(acoesNewMinimun.Count, messageNotification.ToString());
        }

        #endregion

        #region Static Methods

        private AcoesCollection LoadFromFile(string file)
        {
            var acoes = new AcoesCollection();

            using (var sr = new StreamReader(file))
                while (true)
                {
                    var line = (sr.ReadLine() ?? "").Trim();

                    if (string.IsNullOrEmpty(line))
                        break;

                    line = NormalizeLine(line);

                    var splitedLine = line.Split(';');

                    if (!acoes.Acoes.Any())
                        acoes.Name = splitedLine[0];

                    acoes.Acoes.Add(new Acao()
                    {
                        RequestedDate = Convert.ToDateTime(splitedLine[1]),
                        Date = Convert.ToDateTime(splitedLine[2]),
                        OppeningPrice = splitedLine[3].ToDecimal(),
                        Price = splitedLine[4].ToDecimal(),
                        MinimunPrice = splitedLine[5].ToDecimal(),
                        MaximunPrice = splitedLine[6].ToDecimal(),
                        AveragePrice = splitedLine[7].ToDecimal(),
                        Volume = splitedLine[8].ToDecimal(),
                        ClosedPrice = splitedLine[9].ToDecimal()
                    });
                }

            if (!acoes.Acoes.Any())
                return null;

            //30 min X max points
            var max = MAX_CANDLES_IN_GRAPH * this._configsService.CandlePeriod;

            if (acoes.Acoes.Count > max)
                acoes.Acoes = acoes.Acoes.TakeLast(max).ToList();

            return acoes;
        }

        private static string NormalizeLine(string line)
        {
            var columns = line.Count(x => x == ';');
            if (columns < 9)
                line = string.Concat(line, new string(';', 9 - columns));

            return line;
        }

        private static void SaveToFile(string name, Acao acao)
        {
            if (!Directory.Exists("DataFiles"))
                Directory.CreateDirectory("DataFiles");

            var spliter = ";";
            using (var sw = new StreamWriter(Path.Combine("DataFiles", name + ".txt"), true))
                sw.WriteLine(
                    name + spliter +
                    acao.RequestedDate + spliter +
                    acao.Date + spliter +
                    acao.OppeningPrice + spliter +
                    acao.Price + spliter +
                    acao.MinimunPrice + spliter +
                    acao.MaximunPrice + spliter +
                    acao.AveragePrice + spliter +
                    acao.Volume + spliter +
                    acao.ClosedPrice);
        }


        #endregion
    }
}
