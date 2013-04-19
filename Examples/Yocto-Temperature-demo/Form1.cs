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
    public Form1()
    {
      InitializeComponent();
    }

    private void moduleInventory()
    {
      YModule m, currentmodule;
      YTemperature sensor;
      int index;

      comboBox1.Items.Clear();
      currentmodule = null;
      sensor = YTemperature.FirstTemperature();
      while (sensor != null)
      {
        m = sensor.get_module();
        comboBox1.Items.Add(m);
        sensor = sensor.nextTemperature();
      }

      if (comboBox1.Items.Count == 0)
      {
        comboBox1.Enabled = false;
        Beacon.Enabled = false;
        label1.Enabled = false;
        label1.Text = "N/A";
        toolStripStatusLabel2.Text = "Connect a device featuring a temperature sensor";
      }
      else
      {
        index = 0;
        comboBox1.Enabled = true;
        Beacon.Enabled = true;

        for (int i = 0; i < comboBox1.Items.Count; i++)
        {
          if (comboBox1.Items[i].Equals(currentmodule)) index = i;
        }

        if (comboBox1.Items.Count == 1)
          toolStripStatusLabel2.Text = "One  device connected";
        else
          toolStripStatusLabel2.Text = comboBox1.Items.Count.ToString() + " devices connected";

        comboBox1.SelectedIndex = index;
      }
    }

    private void refreshUI()
    {
      YModule m;
      YTemperature sensor;
      if (comboBox1.Enabled)
      {
        m = (YModule)comboBox1.Items[comboBox1.SelectedIndex];
        sensor = YTemperature.FindTemperature(m.get_serialNumber() + ".temperature");
        if (sensor.isOnline())
        {
          label1.Enabled = true;
          label1.Text = sensor.get_currentValue().ToString() + " °C";
        }
        Beacon.Checked = (m.get_beacon() == YModule.BEACON_ON);
      }
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
      RefreshTimer.Interval = 200;
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

    private void Beacon_Click(object sender, EventArgs e)
    {
      YModule m;
      if (!comboBox1.Enabled) return;
      m = (YModule)comboBox1.Items[comboBox1.SelectedIndex];
      if (!m.isOnline()) return;

      if (m.get_beacon() == YModule.BEACON_OFF)
        m.set_beacon(YModule.BEACON_ON);
      else
        m.set_beacon(YModule.BEACON_OFF);

      refreshUI();
    }
  }
}
