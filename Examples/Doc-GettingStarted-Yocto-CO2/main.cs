using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
  class Program
  {
    static void usage()
    {
      string execname = System.AppDomain.CurrentDomain.FriendlyName;
      Console.WriteLine(execname + " <serial_number>");
      Console.WriteLine(execname + " <logical_name>");
      Console.WriteLine(execname + " any  ");
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;

      YCarbonDioxide co2sensor;

      if (args.Length < 1) usage();
      target = args[0].ToUpper();

      // Setup the API to use local USB devices
      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        co2sensor = YCarbonDioxide.FirstCarbonDioxide();

        if (co2sensor == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
        Console.WriteLine("using " + co2sensor.get_module().get_serialNumber());
      } else {
        co2sensor = YCarbonDioxide.FindCarbonDioxide(target + ".carbonDioxide");
      }


      if (!co2sensor.isOnline()) {
        Console.WriteLine("Module not connected (check identification and USB cable)");
        Environment.Exit(0);
      }
      while (co2sensor.isOnline()) {
        Console.WriteLine("CO2: " + co2sensor.get_currentValue().ToString() + " ppm");
        Console.WriteLine("  (press Ctrl-C to exit)");

        YAPI.Sleep(1000, ref errmsg);
      }
      YAPI.FreeAPI();
    }
  }
}