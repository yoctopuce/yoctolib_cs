using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace ConsoleApplication1
{
  class Program
  {
    static void usage()
    { string errmsg  = "";
      string  exe = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
      Console.WriteLine("Bad command line arguments");
      Console.WriteLine(exe + " <serial_number>  [ color | rgb ]");
      Console.WriteLine(exe + " <logical_name> [ color | rgb ]");
      Console.WriteLine(exe + "   any  [ color | rgb ] ");
      Console.WriteLine("Eg.");
      Console.WriteLine(exe + " any FF1493 ");
      Console.WriteLine(exe + " YRGBHI01-123456 red");
      YAPI.Sleep(2500,ref errmsg);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    { string errmsg = "";
      string target;
      YColorLed led1;
     
      string color_str;
      int color;

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS)
      {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (args.Length < 2) usage();

      target = args[0].ToUpper();
      color_str = args[1].ToUpper();

      if (color_str == "RED") color = 0xFF0000;
      else if (color_str == "GREEN") color = 0x00FF00;
      else if (color_str == "BLUE") color = 0x0000FF;
      else color = Convert.ToInt32("0x" + color_str, 16);

      if (target == "ANY")
      {
        led1 = YColorLed.FirstColorLed();
        if (led1 == null)
        {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
       }
      else
      {
        led1 = YColorLed.FindColorLed(target + ".colorLed1");
      }


      if (led1.isOnline())
      {
        led1.rgbMove(color, 1000); // smooth transition  
      }
      else
        Console.WriteLine("Module not connected (check identification and USB cable)");

    }
  }
}
