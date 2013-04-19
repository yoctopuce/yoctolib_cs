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
        private YoctoGauge[] gauges;

        private int gaugesCount = 4;   // show 4 gauges

        // some rendering contants
        private int gaugeWidth = 250;
        private int countPerRaw = 2;
        private int marginV = 10;
        private int marginH = 40;

        public Form1()
        {
            InitializeComponent();
            gauges = new YoctoGauge[gaugesCount];

            int clientWidth = countPerRaw * (gaugeWidth + marginV);
            if (countPerRaw > gaugesCount) clientWidth = gaugesCount * (gaugeWidth + marginV); ;
            int clientHeight = 20 + (gaugeWidth + marginH) * (1 + (int)((gaugesCount - 1) / countPerRaw));

            this.ClientSize = new Size(clientWidth, clientHeight);

            for (int i = 0; i < gaugesCount; i++)
            {
                int x = i % countPerRaw;
                int y = (int)i / countPerRaw;
                // create a new combo box and add it to force
                ComboBox d = new ComboBox();
                d.Width = gaugeWidth;
                d.Left = (marginV / 2) + (x * (gaugeWidth + marginV));
                d.Top = 5 + (y * (gaugeWidth + marginH));
                d.DropDownStyle = ComboBoxStyle.DropDownList;
                d.Enabled = false;
                this.Controls.Add(d);
                // create the gauge drawing area as well
                PictureBox p = new PictureBox();
                p.Height = gaugeWidth;
                p.Width = gaugeWidth;
                p.Left = d.Left;
                p.Top = d.Top + d.Height + 5;
                this.Controls.Add(p);

                // init gauge list with ui's elements
                gauges[i] = new YoctoGauge(d, p, 120);

            }

        }

        // called when a device is plugged
        public void deviceArrival(YModule m)
        { // new device arrived, lets check if any Ytemperature function are hosted
            int fctCount = m.functionCount();
            YTemperature t;
            // list the functions availble on the device
            for (int i = 0; i < fctCount; i++)
            {
                string fname = m.functionId(i);
                if (fname.StartsWith("temperature"))
                {
                    t = YTemperature.FindTemperature(m.get_serialNumber() + '.' + fname);
                    // used in deviceRemoval
                    t.set_userData(t.get_module().get_serialNumber());

                    // ok  temperature found, lets add it to all gauges control dropdown
                    for (int j = 0; j < gauges.GetLength(0); j++)
                    {
                        ComboBox d = gauges[j].getDropDown();
                        MyTemperature mt = new MyTemperature(t);
                        d.Items.Add(mt);

                    }
                }
            }

            // enable the dropdowns if  not empty 
            for (int j = 0; j < gauges.GetLength(0); j++)
            {
                ComboBox d = gauges[j].getDropDown();
                if (!d.Enabled && d.Items.Count > 0)
                {
                    if (j < d.Items.Count)
                    {
                        d.SelectedIndex = j;
                        d.Enabled = true;
                    }
                }
            }
        }

        // called when a device is unplugged
        public void deviceRemoval(YModule m)
        {
            //Cycle on All DropDown gauges
            for (int j = 0; j < gauges.GetLength(0); j++)
            {
                ComboBox d = gauges[j].getDropDown();
                int selected = d.SelectedIndex;
                // cycle on all gauge dropdown items
                for (int i = d.Items.Count - 1; i >= 0; i--)
                { //search for function stored in the drop down list
                    MyTemperature mt = (MyTemperature)d.Items[i];
                    YTemperature t = mt.getYTemperature();
                    // test if the fucntion parent module is the the one removed
                    // note : it's too late to use get_module on t, so with use
                    // a little trick: with stored the module serial in the function
                    // userdate.

                    if ((string)t.get_userData() == m.get_serialNumber())
                    { // remove it from the drop down
                        d.Items.RemoveAt(i);
                        // selected index update
                        if (selected == i) selected--;
                        if (selected >= d.Items.Count) selected = d.Items.Count - 1;
                    }
                }
                if (selected >= 0) d.SelectedIndex = selected;
            }

            // disable empty dropdowns
            for (int j = 0; j < gauges.GetLength(0); j++)
            {
                ComboBox d = gauges[j].getDropDown();
                if (d.Enabled && d.Items.Count <= 0)
                {
                    d.Enabled = false;
                }
            }
        }

        // poll temperature function for all gauges and
        // update UI acordingly
        private void refreshUI()
        {
            for (int j = 0; j < gauges.GetLength(0); j++)
            {
                ComboBox d = gauges[j].getDropDown();
                if (d.Enabled)
                    if (d.SelectedIndex >= 0)
                    {
                        MyTemperature mt = (MyTemperature)d.Items[d.SelectedIndex];
                        YTemperature t = mt.getYTemperature();
                        gauges[j].setValue(t.get_currentValue());
                    }
                gauges[j].refresh();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // we wanna know when device list changes
            YAPI.RegisterDeviceArrivalCallback(deviceArrival);
            YAPI.RegisterDeviceRemovalCallback(deviceRemoval);

            toolStripStatusLabel1.Text = "Connect a device featuring a temperature sensor";
            // start timers
            InventoryTimer.Interval = 1000;
            InventoryTimer.Start();
            RefreshTimer.Interval = 100;
            RefreshTimer.Start();
        }

        private void InventoryTimer_Tick(object sender, EventArgs e)
        {
            string errmsg = "";
            YAPI.UpdateDeviceList(ref errmsg);
        }



        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            refreshUI();
        }


    }
    class MyTemperature
    {
        YTemperature _t;

        public MyTemperature(YTemperature t)
        {
            _t = t;
        }

        public YTemperature getYTemperature()
        {
            return _t;
        }

        public override string ToString()
        {
            string module = _t.module().get_logicalName();
            if (module== "")
            {
                module = _t.module().get_serialNumber();
            }
            string func = _t.get_logicalName();
            if (func == "")
            {
                func = _t.get_hardwareId();
                func = func.Substring(func.IndexOf('.')+1);
            }
            return module+" "+func;
        }
    }
}
