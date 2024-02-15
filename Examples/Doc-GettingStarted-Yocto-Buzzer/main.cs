/*********************************************************************
 *
 *  $Id: main.cs 58233 2023-12-04 10:57:58Z seb $
 *
 *  An example that shows how to use a  Yocto-Buzzer
 *
 *  You can find more information on our web site:
 *   Yocto-Buzzer documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-buzzer/doc.html
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
      Console.WriteLine(execname + "  <serial_number> ");
      Console.WriteLine(execname + "  <logical_name> ");
      Console.WriteLine(execname + "  any ");
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target, serial;
      YBuzzer buz;
      YLed led, led1, led2;
      YAnButton button1, button2;

      if (args.Length < 1) usage();
      target = args[0].ToUpper();

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        buz = YBuzzer.FirstBuzzer();
        if (buz == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
      } else buz = YBuzzer.FindBuzzer(target + ".buzzer");

      if (!buz.isOnline()) {
        Console.WriteLine("Module not connected");
        Console.WriteLine("check identification and USB cable");
        Environment.Exit(0);
      }

      serial = buz.get_module().get_serialNumber();
      led1 = YLed.FindLed(serial + ".led1");
      led2 = YLed.FindLed(serial + ".led2");
      button1 = YAnButton.FindAnButton(serial + ".anButton1");
      button2 = YAnButton.FindAnButton(serial + ".anButton2");

      Console.WriteLine("press a test button or hit Ctrl-C");

      while (buz.isOnline()) {
        int frequency;
        bool b1 = (button1.get_isPressed() == YAnButton.ISPRESSED_TRUE);
        bool b2 = (button2.get_isPressed() == YAnButton.ISPRESSED_TRUE);
        if (b1 || b2) {
          if (b1) {
            led = led1;
            frequency = 1500;
          } else {
            led = led2;
            frequency = 750;
          }

          led.set_power(YLed.POWER_ON);
          led.set_luminosity(100);
          led.set_blinking(YLed.BLINKING_PANIC);
          for (int i = 0; i < 5; i++) { // this can be done using sequence as well
            buz.set_frequency(frequency);
            buz.freqMove(2 * frequency, 250);
            YAPI.Sleep(250, ref errmsg);
          }
          buz.set_frequency(0);
          led.set_power(YLed.POWER_OFF);
        }

      }
      YAPI.FreeAPI();
    }
  }
}
