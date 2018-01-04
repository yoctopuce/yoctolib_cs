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
        TextBox[] caledit = new TextBox[5];
        TextBox[] rawedit = new TextBox[5];

        private void arrivalCallback(YModule m)
        { // add the device in the 1srt combo list
            devicesList.Items.Add(m);
            if (devicesList.Items.Count == 1)
            {
                devicesList.SelectedIndex = 0;
                choosenDeviceChanged();
            }
        }

        private void removalCallback(YModule m)
        {
            int index = -1;
            for (int i = 0; i < devicesList.Items.Count; i++)
                if (devicesList.Items[i] == m) index = i;

            // if we removed the current module, we must fully refresh the ui
            bool mustrefresh = index == devicesList.SelectedIndex;

            if (index >= 0)
            { // remove it from the combo box
                devicesList.Items.RemoveAt(index);
                if (devicesList.SelectedIndex >= devicesList.Items.Count) devicesList.SelectedIndex = 0;
                // if  we  deleted the active device, we need a refresh
                if (mustrefresh)
                {
                    functionsList.Enabled = false;
                    choosenDeviceChanged();
                }
            }

        }


        public Form1()
        {
            InitializeComponent();

            caledit[0] = C0;
            caledit[1] = C1;
            caledit[2] = C2;
            caledit[3] = C3;
            caledit[4] = C4;

            rawedit[0] = R0;
            rawedit[1] = R1;
            rawedit[2] = R2;
            rawedit[3] = R3;
            rawedit[4] = R4;

            //register arrival/removal callbacks
            YAPI.RegisterDeviceArrivalCallback(arrivalCallback);
            YAPI.RegisterDeviceRemovalCallback(removalCallback);

            timer1.Enabled = true;
        }


        private void choosenDeviceChanged()
        {
            YModule currentDevice;

            devicesList.Enabled = devicesList.Items.Count > 0;
            // clear the functions drop down
            functionsList.Items.Clear();
            functionsList.Enabled = devicesList.Enabled;

            if (!devicesList.Enabled)
            {
                unsupported_warning.Visible = false;
                nosensorfunction.Visible = false;
                return;  // no device at all connected,
            }

            if (devicesList.SelectedIndex < 0) devicesList.SelectedIndex = 0;
            currentDevice = (YModule)devicesList.Items[devicesList.SelectedIndex];

            // populate the second drop down
            if (currentDevice.isOnline())
            {  // device capabilities inventory
                int fctcount = currentDevice.functionCount();
                for (int i = 0; i < fctcount; i++)
                {
                    string fctName = currentDevice.functionId(i);
                    string fctFullName = currentDevice.get_serialNumber() + '.' + fctName;
                    YSensor fct = YSensor.FindSensor(fctFullName);
                    // add the function in the second drop down
                    if (fct.isOnline()) functionsList.Items.Add(fct);
                }

            }
            functionsList.Enabled = functionsList.Items.Count > 0;
            if (functionsList.Enabled) functionsList.SelectedIndex = 0;

            refreshFctUI(true);
        }

        private void refreshFctUI(bool newone)
        {
            nosensorfunction.Visible = false;
            toolStripStatusLabel1.Text = devicesList.Items.Count.ToString() + " device(s) found";

            if (!functionsList.Enabled)
            { // disable the UI
                ValueDisplay.Text = "N/A";
                ValueDisplayUnits.Text = "-";
                RawValueDisplay.Text = "-";
                EnableCalibrationUI(false);
                if (devicesList.Enabled)
                    nosensorfunction.Visible = true;
                else
                    toolStripStatusLabel1.Text = "Plug a Yocto-device";
                return;
            }

            YSensor fct = (YSensor)functionsList.Items[functionsList.SelectedIndex];
            if (newone)
            { // enable the ui
                EnableCalibrationUI(true);
                for (int i = 0; i < 5; i++)
                {
                    caledit[i].Text = "";
                    caledit[i].BackColor = System.Drawing.SystemColors.Window;
                    rawedit[i].Text = "";
                    rawedit[i].BackColor = System.Drawing.SystemColors.Window;
                }
                DisplayCalPoints(fct);
            }

            if (fct.isOnline()) DisplayValue(fct);
        }


        private void DisplayValue(YSensor fct)
        {
            double value = fct.get_currentValue();
            double rawvalue = fct.get_currentRawValue();
            double resolution= fct.get_resolution();
            string valunit = fct.get_unit();
            // displays the sensor value on the ui
            ValueDisplayUnits.Text = valunit;

            if (resolution != YSensor.RESOLUTION_INVALID)
            {  // if resolution is available on the device the use it to  round the value
                string format = "F" + ((int)-Math.Round(Math.Log10(resolution))).ToString();

                RawValueDisplay.Text = "(raw value: " + (resolution * Math.Round(rawvalue / resolution)).ToString(format) + ")";
                ValueDisplay.Text = (resolution * Math.Round(value / resolution)).ToString(format);
            }
            else
            {
                ValueDisplay.Text = value.ToString();
                RawValueDisplay.Text = "";
            }
        }

        // enable /disbale the calibration data edition
        private void EnableCalibrationUI(bool state)
        {
            int i;
            for (i = 0; i < 5; i++)
            {
                caledit[i].Enabled = state;
                rawedit[i].Enabled = state;
                if (!state)
                {
                    caledit[i].Text = "";
                    rawedit[i].Text = "";
                    caledit[i].BackColor = System.Drawing.SystemColors.Window;
                    rawedit[i].BackColor = System.Drawing.SystemColors.Window;
                }
            }
            RawLabel.Enabled = state;
            CalibratedLabel.Enabled = state;
            saveBtn.Enabled = state;
            cancelBtn.Enabled = state;
        }


        private void DisplayCalPoints(YSensor fct)
        {
            List<double> ValuesRaw = new List<double>();
            List<double> ValuesCal = new List<double>();

            int retcode = fct.loadCalibrationPoints(ValuesRaw, ValuesCal);
            if (retcode == YAPI.NOT_SUPPORTED)
            {
                EnableCalibrationUI(false);
                unsupported_warning.Visible = true;
                return;
            }

            // display the calibration points
            unsupported_warning.Visible = false;
            int i;
            for (i = 0; i < ValuesRaw.Count; i++)
            {
                rawedit[i].Text = YAPI._floatToStr(ValuesRaw[i]);
                caledit[i].Text = YAPI._floatToStr(ValuesCal[i]);
                rawedit[i].BackColor = System.Drawing.Color.FromArgb(0xA0, 0xFF, 0xA0);
                caledit[i].BackColor = System.Drawing.Color.FromArgb(0xA0, 0xFF, 0xA0);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string errmsg = "";

            // force an inventory, arrivalCallback and removalCallback
            // will be called if something changed
            YAPI.UpdateDeviceList(ref errmsg);

            // refresh the UI values
            refreshFctUI(false);

        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            // reload the device configuration from the flash
            YModule m = (YModule)devicesList.Items[devicesList.SelectedIndex];
            if (m.isOnline()) m.revertFromFlash();
            refreshFctUI(true);

        }

        private void saveBtn_Click(object sender, EventArgs e)
        { // saves the device current configuration into flash
            YModule m = (YModule)devicesList.Items[devicesList.SelectedIndex];
            if (m.isOnline()) m.saveToFlash();
        }

        //  This is the key function: it sets the calibration
        //  data in the device. Note: the parameters are written
        //  in the device RAM, if you want the calibration
        //  to be persistent, you have to call saveToflash();

        private void CalibrationChange(object sender, EventArgs e)
        {
            List<double> ValuesRaw = new List<double>();
            List<double> ValuesCal = new List<double>();
            List<int> ParseRaw = new List<int>();
            List<int> ParseCal = new List<int>();
            int i = 0, j;

            if (functionsList.SelectedIndex < 0) return;

            while ((caledit[i].Text != "") && (rawedit[i].Text != "") && (i < 5))
            {
                ParseRaw = YAPI._decodeFloats(rawedit[i].Text);
                ParseCal = YAPI._decodeFloats(caledit[i].Text);
                if (ParseRaw.Count != 1 || ParseCal.Count != 1) break;
                if (i > 0)
                {
                    if (ParseRaw[0] / 1000.0 <= ValuesRaw[i - 1]) break;
                }
                ValuesRaw.Add(ParseRaw[0] / 1000.0);
                ValuesCal.Add(ParseCal[0] / 1000.0);
                i++;
            }

            // some ui cosmetics: correct values are turned to green
            for (j = 0; j < i; j++)
            {
                caledit[j].BackColor = System.Drawing.Color.FromArgb(0xA0, 0xFF, 0xA0);
                rawedit[j].BackColor = System.Drawing.Color.FromArgb(0xA0, 0xFF, 0xA0);
            }
            for (j = i; j < 5; j++)
            {
                caledit[j].BackColor = System.Drawing.SystemColors.Window;
                rawedit[j].BackColor = System.Drawing.SystemColors.Window;
            }

            // send the calibration point to the device
            YSensor fct = (YSensor)functionsList.Items[functionsList.SelectedIndex];
            if (fct.isOnline()) fct.calibrateFromPoints(ValuesRaw, ValuesCal);
        }

        private void devicesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            choosenDeviceChanged();
        }

        private void functionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshFctUI(true);
        }

    }
}



