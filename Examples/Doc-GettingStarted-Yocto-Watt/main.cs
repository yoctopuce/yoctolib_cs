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
      string exename = System.AppDomain.CurrentDomain.FriendlyName;
      Console.WriteLine("Usage:");
      Console.WriteLine(exename + " <serial_number>");
      Console.WriteLine(exename + " <logical_name>");
      Console.WriteLine(exename + " any  ");
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;

      YPower psensor;

      if (args.Length < 1) usage();
      target = args[0].ToUpper();

      // Setup the API to use local USB devices
      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        psensor = YPower.FirstPower();

        if (psensor == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
      } else {
        psensor = YPower.FindPower(target + ".power");
      }

      if (!psensor.isOnline()) {
        Console.WriteLine("Module not connected (check identification and USB cable)");
        Environment.Exit(0);
      }

      while (psensor.isOnline()) {
        Console.WriteLine("Current power: " + psensor.get_currentValue().ToString()
                          + " W");
        Console.WriteLine("  (press Ctrl-C to exit)");

        YAPI.Sleep(1000, ref errmsg);
      }
      YAPI.FreeAPI();
    }
  }
}