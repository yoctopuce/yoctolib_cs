/*********************************************************************
 *
 *  $Id: main.cs 58233 2023-12-04 10:57:58Z seb $
 *
 *  An example that shows how to use a  Yocto-SPI
 *
 *  You can find more information on our web site:
 *   Yocto-SPI documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-spi/doc.html
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
      Console.WriteLine("Usage");
      Console.WriteLine(execname + " <serial_number> <value>");
      Console.WriteLine(execname + " <logical_name>  <value>");
      Console.WriteLine(execname + " any  <value>   (use any discovered device)");
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      int value;
      YSpiPort spiPort;

      if (args.Length < 2)
        usage();
      target = args[0].ToUpper();
      value = Convert.ToInt32(args[1]);

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        spiPort = YSpiPort.FirstSpiPort();
        if (spiPort == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
        target = spiPort.get_module().get_serialNumber();
      }

      spiPort = YSpiPort.FindSpiPort(target + ".spiPort");
      if (spiPort.isOnline()) {
        spiPort.set_spiMode("250000,3,msb");
        spiPort.set_ssPolarity(YSpiPort.SSPOLARITY_ACTIVE_LOW);
        spiPort.set_protocol("Frame:5ms");
        spiPort.reset();
        // do not forget to configure the powerOutput of the Yocto-SPI
        // ( for SPI7SEGDISP8.56 powerOutput need to be set at 5v )
        Console.WriteLine("****************************");
        Console.WriteLine("* make sure voltage levels *");
        Console.WriteLine("* are properly configured  *");
        Console.WriteLine("****************************");

        spiPort.writeHex("0c01"); // Exit from shutdown state
        spiPort.writeHex("09ff"); // Enable BCD for all digits
        spiPort.writeHex("0b07"); // Enable digits 0-7 (=8 in total)
        spiPort.writeHex("0a0a"); // Set medium brightness
        for (int i = 1; i <= 8; i++) {
          int digit = value % 10; // digit value
          spiPort.writeArray(new List<int> { i, digit });
          value = value / 10;
        }
      } else
        Console.WriteLine("Module not connected");
      Console.WriteLine("check identification and USB cable");

      YAPI.FreeAPI();
    }
  }
}
