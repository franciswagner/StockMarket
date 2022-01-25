using PriceMonitor;
using System.Collections.Generic;

namespace StockMarket.Services
{
    public interface IPersistenceService
    {
        List<AcoesCollection> LoadFromFiles(string[] ticketNames);

        void SaveToFile(string name, Acao acao);
    }
}
