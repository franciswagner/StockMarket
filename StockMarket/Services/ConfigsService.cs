using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StockMarket.Services
{
    public class ConfigsService : IConfigsService
    {
        #region Attributes and Properties

        public string Acoes { get; set; }

        public TimeSpan Opening { get; set; }

        public TimeSpan Closing { get; set; }

        public int CandlePeriod { get; set; }

        public DateTime MarketOpening
        {
            get { return DateTime.Today + this.Opening; }
        }

        public DateTime MarketClosing
        {
            get { return DateTime.Today + this.Closing; }
        }

        public bool IsMarketOpened
        {
            get
            {
                var now = DateTime.Now;
                if (now < this.MarketOpening || now >= this.MarketClosing)
                    return false;

                return true;
            }
        }

        #endregion

        #region Private Methods

        private string ReadConfig(Dictionary<string, string> configs, string key)
        {
            if (configs.ContainsKey(key))
                return configs[key];

            return string.Empty;
        }

        private Dictionary<string, string> ReadFile()
        {
            var configs = new Dictionary<string, string>();

            if (!File.Exists("config.ini"))
                return configs;

            using (var sr = new StreamReader("config.ini", Encoding.UTF8))
            {
                while (true)
                {
                    var line = sr.ReadLine();

                    if (line == null)
                        break;

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    configs.Add(line.Split('=')[0], line.Split('=')[1]);
                }
            }

            return configs;
        }

        #endregion

        #region Public Methods

        public void Load()
        {
            var raw = ReadFile();
            if (string.IsNullOrWhiteSpace(ReadConfig(raw, "ABERTURA")))
                this.Acoes = "IBOV";
            else
                this.Acoes = ReadConfig(raw, "ACOES");

            if (string.IsNullOrWhiteSpace(ReadConfig(raw, "ABERTURA")))
                this.Opening = new TimeSpan(10, 0, 0);
            else
                this.Opening = TimeSpan.Parse(ReadConfig(raw, "ABERTURA"));

            if(string.IsNullOrWhiteSpace(ReadConfig(raw, "FECHAMENTO")))
                this.Closing = new TimeSpan(18, 30, 0);
            else
                this.Closing = TimeSpan.Parse(ReadConfig(raw, "FECHAMENTO"));

            if(string.IsNullOrWhiteSpace(ReadConfig(raw, "CANDLEPERIOD")))
                this.CandlePeriod = 30;
            else
                this.CandlePeriod = int.Parse(ReadConfig(raw, "CANDLEPERIOD"));
        }

        public void Save()
        {
            var sb = new StringBuilder();
            sb.AppendLine("ACOES=" + this.Acoes);
            sb.AppendLine("ABERTURA=" + this.Opening.ToString("hh\\:mm\\:ss"));
            sb.AppendLine("FECHAMENTO=" + this.Closing.ToString("hh\\:mm\\:ss"));
            sb.AppendLine("CANDLEPERIOD=" + this.CandlePeriod);

            using (var sw = new StreamWriter("config.ini"))
                sw.Write(sb.ToString().Trim());
        }

        #endregion
    }
}
