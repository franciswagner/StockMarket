using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace StockMarket
{
    public class CustomChart : Chart
    {
        public CustomChart()
        {
            this.DoubleBuffered = true;
        }
    }
}
