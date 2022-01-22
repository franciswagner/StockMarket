using Newtonsoft.Json;
using StockMarket;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PriceMonitor
{
    public class WebMonitor
    {
        #region Consts

        public const int MAX_CANDLES_IN_GRAPH = 100;
        public const int UPDATE_INTERVAL = 60; // seconds

        public static DateTime MARKET_OPENING
        {
            get { return DateTime.Today + Configs.Opening; }
        }

        public static DateTime MARKET_CLOSING
        {
            get { return DateTime.Today + Configs.Closing; }
        }

        #endregion

        #region Constructors

        public WebMonitor(string[] acoes)
        {
            this._acoes = acoes;
            this.LoadBesteGateway(acoes);
        }

        #endregion

        #region Private Fields

        private string _url;
        private readonly string[] _acoes;
        private readonly string[] _gateways = { "mdgateway", "mdgateway01", "mdgateway02", "mdgateway03", "mdgateway04", "mdgateway06" };
        private readonly Dictionary<string, decimal> _minValues = new Dictionary<string, decimal>();

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

        public static bool IsMarketOpened
        {
            get
            {
                var now = DateTime.Now;
                if (now < WebMonitor.MARKET_OPENING || now >= WebMonitor.MARKET_CLOSING)
                    return false;

                return true;
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


        private void LoadBesteGateway(string[] acoes)
        {
            var urlGatewayemplate = ConfigurationManager.AppSettings["UrlGatewayTemplate"];

            var bestCount = 0;
            foreach (var gateway in this._gateways)
            {
                var url = string.Format(urlGatewayemplate, gateway);

                var sbUrl = new StringBuilder();
                sbUrl.Append(url);

                foreach (var acao in acoes)
                {
                    sbUrl.Append(acao);
                    sbUrl.Append(",1,0|");
                }

                url = sbUrl.ToString().Trim('|');

                int count;
                if (TestGateway(url, out count) && count > bestCount)
                {
                    bestCount = count;
                    this._url = url;

                    if (bestCount == this._acoes.Length)
                        break;
                }
            }
        }

        private DateTime? ProcessJsonResult(AcoesJsonReaderPriceCollection acoesJsonReader, List<string> zeroValue)
        {
            DateTime? lastDate = null;

            if (acoesJsonReader != null)
            {
                foreach (var jsonAcao in acoesJsonReader.Value)
                {
                    var name = jsonAcao.S;
                    var acao = new Acao(jsonAcao);

                    lastDate = acao.RequestedDate;

                    if (acao.OppeningPrice <= 0 || acao.Price <= 0)
                    {
                        zeroValue.Add(name);
                        continue;
                    }

                    SaveToFile(name, acao);

                    var acoesMonitor = this.AcoesCollections.FirstOrDefault(x => x.Name == name);
                    if (acoesMonitor != null)
                        acoesMonitor.Acoes.Add(acao);
                    else
                        this.AcoesCollections.Add(new AcoesCollection(name, acao));
                }
            }

            return lastDate;
        }

        #endregion

        #region Public Methods

        public void Run(Action<List<AcoesCollection>> callback, Action callback_OpenedMarket, Action callback_ClosedMarket, Action<string> callback_Error, Action<int, string> callback_Nitification)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    // Verifica se o mercado esta aberto
                    if (!IsMarketOpened)
                    {
                        callback_ClosedMarket();
                        Thread.Sleep(60000);
                        continue;
                    }

                    if (string.IsNullOrEmpty(this._url))
                    {
                        callback_Error("Nenhum gateway válido foi identificado.");

                        //Retesta os gateways
                        this.LoadBesteGateway(this._acoes);

                        Thread.Sleep(60000);
                        continue;
                    }

                    var currentGateway = new Uri(this._url).Host;
                    currentGateway = currentGateway.Substring(0, currentGateway.IndexOf("."));

                    var json = RequestJson(this._url);

                    if (string.IsNullOrWhiteSpace(json))
                    {
                        callback_Error($"Falha ao comunicar com a internet ({currentGateway}).");

                        //Retesta os gateways
                        this.LoadBesteGateway(this._acoes);

                        Thread.Sleep(60000);
                        continue;
                    }

                    var acoesJsonReader = DeserializeJson(json);

                    var errorMessage = string.Empty;

                    if (acoesJsonReader.Value.Count == this._acoes.Length)
                        callback_OpenedMarket();
                    else
                        errorMessage = $"A URL designada não retornou todos os dados solicitados ({currentGateway}).\r\n    Apenas {acoesJsonReader.Value.Count} de {this._acoes.Length}.";

                    var zeroValue = new List<string>();
                    var lastDate = this.ProcessJsonResult(acoesJsonReader, zeroValue);

                    if (zeroValue.Any())
                    {
                        if (!string.IsNullOrWhiteSpace(errorMessage))
                            errorMessage += Environment.NewLine + Environment.NewLine;

                        errorMessage += $"As Seguintes acões possuem valor zerado ({currentGateway}):{Environment.NewLine}    {string.Join(Environment.NewLine + "    ", zeroValue)}";
                    }

                    if (!string.IsNullOrEmpty(errorMessage))
                        callback_Error(errorMessage);

                    callback(this.AcoesCollections);

                    var messageNotification = new StringBuilder();
                    var notifications = 0;
                    var count = 0;

                    foreach (var acoes in this.AcoesCollections)
                    {
                        var last = acoes.Acoes.LastOrDefault();

                        if (last == null || last.RequestedDate.ToString("dd/MM/yyyy HH:mm") != lastDate.GetValueOrDefault().ToString("dd/MM/yyyy HH:mm"))
                        {
                            count++;
                            continue;
                        }

                        if (this._minValues[acoes.Name] != 0 && (last.MinimunPrice < this._minValues[acoes.Name] || last.Price <= this._minValues[acoes.Name]))
                        {
                            this._minValues[acoes.Name] = Math.Min(last.MinimunPrice, last.Price); //Esperasse que seja sempre o mesmo valor
                            messageNotification.AppendLine($"{acoes.Name}: {this._minValues[acoes.Name]:#,##0.00}");
                            notifications++;
                        }
                    }

                    if (count > 0)
                        this.LoadBesteGateway(this._acoes);

                    if (notifications > 0)
                        callback_Nitification(notifications, messageNotification.ToString());

                    Thread.Sleep(UPDATE_INTERVAL * 1000);
                }
            });
        }

        #endregion

        #region Static Methods

        private static AcoesJsonReaderPriceCollection DeserializeJson(string json)
        {
            try
            {
                var teste = JsonConvert.DeserializeObject<AcoesJsonReaderPriceCollection>(json);
                return teste;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "WTF Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private static AcoesCollection LoadFromFile(string file)
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
                        OppeningPrice = Convert.ToDecimal(splitedLine[3]),
                        Price = Convert.ToDecimal(splitedLine[4]),
                        MinimunPrice = Convert.ToDecimal(string.IsNullOrEmpty(splitedLine[5]) ? "0" : splitedLine[5]),
                        MaximunPrice = Convert.ToDecimal(string.IsNullOrEmpty(splitedLine[6]) ? "0" : splitedLine[6]),
                        AveragePrice = Convert.ToDecimal(string.IsNullOrEmpty(splitedLine[7]) ? "0" : splitedLine[7]),
                        Volume = Convert.ToDecimal(string.IsNullOrEmpty(splitedLine[8]) ? "0" : splitedLine[8]),
                        ClosedPrice = Convert.ToDecimal(string.IsNullOrEmpty(splitedLine[9]) ? "0" : splitedLine[9])
                    });
                }

            if (!acoes.Acoes.Any())
                return null;

            //30 min X max points
            var max = MAX_CANDLES_IN_GRAPH * Configs.CandlePeriod;

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

        private static string RequestJson(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            var uri = new Uri(url);

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "GET";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:36.0) Gecko/20100101 Firefox/36.0";
            request.Headers.Add("Accept-Language", "pt-BR,pt;q=0.8,en-US;q=0.5,en;q=0.3");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Headers.Add("Pragma", "no-cache");
            request.Headers.Add("Cache-Control", "no-cache");

            try
            {
                var htmlSource = string.Empty;
                var response = (HttpWebResponse)request.GetResponse();

                try
                {
                    Encoding encoding;
                    if (response.ContentType.ToLower().Contains("utf-8"))
                        encoding = Encoding.UTF8;
                    else
                        encoding = Encoding.GetEncoding("ISO-8859-1");

                    using (var sr = new StreamReader(response.GetResponseStream(), encoding))
                    {
                        if(sr.BaseStream.CanTimeout)
                            sr.BaseStream.ReadTimeout = 5000;

                        htmlSource = sr.ReadToEnd();

                        return htmlSource;
                    }
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
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

        private static bool TestGateway(string url, out int count)
        {
            var json = RequestJson(url);

            if (string.IsNullOrWhiteSpace(json))
            {
                count = 0;
                return false;
            }

            var acoesJsonReader = DeserializeJson(json);

            var validCount = acoesJsonReader.Value.Count(x => x.Ps.P != 0 && x.Ps.OP != 0 && x.Ps.CP != 0);
            if (validCount != 0)
            {
                count = validCount;
                return true;
            }

            count = 0;
            return false;
        }


        #endregion
    }
}
