using PriceMonitor;
using System;
using System.ComponentModel;

namespace StockMarket
{
    public class Acao
    {
        public Acao()
        {

        }

        public Acao(AcoesJsonReaderPrice acoesJsonReader)
        {
            this.RequestedDate = DateTime.Now;
            this.Date = acoesJsonReader.UT;
            this.OppeningPrice = acoesJsonReader.Ps.OP;
            this.ClosedPrice = acoesJsonReader.Ps.CP;
            this.Price = acoesJsonReader.Ps.P;
            this.MinimunPrice = acoesJsonReader.Ps.MnP;
            this.MaximunPrice = acoesJsonReader.Ps.MxP;
            this.AveragePrice = acoesJsonReader.Ps.AvP;
            this.Volume = acoesJsonReader.Ps.V;
        }

        public DateTime RequestedDate { get; set; }
        public DateTime Date { get; set; }

        [Browsable(false)]
        public decimal OppeningPrice { get; set; }

        [Browsable(false)]
        public decimal ClosedPrice { get; set; }

        [Browsable(false)]
        public decimal Price { get; set; }

        [Browsable(false)]
        public decimal MinimunPrice { get; set; }

        [Browsable(false)]
        public decimal MaximunPrice { get; set; }

        [Browsable(false)]
        public decimal AveragePrice { get; set; }

        [Browsable(false)]
        public decimal Volume { get; set; }

        public string OppeningPrice_Formated
        {
            get { return this.OppeningPrice.ToString("#,##0.00"); }
        }
        
        [DisplayName("Preço de Fechamento (R$)")]
        public string ClosedPrice_Formated
        {
            get { return this.ClosedPrice.ToString("#,##0.00"); }
        }

        public string Price_Formated
        {
            get { return this.Price.ToString("#,##0.00"); }
        }

        [DisplayName("Mínimo")]
        public string MinimunPrice_Formated
        {
            get { return this.MinimunPrice.ToString("#,##0.00"); }
        }

        [DisplayName("Máximo")]
        public string MaximunPrice_Formated
        {
            get { return this.MaximunPrice.ToString("#,##0.00"); }
        }

        [DisplayName("Médio")]
        public string AveragePrice_Formated
        {
            get { return this.AveragePrice.ToString("#,##0.00"); }
        }

        [DisplayName("Volume")]
        public string Volume_Formated
        {
            get
            {
                if (this.Volume > 1000000)
                    return (this.Volume / 1000000M).ToString("#,##0.000") + "M";

                return this.Volume.ToString("#,##0");
            }
        }
        
        [Browsable(false)]
        public string Volume_Formated_Graph
        {
            get
            {
                if (this.Volume > 1000000)
                    return (this.Volume / 1000000M).ToString("0.000");

                return this.Volume.ToString("0");
            }
        }

        public string CalculateDatePeriod()
        {
            var day = this.RequestedDate.Day;
            var month = this.RequestedDate.Month;
            var year = this.RequestedDate.Year;

            var refDate = new DateTime(year, month, day, 10, 00, 00);
            for (var i = 0; i < 17; i++) // from 10:30 to 18:00
            {
                refDate = refDate.AddMinutes(30);

                if (this.RequestedDate <= refDate && this.RequestedDate > refDate.AddMinutes(-30))
                    return $"Time{refDate:HHmm}";
            }

            return string.Empty;
        }
    }
}
