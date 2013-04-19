using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
  public partial class Form1 : Form
  {
    private const int DialWidth = 300;
    private const int DialHeight = 153;
    private double maxvalue = 200;
    private double needleposition = -5;

    public Form1()
    {
      InitializeComponent();
    }

    private void moduleInventory()
    {
      YModule m, currentmodule;
      string name;
      int index;

      comboBox1.Items.Clear();
      currentmodule = null;
      m = YModule.FirstModule();
      while (m != null)
      {
        name = m.get_serialNumber();
        if (name.Substring(0, 8) == "YAMPMK01")
        { comboBox1.Items.Add(m); }
        m = m.nextModule();
      }

      if (comboBox1.Items.Count == 0)
      {
        comboBox1.Enabled = false;
        bt_200mA.Enabled = false;
        bt_2A.Enabled = false;
        bt_20A.Enabled = false;
        ACDCcheckBox.Enabled = false;
        toolStripStatusLabel2.Text = "Connect a Yocto-Amp device";
      }
      else
      {
        comboBox1.Enabled = true;
        bt_200mA.Enabled = true;
        bt_2A.Enabled = true;
        bt_20A.Enabled = true;
        ACDCcheckBox.Enabled = true;
        index = 0;

        for (int i = 0; i < comboBox1.Items.Count; i++)
        {
          if (comboBox1.Items[i].Equals(currentmodule)) index = i;
        }

        if (comboBox1.Items.Count == 1)
          toolStripStatusLabel2.Text = "One Yocto-Amp device connected";
        else
          toolStripStatusLabel2.Text = comboBox1.Items.Count.ToString() + " Yocto-Amp devices connected";

        comboBox1.SelectedIndex = index;
      }
    }

    private void refreshUI()
    { // draw  the dial.
      double value = -5;
      bool on = false;

      if (comboBox1.Enabled)
      { // if a yocto-amp device is connected, lets check it value
        YModule m = (YModule)comboBox1.Items[comboBox1.SelectedIndex];
        YCurrent DC = YCurrent.FindCurrent(m.get_serialNumber() + ".current1");
        YCurrent AC = YCurrent.FindCurrent(m.get_serialNumber() + ".current2");
        if (DC.isOnline())
        { // read DC or AC value, according to ACDCcheckBox
          if (ACDCcheckBox.Checked) value = 100 * (AC.get_currentValue() / maxvalue);
          else value = 100 * (DC.get_currentValue() / maxvalue);
          on = true;
        }
      }

      // lets use a double buffering technique to avoid flickering 
      Bitmap BackBuffer = new Bitmap(on ? Properties.Resources.bg : Properties.Resources.bgoff);
      Graphics buffer = Graphics.FromImage(BackBuffer);
      buffer.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
      int DialWidth = BackBuffer.Width;
      int DialHeight = BackBuffer.Height;

      // add inertia to the needle
      needleposition = needleposition + (value - needleposition) / 10;

      // make sure une needle won't go off chart
      if (needleposition < -5) needleposition = -5;
      if (needleposition > 105) needleposition = 105;

      double angle = 3.1416 * (180 - 30 - 120 * (needleposition / 100)) / 180;
      int x = Convert.ToInt32(DialWidth / 2 + Math.Cos(angle) * (DialHeight - 15));
      int y = Convert.ToInt32(DialHeight * 1.066 - Math.Sin(angle) * (DialHeight - 15));

      // draw the needle shadow
      Pen shadow = new Pen(Color.FromArgb(16, 0, 0, 00), 3);
      Point point1 = new Point(DialWidth / 2 - 3, DialHeight + 3);
      Point point2 = new Point(Convert.ToInt32(x) - 3, Convert.ToInt32(y) + 3);
      buffer.DrawLine(shadow, point1, point2);

      // draw the needle
      Pen red = new Pen(on ? Color.FromArgb(255, 255, 0, 00) : Color.FromArgb(255, 64, 0, 00), 3);
      point1 = new Point(DialWidth / 2, DialHeight);
      point2 = new Point(Convert.ToInt32(x), Convert.ToInt32(y));
      buffer.DrawLine(red, point1, point2);

      // draw the scale
      FontFamily fontFamily = new FontFamily("Arial");
      Font font = new Font(fontFamily, DialHeight / 10, FontStyle.Regular, GraphicsUnit.Pixel);
      buffer.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
      SolidBrush solidBrush = new SolidBrush(Color.FromArgb(255, 20, 20, 20));

      for (int i = 0; i <= 10; i++)
      {
        double dvalue = ((maxvalue/1000) * i / 10);
        angle = 3.1416 * (180 - 30 - 120 * (i / 10.0)) / 180;
        string text = dvalue.ToString();
        SizeF size = buffer.MeasureString(text, font);
        int tx = Convert.ToInt32(DialWidth / 2 + Math.Cos(angle) * DialHeight * 1.01 - size.Width / 2);
        int ty = Convert.ToInt32(DialHeight * 1.066 - Math.Sin(angle) * DialHeight * 0.98 - size.Height / 2);
        buffer.DrawString(dvalue.ToString(), font, solidBrush, new PointF(tx, ty));
      }

      Bitmap frame = new Bitmap(Properties.Resources.frame);
      buffer.DrawImage(frame, 0, 0);

      Graphics Viewable = pictureBox1.CreateGraphics();

      // fast rendering
      //Viewable.DrawImageUnscaled(BackBuffer, 0, 0); 

      // slower, but pictureBox can be resized, rendering will still be ok, 
      // try to respect a 2:1 ratio anyway
      Viewable.DrawImage(BackBuffer, new Rectangle(0, 0, pictureBox1.Width, pictureBox1.Height));

      Viewable.Dispose();
    }

    public void devicelistchanged(YModule m)
    {
      moduleInventory();
      refreshUI();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      moduleInventory();
      // we wanna know when device list changes
      YAPI.RegisterDeviceArrivalCallback(devicelistchanged);
      YAPI.RegisterDeviceRemovalCallback(devicelistchanged);
      InventoryTimer.Interval = 1000;
      InventoryTimer.Start();
      RefreshTimer.Interval = 20;
      RefreshTimer.Start();
    }

    private void InventoryTimer_Tick(object sender, EventArgs e)
    {
      string errmsg = "";
      YAPI.UpdateDeviceList(ref errmsg);
    }

    private void button2_Click(object sender, EventArgs e)
    {
      maxvalue = 2000;
    }

    private void button3_Click(object sender, EventArgs e)
    {
      maxvalue = 20000;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      maxvalue = 200;
    }

    private void RefreshTimer_Tick(object sender, EventArgs e)
    {
      refreshUI();
    }
  }
}
