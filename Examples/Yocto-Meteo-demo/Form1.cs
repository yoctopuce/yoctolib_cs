using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;



namespace WindowsFormsApplication1
{

  

    public partial class Form1 : Form
    {

        public struct GraphValue
        {
            public DataPoint temperature;
            public DataPoint humidity;
            public DataPoint pressure;

        }


        public class Stream {
            public List<GraphValue> values;

            public int Interval;
            public DateTime StartTime;
            public Stream(int interval, DateTime start)
            {
                Interval = interval;
                StartTime = start;
                values = new List<GraphValue>();
            }
        }



       
        private YDataLogger meteoLogger=null;
        private BindingSource bindingSource = new BindingSource();
        DataTable displayedData = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }



        private void StartRecording(YDataLogger dataLogger)
        {
            // ensure that the record will start automaticaly if the 
            // device reboot
            dataLogger.set_autoStart(YDataLogger.AUTOSTART_ON);
            dataLogger.module().saveToFlash();
            // update device utctime
            DateTime now = DateTime.UtcNow;
            TimeSpan span = (now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            dataLogger.set_timeUTC((int)span.TotalSeconds);
            // ensure the device is recording
            dataLogger.set_recording(YDataLogger.RECORDING_ON);
        }

        private void log(string logstr)
        {
            //DateTime now= DateTime.Now;
            //logMessage.AppendText(now.Hour+":"+now.Minute+":"+now.Second+": "+logstr+"\n");
        }

        private void DevicesPlugUnplug(YModule m)
        {
            YDataLogger logger;

            logger = YDataLogger.FirstDataLogger();
            if (logger == null)
            {
                toolStripStatusLabel1.Text = "Please connect a Yocto-Meteo and a Yocto-Light device";
                meteoLogger =null;
                return;
            }
            while (logger != null)
            {
                log("dump logger:" + logger.module().get_serialNumber());
                switch (logger.module().get_productName())
                {                    
                    case "Yocto-Meteo":
                        if (meteoLogger == null)
                        {
                            meteoLogger = logger;
                            StartRecording(meteoLogger);
                        }
                        break;
                    default:
                        break;
                }
                logger = logger.nextDataLogger();
            }
        }

        private DataTable CreateNewDataTable()
        {

            DataTable table = new DataTable();
            table.Columns.Add("Time", typeof(DateTime));
            table.Columns.Add("Temperature", typeof(double));
            table.Columns.Add("Humidity", typeof(double));
            table.Columns.Add("Pressure", typeof(double));
            table.DefaultView.Sort = "Time";
            return table;
        }


      
        private void Form1_Load(object sender, EventArgs e)
        {
            string errmsg = "";

            // No exception please
            YAPI.DisableExceptions();

            if (YAPI.RegisterHub("172.17.17.53", ref errmsg) != YAPI.SUCCESS)
            {
                MessageBox.Show(errmsg);
                statusbar.Text = "RegisterHub error: " + errmsg;
            }
            DevicesPlugUnplug(null);
            YAPI.RegisterDeviceArrivalCallback(DevicesPlugUnplug);
            YAPI.RegisterDeviceRemovalCallback(DevicesPlugUnplug);
            timer1.Enabled = true;
            backgroundWorker1.RunWorkerAsync(meteoLogger);


            displayedData = CreateNewDataTable();
            foreach (DataColumn col in displayedData.Columns)
            {
                serieBox1.Items.Add(col.ToString());
                serieBox2.Items.Add(col.ToString());
            }

            // Bind the list to the BindingSource.
            this.bindingSource.DataSource = displayedData;


            displayedData.Rows.Add(DateTime.Now,23.3,80,900);
            displayedData.Rows.Add(DateTime.Now.AddMinutes(-3), 24.3, 70, 1000);
            displayedData.Rows.Add(DateTime.Now.AddMinutes(-4), 25.3, 60, 900);

            // Attach the BindingSource to the DataGridView.
            rawDataView.DataSource = this.bindingSource;
            chart1.DataSource = this.bindingSource;
            
            chart1.AlignDataPointsByAxisLabel();
            chart1.Series.Add("Serie 2");
            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.Series[1].ChartType = SeriesChartType.Line;
            chart1.ChartAreas[0].AxisX.IsStartedFromZero = false;
            chart1.ChartAreas[0].AxisX.MaximumAutoSize = 98;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm";
            chart1.Series[1].YAxisType = AxisType.Secondary;

            SetAxis(this.chart1.Series[0], "Temperature");
            serieBox1.SelectedItem="Temperature";
            SetAxis(this.chart1.Series[1],"Humidity");
            serieBox2.SelectedItem="Humidity";
            
        }

        private void SetAxis(Series serie,string col)
        {
            serie.XValueMember = "Time";
            serie.IsXValueIndexed = false;
            serie.YValueMembers = col;
            serie.LegendText = col;
            serie.XValueType = ChartValueType.DateTime;

        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            string errmsg = "";
            YAPI.UpdateDeviceList(ref errmsg);
            YAPI.HandleEvents(ref errmsg);
        }

   


        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(meteoLogger);
        }

  
       

        private void RefeshDataSource(DataTable table)
        {
            this.bindingSource.DataSource = table;
            this.bindingSource.ResetBindings(true);
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box =(CheckBox)sender;
            if (box.Checked)
            {
                chart1.Visible = false;
                rawDataView.Visible = true;
            }
            else
            {
                chart1.Visible = true;
                rawDataView.Visible = false;
            }
          
        }

        private void SeriesBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            ComboBox box = (ComboBox)sender;
            string col = (string)box.SelectedItem;
            SetAxis(this.chart1.Series[1], col);
            chart1.DataBind();
        }

        private void serieBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            string col = (string)box.SelectedItem;
            SetAxis(this.chart1.Series[0],col);
            chart1.DataBind();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            timer2.Enabled = false;
            YDataLogger dataLogger = e.Argument as YDataLogger;
            if (dataLogger == null)
                return;

            string msg ="Loading " + dataLogger.module();
            backgroundWorker1.ReportProgress(0, msg);
            Stopwatch full = new Stopwatch();
            Stopwatch load = new Stopwatch();

            full.Start();

            List<YDataStream> dataStreams = new List<YDataStream>();
            dataLogger.get_dataStreams(dataStreams);

            log("Parsing " + dataStreams.Count);
            if (dataStreams.Count == 0)
                return;
            // create new
            DataTable table = CreateNewDataTable();
            int startfrom = dataStreams.Count - 1;

            while (startfrom >= 0)
            {
                YDataStream stream = dataStreams[startfrom];
                long unixtime = stream.get_startTimeUTC();
                if (unixtime != 0)
                {
                    DateTime t = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(unixtime);
                    TimeSpan delta = DateTime.Now - t;
                    if (delta.TotalDays > 2)
                    {
                        // we have reach the oldest value
                        // restart form there
                        break;
                    }
                }
                startfrom--;
            }
            if (startfrom == -1)
                startfrom = 0;

            //log("we have to load " + (dataStreams.Count - startfrom) + " streams");
            backgroundWorker1.ReportProgress(0, "we have to load " + (dataStreams.Count - startfrom) + " streams");
            
            for (int i = startfrom; i < dataStreams.Count; i++)
            {
                YDataStream stream = dataStreams[i];
                int progress = (i - startfrom) * 100 / (dataStreams.Count - startfrom);
                backgroundWorker1.ReportProgress(progress, "Loading " + dataLogger.module());
                // drop data that do not have a timestamp
                long unixtime = stream.get_startTimeUTC();
                if (unixtime == 0)
                    continue;

                long utc = stream.get_startTimeUTC();
                DateTime tstart = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(utc);
                log("parse " + stream.get_runIndex() + ":" + stream.get_startTime());


           

                List<String> names = stream.get_columnNames();
                int increment = 1;
                if (stream.get_dataSamplesInterval() == 1)
                {
                    // we take only one mesure per minute
                    increment = 60;
                }

                for (int row = 0; row < stream.get_rowCount(); row += increment)
                {
                    long nbsec = unixtime + row * stream.get_dataSamplesInterval();
                    DateTime t = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(nbsec);
                    TimeSpan delta = DateTime.Now - t;
                    if (delta.TotalDays > 2)
                    {
                        continue;
                    }
                    DataRow rowData = table.NewRow();
                    rowData["Time"] = t;
                    for (int c = 0; c < stream.get_columnCount(); c++)
                    {
                        switch (names[c])
                        {
                            case "temperature":
                                rowData["Temperature"] = stream.get_data(row, c);
                                break;
                            case "pressure":
                                rowData["Pressure"] = stream.get_data(row, c);
                                break;
                            case "humidity":
                                rowData["Humidity"] = stream.get_data(row, c);
                                break;
                            default:
                                continue;
                        }
                    }
                    table.Rows.Add(rowData);
                }
            }
            log("Parsing done");
            log("refresh done");
            full.Stop();
            TimeSpan full_ts = full.Elapsed;
            TimeSpan load_ts = load.Elapsed;
            string full_str = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
              full_ts.Hours, full_ts.Minutes, full_ts.Seconds,
              full_ts.Milliseconds / 10);
            string load_str = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                load_ts.Hours, load_ts.Minutes, load_ts.Seconds,
                load_ts.Milliseconds / 10);
            log("load=" + load_str);
            log("full=" + full_str);
            e.Result = table;

        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Value = e.ProgressPercentage;
            toolStripStatusLabel1.Text = e.UserState as String;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RefeshDataSource((DataTable)e.Result);
            toolStripProgressBar1.Value = 100;
            toolStripStatusLabel1.Text = "Loading done";
            toolStripProgressBar1.Visible = false;
            chart1.DataBind();
            timer2.Enabled = true;
            
            // Format and display the TimeSpan value.
          

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
              backgroundWorker1.RunWorkerAsync(meteoLogger);
        }

  
    }
}
