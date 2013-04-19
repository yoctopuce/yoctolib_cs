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
      this.btA = new System.Windows.Forms.Button();
      this.btB = new System.Windows.Forms.Button();
      this.btBeacon = new System.Windows.Forms.Button();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.refreshTimer = new System.Windows.Forms.Timer(this.components);
      this.deviceScanTimer = new System.Windows.Forms.Timer(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // comboBox1
      // 
      this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Location = new System.Drawing.Point(6, 6);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(256, 21);
      this.comboBox1.TabIndex = 0;
      // 
      // pictureBox1
      // 
      this.pictureBox1.Location = new System.Drawing.Point(6, 33);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(256, 138);
      this.pictureBox1.TabIndex = 1;
      this.pictureBox1.TabStop = false;
      // 
      // btA
      // 
      this.btA.Location = new System.Drawing.Point(6, 177);
      this.btA.Name = "btA";
      this.btA.Size = new System.Drawing.Size(75, 23);
      this.btA.TabIndex = 2;
      this.btA.Text = "A";
      this.btA.UseVisualStyleBackColor = true;
      this.btA.Click += new System.EventHandler(this.btA_Click);
      // 
      // btB
      // 
      this.btB.Location = new System.Drawing.Point(96, 177);
      this.btB.Name = "btB";
      this.btB.Size = new System.Drawing.Size(75, 23);
      this.btB.TabIndex = 3;
      this.btB.Text = "B";
      this.btB.UseVisualStyleBackColor = true;
      this.btB.Click += new System.EventHandler(this.btB_Click);
      // 
      // btBeacon
      // 
      this.btBeacon.Location = new System.Drawing.Point(187, 177);
      this.btBeacon.Name = "btBeacon";
      this.btBeacon.Size = new System.Drawing.Size(75, 23);
      this.btBeacon.TabIndex = 4;
      this.btBeacon.Text = "Beacon";
      this.btBeacon.UseVisualStyleBackColor = true;
      this.btBeacon.Click += new System.EventHandler(this.btBeacon_Click);
      // 
      // statusStrip1
      // 
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
      this.statusStrip1.Location = new System.Drawing.Point(0, 210);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(267, 22);
      this.statusStrip1.SizingGrip = false;
      this.statusStrip1.TabIndex = 5;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // toolStripStatusLabel1
      // 
      this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
      this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
      this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
      // 
      // refreshTimer
      // 
      this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
      // 
      // deviceScanTimer
      // 
      this.deviceScanTimer.Tick += new System.EventHandler(this.deviceScanTimer_Tick);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(267, 232);
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.btBeacon);
      this.Controls.Add(this.btB);
      this.Controls.Add(this.btA);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.comboBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = "Form1";
      this.Text = "Yocto-PowerRelay demo";
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
    private System.Windows.Forms.Button btA;
    private System.Windows.Forms.Button btB;
    private System.Windows.Forms.Button btBeacon;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    private System.Windows.Forms.Timer refreshTimer;
    private System.Windows.Forms.Timer deviceScanTimer;
  }
}

