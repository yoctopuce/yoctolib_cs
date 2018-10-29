/*********************************************************************
 *
 *  $Id: main.cs 32713 2018-10-19 15:30:53Z seb $
 *
 *  An example that show how to use a  Yocto-RangeFinder
 *
 *  You can find more information on our web site:
 *   Yocto-RangeFinder documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-rangefinder/doc.html
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
      Console.WriteLine(execname + " <serial_number>");
      Console.WriteLine(execname + " <logical_name>");
      Console.WriteLine(execname + " any  ");
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YRangeFinder rf;
      YLightSensor ir;
      YTemperature tmp;

      if (args.Length < 1) usage();
      target = args[0].ToUpper();

      // Setup the API to use local USB devices
      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        rf = YRangeFinder.FirstRangeFinder();
        if (rf == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
        target = rf.get_module().get_serialNumber();
      } else rf = YRangeFinder.FindRangeFinder(target + ".rangefinder1");

      if (!rf.isOnline()) {
        Console.WriteLine("Module not connected");
        Console.WriteLine("check identification and USB cable");
        Environment.Exit(0);
      }
      ir = YLightSensor.FindLightSensor(target + ".lightSensor1");
      tmp = YTemperature.FindTemperature(target + ".temperature1");

      while (rf.isOnline()) {
        Console.WriteLine("Distance    : " + rf.get_currentValue().ToString());
        Console.WriteLine("Ambiant IR  : " + ir.get_currentValue().ToString());
        Console.WriteLine("Temperature : " + tmp.get_currentValue().ToString());
        Console.WriteLine("  (press Ctrl-C to exit)");
        YAPI.Sleep(1000, ref errmsg);
      }
      YAPI.FreeAPI();
    }
  }
}