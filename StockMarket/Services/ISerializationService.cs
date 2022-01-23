using PriceMonitor;

namespace StockMarket.Services
{
    public interface ISerializationService
    {
        AcoesJsonReaderPriceCollection DeserializeJson(string json);
    }
}
