using StockMarket.Services;
using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace StockMarket
{
    public partial class ConfigForm : Form
    {
        #region Constructors

        public ConfigForm(IConfigsService configsService)
        {
            this._configsService = configsService;

            this.InitializeComponent();

            this.txtAcoes.Text = this._configsService.Acoes;
            this.txtOpening.Text = this._configsService.Opening.ToString("hh\\:mm\\:ss");
            this.txtClosing.Text = this._configsService.Closing.ToString("hh\\:mm\\:ss");

            switch (this._configsService.CandlePeriod)
            {
                case 5:
                    this.rtb5Min.Checked = true;
                    break;
                case 15:
                    this.rtb15Min.Checked = true;
                    break;
                case 30:
                    this.rtb30Min.Checked = true;
                    break;
                case 60:
                    this.rtb60Min.Checked = true;
                    break;
                case 360:
                    this.rtb360Min.Checked = true;
                    break;
            }
        }

        #endregion

        #region Private Fields

        private IConfigsService _configsService;

        #endregion

        #region Private Methods

        private void ManageButton()
        {
            if (this.txtAcoes.ForeColor == Color.Black &&
                this.txtClosing.ForeColor == Color.Black &&
                this.txtOpening.ForeColor == Color.Black)
            {
                this.btnOK.Enabled = true;
            }
            else
            {
                this.btnOK.Enabled = false;
            }
        }


        private bool Save()
        {
            if (!ValidateAcoes(this.txtAcoes.Text) || !ValidateTime(this.txtOpening.Text) || !ValidateTime(this.txtClosing.Text))
            {
                MessageBox.Show("Nem todos os campos são válidos", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (this.rtb5Min.Checked)
                this._configsService.CandlePeriod = 5;
            else if (this.rtb15Min.Checked)
                this._configsService.CandlePeriod = 15;
            else if (this.rtb30Min.Checked)
                this._configsService.CandlePeriod = 30;
            else if (this.rtb60Min.Checked)
                this._configsService.CandlePeriod = 60;
            else if (this.rtb360Min.Checked)
                this._configsService.CandlePeriod = 360;

            this._configsService.Acoes = this.txtAcoes.Text;
            this._configsService.Opening = TimeSpan.Parse(this.txtOpening.Text);
            this._configsService.Closing = TimeSpan.Parse(this.txtClosing.Text);
            this._configsService.Save();

            MessageBox.Show("Configurações salvas com sucesso", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

        #endregion

        #region Static Methods

        private static bool ValidateAcoes(string acoes)
        {
            var regex = new Regex(@"(([A-Z]{4}[0-9]{0,2})[;]{0,1})", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline);

            var result = new StringBuilder();
            foreach (Match match in regex.Matches(acoes))
                result.Append(match.Value);

            if (acoes == result.ToString())
                return true;
            else
                return false;
        }

        private static bool ValidateTime(string time)
        {
            try
            {
                //Testa se consegue dar parse
                var result = TimeSpan.Parse(time);
                if (result.Days != 0)
                    return false;

                var regex = new Regex(@"^[0-9]{2}:[0-9]{2}:[0-9]{2}$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline);
                if (regex.IsMatch(time))
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Signed Event Methods

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.Save())
                this.Dispose();
        }


        private void txtAcoes_TextChanged(object sender, EventArgs e)
        {
            if (ValidateAcoes(this.txtAcoes.Text))
                this.txtAcoes.ForeColor = Color.Black;
            else
                this.txtAcoes.ForeColor = Color.Red;

            this.ManageButton();
        }

        private void txtClosing_TextChanged(object sender, EventArgs e)
        {
            if (ValidateTime(this.txtClosing.Text))
                this.txtClosing.ForeColor = Color.Black;
            else
                this.txtClosing.ForeColor = Color.Red;

            this.ManageButton();
        }

        private void txtOpening_TextChanged(object sender, EventArgs e)
        {
            if (ValidateTime(this.txtOpening.Text))
                this.txtOpening.ForeColor = Color.Black;
            else
                this.txtOpening.ForeColor = Color.Red;

            this.ManageButton();
        }

        #endregion
    }
}
