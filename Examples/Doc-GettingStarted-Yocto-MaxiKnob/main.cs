/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  An example that show how to use a  Yocto-MaxiKnob
 *
 *  You can find more information on our web site:
 *   Yocto-MaxiKnob documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-maxiknob/doc.html
 *   C# API Reference:
 *      https://www.yoctopuce.com/EN/doc/reference/yoctolib-cs-EN.html
 *
 *********************************************************************/

using System;

namespace ConsoleApplication1
{
  class Program
  {
    static void usage()
    {
      string execname = AppDomain.CurrentDomain.FriendlyName;
      Console.WriteLine("Usage:");
      Console.WriteLine(execname + "  <serial_number> ");
      Console.WriteLine(execname + "  <logical_name> ");
      Console.WriteLine(execname + "  any ");
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static int notefreq(int note)
    {
      return (int) (220.0 * Math.Exp(note * Math.Log(2) / 12));
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target, serial;
      YBuzzer buz;
      YColorLedCluster leds;
      YQuadratureDecoder qd;
      YAnButton button;

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
      leds = YColorLedCluster.FindColorLedCluster(serial + ".colorLedCluster");
      button = YAnButton.FindAnButton(serial + ".anButton1");
      qd = YQuadratureDecoder.FindQuadratureDecoder(serial + ".quadratureDecoder1");

      if ((!button.isOnline()) || (!qd.isOnline())) {
        Console.WriteLine(
          "Make sure the Yocto-MaxiBuzzer is configured with at least one anButton and one quadrature Decoder");
        Environment.Exit(0);
      }

      Console.WriteLine("press a test button, or turn the encoder or hit Ctrl-C");

      int lastPos = (int) qd.get_currentValue();
      buz.set_volume(75);
      while (button.isOnline()) {
        if ((button.get_isPressed() == YAnButton.ISPRESSED_TRUE) && (lastPos != 0)) {
          lastPos = 0;
          qd.set_currentValue(0);
          buz.playNotes("'E32 C8");
          leds.set_rgbColor(0, 1, 0x000000);
        } else {
          int p = (int) qd.get_currentValue();
          if (lastPos != p) {
            lastPos = p;
            buz.pulse(notefreq(p), 500);
            leds.set_hslColor(0, 1, 0x00FF7f | (p % 255) << 16);
          }
        }
      }

      YAPI.FreeAPI();
    }
  }
}