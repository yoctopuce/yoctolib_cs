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
      this.myMap = new GMap.NET.WindowsForms.GMapControl();
      this.comboBox1 = new System.Windows.Forms.ComboBox();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.timer2 = new System.Windows.Forms.Timer(this.components);
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.Lat_value = new System.Windows.Forms.Label();
      this.Lon_value = new System.Windows.Forms.Label();
      this.Speed_value = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.Orient_value = new System.Windows.Forms.Label();
      this.GPS_Status = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // myMap
      // 
      this.myMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.myMap.Bearing = 0F;
      this.myMap.CanDragMap = true;
      this.myMap.EmptyTileColor = System.Drawing.Color.Navy;
      this.myMap.GrayScaleMode = false;
      this.myMap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
      this.myMap.LevelsKeepInMemmory = 5;
      this.myMap.Location = new System.Drawing.Point(3, 30);
      this.myMap.MarkersEnabled = true;
      this.myMap.MaxZoom = 2;
      this.myMap.MinZoom = 2;
      this.myMap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
      this.myMap.Name = "myMap";
      this.myMap.NegativeMode = false;
      this.myMap.PolygonsEnabled = true;
      this.myMap.RetryLoadTile = 0;
      this.myMap.RoutesEnabled = true;
      this.myMap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
      this.myMap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
      this.myMap.ShowTileGridLines = false;
      this.myMap.Size = new System.Drawing.Size(394, 237);
      this.myMap.TabIndex = 0;
      this.myMap.Zoom = 0D;
      this.myMap.Load += new System.EventHandler(this.myMap_Load);
      this.myMap.Paint += new System.Windows.Forms.PaintEventHandler(this.myMap_Paint);
      // 
      // comboBox1
      // 
      this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBox1.DisplayMember = "FriendlyName";
      this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Location = new System.Drawing.Point(3, 3);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(394, 21);
      this.comboBox1.TabIndex = 1;
      this.comboBox1.ValueMember = "get_friendlyName";
      this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
      // 
      // timer1
      // 
      this.timer1.Enabled = true;
      this.timer1.Interval = 500;
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // timer2
      // 
      this.timer2.Enabled = true;
      this.timer2.Interval = 25;
      this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(0, 270);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(48, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Latitude:";
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(0, 285);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(54, 13);
      this.label2.TabIndex = 3;
      this.label2.Text = "Longitude";
      // 
      // Lat_value
      // 
      this.Lat_value.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.Lat_value.Location = new System.Drawing.Point(54, 270);
      this.Lat_value.Name = "Lat_value";
      this.Lat_value.Size = new System.Drawing.Size(96, 13);
      this.Lat_value.TabIndex = 4;
      this.Lat_value.Text = "N/A";
      // 
      // Lon_value
      // 
      this.Lon_value.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.Lon_value.Location = new System.Drawing.Point(54, 285);
      this.Lon_value.Name = "Lon_value";
      this.Lon_value.Size = new System.Drawing.Size(96, 13);
      this.Lon_value.TabIndex = 5;
      this.Lon_value.Text = "N/A";
      // 
      // Speed_value
      // 
      this.Speed_value.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.Speed_value.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Speed_value.Location = new System.Drawing.Point(154, 269);
      this.Speed_value.Name = "Speed_value";
      this.Speed_value.Size = new System.Drawing.Size(89, 28);
      this.Speed_value.TabIndex = 6;
      this.Speed_value.Text = "N/A";
      this.Speed_value.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label4
      // 
      this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(237, 284);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(33, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "Km/h";
      // 
      // Orient_value
      // 
      this.Orient_value.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.Orient_value.Location = new System.Drawing.Point(340, 270);
      this.Orient_value.Name = "Orient_value";
      this.Orient_value.Size = new System.Drawing.Size(57, 13);
      this.Orient_value.TabIndex = 8;
      this.Orient_value.Text = "N/A";
      this.Orient_value.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // GPS_Status
      // 
      this.GPS_Status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.GPS_Status.Location = new System.Drawing.Point(276, 283);
      this.GPS_Status.Name = "GPS_Status";
      this.GPS_Status.Size = new System.Drawing.Size(121, 14);
      this.GPS_Status.TabIndex = 9;
      this.GPS_Status.Text = "N/A";
      this.GPS_Status.TextAlign = System.Drawing.ContentAlignment.BottomRight;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(401, 298);
      this.Controls.Add(this.GPS_Status);
      this.Controls.Add(this.Orient_value);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.Speed_value);
      this.Controls.Add(this.Lon_value);
      this.Controls.Add(this.Lat_value);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.comboBox1);
      this.Controls.Add(this.myMap);
      this.MinimumSize = new System.Drawing.Size(400, 300);
      this.Name = "Form1";
      this.Text = "Yocto-GPS demo";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private GMap.NET.WindowsForms.GMapControl myMap;
    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.Timer timer2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label Lat_value;
    private System.Windows.Forms.Label Lon_value;
    private System.Windows.Forms.Label Speed_value;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label Orient_value;
    private System.Windows.Forms.Label GPS_Status;
  }
}

