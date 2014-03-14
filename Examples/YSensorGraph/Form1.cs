using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace WindowsFormsApplication1
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // used to compute the graph length and time label format
        private double FirstPointDate;
        private double LastPointDate;


        // form contents init
        private void Form1_Load(object sender, EventArgs e)
        {
            // we wanna know when device list changes
            YAPI.RegisterDeviceArrivalCallback(deviceArrival);
            YAPI.RegisterDeviceRemovalCallback(deviceRemoval);
            InventoryTimer.Interval = 500;
            InventoryTimer.Start();
            RefreshTimer.Interval = 500;
            RefreshTimer.Start();
        }


        // MS doesn't seem to like UNIX timestamps, we have to do the convertion ourself :-)
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        // update the UI according to the sensors count
        public void setSensorCount()
        {
            if (comboBox1.Items.Count <= 0) Status.Text = "No sensor found, check USB cable";
            else if (comboBox1.Items.Count == 1) Status.Text = "One sensor found";
            else  Status.Text = comboBox1.Items.Count+" sensors found";
            if (comboBox1.Items.Count<=0) chart1.Visible = false;
            ConnectPlz.Visible = comboBox1.Items.Count <= 0;
            Application.DoEvents();

        }

        // automatically called each time a new yoctopuce device is plugged
        public void deviceArrival(YModule m)
        {   // new device just arrived, lets enumerate all sensors and
            // add the one missing to the combobox           
            YSensor s = YSensor.FirstSensor();
            while (s != null)
            {
                if (!comboBox1.Items.Contains(s))
                {
                   int index =  comboBox1.Items.Add(s);
                }
                s = s.nextSensor();
            }
            comboBox1.Enabled = comboBox1.Items.Count>0;
            if ((comboBox1.SelectedIndex<0) && (comboBox1.Items.Count>0))
               comboBox1.SelectedIndex=0;

            setSensorCount();
        }

        // automatically called each time a new yoctopuce device is unplugged
        public void deviceRemoval(YModule m)
        {   // a device vas just removed, lets remove the offline sensors
            // from the combo box
            for (int i = comboBox1.Items.Count - 1; i >= 0; i--)
            {
                if (!((YSensor)comboBox1.Items[i]).isOnline())
                    comboBox1.Items.RemoveAt(i);
            }
            setSensorCount();
        }

        // automatically called on a regular basis with sensor value
        public void newSensorValue(YFunction f,  YMeasure m)
        {   double t = m.get_endTimeUTC();
            chart1.Series[0].Points.AddXY(UnixTimeStampToDateTime(t), m.get_averageValue());
            if (FirstPointDate<0)  FirstPointDate=t ;
            LastPointDate = t;
            setGraphScale();
        }
        
        // will force a new an USB device inventory 
        private void InventoryTimer_Tick(object sender, EventArgs e)
        {   string errmsg="";
            YAPI.UpdateDeviceList(ref errmsg);
        }

        // returns the sensor selected in the combobox
        private YSensor getSelectedSensor()
        {  int index= comboBox1.SelectedIndex;
            if (index<0) return null;
            return  (YSensor)comboBox1.Items[index];
        }
     
        // update the datalogger control buttons
        private void refreshDatloggerButton(YSensor s)
        {   if (s != null)
            {   YModule m = s.get_module();  // get the module harboring the sensor
                YDataLogger dtl = YDataLogger.FindDataLogger(m.get_serialNumber() + ".dataLogger");
                if (dtl.isOnline())
                {   if (dtl.get_recording() == YDataLogger.RECORDING_ON)
                    {   RecordButton.Enabled = false;
                        PauseButton.Enabled = true;
                        DeleteButton.Enabled = false;
                        return;
                    }
                    else
                    {   RecordButton.Enabled = true;
                        PauseButton.Enabled = false;
                        DeleteButton.Enabled = true;
                        return;
                    }
                }
            }
            RecordButton.Enabled = false;
            PauseButton.Enabled = false;
            DeleteButton.Enabled = false;
        }

        // update the date labels format according to graph length
        private void setGraphScale()
        {   int count = chart1.Series[0].Points.Count;
            if (count > 0)
            {   double total = LastPointDate - FirstPointDate;
                if (total < 180) chart1.ChartAreas[0].AxisX.LabelStyle.Format = "H:mm:ss";
                else if (total < 3600) chart1.ChartAreas[0].AxisX.LabelStyle.Format = "H:mm";
                else if (total < 3600 * 24) chart1.ChartAreas[0].AxisX.LabelStyle.Format = "h:mm";
                else if (total < 3600 * 24 * 7) chart1.ChartAreas[0].AxisX.LabelStyle.Format = "ddd H";
                else if (total < 3600 * 24 * 30) chart1.ChartAreas[0].AxisX.LabelStyle.Format = "dd-MMM";
                else chart1.ChartAreas[0].AxisX.LabelStyle.Format = "MMM";
            }
            else chart1.ChartAreas[0].AxisX.LabelStyle.Format = "mm:ss";
        }

        // clear the graph
        private void clearGraph()
        {
            chart1.Series[0].XValueType = ChartValueType.DateTime;
            chart1.Series[0].Points.SuspendUpdates();
            //chart1.Series[0].Points.Clear();  indecently slow
            while (chart1.Series[0].Points.Count > 0)
                chart1.Series[0].Points.RemoveAt(chart1.Series[0].Points.Count - 1);
            chart1.Series[0].Points.ResumeUpdates();
        }

        // the core function :  load data from datalogger to send it to the graph
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // lets hide the graph wgile updating
            chart1.Visible=false;
            comboBox1.Enabled = false;
              

            // remove any previous timed report call back 
            for (int i = 0; i < comboBox1.Items.Count; i++)
              ((YSensor)comboBox1.Items[i]).registerTimedReportCallback(null);

            // allow zooming
            chart1.ChartAreas[0].CursorX.Interval = 0.001;
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].CursorX.AutoScroll = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;

            int index = comboBox1.SelectedIndex;
            if (index >= 0) clearGraph();
          

           YSensor s= getSelectedSensor();
           if (s != null)
           {   FirstPointDate=-1;
               LastPointDate=-1;
               // some ui control
               loading.Visible=true;
               refreshDatloggerButton(null);
               progressBar.Visible = true;
               Status.Text = "Loading data from datalogger...";
               for (int i = 0; i < 100;i++ ) Application.DoEvents(); // makes sure the UI changes are repainted
               
               // load data from datalogger
               YDataSet data = s.get_recordedData(0, 0);
               int progress = data.loadMore();
               while (progress < 100)
               {
                   try {
                       progressBar.Value = progress;
                   } catch { return;}
                   
                   Application.DoEvents();
                   progress = data.loadMore();
               }

               // sets the unit (because ° is not a ASCII-128  character, Yoctopuce temperature
               // sensors report unit as 'C , so we fix it).
               chart1.ChartAreas[0].AxisY.Title = s.get_unit().Replace("'C","°C");
               chart1.ChartAreas[0].AxisY.TitleFont = new Font("Arial", 12, FontStyle.Regular);

               // send the data to the graph
               List<YMeasure> alldata = data.get_measures();
               for (int i = 0; i < alldata.Count; i++)
               {   
                   chart1.Series[0].Points.AddXY(UnixTimeStampToDateTime(alldata[i].get_endTimeUTC()), alldata[i].get_averageValue());

               }

               // used to compute graph length
               if (alldata.Count>0)
               {
                   FirstPointDate = alldata[0].get_endTimeUTC();
                   LastPointDate = alldata[alldata.Count - 1].get_endTimeUTC();

               }
               setGraphScale();
               
               // restore UI
               comboBox1.Enabled=true;
               progressBar.Visible = false;
               setSensorCount();
               s.set_reportFrequency("3/s");
               s.registerTimedReportCallback(newSensorValue);
               loading.Visible = false;
               chart1.Visible=true;
               refreshDatloggerButton(s);

           }
        }

        // will cause Timed report to pop
        private void refreshTimer_Tick(object sender, EventArgs e)
        {  
            string errmsg = "";
            YAPI.HandleEvents(ref errmsg);
        }

        // Datalogger buttons handling
        private void DataLoggerButton_Click(object sender, EventArgs e)
        {   YSensor s = getSelectedSensor();
            if (s != null)
            {   YModule m = s.get_module();  // get the module harboring the sensor
                YDataLogger dtl = YDataLogger.FindDataLogger(m.get_serialNumber() + ".dataLogger");
                if (dtl.isOnline())
                { if (sender==RecordButton) dtl.set_recording(YDataLogger.RECORDING_ON);
                  if (sender == PauseButton) dtl.set_recording(YDataLogger.RECORDING_OFF);
                  if (sender == DeleteButton)
                  {   dtl.set_recording(YDataLogger.RECORDING_OFF);
                      MessageBox.Show("clear");
                      dtl.forgetAllDataStreams();
                      clearGraph();
                  }             
                }
            }
            refreshDatloggerButton(s);
        }
    }
}
