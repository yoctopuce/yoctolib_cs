/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  An example that show how to use a  Yocto-MaxiBuzzer
 *
 *  You can find more information on our web site:
 *   Yocto-MaxiBuzzer documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-maxibuzzer/doc.html
 *   C# API Reference:
 *      https://www.yoctopuce.com/EN/doc/reference/yoctolib-cs-EN.html
 *
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
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
      YColorLed led;
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
      led = YColorLed.FindColorLed(serial + ".colorLed");
      button1 = YAnButton.FindAnButton(serial + ".anButton1");
      button2 = YAnButton.FindAnButton(serial + ".anButton2");

      Console.WriteLine("press a test button or hit Ctrl-C");

      while (buz.isOnline()) {
        int frequency;
        int color;
        int volume;
        bool b1 = (button1.get_isPressed() == YAnButton.ISPRESSED_TRUE);
        bool b2 = (button2.get_isPressed() == YAnButton.ISPRESSED_TRUE);
        if (b1 || b2) {
          if (b1) {
            volume = 60;
            frequency = 1500;
            color = 0xff0000;
          } else {
            volume = 30;
            color = 0x00ff00;
            frequency = 750;
          }
          led.resetBlinkSeq();
          led.addRgbMoveToBlinkSeq(color, 100);
          led.addRgbMoveToBlinkSeq(0, 100);
          led.startBlinkSeq();
          buz.set_volume(volume);
          for (int i = 0; i < 5; i++) {
            // this can be done using sequence as well
            buz.set_frequency(frequency);
            buz.freqMove(2 * frequency, 250);
            YAPI.Sleep(250, ref errmsg);
          }

          buz.set_frequency(0);
          led.stopBlinkSeq();
          led.set_rgbColor(0);
        }
      }

      YAPI.FreeAPI();
    }
  }
}