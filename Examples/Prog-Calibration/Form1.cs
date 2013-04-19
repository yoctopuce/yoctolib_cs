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
          YFunction fct = null;
          // We have to have handle each sensor type independtly, (sorry about that)
          if (fctName.IndexOf("temperature") == 0) fct = (YFunction)YTemperature.FindTemperature(fctFullName);
          if (fctName.IndexOf("humidity") == 0) fct = (YFunction)YHumidity.FindHumidity(fctFullName);
          if (fctName.IndexOf("pressure") == 0) fct = (YFunction)YPressure.FindPressure(fctFullName);
          if (fctName.IndexOf("lightSensor") == 0) fct = (YFunction)YLightSensor.FindLightSensor(fctFullName);
          if (fctName.IndexOf("carbonDioxide") == 0) fct = (YFunction)YCarbonDioxide.FindCarbonDioxide(fctFullName);
          if (fctName.IndexOf("voltage") == 0) fct = (YFunction)YVoltage.FindVoltage(fctFullName);
          if (fctName.IndexOf("current") == 0) fct = (YFunction)YCurrent.FindCurrent(fctFullName);
          // add the function in the second drop down
          if (fct != null) functionsList.Items.Add(fct);
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

      YFunction fct = (YFunction)functionsList.Items[functionsList.SelectedIndex];
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

        if (fct is YTemperature) DisplayTemperatureCalPoints((YTemperature)fct);
        if (fct is YPressure) DisplayPressureCalPoints((YPressure)fct);
        if (fct is YHumidity) DisplayHumidityCalPoints((YHumidity)fct);
        if (fct is YLightSensor) DisplayLightSensorCalPoints((YLightSensor)fct);
        if (fct is YCarbonDioxide) DisplayCarbonDioxideCalPoints((YCarbonDioxide)fct);
        if (fct is YVoltage) DisplayVoltageCalPoints((YVoltage)fct);
        if (fct is YCurrent) DisplayCurrentCalPoints((YCurrent)fct);
      }

      if (fct.isOnline())
      {
        if (fct is YTemperature) DisplayTemperature((YTemperature)fct);
        if (fct is YPressure) DisplayPressure((YPressure)fct);
        if (fct is YHumidity) DisplayHumidity((YHumidity)fct);
        if (fct is YLightSensor) DisplayLightSensor((YLightSensor)fct);
        if (fct is YCarbonDioxide) DisplayCarbonDioxide((YCarbonDioxide)fct);
        if (fct is YVoltage) DisplayVoltage((YVoltage)fct);
        if (fct is YCurrent) DisplayCurrent((YCurrent)fct);
      }
    }


    private void displayValue(double value, double rawvalue, double resolution, string valunit)
    {
      // displays the sensor value on the ui
      ValueDisplayUnits.Text = valunit;

      if (resolution != YTemperature.RESOLUTION_INVALID)
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


    private void DisplayCalPoints(double[] ValuesRaw, double[] ValuesCal, double resolution)
    {
      int i;
      // little trick: if resolution is not available on the device, the
      // calibration in not available either
      if (resolution == YTemperature.RESOLUTION_INVALID)
      {
        EnableCalibrationUI(false);
        unsupported_warning.Visible = true;
        return;
      }

      // display the calibration points
      unsupported_warning.Visible = false;
      for (i = 0; i < ValuesRaw.Length; i++)
      {
        rawedit[i].Text = ValuesRaw[i].ToString();
        caledit[i].Text = ValuesCal[i].ToString();
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


    // this the weak point of the API, methods get_currentValue,
    // get_resolution,  get_unit etc... are present in all sensor classes, but 
    // are  not inherited from the parent class (to keep the object model
    // simple) we have to handle them independtly for each sensor type.

    private void DisplayTemperature(YTemperature fct)
    { displayValue(fct.get_currentValue(), fct.get_currentRawValue(), fct.get_resolution(), fct.get_unit()); }

    private void DisplayPressure(YPressure fct)
    { displayValue(fct.get_currentValue(), fct.get_currentRawValue(), fct.get_resolution(), fct.get_unit()); }


    private void DisplayHumidity(YHumidity fct)
    { displayValue(fct.get_currentValue(), fct.get_currentRawValue(), fct.get_resolution(), fct.get_unit()); }

    private void DisplayLightSensor(YLightSensor fct)
    { displayValue(fct.get_currentValue(), fct.get_currentRawValue(), fct.get_resolution(), fct.get_unit()); }

    private void DisplayCarbonDioxide(YCarbonDioxide fct)
    { displayValue(fct.get_currentValue(), fct.get_currentRawValue(), fct.get_resolution(), fct.get_unit()); }

    private void DisplayVoltage(YVoltage fct)
    { displayValue(fct.get_currentValue(), fct.get_currentRawValue(), fct.get_resolution(), fct.get_unit()); }

    private void DisplayCurrent(YCurrent fct)
    { displayValue(fct.get_currentValue(), fct.get_currentRawValue(), fct.get_resolution(), fct.get_unit()); }



    private void DisplayTemperatureCalPoints(YTemperature fct)
    {
      double[] ValuesRaw = new Double[0];
      double[] ValuesCal = new Double[0];

      fct.loadCalibrationPoints(ref ValuesRaw, ref ValuesCal);
      DisplayCalPoints(ValuesRaw, ValuesCal, fct.get_resolution());
    }

    private void DisplayPressureCalPoints(YPressure fct)
    {
      double[] ValuesRaw = null;
      double[] ValuesCal = null;

      fct.loadCalibrationPoints(ref ValuesRaw, ref ValuesCal);
      DisplayCalPoints(ValuesRaw, ValuesCal, fct.get_resolution());
    }

    private void DisplayHumidityCalPoints(YHumidity fct)
    {
      double[] ValuesRaw = null;
      double[] ValuesCal = null;

      fct.loadCalibrationPoints(ref ValuesRaw, ref ValuesCal);
      DisplayCalPoints(ValuesRaw, ValuesCal, fct.get_resolution());
    }

    private void DisplayLightSensorCalPoints(YLightSensor fct)
    {
      double[] ValuesRaw = null;
      double[] ValuesCal = null;

      fct.loadCalibrationPoints(ref ValuesRaw, ref ValuesCal);
      DisplayCalPoints(ValuesRaw, ValuesCal, fct.get_resolution());
    }

    private void DisplayCarbonDioxideCalPoints(YCarbonDioxide fct)
    {
      double[] ValuesRaw = null;
      double[] ValuesCal = null;

      fct.loadCalibrationPoints(ref ValuesRaw, ref ValuesCal);
      DisplayCalPoints(ValuesRaw, ValuesCal, fct.get_resolution());
    }

    private void DisplayVoltageCalPoints(YVoltage fct)
    {
      double[] ValuesRaw = null;
      double[] ValuesCal = null;

      fct.loadCalibrationPoints(ref ValuesRaw, ref ValuesCal);
      DisplayCalPoints(ValuesRaw, ValuesCal, fct.get_resolution());
    }

    private void DisplayCurrentCalPoints(YCurrent fct)
    {
      double[] ValuesRaw = null;
      double[] ValuesCal = null;

      fct.loadCalibrationPoints(ref ValuesRaw, ref ValuesCal);
      DisplayCalPoints(ValuesRaw, ValuesCal, fct.get_resolution());
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
      Double[] ValuesRaw = new Double[0];
      Double[] ValuesCal = new Double[0];
      YFunction fct;
      bool stop = false;
      int i = 0, j;

      if (functionsList.SelectedIndex < 0) return;

      try
      {
        while ((caledit[i].Text != "") && (rawedit[i].Text != "") && (i < 5) && (!stop))
        {
          Array.Resize(ref ValuesRaw, i + 1);
          Array.Resize(ref ValuesCal, i + 1);
          ValuesCal[i] = Convert.ToDouble(caledit[i].Text);
          ValuesRaw[i] = Convert.ToDouble(rawedit[i].Text);
          if (i > 0)
            if (ValuesRaw[i] <= ValuesRaw[i - 1])
            {
              stop = true;
              Array.Resize(ref ValuesRaw, i);
              Array.Resize(ref ValuesCal, i);
              i--;
            }
          i++;
        }
      }
      catch (Exception)
      {
        Array.Resize(ref ValuesRaw, i);
        Array.Resize(ref ValuesCal, i);
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
      fct = (YFunction)functionsList.Items[functionsList.SelectedIndex];
      if (fct.isOnline())
      {
        if (fct is YTemperature) ((YTemperature)fct).calibrateFromPoints(ValuesRaw, ValuesCal);
        if (fct is YPressure) ((YHumidity)fct).calibrateFromPoints(ValuesRaw, ValuesCal);
        if (fct is YLightSensor) ((YLightSensor)fct).calibrateFromPoints(ValuesRaw, ValuesCal);
        if (fct is YCarbonDioxide) ((YCarbonDioxide)fct).calibrateFromPoints(ValuesRaw, ValuesCal);
        if (fct is YVoltage) ((YVoltage)fct).calibrateFromPoints(ValuesRaw, ValuesCal);
        if (fct is YCurrent) ((YCurrent)fct).calibrateFromPoints(ValuesRaw, ValuesCal);
        if (fct is YHumidity) ((YHumidity)fct).calibrateFromPoints(ValuesRaw, ValuesCal);
      }
  

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



