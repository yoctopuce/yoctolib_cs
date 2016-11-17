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
      Console.WriteLine(execname + " <serial_number>  <frequency> <dutyCycle>");
      Console.WriteLine(execname + " <logical_name> <frequency> <dutyCycle>");
      Console.WriteLine(execname + " any  <frequency> <dutyCycle>   (use any discovered device)");
      Console.WriteLine("     <frequency>: integer between 1Hz and 1000000Hz");
      Console.WriteLine("     <dutyCycle>: floating point number between 0.0 and 100.0");
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YPwmOutput pwmoutput1;
      YPwmOutput pwmoutput2;
      int frequency;
      double dutyCycle;

      if (args.Length < 3) usage();
      target = args[0].ToUpper();
      frequency = Convert.ToInt32(args[1]);
      dutyCycle = Convert.ToDouble(args[2]);

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        pwmoutput1 = YPwmOutput.FirstPwmOutput();
        if (pwmoutput1 == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
        target = pwmoutput1.get_module().get_serialNumber();
      }

      pwmoutput1 = YPwmOutput.FindPwmOutput(target + ".pwmOutput1");
      pwmoutput2 = YPwmOutput.FindPwmOutput(target + ".pwmOutput2");

      if (pwmoutput1.isOnline()) {
        // output 1 : immediate change
        pwmoutput1.set_frequency(frequency);
        pwmoutput1.set_enabled(YPwmOutput.ENABLED_TRUE);
        pwmoutput1.set_dutyCycle(dutyCycle);
        // output 2 : smooth change
        pwmoutput2.set_frequency(frequency);
        pwmoutput2.set_enabled(YPwmOutput.ENABLED_TRUE);
        pwmoutput2.dutyCycleMove(dutyCycle, 3000);
      } else {
        Console.WriteLine("Module not connected (check identification and USB cable)");
      }
      YAPI.FreeAPI();
    }
  }
}
