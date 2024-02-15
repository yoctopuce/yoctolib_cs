/*********************************************************************
 *
 *  $Id: main.cs 58233 2023-12-04 10:57:58Z seb $
 *
 *  An example that shows how to use a  Yocto-Proximity
 *
 *  You can find more information on our web site:
 *   Yocto-Proximity documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-proximity/doc.html
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
      YLightSensor ir, al;
      YProximity p;

      if (args.Length < 1) usage();
      target = args[0].ToUpper();

      // Setup the API to use local USB devices
      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        p = YProximity.FirstProximity();
        if (p == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
        target = p.get_module().get_serialNumber();
      } else p = YProximity.FindProximity(target + ".proximity1");

      if (!p.isOnline()) {
        Console.WriteLine("Module not connected");
        Console.WriteLine("check identification and USB cable");
        Environment.Exit(0);
      }
      al = YLightSensor.FindLightSensor(target + ".lightSensor1");
      ir = YLightSensor.FindLightSensor(target + ".lightSensor2");

      while (p.isOnline()) {
        Console.Write(" Proximity: " + p.get_currentValue().ToString()  );
        Console.Write(" Ambiant: "  + al.get_currentValue().ToString());
        Console.Write(" IR: "       + ir.get_currentValue().ToString());

        Console.WriteLine("  (press Ctrl-C to exit)");
        YAPI.Sleep(1000, ref errmsg);
      }
      YAPI.FreeAPI();
    }
  }
}