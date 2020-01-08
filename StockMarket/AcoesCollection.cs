using System.Collections.Generic;
using StockMarket;

namespace PriceMonitor
{
    public class AcoesCollection
    {
        public AcoesCollection() { }

        public AcoesCollection(string name, Acao acao)
        {
            this.Name = name;
            this.Acoes.Add(acao);
        }

        public string Name { get; set; }
        public List<Acao> Acoes { get; set; } = new List<Acao>();
    }
}
