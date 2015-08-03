using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
  class Program
  {
    static void usage()
    { string execname = System.AppDomain.CurrentDomain.FriendlyName;
      Console.WriteLine("Usage:");
      Console.WriteLine(execname+" <serial_number>");
      Console.WriteLine(execname+" <logical_name>");
      Console.WriteLine(execname+" any  ");
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;

      YGps gps;

      if (args.Length < 1) usage();
      target = args[0].ToUpper();

      // Setup the API to use local USB devices
      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS)
      {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY")
      {
        gps = YGps.FirstGps();

        if (gps == null)
        {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
      }
      else
      {
        gps = YGps.FindGps(target + ".temperature");
      }


      while (true)
      {
        if (!gps.isOnline())
        {
          Console.WriteLine("Module not connected (check identification and USB cable)");
          Environment.Exit(0);
        }
        if (gps.get_isFixed() != YGps.ISFIXED_TRUE)      
            Console.WriteLine("fixing... ");
        else
            Console.WriteLine(gps.get_latitude() + "  " + gps.get_longitude());
        Console.WriteLine("  (press Ctrl-C to exit)");

        YAPI.Sleep(1000, ref errmsg);
      }
    }
  }
}