/*********************************************************************
 *
 *  $Id: main.cs 32621 2018-10-10 13:10:25Z seb $
 *
 *  An example that show how to use a  Yocto-Serial
 *
 *  You can find more information on our web site:
 *   Yocto-Serial documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-serial/doc.html
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
    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YSerialPort serialPort;

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (args.Length > 0 && args[0] != "any") {
        target = args[0];
        serialPort = YSerialPort.FindSerialPort(target + ".serialPort");
        if (!serialPort.isOnline()) {
          Console.WriteLine("No module connected (check cable)");
          Environment.Exit(0);
        }
      } else {
        serialPort = YSerialPort.FirstSerialPort();
        if (serialPort == null) {
          Console.WriteLine("No module connected (check USB cable)");
          Environment.Exit(0);
        }
      }

      Console.WriteLine("****************************");
      Console.WriteLine("* make sure voltage levels *");
      Console.WriteLine("* are properly configured  *");
      Console.WriteLine("****************************");

      serialPort.set_serialMode("9600,8N1");
      serialPort.set_protocol("Line");
      serialPort.reset();

      string line;
      do {
        YAPI.Sleep(500, ref errmsg);
        do {
          line = serialPort.readLine();
          if (line != "") {
            Console.WriteLine("Received: " + line);
          }
        } while (line != "");
        Console.WriteLine("Type line to send, or Ctrl-C to exit: ");
        line = Console.ReadLine();
        serialPort.writeLine(line);
      } while (line != "");
      YAPI.FreeAPI();
    }
  }
}
