using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Services
{
    public interface IGatewayService
    {
        string Url { get; set; }

        void InvalidateGateway();

        void LoadBestGateway(string[] acoes);

        string RequestJson(string url);
    }
}
