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
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;

      YMultiCellWeighScale sensor;

      if (args.Length < 1) usage();
      target = args[0].ToUpper();

      // Setup the API to use local USB devices
      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        sensor = YMultiCellWeighScale.FirstMultiCellWeighScale();
        if (sensor == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }

        Console.WriteLine("Using: " + sensor.get_module().get_serialNumber());
      } else {
        sensor = YMultiCellWeighScale.FindMultiCellWeighScale(target + ".multiCellWeighScale");
      }

      if (sensor.isOnline()) {
        string unit = "";
        // On startup, enable excitation and tare weigh scale
        Console.WriteLine("Resetting tare weight...");
        sensor.set_excitation(YMultiCellWeighScale.EXCITATION_AC);
        YAPI.Sleep(3000, ref errmsg);
        sensor.tare();
        unit = sensor.get_unit();

        // Show measured weight continuously
        while (sensor.isOnline()) {
          Console.Write("Weight : " + sensor.get_currentValue().ToString() + unit);
          Console.WriteLine("  (press Ctrl-C to exit)");
          YAPI.Sleep(1000, ref errmsg);
        }
      }

      YAPI.FreeAPI();
      Console.WriteLine("Module not connected (check identification and USB cable)");
    }
  }
}