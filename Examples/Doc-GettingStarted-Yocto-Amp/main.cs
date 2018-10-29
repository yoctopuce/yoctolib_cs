/*********************************************************************
 *
 *  $Id: main.cs 32621 2018-10-10 13:10:25Z seb $
 *
 *  An example that show how to use a  Yocto-Amp
 *
 *  You can find more information on our web site:
 *   Yocto-Amp documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-amp/doc.html
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
      Console.WriteLine("Usage");
      Console.WriteLine(execname + " <serial_number>");
      Console.WriteLine(execname + " <logical_name>");
      Console.WriteLine(execname + " any  ");
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void die(string msg)
    {
      Console.WriteLine(msg + " (check USB cable) ");
      Environment.Exit(0);

    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YCurrent sensor;
      YCurrent sensorDC = null;
      YCurrent sensorAC = null;
      YModule m = null;

      if (args.Length < 1) usage();
      target = args[0].ToUpper();

      // Setup the API to use local USB devices
      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        // retreive any voltage sensor (can be AC or DC)
        sensor = YCurrent.FirstCurrent();
        if (sensor == null) die("No module connected");

      } else sensor = YCurrent.FindCurrent(target + ".current1");

      // we need to retreive both DC and AC voltage from the device.
      if (sensor.isOnline()) {
        m = sensor.get_module();
        sensorDC = YCurrent.FindCurrent(m.get_serialNumber() + ".current1");
        sensorAC = YCurrent.FindCurrent(m.get_serialNumber() + ".current2");
      } else die("Module not connected");

      if (!m.isOnline())
        die("Module not connected");

      while (m.isOnline()) {
        Console.Write("DC: " + sensorDC.get_currentValue().ToString() + " mA ");
        Console.Write("AC: " + sensorAC.get_currentValue().ToString() + " mA ");
        Console.WriteLine("  (press Ctrl-C to exit)");

        YAPI.Sleep(1000, ref errmsg);
      }
      YAPI.FreeAPI();
    }
  }
}