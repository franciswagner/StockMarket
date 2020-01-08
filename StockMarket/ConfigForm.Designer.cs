namespace StockMarket
{
    partial class ConfigForm
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblAcoes = new System.Windows.Forms.Label();
            this.txtAcoes = new System.Windows.Forms.TextBox();
            this.lblOpening = new System.Windows.Forms.Label();
            this.txtOpening = new System.Windows.Forms.TextBox();
            this.lblClosing = new System.Windows.Forms.Label();
            this.txtClosing = new System.Windows.Forms.TextBox();
            this.rtb360Min = new System.Windows.Forms.RadioButton();
            this.rtb60Min = new System.Windows.Forms.RadioButton();
            this.rtb30Min = new System.Windows.Forms.RadioButton();
            this.rtb15Min = new System.Windows.Forms.RadioButton();
            this.rtb5Min = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(311, 201);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 40;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(392, 201);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 50;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblAcoes
            // 
            this.lblAcoes.AutoSize = true;
            this.lblAcoes.Location = new System.Drawing.Point(12, 16);
            this.lblAcoes.Name = "lblAcoes";
            this.lblAcoes.Size = new System.Drawing.Size(37, 13);
            this.lblAcoes.TabIndex = 1;
            this.lblAcoes.Text = "Ações";
            // 
            // txtAcoes
            // 
            this.txtAcoes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAcoes.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAcoes.Location = new System.Drawing.Point(84, 12);
            this.txtAcoes.Name = "txtAcoes";
            this.txtAcoes.Size = new System.Drawing.Size(383, 20);
            this.txtAcoes.TabIndex = 10;
            this.txtAcoes.TextChanged += new System.EventHandler(this.txtAcoes_TextChanged);
            // 
            // lblOpening
            // 
            this.lblOpening.AutoSize = true;
            this.lblOpening.Location = new System.Drawing.Point(12, 42);
            this.lblOpening.Name = "lblOpening";
            this.lblOpening.Size = new System.Drawing.Size(47, 13);
            this.lblOpening.TabIndex = 1;
            this.lblOpening.Text = "Abertura";
            // 
            // txtOpening
            // 
            this.txtOpening.Location = new System.Drawing.Point(84, 39);
            this.txtOpening.Name = "txtOpening";
            this.txtOpening.Size = new System.Drawing.Size(150, 20);
            this.txtOpening.TabIndex = 20;
            this.txtOpening.TextChanged += new System.EventHandler(this.txtOpening_TextChanged);
            // 
            // lblClosing
            // 
            this.lblClosing.AutoSize = true;
            this.lblClosing.Location = new System.Drawing.Point(12, 68);
            this.lblClosing.Name = "lblClosing";
            this.lblClosing.Size = new System.Drawing.Size(66, 13);
            this.lblClosing.TabIndex = 1;
            this.lblClosing.Text = "Fechamento";
            // 
            // txtClosing
            // 
            this.txtClosing.Location = new System.Drawing.Point(84, 65);
            this.txtClosing.Name = "txtClosing";
            this.txtClosing.Size = new System.Drawing.Size(150, 20);
            this.txtClosing.TabIndex = 30;
            this.txtClosing.Text = "\r\n";
            this.txtClosing.TextChanged += new System.EventHandler(this.txtClosing_TextChanged);
            // 
            // rtb360Min
            // 
            this.rtb360Min.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb360Min.AutoSize = true;
            this.rtb360Min.Location = new System.Drawing.Point(6, 111);
            this.rtb360Min.Name = "rtb360Min";
            this.rtb360Min.Size = new System.Drawing.Size(60, 17);
            this.rtb360Min.TabIndex = 51;
            this.rtb360Min.Text = "6 horas";
            this.rtb360Min.UseVisualStyleBackColor = true;
            // 
            // rtb60Min
            // 
            this.rtb60Min.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb60Min.AutoSize = true;
            this.rtb60Min.Location = new System.Drawing.Point(6, 88);
            this.rtb60Min.Name = "rtb60Min";
            this.rtb60Min.Size = new System.Drawing.Size(55, 17);
            this.rtb60Min.TabIndex = 52;
            this.rtb60Min.Text = "1 hora";
            this.rtb60Min.UseVisualStyleBackColor = true;
            // 
            // rtb30Min
            // 
            this.rtb30Min.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb30Min.AutoSize = true;
            this.rtb30Min.Checked = true;
            this.rtb30Min.Location = new System.Drawing.Point(6, 65);
            this.rtb30Min.Name = "rtb30Min";
            this.rtb30Min.Size = new System.Drawing.Size(76, 17);
            this.rtb30Min.TabIndex = 53;
            this.rtb30Min.TabStop = true;
            this.rtb30Min.Text = "30 minutos";
            this.rtb30Min.UseVisualStyleBackColor = true;
            // 
            // rtb15Min
            // 
            this.rtb15Min.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb15Min.AutoSize = true;
            this.rtb15Min.Location = new System.Drawing.Point(6, 42);
            this.rtb15Min.Name = "rtb15Min";
            this.rtb15Min.Size = new System.Drawing.Size(76, 17);
            this.rtb15Min.TabIndex = 54;
            this.rtb15Min.Text = "15 minutos";
            this.rtb15Min.UseVisualStyleBackColor = true;
            // 
            // rtb5Min
            // 
            this.rtb5Min.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rtb5Min.AutoSize = true;
            this.rtb5Min.Location = new System.Drawing.Point(6, 19);
            this.rtb5Min.Name = "rtb5Min";
            this.rtb5Min.Size = new System.Drawing.Size(70, 17);
            this.rtb5Min.TabIndex = 55;
            this.rtb5Min.Text = "5 minutos";
            this.rtb5Min.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rtb5Min);
            this.groupBox1.Controls.Add(this.rtb360Min);
            this.groupBox1.Controls.Add(this.rtb15Min);
            this.groupBox1.Controls.Add(this.rtb60Min);
            this.groupBox1.Controls.Add(this.rtb30Min);
            this.groupBox1.Location = new System.Drawing.Point(12, 91);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 136);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Período do grafico de candle";
            // 
            // ConfigForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(479, 236);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtClosing);
            this.Controls.Add(this.lblClosing);
            this.Controls.Add(this.txtOpening);
            this.Controls.Add(this.lblOpening);
            this.Controls.Add(this.txtAcoes);
            this.Controls.Add(this.lblAcoes);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(495, 275);
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configurações";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblAcoes;
        private System.Windows.Forms.TextBox txtAcoes;
        private System.Windows.Forms.Label lblOpening;
        private System.Windows.Forms.TextBox txtOpening;
        private System.Windows.Forms.Label lblClosing;
        private System.Windows.Forms.TextBox txtClosing;
        private System.Windows.Forms.RadioButton rtb360Min;
        private System.Windows.Forms.RadioButton rtb60Min;
        private System.Windows.Forms.RadioButton rtb30Min;
        private System.Windows.Forms.RadioButton rtb15Min;
        private System.Windows.Forms.RadioButton rtb5Min;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}