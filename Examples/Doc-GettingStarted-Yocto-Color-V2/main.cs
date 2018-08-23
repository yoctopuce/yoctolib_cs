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
      Console.WriteLine(execname + " <serial_number>  [ color | rgb ]");
      Console.WriteLine(execname + " <logical_name> [ color | rgb ]");
      Console.WriteLine(execname + "  any  [ color | rgb ] ");
      Console.WriteLine("Eg.");
      Console.WriteLine(execname + " any FF1493 ");
      Console.WriteLine(execname + " YRGBLED1-123456 red");
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YColorLedCluster ledCluster;
      string color_str;
      int color;

      if (args.Length < 2) usage();

      target = args[0].ToUpper();
      color_str = args[1].ToUpper();

      if (color_str == "RED") color = 0xFF0000;
      else if (color_str == "GREEN") color = 0x00FF00;
      else if (color_str == "BLUE") color = 0x0000FF;
      else color = Convert.ToInt32("0x" + color_str, 16);

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        ledCluster = YColorLedCluster.FirstColorLedCluster();
        if (ledCluster == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
      } else {
        ledCluster = YColorLedCluster.FindColorLedCluster(target + ".colorLedCluster");
      }

      if (!ledCluster.isOnline()) {
        Console.WriteLine("Module not connected (check identification and USB cable)");
        Environment.Exit(0);
      }

      //configure led cluster
      int nb_leds = 2;
      ledCluster.set_activeLedCount(nb_leds);
      ledCluster.set_ledType(YColorLedCluster.LEDTYPE_RGB);

      // immediate transition for fist half of leds
      ledCluster.set_rgbColor(0, nb_leds / 2, color);
      // immediate transition for second half of leds
      ledCluster.rgb_move(nb_leds / 2, nb_leds / 2, color, 2000);

      YAPI.FreeAPI();
    }
  }
}