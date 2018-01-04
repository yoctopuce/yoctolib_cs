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
      Console.WriteLine(execname + " <serial_number>  [ -1000 | ... | 1000 ]");
      Console.WriteLine(execname + " <logical_name> [ -1000 | ... | 1000 ]");
      Console.WriteLine(execname + " any [ -1000 | ... | 1000 ]");
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YServo servo1;
      YServo servo5;
      int pos;

      if (args.Length < 2) usage();
      target = args[0].ToUpper();
      pos = Convert.ToInt32(args[1]);

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        servo1 = YServo.FirstServo();
        if (servo1 == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
        target = servo1.get_module().get_serialNumber();
      }

      servo1 = YServo.FindServo(target + ".servo1");
      servo5 = YServo.FindServo(target + ".servo5");

      if (servo1.isOnline()) {
        servo1.set_position(pos);
        servo5.move(pos, 3000);
      } else
        Console.WriteLine("Module not connected (check identification and USB cable)");
    }
  }
}
