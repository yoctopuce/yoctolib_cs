using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
  class Program
  {
    static void dumpSensor(YSensor sensor)
    {
      string fmt = "dd MMM yyyy hh:mm:ss,fff";
      Console.WriteLine("Using DataLogger of " + sensor.get_friendlyName());
      YDataSet dataset = sensor.get_recordedData(0, 0);
      Console.WriteLine("loading summary... ");
      dataset.loadMore();
      YMeasure summary = dataset.get_summary();
      String line = String.Format("from {0} to {1} : min={2:0.00}{5} avg={3:0.00}{5}  max={4:0.00}{5}",
                                  summary.get_startTimeUTC_asDateTime().ToString(fmt), summary.get_endTimeUTC_asDateTime().ToString(fmt), summary.get_minValue(), summary.get_averageValue(),  summary.get_maxValue(), sensor.get_unit());
      Console.WriteLine(line);
      Console.Write("loading details :   0%");
      int progress = 0;
      do {
        progress = dataset.loadMore();
        Console.Write(String.Format("\b\b\b\b{0,3:##0}%", progress));
      } while(progress < 100);
      Console.WriteLine("");
      List<YMeasure> details = dataset.get_measures();
      foreach (YMeasure m in details) {
        Console.WriteLine(String.Format("from {0} to {1} : min={2:0.00}{5} avg={3:0.00}{5}  max={4:0.00}{5}",
                                        m.get_startTimeUTC_asDateTime().ToString(fmt), m.get_endTimeUTC_asDateTime().ToString(fmt), m.get_minValue(),  m.get_averageValue(), m.get_maxValue(), sensor.get_unit()));
      }
    }

    static void Main(string[] args)
    {
      string errmsg = "";

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(1);
      }

      YSensor sensor;
      if (args.Length == 0 || args[0] == "any") {
        sensor = YSensor.FirstSensor();
        if (sensor == null) {
          Console.WriteLine("No module connected (check USB cable)");
          Environment.Exit(1);
        }
      } else {
        sensor = YSensor.FindSensor(args[0]);
        if (!sensor.isOnline()) {
          Console.WriteLine("Sensor " + sensor + " is not connected (check USB cable)");
          Environment.Exit(1);
        }
      }
      dumpSensor(sensor);
      YAPI.FreeAPI();
      Console.WriteLine("Done. Have a nice day :)");
    }
  }
}
