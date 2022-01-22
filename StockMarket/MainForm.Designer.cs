using System.Windows.Forms;

namespace StockMarket
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ntiTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.tcmMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tcmMenuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.tbcPriceMonitor = new System.Windows.Forms.TabControl();
            this.tbpAbstract = new System.Windows.Forms.TabPage();
            this.dgvAbstract = new CustomDataGridView();
            this.mnsMenu = new System.Windows.Forms.MenuStrip();
            this.tsmMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmExportTo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmToXml = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRestartApplication = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmClose = new System.Windows.Forms.ToolStripMenuItem();
            this.tstStatusIcon = new System.Windows.Forms.ToolStripMenuItem();
            this.Day = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Minimun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Maximun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Closing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Opening = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time1030 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time1100 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time1130 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time1200 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time1230 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time1300 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time1330 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time1400 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time1430 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time1500 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time1530 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time1600 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time1630 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time1700 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time1730 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time1800 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RentabilidadePerc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RentabilidadeValor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tcmMenu.SuspendLayout();
            this.tbcPriceMonitor.SuspendLayout();
            this.tbpAbstract.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAbstract)).BeginInit();
            this.mnsMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ntiTrayIcon
            // 
            this.ntiTrayIcon.ContextMenuStrip = this.tcmMenu;
            this.ntiTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("ntiTrayIcon.Icon")));
            this.ntiTrayIcon.Text = "WTFMonitor";
            this.ntiTrayIcon.Visible = true;
            this.ntiTrayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ntiTrayIcon_MouseClick);
            // 
            // tcmMenu
            // 
            this.tcmMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tcmMenuClose});
            this.tcmMenu.Name = "cmsTrayContextMenu";
            this.tcmMenu.Size = new System.Drawing.Size(110, 26);
            // 
            // tcmMenuClose
            // 
            this.tcmMenuClose.Name = "tcmMenuClose";
            this.tcmMenuClose.Size = new System.Drawing.Size(109, 22);
            this.tcmMenuClose.Text = "Fechar";
            this.tcmMenuClose.Click += new System.EventHandler(this.tcmMenuClose_Click);
            // 
            // tbcPriceMonitor
            // 
            this.tbcPriceMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbcPriceMonitor.Controls.Add(this.tbpAbstract);
            this.tbcPriceMonitor.Location = new System.Drawing.Point(0, 27);
            this.tbcPriceMonitor.Name = "tbcPriceMonitor";
            this.tbcPriceMonitor.SelectedIndex = 0;
            this.tbcPriceMonitor.Size = new System.Drawing.Size(941, 629);
            this.tbcPriceMonitor.TabIndex = 1;
            this.tbcPriceMonitor.SelectedIndexChanged += new System.EventHandler(this.tbcPriceMonitor_SelectedIndexChanged);
            // 
            // tbpAbstract
            // 
            this.tbpAbstract.Controls.Add(this.dgvAbstract);
            this.tbpAbstract.Location = new System.Drawing.Point(4, 22);
            this.tbpAbstract.Name = "tbpAbstract";
            this.tbpAbstract.Padding = new System.Windows.Forms.Padding(3);
            this.tbpAbstract.Size = new System.Drawing.Size(933, 603);
            this.tbpAbstract.TabIndex = 2;
            this.tbpAbstract.Text = "Resumo";
            this.tbpAbstract.UseVisualStyleBackColor = true;
            // 
            // dgvAbstract
            // 
            this.dgvAbstract.AllowUserToAddRows = false;
            this.dgvAbstract.AllowUserToDeleteRows = false;
            this.dgvAbstract.AllowUserToResizeRows = false;
            this.dgvAbstract.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvAbstract.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Day,
            this.Minimun,
            this.Maximun,
            this.Closing,
            this.Opening,
            this.Time1030,
            this.Time1100,
            this.Time1130,
            this.Time1200,
            this.Time1230,
            this.Time1300,
            this.Time1330,
            this.Time1400,
            this.Time1430,
            this.Time1500,
            this.Time1530,
            this.Time1600,
            this.Time1630,
            this.Time1700,
            this.Time1730,
            this.Time1800,
            this.RentabilidadePerc,
            this.RentabilidadeValor});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAbstract.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvAbstract.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAbstract.Location = new System.Drawing.Point(3, 3);
            this.dgvAbstract.Name = "dgvAbstract";
            this.dgvAbstract.ReadOnly = true;
            this.dgvAbstract.RowHeadersVisible = false;
            this.dgvAbstract.Size = new System.Drawing.Size(927, 597);
            this.dgvAbstract.TabIndex = 0;
            this.dgvAbstract.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvAbstract_CellPainting);
            // 
            // mnsMenu
            // 
            this.mnsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmMenu,
            this.tstStatusIcon});
            this.mnsMenu.Location = new System.Drawing.Point(0, 0);
            this.mnsMenu.Name = "mnsMenu";
            this.mnsMenu.ShowItemToolTips = true;
            this.mnsMenu.Size = new System.Drawing.Size(941, 24);
            this.mnsMenu.TabIndex = 2;
            this.mnsMenu.Text = "darkMenuStrip1";
            // 
            // tsmMenu
            // 
            this.tsmMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmConfig,
            this.tsmExportTo,
            this.btnRestartApplication,
            this.toolStripSeparator1,
            this.tsmClose});
            this.tsmMenu.Name = "tsmMenu";
            this.tsmMenu.Size = new System.Drawing.Size(50, 20);
            this.tsmMenu.Text = "Menu";
            // 
            // tsmConfig
            // 
            this.tsmConfig.Name = "tsmConfig";
            this.tsmConfig.Size = new System.Drawing.Size(152, 22);
            this.tsmConfig.Text = "Configurar";
            this.tsmConfig.Click += new System.EventHandler(this.tsmConfig_Click);
            // 
            // tsmExportTo
            // 
            this.tsmExportTo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmToXml});
            this.tsmExportTo.Name = "tsmExportTo";
            this.tsmExportTo.Size = new System.Drawing.Size(152, 22);
            this.tsmExportTo.Text = "Exportar para...";
            // 
            // tsmToXml
            // 
            this.tsmToXml.Name = "tsmToXml";
            this.tsmToXml.Size = new System.Drawing.Size(98, 22);
            this.tsmToXml.Text = "XML";
            this.tsmToXml.Click += new System.EventHandler(this.tsmToXml_Click);
            // 
            // btnRestartApplication
            // 
            this.btnRestartApplication.Name = "btnRestartApplication";
            this.btnRestartApplication.Size = new System.Drawing.Size(152, 22);
            this.btnRestartApplication.Text = "Restart";
            this.btnRestartApplication.Click += new System.EventHandler(this.btnRestartApplication_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // tsmClose
            // 
            this.tsmClose.Name = "tsmClose";
            this.tsmClose.Size = new System.Drawing.Size(152, 22);
            this.tsmClose.Text = "Fechar";
            this.tsmClose.Click += new System.EventHandler(this.tcmMenuClose_Click);
            // 
            // tstStatusIcon
            // 
            this.tstStatusIcon.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tstStatusIcon.AutoToolTip = true;
            this.tstStatusIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.tstStatusIcon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tstStatusIcon.Image = global::StockMarket.Properties.Resources.alert_white;
            this.tstStatusIcon.Name = "tstStatusIcon";
            this.tstStatusIcon.Size = new System.Drawing.Size(28, 20);
            this.tstStatusIcon.Text = "  ";
            this.tstStatusIcon.ToolTipText = "Desconhecido";
            // 
            // Day
            // 
            this.Day.DataPropertyName = "Name";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.Day.DefaultCellStyle = dataGridViewCellStyle1;
            this.Day.HeaderText = "Dia 00/00/0000";
            this.Day.Name = "Day";
            this.Day.ReadOnly = true;
            // 
            // Minimun
            // 
            this.Minimun.DataPropertyName = "Minimun";
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Red;
            this.Minimun.DefaultCellStyle = dataGridViewCellStyle2;
            this.Minimun.HeaderText = "Mínimo";
            this.Minimun.Name = "Minimun";
            this.Minimun.ReadOnly = true;
            this.Minimun.Width = 80;
            // 
            // Maximun
            // 
            this.Maximun.DataPropertyName = "Maximun";
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Green;
            this.Maximun.DefaultCellStyle = dataGridViewCellStyle3;
            this.Maximun.HeaderText = "Máximo";
            this.Maximun.Name = "Maximun";
            this.Maximun.ReadOnly = true;
            this.Maximun.Width = 80;
            // 
            // Closing
            // 
            this.Closing.DataPropertyName = "Closing";
            this.Closing.HeaderText = "Fec. Preg Ant.";
            this.Closing.Name = "Closing";
            this.Closing.ReadOnly = true;
            // 
            // Opening
            // 
            this.Opening.DataPropertyName = "Opening";
            this.Opening.HeaderText = "Abertura";
            this.Opening.Name = "Opening";
            this.Opening.ReadOnly = true;
            this.Opening.Width = 72;
            // 
            // Time1030
            // 
            this.Time1030.DataPropertyName = "Time1030";
            this.Time1030.HeaderText = "10:30";
            this.Time1030.Name = "Time1030";
            this.Time1030.ReadOnly = true;
            this.Time1030.Width = 59;
            // 
            // Time1100
            // 
            this.Time1100.DataPropertyName = "Time1100";
            this.Time1100.HeaderText = "11:00";
            this.Time1100.Name = "Time1100";
            this.Time1100.ReadOnly = true;
            this.Time1100.Width = 59;
            // 
            // Time1130
            // 
            this.Time1130.DataPropertyName = "Time1130";
            this.Time1130.HeaderText = "11:30";
            this.Time1130.Name = "Time1130";
            this.Time1130.ReadOnly = true;
            this.Time1130.Width = 59;
            // 
            // Time1200
            // 
            this.Time1200.DataPropertyName = "Time1200";
            this.Time1200.HeaderText = "12:00";
            this.Time1200.Name = "Time1200";
            this.Time1200.ReadOnly = true;
            this.Time1200.Width = 59;
            // 
            // Time1230
            // 
            this.Time1230.DataPropertyName = "Time1230";
            this.Time1230.HeaderText = "12:30";
            this.Time1230.Name = "Time1230";
            this.Time1230.ReadOnly = true;
            this.Time1230.Width = 59;
            // 
            // Time1300
            // 
            this.Time1300.DataPropertyName = "Time1300";
            this.Time1300.HeaderText = "13:00";
            this.Time1300.Name = "Time1300";
            this.Time1300.ReadOnly = true;
            this.Time1300.Width = 59;
            // 
            // Time1330
            // 
            this.Time1330.DataPropertyName = "Time1330";
            this.Time1330.HeaderText = "13:30";
            this.Time1330.Name = "Time1330";
            this.Time1330.ReadOnly = true;
            this.Time1330.Width = 59;
            // 
            // Time1400
            // 
            this.Time1400.DataPropertyName = "Time1400";
            this.Time1400.HeaderText = "14:00";
            this.Time1400.Name = "Time1400";
            this.Time1400.ReadOnly = true;
            this.Time1400.Width = 59;
            // 
            // Time1430
            // 
            this.Time1430.DataPropertyName = "Time1430";
            this.Time1430.HeaderText = "14:30";
            this.Time1430.Name = "Time1430";
            this.Time1430.ReadOnly = true;
            this.Time1430.Width = 59;
            // 
            // Time1500
            // 
            this.Time1500.DataPropertyName = "Time1500";
            this.Time1500.HeaderText = "15:00";
            this.Time1500.Name = "Time1500";
            this.Time1500.ReadOnly = true;
            this.Time1500.Width = 59;
            // 
            // Time1530
            // 
            this.Time1530.DataPropertyName = "Time1530";
            this.Time1530.HeaderText = "15:30";
            this.Time1530.Name = "Time1530";
            this.Time1530.ReadOnly = true;
            this.Time1530.Width = 59;
            // 
            // Time1600
            // 
            this.Time1600.DataPropertyName = "Time1600";
            this.Time1600.HeaderText = "16:00";
            this.Time1600.Name = "Time1600";
            this.Time1600.ReadOnly = true;
            this.Time1600.Width = 59;
            // 
            // Time1630
            // 
            this.Time1630.DataPropertyName = "Time1630";
            this.Time1630.HeaderText = "16:30";
            this.Time1630.Name = "Time1630";
            this.Time1630.ReadOnly = true;
            this.Time1630.Width = 59;
            // 
            // Time1700
            // 
            this.Time1700.DataPropertyName = "Time1700";
            this.Time1700.HeaderText = "17:00";
            this.Time1700.Name = "Time1700";
            this.Time1700.ReadOnly = true;
            this.Time1700.Width = 59;
            // 
            // Time1730
            // 
            this.Time1730.DataPropertyName = "Time1730";
            this.Time1730.HeaderText = "17:30";
            this.Time1730.Name = "Time1730";
            this.Time1730.ReadOnly = true;
            this.Time1730.Width = 59;
            // 
            // Time1800
            // 
            this.Time1800.DataPropertyName = "Time1800";
            this.Time1800.HeaderText = "18:00";
            this.Time1800.Name = "Time1800";
            this.Time1800.ReadOnly = true;
            this.Time1800.Width = 59;
            // 
            // RentabilidadePerc
            // 
            this.RentabilidadePerc.DataPropertyName = "RentabilidadePerc";
            this.RentabilidadePerc.HeaderText = "Rent/dia %";
            this.RentabilidadePerc.Name = "RentabilidadePerc";
            this.RentabilidadePerc.ReadOnly = true;
            this.RentabilidadePerc.Width = 85;
            // 
            // RentabilidadeValor
            // 
            this.RentabilidadeValor.DataPropertyName = "RentabilidadeValor";
            this.RentabilidadeValor.HeaderText = "Rent/dia R$";
            this.RentabilidadeValor.Name = "RentabilidadeValor";
            this.RentabilidadeValor.ReadOnly = true;
            this.RentabilidadeValor.Width = 91;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 656);
            this.Controls.Add(this.mnsMenu);
            this.Controls.Add(this.tbcPriceMonitor);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "WTFMonitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tcmMenu.ResumeLayout(false);
            this.tbcPriceMonitor.ResumeLayout(false);
            this.tbpAbstract.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAbstract)).EndInit();
            this.mnsMenu.ResumeLayout(false);
            this.mnsMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NotifyIcon ntiTrayIcon;
        private ContextMenuStrip tcmMenu;
        private System.Windows.Forms.ToolStripMenuItem tcmMenuClose;
        private TabControl tbcPriceMonitor;
        private MenuStrip mnsMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmClose;
        private System.Windows.Forms.ToolStripMenuItem tsmConfig;
        private System.Windows.Forms.ToolStripMenuItem btnRestartApplication;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem tstStatusIcon;
        private TabPage tbpAbstract;
        private CustomDataGridView dgvAbstract;
        private ToolStripMenuItem tsmExportTo;
        private ToolStripMenuItem tsmToXml;
        private DataGridViewTextBoxColumn Day;
        private DataGridViewTextBoxColumn Minimun;
        private DataGridViewTextBoxColumn Maximun;
        private DataGridViewTextBoxColumn Closing;
        private DataGridViewTextBoxColumn Opening;
        private DataGridViewTextBoxColumn Time1030;
        private DataGridViewTextBoxColumn Time1100;
        private DataGridViewTextBoxColumn Time1130;
        private DataGridViewTextBoxColumn Time1200;
        private DataGridViewTextBoxColumn Time1230;
        private DataGridViewTextBoxColumn Time1300;
        private DataGridViewTextBoxColumn Time1330;
        private DataGridViewTextBoxColumn Time1400;
        private DataGridViewTextBoxColumn Time1430;
        private DataGridViewTextBoxColumn Time1500;
        private DataGridViewTextBoxColumn Time1530;
        private DataGridViewTextBoxColumn Time1600;
        private DataGridViewTextBoxColumn Time1630;
        private DataGridViewTextBoxColumn Time1700;
        private DataGridViewTextBoxColumn Time1730;
        private DataGridViewTextBoxColumn Time1800;
        private DataGridViewTextBoxColumn RentabilidadePerc;
        private DataGridViewTextBoxColumn RentabilidadeValor;
    }
}

