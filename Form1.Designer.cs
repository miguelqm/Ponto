namespace Ponto
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.txtInput = new System.Windows.Forms.TextBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.tmrClock = new System.Windows.Forms.Timer(this.components);
            this.lblWarning = new System.Windows.Forms.Label();
            this.tmrInput = new System.Windows.Forms.Timer(this.components);
            this.lblDebug = new System.Windows.Forms.Label();
            this.lblWarning2 = new System.Windows.Forms.Label();
            this.lblOk = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.globalEventProvider1 = new Gma.UserActivityMonitor.GlobalEventProvider();
            this.dbPontoDataSet = new Ponto.dbPontoDataSet();
            this.funcionarioBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.funcionarioTableAdapter = new Ponto.dbPontoDataSetTableAdapters.FuncionarioTableAdapter();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dbPontoDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.funcionarioBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.BackColor = System.Drawing.Color.Blue;
            this.txtInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtInput.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtInput.Location = new System.Drawing.Point(175, 287);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(50, 13);
            this.txtInput.TabIndex = 0;
            this.txtInput.Text = "12341234";
            this.txtInput.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.txtInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.BackColor = System.Drawing.Color.Black;
            this.lblTime.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTime.Font = new System.Drawing.Font("Footlight MT Light", 40F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.Color.Red;
            this.lblTime.Location = new System.Drawing.Point(452, 108);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(241, 60);
            this.lblTime.TabIndex = 1;
            this.lblTime.Text = "12:45:00";
            // 
            // tmrClock
            // 
            this.tmrClock.Enabled = true;
            this.tmrClock.Interval = 500;
            this.tmrClock.Tick += new System.EventHandler(this.tmrClock_Tick);
            // 
            // lblWarning
            // 
            this.lblWarning.BackColor = System.Drawing.Color.Transparent;
            this.lblWarning.Font = new System.Drawing.Font("Century", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarning.ForeColor = System.Drawing.Color.White;
            this.lblWarning.Location = new System.Drawing.Point(-237, 397);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(1600, 57);
            this.lblWarning.TabIndex = 3;
            this.lblWarning.Text = "11111111";
            this.lblWarning.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblWarning.TextChanged += new System.EventHandler(this.lblWarning_TextChanged);
            // 
            // tmrInput
            // 
            this.tmrInput.Interval = 5000;
            this.tmrInput.Tick += new System.EventHandler(this.tmrInput_Tick);
            // 
            // lblDebug
            // 
            this.lblDebug.AutoSize = true;
            this.lblDebug.BackColor = System.Drawing.Color.Black;
            this.lblDebug.ForeColor = System.Drawing.Color.White;
            this.lblDebug.Location = new System.Drawing.Point(153, 120);
            this.lblDebug.Name = "lblDebug";
            this.lblDebug.Size = new System.Drawing.Size(49, 13);
            this.lblDebug.TabIndex = 4;
            this.lblDebug.Text = "lblDebug";
            this.lblDebug.Visible = false;
            // 
            // lblWarning2
            // 
            this.lblWarning2.BackColor = System.Drawing.Color.Transparent;
            this.lblWarning2.Font = new System.Drawing.Font("Century", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarning2.ForeColor = System.Drawing.Color.White;
            this.lblWarning2.Location = new System.Drawing.Point(-237, 485);
            this.lblWarning2.Name = "lblWarning2";
            this.lblWarning2.Size = new System.Drawing.Size(1600, 57);
            this.lblWarning2.TabIndex = 5;
            this.lblWarning2.Text = "11111111";
            this.lblWarning2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblWarning2.TextChanged += new System.EventHandler(this.lblWarning2_TextChanged);
            // 
            // lblOk
            // 
            this.lblOk.AutoSize = true;
            this.lblOk.BackColor = System.Drawing.Color.Black;
            this.lblOk.Font = new System.Drawing.Font("Century", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOk.ForeColor = System.Drawing.Color.Yellow;
            this.lblOk.Location = new System.Drawing.Point(444, 19);
            this.lblOk.Name = "lblOk";
            this.lblOk.Size = new System.Drawing.Size(249, 57);
            this.lblOk.TabIndex = 6;
            this.lblOk.Text = "11111111";
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::Ponto.Properties.Resources.avencaPonto;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(319, 171);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(488, 185);
            this.panel1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.Font = new System.Drawing.Font("Impact", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(148, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(203, 36);
            this.label3.TabIndex = 0;
            this.label3.Text = "Folha de Ponto";
            // 
            // globalEventProvider1
            // 
            this.globalEventProvider1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.globalEventProvider1_KeyPress_1);
            // 
            // dbPontoDataSet
            // 
            this.dbPontoDataSet.DataSetName = "dbPontoDataSet";
            this.dbPontoDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // funcionarioBindingSource
            // 
            this.funcionarioBindingSource.DataMember = "Funcionario";
            this.funcionarioBindingSource.DataSource = this.dbPontoDataSet;
            // 
            // funcionarioTableAdapter
            // 
            this.funcionarioTableAdapter.ClearBeforeFill = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1584, 761);
            this.Controls.Add(this.lblOk);
            this.Controls.Add(this.lblWarning2);
            this.Controls.Add(this.lblDebug);
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.txtInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Relógio de Ponto";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dbPontoDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.funcionarioBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Timer tmrClock;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Timer tmrInput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDebug;
        private System.Windows.Forms.Label lblWarning2;
        private System.Windows.Forms.Label lblOk;
        private dbPontoDataSet dbPontoDataSet;
        private System.Windows.Forms.BindingSource funcionarioBindingSource;
        private dbPontoDataSetTableAdapters.FuncionarioTableAdapter funcionarioTableAdapter;
        private Gma.UserActivityMonitor.GlobalEventProvider globalEventProvider1;

    }
}

