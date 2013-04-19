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
  class YoctoGauge
  {

    private ComboBox comboBox1;
    private PictureBox drawingArea;
  
    private double value = 0;
    private double needleposition;
    private double maxvalue;
    private int dialWidth;
    private int dialHeight;



    public YoctoGauge(ComboBox controlDropDown, PictureBox GaugeArea,  double maximumvalue)
    {
      comboBox1 = controlDropDown;
      drawingArea = GaugeArea;
      maxvalue = maximumvalue;
      dialWidth = drawingArea.Width;
      dialHeight = drawingArea.Height;
      needleposition = -5;
     
    }

    public void enable(bool state)
    {
      comboBox1.Enabled = state;
      refresh();
    }

    public ComboBox getDropDown()
    {
      return comboBox1;
    }

    public void setValue(double v)
    {
      value = v;   
    }

    // draw  the dial.
    public void refresh()
    { 
      bool on = comboBox1.Enabled;
      if (!on) value = -5;

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
      if (needleposition > maxvalue * 1.1) needleposition = maxvalue * 1.1;

      double angle = 3.1416 * (180 + 90 * (needleposition / maxvalue)) / 180;
      int x1 = Convert.ToInt32(DialWidth  * 0.77);
      int y1 = Convert.ToInt32(DialHeight * 0.77);
      int x2 = Convert.ToInt32(x1 + Math.Cos(angle) * DialWidth * 0.65);
      int y2 = Convert.ToInt32(y1 + Math.Sin(angle) * DialHeight * 0.65);

      // draw the needle shadow
      Pen shadow = new Pen(Color.FromArgb(16, 255, 0, 00), 3);
      Point point1 = new Point(x1+3,y1+3);
      Point point2 = new Point(x2 + 3, y2 + 3);
      buffer.DrawLine(shadow, point1, point2);

      // draw the needle
      Pen red = new Pen(on ? Color.FromArgb(255, 255, 0, 00) : Color.FromArgb(255, 64, 0, 00), 3);
      point1 = new Point(x1, y1);
      point2 = new Point(x2, y2);
      buffer.DrawLine(red, point1, point2);


      Bitmap frame = new Bitmap(Properties.Resources.frame);
      buffer.DrawImage(frame, 0, 0);

      Graphics Viewable = drawingArea.CreateGraphics();

      // fast rendering
      //Viewable.DrawImageUnscaled(BackBuffer, 0, 0); 

      // slower, but pictureBox can be resized, rendering will still be ok, 
      // try to respect a 1:1 ratio anyway
      Viewable.DrawImage(BackBuffer, new Rectangle(0, 0, drawingArea.Width, drawingArea.Height));

      Viewable.Dispose();

    }
  }
}
