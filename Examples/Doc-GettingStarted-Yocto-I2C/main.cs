/*********************************************************************
 *
 *  $Id: svn_id $
 *
 *  An example that show how to use a  Yocto-I2C
 *
 *  You can find more information on our web site:
 *   Yocto-I2C documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-i2c/doc.html
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
      Console.WriteLine(execname + " <serial_number>");
      Console.WriteLine(execname + " <logical_name>");
      Console.WriteLine(execname + " any            (use any discovered device)");
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YI2cPort i2cPort;

      if (args.Length < 1)
        usage();
      target = args[0].ToUpper();

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        i2cPort = YI2cPort.FirstI2cPort();
        if (i2cPort == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
        target = i2cPort.get_module().get_serialNumber();
      }

      i2cPort = YI2cPort.FindI2cPort(target + ".i2cPort");
      if (i2cPort.isOnline()) {
        i2cPort.set_i2cMode("400kbps");
        i2cPort.set_voltageLevel(YI2cPort.VOLTAGELEVEL_TTL3V);
        i2cPort.reset();
        // do not forget to configure the powerOutput and 
        // of the Yocto-I2C as well if used
        Console.WriteLine("****************************");
        Console.WriteLine("* make sure voltage levels *");
        Console.WriteLine("* are properly configured  *");
        Console.WriteLine("****************************");

        List<int> toSend = new List<int>(new int[]{0x05});
        List<int> received = i2cPort.i2cSendAndReceiveArray(0x1f, toSend, 2);
        int tempReg = (received[0] << 8) + received[1];
        if ((tempReg & 0x1000) != 0) {
          tempReg -= 0x2000;    // perform sign extension
        } else {
          tempReg &= 0x0fff;    // clear status bits
        }
        Console.WriteLine("Ambiant temperature: "+ String.Format("{0:0.000}", (tempReg / 16.0)));
      } else {
        Console.WriteLine("Module not connected");
        Console.WriteLine("check identification and USB cable");
      }
      YAPI.FreeAPI();

      // wait 5 sec to show the output
      ulong now = YAPI.GetTickCount();
      while (YAPI.GetTickCount() - now < 5000);
    }
  }
}
