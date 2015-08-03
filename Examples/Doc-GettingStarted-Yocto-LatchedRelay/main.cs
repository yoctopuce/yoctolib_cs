using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
  class Program
  {
    static void usage()
    { string execname = System.AppDomain.CurrentDomain.FriendlyName;
      Console.WriteLine("Usage:");
      Console.WriteLine(execname+"  <serial_number>  [ A | B ]");
      Console.WriteLine(execname+"  <logical_name>  [ A | B ]");
      Console.WriteLine(execname+"  any [ A | B ]");
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YRelay relay;
      string state;

      if (args.Length < 2) usage();
      target = args[0].ToUpper();
      state = args[1].ToUpper();

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS)
      {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY")
      {
        relay = YRelay.FirstRelay();
        if (relay == null)
        {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
      }
      else relay = YRelay.FindRelay(target + ".relay1");

      if (relay.isOnline())
      {
        if (state == "A") relay.set_state(YRelay.STATE_A); else relay.set_state(YRelay.STATE_B);
      }
      else Console.WriteLine("Module not connected (check identification and USB cable)");
    }
  }
}
