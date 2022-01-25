using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace StockMarket.Factories
{
    public class HttpWebRequestFactory : IHttpWebRequestFactory
    {
        #region Public Methods

        public IHttpWebRequest Create(string url)
        {
            var uri = new Uri(url);
            return new WrapHttpWebRequest((HttpWebRequest)WebRequest.Create(uri));
        }

        #endregion
    }

    public interface IHttpWebRequest
    {
        // expose the members you need
        string Method { get; set; }

        IHttpWebResponse GetResponse();
    }

    public class WrapHttpWebRequest : IHttpWebRequest
    {
        private readonly HttpWebRequest _request;

        public WrapHttpWebRequest(HttpWebRequest request)
        {
            _request = request;
        }

        public string Method
        {
            get { return _request.Method; }
            set { _request.Method = value; }
        }

        public IHttpWebResponse GetResponse()
        {
            return new WrapHttpWebResponse((HttpWebResponse)_request.GetResponse());
        }
    }

    public class WrapHttpWebResponse : IHttpWebResponse
    {
        private WebResponse _response;

        public WrapHttpWebResponse(HttpWebResponse response)
        {
            _response = response;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_response != null)
                {
                    ((IDisposable)_response).Dispose();
                    _response = null;
                }
            }
        }

        public string ContentType
        {
            get { return _response.ContentType; }
            set { _response.ContentType = value; }
        }

        public Stream GetResponseStream()
        {
            return _response.GetResponseStream();
        }
    }

    public interface IHttpWebResponse : IDisposable
    {
        string ContentType { get; }

        Stream GetResponseStream();
    }
}
