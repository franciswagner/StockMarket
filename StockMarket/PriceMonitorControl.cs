using PriceMonitor;
using StockMarket.Services;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace StockMarket
{
    public partial class PriceMonitorControl : UserControl
    {
        #region Constructors

        public PriceMonitorControl(IConfigsService configsService)
        {
            this._configsService = configsService;

            this.InitializeComponent();
        }

        public PriceMonitorControl(string acao, IConfigsService configsService)
        {
            this.Acao = acao;
            this._configsService = configsService;

            this.InitializeComponent();
        }

        #endregion

        #region Attributes and Properties

        public string Acao { get; }

        #endregion

        #region Private Fields

        private IConfigsService _configsService;

        #endregion

        #region Static Methods

        private static Series CreateCandleSeries(string name, bool showLegend)
        {
            var series = new Series(name);
            series.ChartType = SeriesChartType.Candlestick;
            series.BorderWidth = 2;
            series.IsVisibleInLegend = showLegend;
            series.ToolTip = "#VALX\n#VALY{F2}";

            // Set the style of the open-close marks
            series["OpenCloseStyle"] = "Triangle";

            // Show both open and close marks
            series["ShowOpenClose"] = "Both";

            // Set point width
            series["PointWidth"] = "0.7";

            // Set colors bars
            series["PriceUpColor"] = "Green"; // <<== use text indexer for series
            series["PriceDownColor"] = "Red"; // <<== use text indexer for series

            return series;
        }

        private static Series CreateColumnSeries(string name, bool showLegend)
        {
            var series = new Series(name);
            series.ChartType = SeriesChartType.Column;
            series.BorderWidth = 2;
            series.IsVisibleInLegend = showLegend;
            series.ToolTip = "#VALX\n#VALY{F2}";

            return series;
        }

        #endregion

        #region Public Methods

        public void UpdateControl(List<AcoesCollection> acoesCollections)
        {
            var teste = this.Acao;
            var acaoList = acoesCollections.FirstOrDefault(x => x.Name == teste);

            IEnumerable<Acao> acoes = new List<Acao>();

            if (acaoList != null)
                acoes = acaoList.Acoes;

            this.dgvListPrice.DataSource = acoes.Reverse().Take(60).ToList();

            this.chtChart.Series.Clear();
            this.chtVolumeChart.Series.Clear();

            var listaAcoes = acoes.ToList();
            var groups = listaAcoes.GroupBy(x =>
            {
                var stamp = x.RequestedDate;
                stamp = stamp.AddMinutes(-(stamp.Minute % this._configsService.CandlePeriod));
                stamp = stamp.AddMilliseconds(-stamp.Millisecond - 1000 * stamp.Second);
                return stamp.ToString();
            })
            .Select(g => new { TimeStamp = g.Key, Value = g.ToList() })
            .ToList();

            groups = groups.TakeLastObject(WebMonitor.MAX_CANDLES_IN_GRAPH).ToList();

            #region Coulumn Series

            var columnSeries = CreateColumnSeries("Volume", false);
            columnSeries.Color = Color.Blue;

            this.chtVolumeChart.Series.Add(columnSeries);
            this.chtVolumeChart.ChartAreas[0].AxisY.LabelStyle.Format = "{0:0,,}M";

            foreach (var value in groups.Select(group => group.Value))
            {
                var first = value.OrderBy(x => x.RequestedDate).First().Volume;
                var last = value.OrderBy(x => x.RequestedDate).Last().Volume;
                var volume = last - first;
                var date = value.OrderBy(x => x.RequestedDate).Last().RequestedDate;

                columnSeries.Points.AddXY(date.ToString("dd/MM/yyyy HH:mm"), volume);
            }

            #endregion

            #region Candle Series

            var lineSeries = CreateCandleSeries("price", false);
            this.chtChart.Series.Add(lineSeries);

            this.chtChart.ChartAreas[0].AxisX.IsMarginVisible = false;
            this.chtChart.ChartAreas[0].AxisY.LabelStyle.Format = "#,##0.00";
            this.chtChart.ChartAreas[0].AxisY.IsStartedFromZero = false;

            for (var i = 0; i < groups.Count; i++)
            {
                var acao = groups[i].Value.OrderBy(x => x.RequestedDate).LastOrDefault();
                var min = (double)groups[i].Value.Min(x => x.Price);
                var high = (double)groups[i].Value.Max(x => x.Price);
                var open = (double)acao.Price;
                if (i > 0)
                    open = (double)(groups[i - 1].Value.OrderBy(x => x.RequestedDate).LastOrDefault()?.Price ?? acao.Price);
                var close = (double)acao.Price;

                // adding date and high
                lineSeries.Points.AddXY(acao.RequestedDate.ToString("dd/MM/yyyy HH:mm"), high);
                // adding low
                lineSeries.Points[i].YValues[1] = min;
                //adding open
                lineSeries.Points[i].YValues[2] = open;
                // adding close
                lineSeries.Points[i].YValues[3] = close;

                lineSeries.Color = Color.Black;
                lineSeries.BorderWidth = 2;
            }

            #endregion
        }

        #endregion
    }
}
