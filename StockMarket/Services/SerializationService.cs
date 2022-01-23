using Newtonsoft.Json;
using PriceMonitor;

namespace StockMarket.Services
{
    public class SerializationService : ISerializationService
    {
        #region Public Methods

        public AcoesJsonReaderPriceCollection DeserializeJson(string json)
        {
            return JsonConvert.DeserializeObject<AcoesJsonReaderPriceCollection>(json);
        }

        #endregion
    }
}
