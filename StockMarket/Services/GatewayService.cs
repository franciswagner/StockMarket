using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace StockMarket.Services
{
    public class GatewayService : IGatewayService
    {
        #region Constructors

        public GatewayService(ISerializationService serializationService)
        {
            this._serializationService = serializationService;
        }

        #endregion

        #region Private Fields

        private ISerializationService _serializationService;

        private readonly string[] _gateways = { "mdgateway", "mdgateway01", "mdgateway02", "mdgateway03", "mdgateway04", "mdgateway06" };

        #endregion

        #region Attributes and Properties

        public string Url { get; set; }

        #endregion

        #region Private Methods

        private bool TestGateway(string url, out int count)
        {
            var json = RequestJson(url);

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
