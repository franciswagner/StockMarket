using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using PriceMonitor;

namespace StockMarket
{
    public class AbstractRow : INotifyPropertyChanged
    {
        public AbstractRow()
        {
        }

        public AbstractRow(DateTime reffDate, AcoesCollection acoes)
        {
            this.Id = acoes.GetHashCode();
            this.Name = acoes.Name;

            if (!acoes.Acoes.Any())
                return;

            var day = reffDate.Day;
            var month = reffDate.Month;
            var year = reffDate.Year;

            var sorted = acoes.Acoes.OrderByDescending(x => x.RequestedDate);

            var referenceAcao = sorted.FirstOrDefault(x => x.RequestedDate >= new DateTime(year, month, day, 10, 00, 00) && x.RequestedDate <= new DateTime(year, month, day, 18, 00, 00));

            var differencePriceValue = 0M;
            var differencePricePercentage = 0M;

            var closedPrice = referenceAcao?.ClosedPrice ?? 0M;
            var openingPrice = referenceAcao?.OppeningPrice ?? 0M;
            var lastPrice = sorted.FirstOrDefault()?.Price ?? 0M;
            if (referenceAcao != null)
            {
                differencePriceValue = lastPrice - closedPrice;
                differencePricePercentage = (differencePriceValue / closedPrice) * 100;
            }

            var volume = acoes.Acoes.Last().Volume;

            this.Closing = closedPrice.ToString("#,##0.00");
            this.Opening = openingPrice.ToString("#,##0.00");
            //this.Minimun = acoes.Acoes.Where(x => x.RequestedDate.Date == DateTime.Now.Date).Min(x => x.Price).ToString("#,##0.00");
            this.Minimun = acoes.Acoes.Last().MinimunPrice_Formated;
            //this.Maximun = acoes.Acoes.Where(x => x.RequestedDate.Date == DateTime.Now.Date).Max(x => x.Price).ToString("#,##0.00");
            this.Maximun = acoes.Acoes.Last().MaximunPrice_Formated;
            this.Time1030 = sorted.FirstOrDefault(x => x.RequestedDate <= new DateTime(year, month, day, 10, 30, 00) && x.RequestedDate > new DateTime(year, month, day, 10, 00, 00))?.Price.ToString("#,##0.00");
            this.Time1100 = sorted.FirstOrDefault(x => x.RequestedDate <= new DateTime(year, month, day, 11, 00, 00) && x.RequestedDate > new DateTime(year, month, day, 10, 30, 00))?.Price.ToString("#,##0.00");
            this.Time1130 = sorted.FirstOrDefault(x => x.RequestedDate <= new DateTime(year, month, day, 11, 30, 00) && x.RequestedDate > new DateTime(year, month, day, 11, 00, 00))?.Price.ToString("#,##0.00");
            this.Time1200 = sorted.FirstOrDefault(x => x.RequestedDate <= new DateTime(year, month, day, 12, 00, 00) && x.RequestedDate > new DateTime(year, month, day, 11, 30, 00))?.Price.ToString("#,##0.00");
            this.Time1230 = sorted.FirstOrDefault(x => x.RequestedDate <= new DateTime(year, month, day, 12, 30, 00) && x.RequestedDate > new DateTime(year, month, day, 12, 00, 00))?.Price.ToString("#,##0.00");
            this.Time1300 = sorted.FirstOrDefault(x => x.RequestedDate <= new DateTime(year, month, day, 13, 00, 00) && x.RequestedDate > new DateTime(year, month, day, 12, 30, 00))?.Price.ToString("#,##0.00");
            this.Time1330 = sorted.FirstOrDefault(x => x.RequestedDate <= new DateTime(year, month, day, 13, 30, 00) && x.RequestedDate > new DateTime(year, month, day, 13, 00, 00))?.Price.ToString("#,##0.00");
            this.Time1400 = sorted.FirstOrDefault(x => x.RequestedDate <= new DateTime(year, month, day, 14, 00, 00) && x.RequestedDate > new DateTime(year, month, day, 13, 30, 00))?.Price.ToString("#,##0.00");
            this.Time1430 = sorted.FirstOrDefault(x => x.RequestedDate <= new DateTime(year, month, day, 14, 30, 00) && x.RequestedDate > new DateTime(year, month, day, 14, 00, 00))?.Price.ToString("#,##0.00");
            this.Time1500 = sorted.FirstOrDefault(x => x.RequestedDate <= new DateTime(year, month, day, 15, 00, 00) && x.RequestedDate > new DateTime(year, month, day, 14, 30, 00))?.Price.ToString("#,##0.00");
            this.Time1530 = sorted.FirstOrDefault(x => x.RequestedDate <= new DateTime(year, month, day, 15, 30, 00) && x.RequestedDate > new DateTime(year, month, day, 15, 00, 00))?.Price.ToString("#,##0.00");
            this.Time1600 = sorted.FirstOrDefault(x => x.RequestedDate <= new DateTime(year, month, day, 16, 00, 00) && x.RequestedDate > new DateTime(year, month, day, 15, 30, 00))?.Price.ToString("#,##0.00");
            this.Time1630 = sorted.FirstOrDefault(x => x.RequestedDate <= new DateTime(year, month, day, 16, 30, 00) && x.RequestedDate > new DateTime(year, month, day, 16, 00, 00))?.Price.ToString("#,##0.00");
            this.Time1700 = sorted.FirstOrDefault(x => x.RequestedDate <= new DateTime(year, month, day, 17, 00, 00) && x.RequestedDate > new DateTime(year, month, day, 16, 30, 00))?.Price.ToString("#,##0.00");
            this.Time1730 = sorted.FirstOrDefault(x => x.RequestedDate <= new DateTime(year, month, day, 17, 30, 00) && x.RequestedDate > new DateTime(year, month, day, 17, 00, 00))?.Price.ToString("#,##0.00");
            this.Time1800 = sorted.FirstOrDefault(x => x.RequestedDate <= new DateTime(year, month, day, 18, 00, 00) && x.RequestedDate > new DateTime(year, month, day, 17, 30, 00))?.Price.ToString("#,##0.00");
            this.RentabilidadePerc = differencePricePercentage.ToString("#,##0.00");
            this.RentabilidadeValor = differencePriceValue.ToString("#,##0.00");
            this.Volume = volume > 1000000 ? (volume / 1000000M).ToString("#,##0.000") + "M" : volume.ToString("#,##0");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string CalculateMD5()
        {
            var values = string.Empty;
            var properties = this.GetType().GetProperties();

            foreach (var property in properties)
                if (property.Name != "Id")
                    values += property.GetValue(this);

            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.ASCII.GetBytes(values);
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (byte bytes in hashBytes)
                    sb.Append(bytes.ToString("X2"));

                return sb.ToString();
            }
        }

        [Browsable(false)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Closing { get; set; }
        public string Opening { get; set; }
        public string Minimun { get; set; }
        public string Maximun { get; set; }
        public string Time1030 { get; set; }
        public string Time1100 { get; set; }
        public string Time1130 { get; set; }
        public string Time1200 { get; set; }
        public string Time1230 { get; set; }
        public string Time1300 { get; set; }
        public string Time1330 { get; set; }
        public string Time1400 { get; set; }
        public string Time1430 { get; set; }
        public string Time1500 { get; set; }
        public string Time1530 { get; set; }
        public string Time1600 { get; set; }
        public string Time1630 { get; set; }
        public string Time1700 { get; set; }

        private string _time1730 = null;
        public string Time1730
        {
            get { return this._time1730; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                }
                this._time1730 = value;
            }
        }

        private string _time1800 = null;
        public string Time1800
        {
            get { return this._time1800; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                }
                this._time1800 = value;
            }
        }

        public string RentabilidadePerc { get; set; }
        public string RentabilidadeValor { get; set; }
        public string Volume { get; set; }

        public void UpdateValue(Acao acao)
        {
            var propertyName = acao.CalculateDatePeriod();

            if (string.IsNullOrEmpty(propertyName))
                return;

            #region Valores de Referência

            if (this.Closing != acao.ClosedPrice_Formated)
            {
                this.Closing = acao.ClosedPrice_Formated;
                this.OnPropertyChanged("Closing");
            }

            if (this.Opening != acao.OppeningPrice_Formated)
            {
                this.Opening = acao.OppeningPrice_Formated;
                this.OnPropertyChanged("Opening");
            }

            if (this.Minimun != acao.MinimunPrice_Formated)
            {
                this.Minimun = acao.MinimunPrice_Formated;
                this.OnPropertyChanged("Minimun");
            }

            if (this.Maximun != acao.MaximunPrice_Formated)
            {
                this.Maximun = acao.MaximunPrice_Formated;
                this.OnPropertyChanged("Maximun");
            }

            #endregion

            #region Price

            var currentValue = (this.GetType().GetProperty(propertyName).GetValue(this) ?? "").ToString();

            if (currentValue == acao.Price_Formated)
                return;

            this.GetType().GetProperty(propertyName).SetValue(this, acao.Price_Formated);

            OnPropertyChanged(propertyName);

            #endregion

            #region Rentabilidade

            var closedPrice = acao.ClosedPrice;
            //var openingPrice = acao.OppeningPrice;
            var lastPrice = acao.Price;

            var differencePriceValue = lastPrice - closedPrice;
            var differencePricePercentage = (differencePriceValue / closedPrice) * 100;

            if (this.RentabilidadePerc != differencePricePercentage.ToString("#,##0.00"))
            {
                this.RentabilidadePerc = differencePricePercentage.ToString("#,##0.00");
                this.OnPropertyChanged("RentabilidadePerc");
            }

            if (this.RentabilidadeValor != differencePriceValue.ToString("#,##0.00"))
            {
                this.RentabilidadeValor = differencePriceValue.ToString("#,##0.00");
                this.OnPropertyChanged("RentabilidadeValor");
            }

            #endregion

            #region Volume

            this.Volume = acao.Volume > 1000000 ? (acao.Volume / 1000000M).ToString("#,##0.000") + "M" : acao.Volume.ToString("#,##0");
            this.OnPropertyChanged("Volume");

            #endregion
        }

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}