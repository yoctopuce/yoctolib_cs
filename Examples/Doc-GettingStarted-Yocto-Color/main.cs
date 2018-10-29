/*********************************************************************
 *
 *  $Id: main.cs 32713 2018-10-19 15:30:53Z seb $
 *
 *  An example that show how to use a  Yocto-Color
 *
 *  You can find more information on our web site:
 *   Yocto-Color documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-color/doc.html
 *   C# API Reference:
 *      https://www.yoctopuce.com/EN/doc/reference/yoctolib-cs-EN.html
 *
 *********************************************************************/

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
      YColorLed led1;
      YColorLed led2;
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
        led1 = YColorLed.FirstColorLed();
        if (led1 == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }

        led2 = led1.nextColorLed();
      } else {
        led1 = YColorLed.FindColorLed(target + ".colorLed1");
        led2 = YColorLed.FindColorLed(target + ".colorLed2");
      }

      if (led1.isOnline()) {
        led1.set_rgbColor(color);// immediate switch
        led2.rgbMove(color, 1000); // smooth transition
      } else {
        Console.WriteLine("Module not connected");
        Console.WriteLine("check identification and USB cable");
      }
      YAPI.FreeAPI();
    }
  }
}
