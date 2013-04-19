namespace WindowsFormsApplication1
{
    partial class Form1
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
          this.comboBox1 = new System.Windows.Forms.ComboBox();
          this.statusStrip1 = new System.Windows.Forms.StatusStrip();
          this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
          this.Beacon = new System.Windows.Forms.CheckBox();
          this.InventoryTimer = new System.Windows.Forms.Timer(this.components);
          this.RefreshTimer = new System.Windows.Forms.Timer(this.components);
          this.label1 = new System.Windows.Forms.Label();
          this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
          this.statusStrip1.SuspendLayout();
          this.SuspendLayout();
          // 
          // comboBox1
          // 
          this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                      | System.Windows.Forms.AnchorStyles.Right)));
          this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
          this.comboBox1.Enabled = false;
          this.comboBox1.FormattingEnabled = true;
          this.comboBox1.Location = new System.Drawing.Point(4, 5);
          this.comboBox1.Name = "comboBox1";
          this.comboBox1.Size = new System.Drawing.Size(296, 21);
          this.comboBox1.TabIndex = 0;
          // 
          // statusStrip1
          // 
          this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2});
          this.statusStrip1.Location = new System.Drawing.Point(0, 104);
          this.statusStrip1.Name = "statusStrip1";
          this.statusStrip1.Size = new System.Drawing.Size(310, 22);
          this.statusStrip1.SizingGrip = false;
          this.statusStrip1.TabIndex = 2;
          this.statusStrip1.Text = "statusStrip1";
          // 
          // toolStripStatusLabel1
          // 
          this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
          this.toolStripStatusLabel1.Size = new System.Drawing.Size(109, 17);
          this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
          // 
          // Beacon
          // 
          this.Beacon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
          this.Beacon.AutoSize = true;
          this.Beacon.Location = new System.Drawing.Point(237, 64);
          this.Beacon.Name = "Beacon";
          this.Beacon.Size = new System.Drawing.Size(63, 17);
          this.Beacon.TabIndex = 3;
          this.Beacon.Text = "Beacon";
          this.Beacon.UseVisualStyleBackColor = true;
          this.Beacon.Click += new System.EventHandler(this.Beacon_Click);
          // 
          // InventoryTimer
          // 
          this.InventoryTimer.Tick += new System.EventHandler(this.InventoryTimer_Tick);
          // 
          // RefreshTimer
          // 
          this.RefreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
          // 
          // label1
          // 
          this.label1.AutoSize = true;
          this.label1.Enabled = false;
          this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.label1.Location = new System.Drawing.Point(108, 44);
          this.label1.Name = "label1";
          this.label1.Size = new System.Drawing.Size(72, 37);
          this.label1.TabIndex = 4;
          this.label1.Text = "N/A";
          // 
          // toolStripStatusLabel2
          // 
          this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
          this.toolStripStatusLabel2.Size = new System.Drawing.Size(109, 17);
          this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
          // 
          // Form1
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(310, 126);
          this.Controls.Add(this.label1);
          this.Controls.Add(this.Beacon);
          this.Controls.Add(this.statusStrip1);
          this.Controls.Add(this.comboBox1);
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
          this.MaximizeBox = false;
          this.Name = "Form1";
          this.Text = "Yocto Temperature Demo";
          this.Load += new System.EventHandler(this.Form1_Load);
          this.statusStrip1.ResumeLayout(false);
          this.statusStrip1.PerformLayout();
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.CheckBox Beacon;
        private System.Windows.Forms.Timer InventoryTimer;
        private System.Windows.Forms.Timer RefreshTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
    }
}

