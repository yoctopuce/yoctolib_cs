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
        private string prevSerialNumber = "";
        private string currentSerialNumber = "";

        // These variables will be updated by the callback function at high frequency
        static double _tilt1 = 0;
        static double _tilt2 = 0;
        static double _compass = 0;
        static double _roll, _pitch, _head;
        static double _ax, _ay, _az;
        static ulong eventTick = 0;
        static int[] eventCount = new int[4];
        static int[] freq = new int[4];

        // Callback function for compass and tilt
        //
        static void valueCallback(YFunction function, string value)
        {
            string functionId = function.get_functionId();
            double dblValue;

            if (!Double.TryParse(value, out dblValue)) return;
            if (functionId == "compass") { _compass = dblValue; eventCount[0]++; }
            if (functionId == "tilt1") { _tilt1 = dblValue; eventCount[1]++; }
            if (functionId == "tilt2") { _tilt2 = dblValue; eventCount[2]++; }
        }

        // Callback function for orientation
        //
        static void anglesCallback(YGyro yGyro, double roll, double pitch, double head)
        {
            if (Double.IsNaN(roll) || Double.IsNaN(pitch) || Double.IsNaN(head)) return;
            _roll = roll;
            _pitch = pitch;
            _head = head;
            eventCount[3]++;
        }

        // Callback function for acceleration
        //
        static void accelCallback(YGyro yGyro, double w, double x, double y, double z)
        {
            // this callback value is returned in milli-g; convert to g
            _ax = x * 0.001;
            _ay = y * 0.001;
            _az = z * 0.001;
            eventCount[3]++;
        }

        public Form1()
        {
            InitializeComponent();
        }

        // Check if we are testing a Yocto-3D or Yocto-3D-V2
        // (The Yocto-3D cannot do high-frequency acceleration output)
        //
        private bool supportsAcceleration(string serialNumber)
        {
            if (serialNumber.Substring(0, 7) != "Y3DMK00") return false;
            if (serialNumber.Substring(7, 1) == "1") return false;
            return true;
        }

        // After device plug or unplug, refresh the device chooser combo
        //
        private void moduleInventory()
        {
            YCompass c;
            string serial;
            int index;

            deviceChooser.Items.Clear();
            c = YCompass.FirstCompass();
            while (c != null)
            {
                serial = c.get_module().get_serialNumber();
                deviceChooser.Items.Add(serial);
                c = c.nextCompass();
            }

            if (deviceChooser.Items.Count == 0)
            {
                deviceChooser.Enabled = false;
                modeChooser.Enabled = false;
                toolStripStatusLabel2.Text = "Connect a Yocto-3D or Yocto-3D-V2 device";
                currentSerialNumber = "";
            }
            else
            {
                deviceChooser.Enabled = true;
                index = 0;

                for (int i = 0; i < deviceChooser.Items.Count; i++)
                {
                    if (deviceChooser.Items[i].Equals(currentSerialNumber)) index = i;
                }

                if (deviceChooser.Items.Count == 1)
                {
                    toolStripStatusLabel2.Text = "One Yocto-3D(-V2) device connected";

                }
                else
                {
                    toolStripStatusLabel2.Text = deviceChooser.Items.Count.ToString() + " Yocto-3D(-V2) devices connected";
                }

                deviceChooser.SelectedIndex = index;
                currentSerialNumber = (string)deviceChooser.Items[deviceChooser.SelectedIndex];
                if (supportsAcceleration(currentSerialNumber))
                {
                    modeChooser.Enabled = true;
                    if(modeChooser.SelectedIndex < 0) modeChooser.SelectedIndex = 0;
                }
                else
                {
                    modeChooser.Enabled = false;
                    modeChooser.SelectedIndex = 0;
                }
                setupDevice();
            }
        }

        // After a change of selected device, update the mode chooser combo
        // and setup callbacks to use the selected device
        //
        private void deviceChooser_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentSerialNumber = (string)deviceChooser.Items[deviceChooser.SelectedIndex];
            if (supportsAcceleration(currentSerialNumber))
            {
                modeChooser.Enabled = true;
            }
            else
            {
                modeChooser.Enabled = false;
                modeChooser.SelectedIndex = 0;
            }
            setupDevice();
        }

        // Configure the value callbacks on the currently selected device
        //
        private void setupDevice()
        {
            YAccelerometer accelerometer;
            YCompass compass;
            YTilt tilt1, tilt2;
            YGyro gyro;
            YQt qt1, qt2, qt3, qt4;

            if (currentSerialNumber != prevSerialNumber && prevSerialNumber != "")
            {
                // Unregister previous device
                tilt1 = YTilt.FindTilt(prevSerialNumber + ".tilt1");
                tilt2 = YTilt.FindTilt(prevSerialNumber + ".tilt2");
                compass = YCompass.FindCompass(prevSerialNumber + ".compass");
                gyro = YGyro.FindGyro(prevSerialNumber + ".gyro");
                compass.registerValueCallback(null);
                tilt1.registerValueCallback(null);
                tilt2.registerValueCallback(null);
                gyro.registerAnglesCallback(null);
            }
            if (currentSerialNumber == "") return;

            // Register the newly selected device
            accelerometer = YAccelerometer.FindAccelerometer(currentSerialNumber + ".accelerometer");
            tilt1 = YTilt.FindTilt(currentSerialNumber + ".tilt1");
            tilt2 = YTilt.FindTilt(currentSerialNumber + ".tilt2");
            compass = YCompass.FindCompass(currentSerialNumber + ".compass");
            gyro = YGyro.FindGyro(currentSerialNumber + ".gyro");
            qt1 = YQt.FindQt(currentSerialNumber + ".qt1");
            qt2 = YQt.FindQt(currentSerialNumber + ".qt2");
            qt3 = YQt.FindQt(currentSerialNumber + ".qt3");
            qt4 = YQt.FindQt(currentSerialNumber + ".qt4");
            compass.registerValueCallback(valueCallback);
            tilt1.registerValueCallback(valueCallback);
            tilt2.registerValueCallback(valueCallback);
            if (modeChooser.SelectedIndex != 1)
            {
                accelerometer.set_bandwidth(7);
                qt1.set_logicalName("w");
                qt2.set_logicalName("x");
                qt3.set_logicalName("y");
                qt4.set_logicalName("z");
                gyro.registerAnglesCallback(anglesCallback);
            }
            else
            {
                accelerometer.set_bandwidth(50);
                qt2.set_logicalName("ax");
                qt3.set_logicalName("ay");
                qt4.set_logicalName("az");
                gyro.registerQuaternionCallback(accelCallback);
            }
        }

        private void refreshUI()
        {
            // Automatically resize drawing to window
            int CanvasWidth = pictureBox1.Width;
            int CanvasHeight = pictureBox1.Height;
            int topMargin = CanvasHeight / 20;
            int xunit = CanvasWidth / 4;
            int yunit = (CanvasHeight - topMargin) / 6;
            int hs = (xunit < yunit ? xunit : yunit);
            int dl = hs * 3 / 4;
            double us = 0.03;
            int select = 0;

            // Bypass refresh if window is minimized
            if (CanvasWidth < 1 || CanvasHeight < 1) return;

            // Draw in an offline buffer to avoid flickering 
            Bitmap BackBuffer = new Bitmap(CanvasWidth, CanvasHeight);
            Graphics buffer = Graphics.FromImage(BackBuffer);
            FontFamily fontFamily = new FontFamily("Arial");
            Font font = new Font(fontFamily, hs / 8, FontStyle.Regular, GraphicsUnit.Pixel);
            Pen black = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
            SolidBrush whiteBrush, blackBrush, colorBrush;
            Point point1, point2;

            // Blank buffer
            whiteBrush = new SolidBrush(Color.FromArgb(255, 255, 255, 255));
            blackBrush = new SolidBrush(Color.FromArgb(255, 20, 20, 20));
            colorBrush = new SolidBrush(Color.FromArgb(255, 255, 0, 00));
            buffer.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            buffer.FillRectangle(whiteBrush, 0, 0, CanvasWidth, CanvasHeight);
            buffer.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            // Draw top legend
            buffer.DrawString("Values from registerValueCallback (7 Hz)", font, colorBrush, new PointF(xunit - hs - (hs >> 4), 0));
            if (modeChooser.SelectedIndex != 1)
            {
                buffer.DrawString("Values from registerAnglesCallback (up to 100Hz)", font, colorBrush, new PointF(3 * xunit - hs - (hs >> 1), 0));
                select = 0;
            }
            else
            {
                buffer.DrawString("Values from registerQuaternionCallback (up to 100Hz)", font, colorBrush, new PointF(3 * xunit - hs - (hs >> 1), 0));
                select = 3;
            }

            for (int dial = 0; dial < 6; dial++)
            {
                int hx = (2*(dial / 3)+1) * xunit;
                int hy = topMargin + (2*(dial % 3)+1) * yunit;
                int mode = dial + (dial>=3 ? select : 0);
                double angle = 0;
                string legend = "";
                switch(mode)
                {
                    case 0: angle = _tilt1; legend = "tilt1";  break;
                    case 1: angle = -_tilt2; legend = "tilt2"; break;
                    case 2: angle = _compass; legend = "compass"; break;
                    case 3: angle = _roll; legend = "roll"; break;
                    case 4: angle = -_pitch; legend = "pitch"; break;
                    case 5: angle = _head; legend = "heading"; break;
                    case 6: angle = 60 * _ax; legend = "ax"; break;
                    case 7: angle = 60 * _ay; legend = "ay"; break;
                    case 8: angle = 60 * _az; legend = "az"; break;
                }

                // draw the legend
                buffer.DrawString(legend, font, colorBrush, new PointF(hx - hs, hy - hs));
                string freqText = freq[mode < 3 ? mode : 3].ToString() + " Hz"; 
                SizeF freqSize = buffer.MeasureString(freqText, font);
                buffer.DrawString(freqText, font, colorBrush, new PointF(hx + hs - freqSize.Width, hy - hs));

                // draw the needle
                if (mode < 6)
                {
                    // draw a needle
                    angle *= Math.PI / 180;
                    double dx = Math.Cos(angle) * dl;
                    double dy = Math.Sin(angle) * dl;
                    double Xx = Math.Cos(angle) * dl * us;
                    double Xy = Math.Sin(angle) * dl * us;
                    double Yx = Math.Cos(angle - Math.PI / 2) * dl * us;
                    double Yy = Math.Sin(angle - Math.PI / 2) * dl * us;
                    Point[] poly = {
                        new Point(Convert.ToInt32(hx - dx + 4*Yx), Convert.ToInt32(hy - dy + 4*Yy)),
                        new Point(Convert.ToInt32(hx - dx + 6*Xx + Yx), Convert.ToInt32(hy - dy + 6*Xy + Yy)),
                        new Point(Convert.ToInt32(hx + dx - 4*Xx + Yx), Convert.ToInt32(hy + dy - 4*Xy + Yy)),
                        new Point(Convert.ToInt32(hx + dx - 4*Xx + 4*Yx), Convert.ToInt32(hy + dy - 4*Xy + 4*Yy)),
                        new Point(Convert.ToInt32(hx + dx), Convert.ToInt32(hy + dy)),
                        new Point(Convert.ToInt32(hx + dx - 4*Xx - 4*Yx), Convert.ToInt32(hy + dy - 4*Xy - 4*Yy)),
                        new Point(Convert.ToInt32(hx + dx - 4*Xx - Yx), Convert.ToInt32(hy + dy - 4*Xy - Yy)),
                        new Point(Convert.ToInt32(hx - dx + 6*Xx - Yx), Convert.ToInt32(hy - dy + 6*Xy - Yy)),
                        new Point(Convert.ToInt32(hx - dx - 4*Yx), Convert.ToInt32(hy - dy - 4*Yy)),
                        new Point(Convert.ToInt32(hx - dx + 2*Xx), Convert.ToInt32(hy - dy + 2*Xy)),
                    };
                    buffer.FillPolygon(colorBrush, poly);
                }
                else
                {
                    // draw a circular gauge
                    Rectangle rect = new Rectangle(hx - dl, hy - dl, 2 * dl, 2 * dl);
                    buffer.FillPie(colorBrush, rect, 0, (float)-angle);
                    int îdl = Convert.ToInt32(dl * 0.8);
                    rect = new Rectangle(hx - îdl, hy - îdl, 2 * îdl, 2 * îdl);
                    buffer.FillEllipse(whiteBrush, rect);
                }

                // draw the scale
                for (int i = 0; i < 360; i++)
                {
                    angle = Math.PI * i / 180;
                    double ddx = Math.Cos(angle) * hs * 0.8;
                    double ddy = Math.Sin(angle) * hs * 0.8;
                    if (i % 10 == 0)
                    {
                        point1 = new Point(Convert.ToInt32(hx + ddx), Convert.ToInt32(hy + ddy));
                        point2 = new Point(Convert.ToInt32(hx + ddx * 0.95), Convert.ToInt32(hy + ddy * 0.95));
                        if (i % 30 == 0)
                        {
                            int value = i;
                            if(mode == 1 || mode == 4 || mode >= 6)
                            {
                                value = 180 - ((i + 180) % 360);
                            }
                            string text = (mode < 6 ? value.ToString() : (value/60.0).ToString()+"g");
                            SizeF size = buffer.MeasureString(text, font);
                            int tx = Convert.ToInt32(hx + Math.Cos(angle) * hs * 0.93 - size.Width / 2);
                            int ty = Convert.ToInt32(hy + Math.Sin(angle) * hs * 0.88 - size.Height / 2);
                            buffer.DrawString(text, font, blackBrush, new PointF(tx, ty));
                        }
                    }
                    else
                    {
                        point1 = new Point(Convert.ToInt32(hx + ddx * 0.95), Convert.ToInt32(hy + ddy * 0.95));
                        point2 = new Point(Convert.ToInt32(hx + ddx * 0.97), Convert.ToInt32(hy + ddy * 0.97));
                    }
                    buffer.DrawLine(black, point1, point2);
                }

            }

            // Update display using buffered image
            Graphics Viewable = pictureBox1.CreateGraphics();
            Viewable.DrawImageUnscaled(BackBuffer, 0, 0); 
            Viewable.Dispose();
            font.Dispose();
            black.Dispose();
            colorBrush.Dispose();
            blackBrush.Dispose();
            whiteBrush.Dispose();
            buffer.Dispose();
            BackBuffer.Dispose();
        }

        private void modeChooser_SelectedIndexChanged(object sender, EventArgs e)
        {
            setupDevice();
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

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            string errmsg = "";
            YAPI.HandleEvents(ref errmsg);
            refreshUI();
            ulong now = YAPI.GetTickCount();
            if (now - eventTick >= 1000)
            {
                for(int i = 0; i < eventCount.Length; i++)
                {
                    freq[i] = (int)Math.Round(eventCount[i] * 1000.0 / (now - eventTick));
                    eventCount[i] = 0;
                }
                eventTick = now;
            }
        }

    }
}
