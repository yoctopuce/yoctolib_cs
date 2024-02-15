/*********************************************************************
 *
 *  $Id: main.cs 58233 2023-12-04 10:57:58Z seb $
 *
 *  An example that shows how to use a  Yocto-Thermocouple
 *
 *  You can find more information on our web site:
 *   Yocto-Thermocouple documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-thermocouple/doc.html
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
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;

      YTemperature tsensor;

      if (args.Length < 1) usage();
      target = args[0].ToUpper();

      // Setup the API to use local USB devices
      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        tsensor = YTemperature.FirstTemperature();

        if (tsensor == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
        Console.WriteLine("Using: " + tsensor.get_module().get_serialNumber());
      } else {
        tsensor = YTemperature.FindTemperature(target + ".temperature1");
      }

      // retreive module serial
      string serial = tsensor.get_module().get_serialNumber();

      // retreive both channels
      YTemperature ch1 = YTemperature.FindTemperature(serial + ".temperature1");
      YTemperature ch2 = YTemperature.FindTemperature(serial + ".temperature2");

      if (!ch2.isOnline()) {
        Console.WriteLine("Module not connected");
        Console.WriteLine("check identification and USB cable");
        Environment.Exit(0);
      }
      while (ch2.isOnline()) {
        Console.Write("Channel 1 : " + ch1.get_currentValue().ToString() + " °C  ");
        Console.Write("Channel 2 : " + ch2.get_currentValue().ToString() + " °C  ");
        Console.WriteLine("  (press Ctrl-C to exit)");
        YAPI.Sleep(1000, ref errmsg);
      }
      YAPI.FreeAPI();
    }
  }
}