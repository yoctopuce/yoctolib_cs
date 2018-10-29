/*********************************************************************
 *
 *  $Id: main.cs 32713 2018-10-19 15:30:53Z seb $
 *
 *  An example that show how to use a  Yocto-4-20mA-Tx
 *
 *  You can find more information on our web site:
 *   Yocto-4-20mA-Tx documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-4-20ma-tx/doc.html
 *   C# API Reference:
 *      https://www.yoctopuce.com/EN/doc/reference/yoctolib-cs-EN.html
 *
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace ConsoleApplication1
{
  class Program
  {
    static void usage()
    {
      string errmsg = "";
      string exe = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
      Console.WriteLine("Bad command line arguments");
      Console.WriteLine(exe + " <serial_number>  value");
      Console.WriteLine(exe + " <logical_name> value");
      Console.WriteLine(exe + "   any value ");
      Console.WriteLine("Eg.");
      Console.WriteLine(exe + " any 15 ");
      Console.WriteLine(exe + " YRGBHI01-123456 4");
      YAPI.Sleep(2500, ref errmsg);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YCurrentLoopOutput loop;
      double value;

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (args.Length < 2) usage();

      target = args[0].ToUpper();
      value = Convert.ToDouble(args[1]);

      if (target == "ANY") {
        loop = YCurrentLoopOutput.FirstCurrentLoopOutput();
        if (loop == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
      } else {
        loop = YCurrentLoopOutput.FindCurrentLoopOutput(target + ".currentLoopOutput");
      }

      if (loop.isOnline()) {
        loop.set_current(value);
        int loopPower = loop.get_loopPower();
        if (loopPower == YCurrentLoopOutput.LOOPPOWER_NOPWR) {
          Console.WriteLine("Current loop not powered");
          Environment.Exit(0);
        }
        if (loopPower == YCurrentLoopOutput.LOOPPOWER_LOWPWR) {
          Console.WriteLine("Insufficient voltage on current loop");
          Environment.Exit(0);
        }

        Console.WriteLine("current loop set to " + value.ToString() + " mA");
      } else {
        Console.WriteLine("Module not connected");
        Console.WriteLine("check identification and USB cable");
      }
      YAPI.FreeAPI();
    }
  }
}
