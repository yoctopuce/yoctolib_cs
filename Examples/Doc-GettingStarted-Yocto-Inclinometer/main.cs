/*********************************************************************
 *
 *  $Id: main.cs 58233 2023-12-04 10:57:58Z seb $
 *
 *  An example that shows how to use a  Yocto-Inclinometer
 *
 *  You can find more information on our web site:
 *   Yocto-Inclinometer documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-inclinometer/doc.html
 *   C# V2 API Reference:
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
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;

      YTilt anytilt, tilt1, tilt2 , tilt3;

      if (args.Length < 1)
        usage();
      target = args[0].ToUpper();

      // Setup the API to use local USB devices
      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        anytilt = YTilt.FirstTilt();
        if (anytilt == null) {
          Console.WriteLine("No module connected (check USB cable)");
          Environment.Exit(0);
        }
      } else {
        anytilt = YTilt.FindTilt(target + ".tilt1");
        if (!anytilt.isOnline()) {
          Console.WriteLine("Module not connected");
          Console.WriteLine("check identification and USB cable");
          Environment.Exit(0);
        }
      }

      string serial = anytilt.get_module().get_serialNumber();
      tilt1 = YTilt.FindTilt(serial + ".tilt1");
      tilt2 = YTilt.FindTilt(serial + ".tilt2");
      tilt3 = YTilt.FindTilt(serial + ".tilt3");

      int count = 0;

      if (!tilt1.isOnline()) {
        Console.WriteLine("device disconnected");
        Environment.Exit(0);
      }

      while (tilt1.isOnline()) {

        if (count % 10 == 0) Console.WriteLine("tilt1\ttilt2\ttilt3");

        Console.Write(tilt1.get_currentValue().ToString() + "\t");
        Console.Write(tilt2.get_currentValue().ToString() + "\t");
        Console.Write(tilt3.get_currentValue().ToString());

        YAPI.Sleep(250, ref errmsg);
      }
      YAPI.FreeAPI();
    }
  }
}