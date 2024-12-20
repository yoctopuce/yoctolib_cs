/*********************************************************************
 *
 *  $Id: main.cs 63788 2024-12-19 08:59:45Z seb $
 *
 *  An example that shows how to use a Yocto-Spectral
 *
 *  You can find more information on our web site:
 *   Yocto-Spectral documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-spectral/doc.html
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
      Console.WriteLine(execname + " any            (use any discovered device)");
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YSpectralSensor spectralSensor;

      if (args.Length < 1)
        usage();
      target = args[0].ToUpper();

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        spectralSensor = YSpectralSensor.FirstSpectralSensor();
        if (spectralSensor == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
        target = spectralSensor.get_module().get_serialNumber();
      }

      spectralSensor = YSpectralSensor.FindSpectralSensor(target + ".spectralSensor");
      if (spectralSensor.isOnline()) {
        spectralSensor.set_gain(6);
        spectralSensor.set_integrationTime(150);
        spectralSensor.set_ledCurrent(6);

        Console.WriteLine("Current near estimated HTML color: " + spectralSensor.get_nearHTMLColor());
      } else {
        Console.WriteLine("Module not connected");
        Console.WriteLine("check identification and USB cable");
      }
      YAPI.FreeAPI();

      // wait 5 sec to show the output
      ulong now = YAPI.GetTickCount();
      while (YAPI.GetTickCount() - now < 5000);
    }
  }
}
