using StockMarket.Factories;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace StockMarket.Services
{
    public class GatewayService : IGatewayService
    {
        #region Constructors

        public GatewayService(ISerializationService serializationService, IHttpWebRequestFactory httpWebRequestFactory)
        {
            this._serializationService = serializationService;
            this._httpWebRequestFactory = httpWebRequestFactory;
        }

        #endregion

        #region Private Fields

        private readonly IHttpWebRequestFactory _httpWebRequestFactory;
        private readonly ISerializationService _serializationService;

        private readonly string[] _gateways = { "mdgateway", "mdgateway01", "mdgateway02", "mdgateway03", "mdgateway04", "mdgateway06" };

        #endregion

        #region Attributes and Properties

        public string Url { get; set; }

        #endregion

        #region Private Methods

        private bool TestGateway(string url, out int count)
        {
            var json = this.RequestJson(url);

            if (string.IsNullOrWhiteSpace(json))
            {
                count = 0;
                return false;
            }

            var acoesJsonReader = this._serializationService.DeserializeJson(json);

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

        #region Public Methods

        public void LoadBestGateway(string[] acoes)
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
                    this.Url = url;

                    if (bestCount == acoes.Length)
                        break;
                }
            }
        }

        public void InvalidateGateway()
        {
            this.Url = null;
        }

        public string RequestJson(string url)
        {
            var request = this._httpWebRequestFactory.Create(url);

            try
            {
                var htmlSource = string.Empty;

                using (var response = request.GetResponse())
                    try
                    {
                        Encoding encoding;
                        if (response.ContentType.ToLower().Contains("utf-8"))
                            encoding = Encoding.UTF8;
                        else
                            encoding = Encoding.GetEncoding("ISO-8859-1");

                        using (var sr = new StreamReader(response.GetResponseStream(), encoding))
                        {
                            if (sr.BaseStream.CanTimeout)
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

        #endregion
    }
}
