using System;

namespace StockMarket.Services
{
    public interface IConfigsService
    {
        string Acoes { get; set; }

        bool IsMarketOpened { get; }

        int CandlePeriod { get; set; }

        TimeSpan Closing { get; set; }

        DateTime MarketClosing { get; }

        DateTime MarketOpening { get; }

        TimeSpan Opening { get; set; }

        void Load();

        void Save();
    }
}
