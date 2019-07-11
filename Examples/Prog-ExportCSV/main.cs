using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
  class Program
  {
    static void Main(string[] args)
    {
      DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      string errmsg = "";

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(1);
      }

      // Enumerate all connected sensors
      List<YSensor> sensorList = new List<YSensor>();
      YSensor sensor;
      sensor = YSensor.FirstSensor();
      while(sensor != null) {
        sensorList.Add(sensor);
        sensor = sensor.nextSensor();
      }
      if (sensorList.Count == 0) {
        Console.WriteLine("No Yoctopuce sensor connected (check USB cable)");
        Environment.Exit(1);
      }

      // Generate consolidated CSV output for all sensors
      YConsolidatedDataSet data = new YConsolidatedDataSet(0, 0, sensorList);
      List<double> record = new List<double>();
      while (data.nextRecord(record) < 100) {
        string line = _epoch.AddSeconds(record[0]).ToString("yyyy-MM-ddTHH:mm:ss.fff");
        for(int i = 1; i < record.Count; i++) {
          line += String.Format(";{0:0.000}", record[i]);
        }
        Console.WriteLine(line);
      }

      YAPI.FreeAPI();
    }
  }
}
