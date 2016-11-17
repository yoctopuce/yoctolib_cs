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
      string  execname  = System.AppDomain.CurrentDomain.FriendlyName;
      Console.WriteLine("Usage:");
      Console.WriteLine(execname + " <serial_number> <channel> [ ON | OFF ]");
      Console.WriteLine(execname + " <logical_name> <channel>  [ ON | OFF ]");
      Console.WriteLine(execname + " any <channel> [ ON | OFF ]");
      Console.WriteLine("Example:");
      Console.WriteLine(execname + " any 2 ON");
      System.Threading.Thread.Sleep(2500);

      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YRelay relay;
      string state;
      string  channel;

      if (args.Length < 3) usage();
      target  = args[0].ToUpper();
      channel = args[1];
      state   = args[2].ToUpper();

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        relay = YRelay.FirstRelay();
        if (relay == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
        target = relay.get_module().get_serialNumber();
      }

      Console.WriteLine("using " + target);
      relay = YRelay.FindRelay(target + ".relay" + channel);

      if (relay.isOnline()) {
        if (state == "ON")
          relay.set_output(YRelay.OUTPUT_ON);
        else
          relay.set_output(YRelay.OUTPUT_OFF);
      } else {
        Console.WriteLine("Module not connected (check identification and USB cable)");
      }
      YAPI.FreeAPI();
    }
  }
}
