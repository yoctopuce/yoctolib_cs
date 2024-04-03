/*********************************************************************
 *
 *  $Id: main.cs 60035 2024-03-20 09:56:43Z seb $
 *
 *  An example that shows how to use a  Yocto-SDI12
 *
 *  You can find more information on our web site:
 *   Yocto-SDI12 documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-sdi12/doc.html
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
      YSdi12Port sdi12Port;

      if (args.Length < 1)
        usage();
      target = args[0].ToUpper();

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        sdi12Port = YSdi12Port.FirstSdi12Port();
        if (sdi12Port == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
        target = sdi12Port.get_module().get_serialNumber();
      }

      sdi12Port = YSdi12Port.FindSdi12Port(target + ".sdi12Port");
      if (sdi12Port.isOnline()) {
        sdi12Port.reset();
        YSdi12SensorInfo singleSensor = sdi12Port.discoverSingleSensor();
        Console.WriteLine("Sensor address : " + singleSensor.get_sensorAddress());
        Console.WriteLine("Sensor SDI-12 compatibility : " + singleSensor.get_sensorProtocol());
        Console.WriteLine("Sensor company name : " + singleSensor.get_sensorVendor());
        Console.WriteLine("Sensor model number : " + singleSensor.get_sensorModel());
        Console.WriteLine("Sensor version : " + singleSensor.get_sensorVersion());
        Console.WriteLine("Sensor serial number : " + singleSensor.get_sensorSerial());

        List<double> valSensor = sdi12Port.readSensor(singleSensor.get_sensorAddress(), "M",
                                 5000);
        Console.WriteLine("Sensor: " + singleSensor.get_sensorAddress());

        for (int i = 0; i < valSensor.Count; i++) {
          if (singleSensor.get_measureCount() > 1) {
            Console.WriteLine(String.Format("{0} : {1,-8:0.00} {2,-10} ({3})",
                                            singleSensor.get_measureSymbol(i), valSensor[i], singleSensor.get_measureUnit(i),
                                            singleSensor.get_measureDescription(i)));
          } else {
            Console.WriteLine(valSensor[i]);
          }
        }
      }

      YAPI.FreeAPI();

      // wait 5 sec to show the output
      ulong now = YAPI.GetTickCount();
      while (YAPI.GetTickCount() - now < 5000);
    }
  }
}
