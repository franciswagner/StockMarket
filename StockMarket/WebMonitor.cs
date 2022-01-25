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
        #region Constructors

        public WebMonitor(string[] acoes, IGatewayService gatewayService, ISerializationService serializationService, IConfigsService configsService, IPersistenceService persistenceService)
        {
            this._acoes = acoes;

            this._configsService = configsService;
            this._gatewayService = gatewayService;
            this._persistenceService = persistenceService;
            this._serializationService = serializationService;

            this._gatewayService.LoadBestGateway(acoes);
        }

        #endregion

        #region Private Fields

        private readonly string[] _acoes;
        private readonly Dictionary<string, decimal> _minValues = new Dictionary<string, decimal>();

        private readonly IConfigsService _configsService;
        private readonly IGatewayService _gatewayService;
        private readonly IPersistenceService _persistenceService;
        private readonly ISerializationService _serializationService;

        #endregion

        #region Attributes and Properties

        private List<AcoesCollection> _acoesCollection = null;
        public List<AcoesCollection> AcoesCollections
        {
            get
            {
                if (this._acoesCollection == null)
                {
                    this._acoesCollection = this._persistenceService.LoadFromFiles(this._acoes);

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

                    this._persistenceService.SaveToFile(name, acao);

                    var acoesCollection = this.AcoesCollections.FirstOrDefault(x => x.Name == name);
                    acoesCollection.Acoes.Add(acao);
                    validAcoesCollections.Add(acoesCollection);
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
    }
}
