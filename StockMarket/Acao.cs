using PriceMonitor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var refDate = new DateTime(year, month, day, 10, 30, 00);
            if (this.RequestedDate <= refDate && this.RequestedDate > refDate.AddMinutes(-30))
                return "Time1030";

            refDate = new DateTime(year, month, day, 11, 00, 00);
            if (this.RequestedDate <= refDate && this.RequestedDate > refDate.AddMinutes(-30))
                return "Time1100";
            
            refDate = new DateTime(year, month, day, 11, 30, 00);
            if (this.RequestedDate <= refDate && this.RequestedDate > refDate.AddMinutes(-30))
                return "Time1130";
            
            refDate = new DateTime(year, month, day, 12, 00, 00);
            if (this.RequestedDate <= refDate && this.RequestedDate > refDate.AddMinutes(-30))
                return "Time1200";
            
            refDate = new DateTime(year, month, day, 12, 30, 00);
            if (this.RequestedDate <= refDate && this.RequestedDate > refDate.AddMinutes(-30))
                return "Time1230";
            
            refDate = new DateTime(year, month, day, 13, 00, 00);
            if (this.RequestedDate <= refDate && this.RequestedDate > refDate.AddMinutes(-30))
                return "Time1300";
            
            refDate = new DateTime(year, month, day, 13, 30, 00);
            if (this.RequestedDate <= refDate && this.RequestedDate > refDate.AddMinutes(-30))
                return "Time1330";
            
            refDate = new DateTime(year, month, day, 14, 00, 00);
            if (this.RequestedDate <= refDate && this.RequestedDate > refDate.AddMinutes(-30))
                return "Time1400";
            
            refDate = new DateTime(year, month, day, 14, 30, 00);
            if (this.RequestedDate <= refDate && this.RequestedDate > refDate.AddMinutes(-30))
                return "Time1430";
            
            refDate = new DateTime(year, month, day, 15, 00, 00);
            if (this.RequestedDate <= refDate && this.RequestedDate > refDate.AddMinutes(-30))
                return "Time1500";
            
            refDate = new DateTime(year, month, day, 15, 30, 00);
            if (this.RequestedDate <= refDate && this.RequestedDate > refDate.AddMinutes(-30))
                return "Time1530";
            
            refDate = new DateTime(year, month, day, 16, 00, 00);
            if (this.RequestedDate <= refDate && this.RequestedDate > refDate.AddMinutes(-30))
                return "Time1600";
            
            refDate = new DateTime(year, month, day, 16, 30, 00);
            if (this.RequestedDate <= refDate && this.RequestedDate > refDate.AddMinutes(-30))
                return "Time1630";
            
            refDate = new DateTime(year, month, day, 17, 00, 00);
            if (this.RequestedDate <= refDate && this.RequestedDate > refDate.AddMinutes(-30))
                return "Time1700";
            
            refDate = new DateTime(year, month, day, 17, 30, 00);
            if (this.RequestedDate <= refDate && this.RequestedDate > refDate.AddMinutes(-30))
                return "Time1730";
            
            refDate = new DateTime(year, month, day, 18, 00, 00);
            if (this.RequestedDate <= refDate && this.RequestedDate > refDate.AddMinutes(-30))
                return "Time1800";

            return string.Empty;
        }
    }
}
