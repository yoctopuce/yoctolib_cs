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
          this.pictureBox1 = new System.Windows.Forms.PictureBox();
          this.statusStrip1 = new System.Windows.Forms.StatusStrip();
          this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
          this.Beacon = new System.Windows.Forms.CheckBox();
          this.TestLed = new System.Windows.Forms.CheckBox();
          this.InventoryTimer = new System.Windows.Forms.Timer(this.components);
          this.RefreshTimer = new System.Windows.Forms.Timer(this.components);
          ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
          this.statusStrip1.SuspendLayout();
          this.SuspendLayout();
          // 
          // comboBox1
          // 
          this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
          this.comboBox1.Enabled = false;
          this.comboBox1.FormattingEnabled = true;
          this.comboBox1.Location = new System.Drawing.Point(4, 5);
          this.comboBox1.Name = "comboBox1";
          this.comboBox1.Size = new System.Drawing.Size(300, 21);
          this.comboBox1.TabIndex = 0;
          // 
          // pictureBox1
          // 
          this.pictureBox1.Location = new System.Drawing.Point(4, 32);
          this.pictureBox1.Name = "pictureBox1";
          this.pictureBox1.Size = new System.Drawing.Size(300, 300);
          this.pictureBox1.TabIndex = 1;
          this.pictureBox1.TabStop = false;
          // 
          // statusStrip1
          // 
          this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
          this.statusStrip1.Location = new System.Drawing.Point(0, 360);
          this.statusStrip1.Name = "statusStrip1";
          this.statusStrip1.Size = new System.Drawing.Size(307, 22);
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
          this.Beacon.AutoSize = true;
          this.Beacon.Location = new System.Drawing.Point(4, 338);
          this.Beacon.Name = "Beacon";
          this.Beacon.Size = new System.Drawing.Size(63, 17);
          this.Beacon.TabIndex = 3;
          this.Beacon.Text = "Beacon";
          this.Beacon.UseVisualStyleBackColor = true;
          this.Beacon.Click += new System.EventHandler(this.Beacon_Click);
          // 
          // TestLed
          // 
          this.TestLed.AutoSize = true;
          this.TestLed.Location = new System.Drawing.Point(224, 338);
          this.TestLed.Name = "TestLed";
          this.TestLed.Size = new System.Drawing.Size(68, 17);
          this.TestLed.TabIndex = 4;
          this.TestLed.Text = "Test Led";
          this.TestLed.UseVisualStyleBackColor = true;
          this.TestLed.Click += new System.EventHandler(this.TestLed_Click);
          // 
          // InventoryTimer
          // 
          this.InventoryTimer.Tick += new System.EventHandler(this.InventoryTimer_Tick);
          // 
          // RefreshTimer
          // 
          this.RefreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
          // 
          // Form1
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(307, 382);
          this.Controls.Add(this.TestLed);
          this.Controls.Add(this.Beacon);
          this.Controls.Add(this.statusStrip1);
          this.Controls.Add(this.pictureBox1);
          this.Controls.Add(this.comboBox1);
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
          this.MaximizeBox = false;
          this.Name = "Form1";
          this.Text = "Yocto-Demo";
          this.Load += new System.EventHandler(this.Form1_Load);
          ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
          this.statusStrip1.ResumeLayout(false);
          this.statusStrip1.PerformLayout();
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.CheckBox Beacon;
        private System.Windows.Forms.CheckBox TestLed;
        private System.Windows.Forms.Timer InventoryTimer;
        private System.Windows.Forms.Timer RefreshTimer;
    }
}

