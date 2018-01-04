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
      string name;
      int index;

      comboBox1.Items.Clear();
      currentmodule = null;
      m = YModule.FirstModule();
      while (m != null)
      {
        name = m.get_serialNumber();
        if (name.Substring(0, 8) == "YCTOPOC1")
        { comboBox1.Items.Add(m); }
        m = m.nextModule();
      }

      if (comboBox1.Items.Count == 0)
      {
        comboBox1.Enabled = false;
        Beacon.Enabled = false;
        TestLed.Enabled = false;
        toolStripStatusLabel1.Text = "Connect a Yocto-Demo device";
      }
      else
      {
        index = 0;
        comboBox1.Enabled = true;
        Beacon.Enabled = true;
        TestLed.Enabled = true;

        for (int i = 0; i < comboBox1.Items.Count; i++)
        {
          if (comboBox1.Items[i].Equals(currentmodule)) index = i;
        }

        if (comboBox1.Items.Count == 1)
          toolStripStatusLabel1.Text = "One Yocto-Demo device connected";
        else
          toolStripStatusLabel1.Text = comboBox1.Items.Count.ToString() + " Yocto-Demo devices connected";

        comboBox1.SelectedIndex = index;
      }
    }

    private void refreshUI()
    {
      int index = 4;
      YModule m;
      YLed led;
      if (comboBox1.Enabled)
      {
        //TestLed.Click -= TestLed_Click;


        index = 0;
        m = (YModule)comboBox1.Items[comboBox1.SelectedIndex];
        led = YLed.FindLed(m.get_serialNumber() + ".led");
        if (led.isOnline())
        {
          if (led.get_power() == YLed.POWER_ON)
          {
            index = index | 1;
            TestLed.Checked = true;
          }
          else TestLed.Checked = false;
          if (m.get_beacon() == YModule.BEACON_ON)
          {
            index = index | 2;
            Beacon.Checked = true;
          }
          else Beacon.Checked = false;
        }
      }

      switch (index)
      {
        case 0: pictureBox1.Image = Properties.Resources.poc; break;
        case 1: pictureBox1.Image = Properties.Resources.pocg; break;
        case 2: pictureBox1.Image = Properties.Resources.pocb; break;
        case 3: pictureBox1.Image = Properties.Resources.pocbg; break;
        case 4: pictureBox1.Image = Properties.Resources.nopoc; break;
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

    private void TestLed_Click(object sender, EventArgs e)
    {
      YModule m;
      YLed led;
      if (!comboBox1.Enabled) return;
      m = (YModule)comboBox1.Items[comboBox1.SelectedIndex];
      if (!m.isOnline()) return;
      led = YLed.FindLed(m.get_serialNumber() + ".led");
      if (led.get_power() == YLed.POWER_OFF)
        led.set_power(YLed.POWER_ON);
      else
        led.set_power(YLed.POWER_OFF);

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
