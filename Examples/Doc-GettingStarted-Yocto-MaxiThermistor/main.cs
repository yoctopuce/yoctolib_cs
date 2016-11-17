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

      // retreive all 6 channels
      YTemperature ch1 = YTemperature.FindTemperature(serial + ".temperature1");
      YTemperature ch2 = YTemperature.FindTemperature(serial + ".temperature2");
      YTemperature ch3 = YTemperature.FindTemperature(serial + ".temperature3");
      YTemperature ch4 = YTemperature.FindTemperature(serial + ".temperature4");
      YTemperature ch5 = YTemperature.FindTemperature(serial + ".temperature5");
      YTemperature ch6 = YTemperature.FindTemperature(serial + ".temperature6");


      if (!ch2.isOnline()) {
        Console.WriteLine("Module not connected (check identification and USB cable)");
        Environment.Exit(0);
      }
      while (ch2.isOnline()) {
        Console.Write("| 1: " + ch1.get_currentValue().ToString(" 0.0"));
        Console.Write(" | 2: " + ch2.get_currentValue().ToString(" 0.0"));
        Console.Write(" | 3: " + ch3.get_currentValue().ToString(" 0.0"));
        Console.Write(" | 4: " + ch4.get_currentValue().ToString(" 0.0"));
        Console.Write(" | 5: " + ch5.get_currentValue().ToString(" 0.0"));
        Console.Write(" | 6: " + ch6.get_currentValue().ToString(" 0.0"));
        Console.WriteLine("|  °C  |");
        YAPI.Sleep(1000, ref errmsg);
      }
      YAPI.FreeAPI();
    }
  }
}