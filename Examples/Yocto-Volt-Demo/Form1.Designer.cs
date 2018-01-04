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
          this.InventoryTimer = new System.Windows.Forms.Timer(this.components);
          this.RefreshTimer = new System.Windows.Forms.Timer(this.components);
          this.bt_10V = new System.Windows.Forms.Button();
          this.bt_50V = new System.Windows.Forms.Button();
          this.bt_300V = new System.Windows.Forms.Button();
          this.ACDCcheckBox = new System.Windows.Forms.CheckBox();
          this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
          this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
          this.statusStrip1 = new System.Windows.Forms.StatusStrip();
          this.pictureBox1 = new System.Windows.Forms.PictureBox();
          this.statusStrip1.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
          this.comboBox1.Size = new System.Drawing.Size(300, 21);
          this.comboBox1.TabIndex = 0;
          // 
          // InventoryTimer
          // 
          this.InventoryTimer.Tick += new System.EventHandler(this.InventoryTimer_Tick);
          // 
          // RefreshTimer
          // 
          this.RefreshTimer.Interval = 10;
          this.RefreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
          // 
          // bt_10V
          // 
          this.bt_10V.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
          this.bt_10V.Location = new System.Drawing.Point(4, 192);
          this.bt_10V.Name = "bt_10V";
          this.bt_10V.Size = new System.Drawing.Size(75, 23);
          this.bt_10V.TabIndex = 4;
          this.bt_10V.Text = "10 v";
          this.bt_10V.UseVisualStyleBackColor = true;
          this.bt_10V.Click += new System.EventHandler(this.button1_Click);
          // 
          // bt_50V
          // 
          this.bt_50V.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
          this.bt_50V.Location = new System.Drawing.Point(85, 192);
          this.bt_50V.Name = "bt_50V";
          this.bt_50V.Size = new System.Drawing.Size(75, 23);
          this.bt_50V.TabIndex = 5;
          this.bt_50V.Text = "50 v";
          this.bt_50V.UseVisualStyleBackColor = true;
          this.bt_50V.Click += new System.EventHandler(this.button2_Click);
          // 
          // bt_300V
          // 
          this.bt_300V.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
          this.bt_300V.Location = new System.Drawing.Point(166, 192);
          this.bt_300V.Name = "bt_300V";
          this.bt_300V.Size = new System.Drawing.Size(75, 23);
          this.bt_300V.TabIndex = 6;
          this.bt_300V.Text = "300 v";
          this.bt_300V.UseVisualStyleBackColor = true;
          this.bt_300V.Click += new System.EventHandler(this.button3_Click);
          // 
          // ACDCcheckBox
          // 
          this.ACDCcheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
          this.ACDCcheckBox.AutoSize = true;
          this.ACDCcheckBox.Location = new System.Drawing.Point(263, 196);
          this.ACDCcheckBox.Name = "ACDCcheckBox";
          this.ACDCcheckBox.Size = new System.Drawing.Size(40, 17);
          this.ACDCcheckBox.TabIndex = 7;
          this.ACDCcheckBox.Text = "AC";
          this.ACDCcheckBox.UseVisualStyleBackColor = true;
          // 
          // toolStripStatusLabel1
          // 
          this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
          this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
          this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
          // 
          // toolStripStatusLabel2
          // 
          this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
          this.toolStripStatusLabel2.Size = new System.Drawing.Size(33, 17);
          this.toolStripStatusLabel2.Text = "demo";
          // 
          // statusStrip1
          // 
          this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2});
          this.statusStrip1.Location = new System.Drawing.Point(0, 227);
          this.statusStrip1.Name = "statusStrip1";
          this.statusStrip1.Size = new System.Drawing.Size(308, 22);
          this.statusStrip1.SizingGrip = false;
          this.statusStrip1.TabIndex = 8;
          this.statusStrip1.Text = "statusStrip1";
          // 
          // pictureBox1
          // 
          this.pictureBox1.Location = new System.Drawing.Point(4, 32);
          this.pictureBox1.Name = "pictureBox1";
          this.pictureBox1.Size = new System.Drawing.Size(300, 150);
          this.pictureBox1.TabIndex = 1;
          this.pictureBox1.TabStop = false;
          // 
          // Form1
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(308, 249);
          this.Controls.Add(this.statusStrip1);
          this.Controls.Add(this.ACDCcheckBox);
          this.Controls.Add(this.bt_300V);
          this.Controls.Add(this.bt_50V);
          this.Controls.Add(this.bt_10V);
          this.Controls.Add(this.pictureBox1);
          this.Controls.Add(this.comboBox1);
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
          this.MaximizeBox = false;
          this.Name = "Form1";
          this.Text = "Yocto-Volt demo";
          this.Load += new System.EventHandler(this.Form1_Load);
          this.statusStrip1.ResumeLayout(false);
          this.statusStrip1.PerformLayout();
          ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer InventoryTimer;
        private System.Windows.Forms.Timer RefreshTimer;
        private System.Windows.Forms.Button bt_10V;
        private System.Windows.Forms.Button bt_50V;
        private System.Windows.Forms.Button bt_300V;
        private System.Windows.Forms.CheckBox ACDCcheckBox;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.StatusStrip statusStrip1;
    }
}

