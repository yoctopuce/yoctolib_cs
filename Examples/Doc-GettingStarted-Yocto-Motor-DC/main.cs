/*********************************************************************
 *
 *  $Id: main.cs 58233 2023-12-04 10:57:58Z seb $
 *
 *  An example that shows how to use a  Yocto-Motor-DC
 *
 *  You can find more information on our web site:
 *   Yocto-Motor-DC documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-motor-dc/doc.html
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
      Console.WriteLine("Usage:");
      Console.WriteLine(execname + "  <serial_number>  power");
      Console.WriteLine(execname + "  <logical_name>  power");
      Console.WriteLine(execname + "  any power");
      Console.WriteLine("  power is a integer between -100 and 100%");
      Console.WriteLine("Example:");
      Console.WriteLine(execname + "  any 75");
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      int power;
      YMotor motor;
      YCurrent current;
      YVoltage voltage;
      YTemperature temperature;

      // parse the  command line
      if (args.Length < 2) usage();
      target = args[0].ToUpper();
      power = Convert.ToInt32(args[1]);

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }
      if (target == "ANY") {
        // find the serial# of the first available motor
        motor = YMotor.FirstMotor();
        if (motor == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
        target = motor.get_module().get_serialNumber();
      }

      motor = YMotor.FindMotor(target + ".motor");
      current = YCurrent.FindCurrent(target + ".current");
      voltage = YVoltage.FindVoltage(target + ".voltage");
      temperature = YTemperature.FindTemperature(target + ".temperature");

      // lets start the motor
      if (motor.isOnline()) {
        // if motor is in error state, reset it.
        if (motor.get_motorStatus() >= YMotor.MOTORSTATUS_LOVOLT) {
          motor.resetStatus();
        }
        motor.drivingForceMove(power, 2000);  // ramp up to power in 2 seconds
        while (motor.isOnline()) {
          // display motor status
          Console.WriteLine("Status=" + motor.get_advertisedValue() + "  " +
                            "Voltage=" + voltage.get_currentValue() + "V  " +
                            "Current=" + current.get_currentValue() / 1000 + "A  " +
                            "Temp=" + temperature.get_currentValue() + "deg C");
          YAPI.Sleep(1000, ref errmsg); // wait for one second
        }
      }
      YAPI.FreeAPI();
    }
  }
}
