/*********************************************************************
 *
 *  $Id: main.cs 65680 2025-04-09 07:07:57Z tiago $
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
      YColorSensor colorSensor;

        if (args.Length < 1)
        usage();
      target = args[0].ToUpper();

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        colorSensor = YColorSensor.FirstColorSensor();
        if (colorSensor == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
        target = colorSensor.get_module().get_serialNumber();
      }

      colorSensor = YColorSensor.FindColorSensor(target + ".colorSensor");
      if (colorSensor.isOnline()) {
        colorSensor.set_workingMode(YColorSensor.WORKINGMODE_AUTO); // Working Mode Auto
        colorSensor.set_estimationModel(YColorSensor.ESTIMATIONMODEL_REFLECTION); // Estimation model Reflexion
        while (colorSensor.isOnline()) 
        {
            Console.WriteLine("Near color: " + colorSensor.get_nearSimpleColor());
            Console.WriteLine("RGB HEX : #" + colorSensor.get_estimatedRGB().ToString("x6"));
            Console.WriteLine("---------------------------");
            // wait 5 sec to show the output
            YAPI.Sleep(5000, ref errmsg);
        }
        
      } else {
        Console.WriteLine("Module not connected");
        Console.WriteLine("check identification and USB cable");
      }
      YAPI.FreeAPI();
    }
  }
}
