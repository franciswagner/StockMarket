using PriceMonitor;
using StockMarket.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockMarket
{
    public partial class MainForm : Form
    {
        #region Constructors

        public MainForm()
        {
            this.InitializeComponent();

            CreateRootFolder();

            if (!string.IsNullOrWhiteSpace(Configs.Acoes))
                this._acoes = Configs.Acoes.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        }

        #endregion

        #region Private Fields

        private bool _bypassFormClosing = false;

        private string[] _acoes;

        private WebMonitor _webMonitor = null;

        private bool _marketOpenNotify = true;
        private bool _marketClosedNotify = false;

        private BindingList<AbstractRow> _table = new BindingList<AbstractRow>();

        #endregion

        #region Private Methods

        private List<TabPage> CreateTabsControls()
        {
            var tabPageControlList = new List<TabPage>();

            this._acoes = this._acoes.Where(x => x.ToUpper() == "IBOV").Concat(this._acoes.Where(x => x.ToUpper() != "IBOV").OrderBy(x => x)).ToArray();

            foreach (var acao in this._acoes)
            {
                var tabPage = new TabPage()
                {
                    Location = new Point(-1, 22),
                    Padding = new Padding(3),
                    Size = new Size(814, 608),
                    Text = acao
                };

                this.tbcPriceMonitor.Controls.Add(tabPage);

                tabPageControlList.Add(tabPage);
            }

            return tabPageControlList;
        }

        private void RunMonitoringControls(List<TabPage> tabPagesControlList)
        {
            var cancellationToken = new CancellationTokenSource();

            Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    this._webMonitor.Run((List<AcoesCollection> acoesCollections) =>
                    {
                        this.Invoke(new Action(() =>
                        {
                            this.UpdateAbstract(acoesCollections);
                            this.UpdateTabsPage(acoesCollections, tabPagesControlList);
                        }));
                    },
                    () => // Callback mercado aberto
                    {
                        this.Invoke(new Action(NotifyMarketOpen));
                    },
                    () => // Callback mercado fechado
                    {
                        this.Invoke(new Action(NotifyClosedMarket));
                    },
                    (message) => // Callback erro na comunicação
                    {
                        this.Invoke(new Action(() => NotifyCommunicationError(message)));
                    },
                    (count, message) => // Callback notificação
                    {
                        this.Invoke(new Action(() => NotifyNewMinimunValue(count, message)));
                    });
                }
            }, cancellationToken.Token);
        }

        private void LoadAbstract(List<AcoesCollection> acoesCollections)
        {
            if (acoesCollections == null)
                return;

            acoesCollections = acoesCollections.Where(x => x.Name.ToLower() == "ibov").Concat(acoesCollections.Where(x => x.Name.ToLower() != "ibov")).ToList();

            foreach (var acao in acoesCollections)
                this._table.Add(new AbstractRow(DateTime.Now, acao));

            this.Day.HeaderText = "Dia " + DateTime.Now.ToString("dd/MM/yyyy");
            this.dgvAbstract.DataSource = this._table;
        }

        private void NotifyClosedMarket()
        {
            if (this._marketClosedNotify)
            {
                this._marketClosedNotify = false;
                this._marketOpenNotify = true;
                this.ntiTrayIcon.BalloonTipTitle = "Mercado fechado";
                this.ntiTrayIcon.BalloonTipText = "Mercado fechado";
                this.ntiTrayIcon.ShowBalloonTip(10000);
            }

            this.tstStatusIcon.Image = Resources.alert_orange;
            this.tstStatusIcon.ToolTipText = $"Mercado fechado ({WebMonitor.MARKET_OPENING:HH:mm}-{WebMonitor.MARKET_CLOSING:HH:mm})";
        }

        private void NotifyCommunicationError(string message)
        {
            if (this._marketOpenNotify)
            {
                this._marketOpenNotify = false;
                this._marketClosedNotify = true;
                this.ntiTrayIcon.BalloonTipTitle = "Mercado aberto";
                this.ntiTrayIcon.BalloonTipText = "Mercado aberto";
                this.ntiTrayIcon.ShowBalloonTip(10000);
            }

            this.tstStatusIcon.Image = Resources.alert_red;
            this.tstStatusIcon.ToolTipText = message;
        }

        private void NotifyMarketOpen()
        {
            if (this._marketOpenNotify)
            {
                this._marketOpenNotify = false;
                this._marketClosedNotify = true;
                this.ntiTrayIcon.BalloonTipTitle = "Mercado aberto";
                this.ntiTrayIcon.BalloonTipText = "Mercado aberto";
                this.ntiTrayIcon.ShowBalloonTip(10000);
            }

            this.tstStatusIcon.Image = Resources.alert_green;
            this.tstStatusIcon.ToolTipText = "Mercado aberto";
        }

        private void NotifyNewMinimunValue(int count, string message)
        {
            this.ntiTrayIcon.BalloonTipTitle = $"Novo valor mínimo ({count})";
            this.ntiTrayIcon.BalloonTipText = message;
            this.ntiTrayIcon.ShowBalloonTip(10000);
        }

        private void UpdateAbstract(List<AcoesCollection> acoesCollections)
        {
            if (acoesCollections == null || !acoesCollections.Any())
                return;

            foreach (var acoesCollection in acoesCollections)
            {
                var acao = acoesCollection.Acoes.LastOrDefault(x => x.RequestedDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"));

                if (acao == null)
                    continue;

                var row = this._table.First(x => x.Name == acoesCollection.Name);
                row.UpdateValue(acao);
            }

            this.Day.HeaderText = "Dia " + DateTime.Now.ToString("dd/MM/yyyy");
        }

        #endregion

        #region Static Methods

        private static void CreateRootFolder()
        {
            if (!Directory.Exists("DataFiles"))
                Directory.CreateDirectory("DataFiles");
        }

        private static void UpdateTabsPage(List<AcoesCollection> acoesCollections, List<TabPage> tabPagesControlList)
        {
            foreach (var tabPageControl in tabPagesControlList.Where(x => x.Visible))
                ((PriceMonitorControl)tabPageControl.Controls[0]).UpdateControl(acoesCollections);
        }

        #endregion

        #region Signed Event Methods

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this._bypassFormClosing)
            {
                this.Visible = false;
                e.Cancel = true;
            }
        }

        private void ntiTrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.Visible = true;
            else if (e.Button == MouseButtons.Right)
            {
                this.tcmMenu.Show(Control.MousePosition);
            }
        }

        private void tcmMenuClose_Click(object sender, EventArgs e)
        {
            this._bypassFormClosing = true;
            this.Dispose();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this._webMonitor = new WebMonitor(this._acoes);

            var acoesMonitorList = this._webMonitor.AcoesCollections;
            this.LoadAbstract(acoesMonitorList);

            var tabsControls = this.CreateTabsControls();

            this.RunMonitoringControls(tabsControls);
        }

        private void btnRestartApplication_Click(object sender, EventArgs e)
        {
            this._bypassFormClosing = true;
            Application.Restart();
        }

        private void tbcPriceMonitor_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (TabPage control in this.tbcPriceMonitor.Controls)
                if (control.Name != "tbpAbstract" && control.Controls.Count > 0)
                    control.Controls.Clear();

            var tabPage = this.tbcPriceMonitor.SelectedTab;
            var name = tabPage.Text;

            if (name == "Resumo")
                return;

            var priceMonitorControl = new PriceMonitorControl(name)
            {
                Dock = DockStyle.Fill,
                Location = new Point(3, 3),
                Size = new Size(808, 602),
                TabIndex = 1
            };

            tabPage.Controls.Add(priceMonitorControl);

            var acoesMonitorList = this._webMonitor.AcoesCollections;

            priceMonitorControl.UpdateControl(acoesMonitorList);
        }

        private void tsmToXml_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();

            var acoesCollections = this._webMonitor.AcoesCollections;

            if (acoesCollections == null)
                return;

            acoesCollections = acoesCollections.Where(x => x.Name.ToLower() == "ibov").Concat(acoesCollections.Where(x => x.Name.ToLower() != "ibov")).ToList();

            var table = new List<AbstractRow>();

            foreach (var acao in acoesCollections)
                table.Add(new AbstractRow(DateTime.Now, acao));

            sb.AppendLine("Dia;Fec. Preg Ant.;Abertura;10:30;1100;1130;1200;1230;1300;1330;1400;1430;1500;1530;1600;1630;1700;1730;1800;Rent/dia %;Ret/dia R$");

            foreach (var row in table)
            {
                sb.AppendLine($"{row.Name};{row.Closing};{row.Opening};{row.Time1030};{row.Time1100};{row.Time1130};{row.Time1200};{row.Time1230};{row.Time1300};{row.Time1330};{row.Time1400};" +
                              $"{row.Time1430};{row.Time1500};{row.Time1530};{row.Time1600};{row.Time1630};{row.Time1700};{row.Time1730};{row.Time1800};{row.RentabilidadePerc};{ row.RentabilidadeValor}");
            }

            var fileDialog = new SaveFileDialog()
            {
                Filter = "CSV|*.csv",
                Title = "Exportar para CSV",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),

            };

            // If the file name is not an empty string open it for saving.
            if (fileDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fileDialog.FileName))
            {
                var path = fileDialog.FileName;

                if (!path.ToLower().EndsWith(".csv"))
                    path += ".csv";

                using (var sw = new StreamWriter(path))
                    sw.Write(sb.ToString());

                MessageBox.Show("Tabela exportada com sucesso", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgvAbstract_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex.IsAnyOfThese(-1, 3, 4))
                return;

            this.dgvAbstract.CellPainting -= dgvAbstract_CellPainting;

            var row = this.dgvAbstract.Rows[e.RowIndex];
            var cell = row.Cells[e.ColumnIndex];

            var columnName = cell.OwningColumn.Name;
            var cellColor = cell.Style.ForeColor;

            if (columnName.StartsWith("Time"))
            {
                var valueCell = cell.Value;

                var rowObj = (AbstractRow)row.DataBoundItem;
                if (Convert.ToDecimal(valueCell) < Convert.ToDecimal(rowObj.Closing) && cellColor != Color.Red)
                    cell.Style.ForeColor = Color.Red;
                else if (Convert.ToDecimal(valueCell) > Convert.ToDecimal(rowObj.Closing) && cellColor != Color.Green)
                    cell.Style.ForeColor = Color.Green;
            }
            else if (columnName.StartsWith("Rent"))
            {
                var valueCell = cell.Value.ToString();

                if (valueCell.StartsWith("-") && cellColor != Color.Red)
                    cell.Style.ForeColor = Color.Red;
                else if (valueCell != "0,00" && cellColor != Color.Green)
                    cell.Style.ForeColor = Color.Green;
            }

            this.dgvAbstract.CellPainting += dgvAbstract_CellPainting;
        }

        private void tsmConfig_Click(object sender, EventArgs e)
        {
            var form = new ConfigForm();
            form.ShowDialog();
        }

        #endregion
    }
}
