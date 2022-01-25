using PriceMonitor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StockMarket.Services
{
    public class PersistenceService : IPersistenceService
    {
        #region MyRegion

        public PersistenceService(IConfigsService configsService)
        {
            this._configsService = configsService;
        }

        #endregion

        #region Private Field

        private readonly IConfigsService _configsService;

        #endregion

        #region Private Methods

        private AcoesCollection LoadFromFile(string file)
        {
            var acoes = new AcoesCollection();

            using (var sr = new StreamReader(file))
                while (true)
                {
                    var line = (sr.ReadLine() ?? "").Trim();

                    if (string.IsNullOrEmpty(line))
                        break;

                    line = NormalizeLine(line);

                    var splitedLine = line.Split(';');

                    if (!acoes.Acoes.Any())
                        acoes.Name = splitedLine[0];

                    acoes.Acoes.Add(new Acao()
                    {
                        RequestedDate = Convert.ToDateTime(splitedLine[1]),
                        Date = Convert.ToDateTime(splitedLine[2]),
                        OppeningPrice = splitedLine[3].ToDecimal(),
                        Price = splitedLine[4].ToDecimal(),
                        MinimunPrice = splitedLine[5].ToDecimal(),
                        MaximunPrice = splitedLine[6].ToDecimal(),
                        AveragePrice = splitedLine[7].ToDecimal(),
                        Volume = splitedLine[8].ToDecimal(),
                        ClosedPrice = splitedLine[9].ToDecimal()
                    });
                }

            if (!acoes.Acoes.Any())
                return null;

            //30 min X max points
            var max = this._configsService.MaxCandlesInGraph * this._configsService.CandlePeriod;

            if (acoes.Acoes.Count > max)
                acoes.Acoes = acoes.Acoes.TakeLast(max).ToList();

            return acoes;
        }

        private static string NormalizeLine(string line)
        {
            var columns = line.Count(x => x == ';');
            if (columns < 9)
                line = string.Concat(line, new string(';', 9 - columns));

            return line;
        }

        #endregion

        #region Public Methods

        public List<AcoesCollection> LoadFromFiles(string[] ticketNames)
        {
            var acoesMonitorList = new List<AcoesCollection>();

            foreach (var file in Directory.EnumerateFiles("DataFiles"))
            {
                if (!ticketNames.Any(x => x == Path.GetFileNameWithoutExtension(file)))
                    continue;

                var acoes = LoadFromFile(file);
                if (acoes != null)
                    acoesMonitorList.Add(acoes);
            }

            return acoesMonitorList;
        }

        public void SaveToFile(string name, Acao acao)
        {
            if (!Directory.Exists("DataFiles"))
                Directory.CreateDirectory("DataFiles");

            var spliter = ";";
            using (var sw = new StreamWriter(Path.Combine("DataFiles", name + ".txt"), true))
                sw.WriteLine(
                    name + spliter +
                    acao.RequestedDate + spliter +
                    acao.Date + spliter +
                    acao.OppeningPrice + spliter +
                    acao.Price + spliter +
                    acao.MinimunPrice + spliter +
                    acao.MaximunPrice + spliter +
                    acao.AveragePrice + spliter +
                    acao.Volume + spliter +
                    acao.ClosedPrice);
        }

        #endregion
    }
}
