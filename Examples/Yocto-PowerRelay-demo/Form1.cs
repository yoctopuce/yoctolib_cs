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
    private int timerindex = 0;

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
        if (name.Substring(0, 8) == "RELAYHI1")
        { comboBox1.Items.Add(m); }
        m = m.nextModule();
      }

      if (comboBox1.Items.Count == 0)
      {
        comboBox1.Enabled = false;
        btBeacon.Enabled = false;
        btA.Enabled = false;
        btB.Enabled = false;
        toolStripStatusLabel1.Text = "Connect a Yocto-PowerRelay device";
      }
      else
      {
        index = 0;
        comboBox1.Enabled = true;
        btBeacon.Enabled = true;
        btA.Enabled = true;
        btB.Enabled = true;


        for (int i = 0; i < comboBox1.Items.Count; i++)
        {
          if (comboBox1.Items[i].Equals(currentmodule)) index = i;
        }

        if (comboBox1.Items.Count == 1)
          toolStripStatusLabel1.Text = "One Yocto-PowerRelay device connected";
        else
          toolStripStatusLabel1.Text = comboBox1.Items.Count.ToString() + " Yocto-PowerRelay devices connected";

        comboBox1.SelectedIndex = index;
      }
    }

    private void setImage(int index)
    { switch (index)
      { case 0: pictureBox1.Image = Properties.Resources.A; break;
        case 1: pictureBox1.Image = Properties.Resources.Abeacon; break;
        case 2: pictureBox1.Image = Properties.Resources.B; break;
        case 3: pictureBox1.Image = Properties.Resources.Bbeacon; break;
        case 4: pictureBox1.Image = Properties.Resources.off; break;
      }
    }

    public void devicelistchanged(YModule m)
    {
      moduleInventory();
    
    }



    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      moduleInventory();
      YAPI.RegisterDeviceArrivalCallback(devicelistchanged);
      YAPI.RegisterDeviceRemovalCallback(devicelistchanged);

      deviceScanTimer.Interval = 1000;
      deviceScanTimer.Start();
      refreshTimer.Interval = 200;
      refreshTimer.Start();

    }

    private void deviceScanTimer_Tick(object sender, EventArgs e)
    {
      string errmsg = "";
      YAPI.UpdateDeviceList(ref errmsg); // scan for devices list changes
    }

    private void btBeacon_Click(object sender, EventArgs e)
    {
      YModule m;
      if (!comboBox1.Enabled) return;
      m = (YModule)comboBox1.Items[comboBox1.SelectedIndex];
      if (!m.isOnline()) return;
      if (m.get_beacon() == YModule.BEACON_ON)
        m.set_beacon(YModule.BEACON_OFF);
      else
        m.set_beacon(YModule.BEACON_ON);
    }

    private void switchRelayOutput(int state)
    {
      YModule m;
      YRelay r;
      if (!comboBox1.Enabled) return;
      m = (YModule)comboBox1.Items[comboBox1.SelectedIndex];
      r = YRelay.FindRelay(m.get_serialNumber()  +".relay1");
      if (r.isOnline()) r.set_state(state);
    }

    private void btA_Click(object sender, EventArgs e)
    {
      switchRelayOutput(YRelay.STATE_A);
    }

    private void btB_Click(object sender, EventArgs e)
    {
        switchRelayOutput(YRelay.STATE_B);
    }

    private void refreshTimer_Tick(object sender, EventArgs e)
    {
      int index;
      YModule m;
      YRelay r;
      int state, beacon;
     
      m = null;
      index = 4;
      beacon = 0;
      state = 0;
      timerindex = (timerindex + 1) % 3;

      if (!comboBox1.Enabled)
      { setImage(4);
        return;
      }

      m = (YModule)comboBox1.Items[comboBox1.SelectedIndex];
      r = YRelay.FindRelay(m.get_serialNumber()+".relay1");
      if (r.isOnline())
      {   state  = r.get_state();
          beacon = m.get_beacon();

      } else
      {setImage(4);
        return;
      }

      if ((beacon == YModule.BEACON_ON) && (timerindex > 0)) beacon = YModule.BEACON_OFF;

      if  (state== YRelay.STATE_A)
        index = (beacon == YModule.BEACON_ON ? 1 : 0);
      else
        index = (beacon == YModule.BEACON_ON ? 3 : 2);

      setImage(index);
    }
  }
}
