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
      Console.WriteLine(execname+" <serial_number>");
      Console.WriteLine(execname+" <logical_name>");
      Console.WriteLine(execname+" any  ");
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YAnButton input1;
      YAnButton input5;

      if (args.Length < 1) usage();
      target = args[0].ToUpper();

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS)
      {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY")
      {
        input1 = YAnButton.FirstAnButton();
        if (input1 == null)
        {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }

        target = input1.get_module().get_serialNumber();
      }

      input1 = YAnButton.FindAnButton(target + ".anButton1");
      input5 = YAnButton.FindAnButton(target + ".anButton5");

      while (true)
      {
        if (!input1.isOnline())
        {
          Console.WriteLine("Module not connected (check identification and USB cable)");
          Environment.Exit(0);
        }

        if (input1.get_isPressed() == YAnButton.ISPRESSED_TRUE)
          Console.Write("Button 1: pressed      ");
        else
          Console.Write("Button 1: not pressed  ");

        Console.WriteLine("- analog value:  " + input1.get_calibratedValue().ToString());

        if (input5.get_isPressed() == YAnButton.ISPRESSED_TRUE)
          Console.Write("Button 5: pressed      ");
        else
          Console.Write("Button 5: not pressed  ");

        Console.WriteLine("- analog value:  " + input5.get_calibratedValue().ToString());

        YAPI.Sleep(1000, ref errmsg);
      }
    }
  }
}