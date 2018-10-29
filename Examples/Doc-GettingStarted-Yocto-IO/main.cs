/*********************************************************************
 *
 *  $Id: main.cs 32621 2018-10-10 13:10:25Z seb $
 *
 *  An example that show how to use a  Yocto-IO
 *
 *  You can find more information on our web site:
 *   Yocto-IO documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-io/doc.html
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
      Console.WriteLine(execname + "  <serial_number>");
      Console.WriteLine(execname + "  <logical_name>");
      Console.WriteLine(execname + "  any");
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YDigitalIO io;

      if (args.Length < 1) usage();
      target = args[0].ToUpper();

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY") {
        io = YDigitalIO.FirstDigitalIO();
        if (io == null) {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
      } else io = YDigitalIO.FindDigitalIO(target + ".digitalIO");

      // lets configure the channels direction
      // bits 0 and 1 as output
      // bits 2 and 3 as input
      io.set_portDirection(0x03);
      io.set_portPolarity(0); // polarity set to regular
      io.set_portOpenDrain(0); // No open drain
      Console.WriteLine("Channels 0..1 are configured as outputs and channels 2..3");
      Console.WriteLine("are configred as inputs, you can connect some inputs to");
      Console.WriteLine("ouputs and see what happens");
      int outputdata = 0;
      while (io.isOnline()) {
        int inputdata = io.get_portState(); // read port values
        string line = "";  // display port value as binary
        for (int i = 0; i < 4; i++) {
          if ((inputdata & (8 >> i)) > 0) {
            line = line + '1';
          } else {
            line = line + '0';
          }
        }
        Console.WriteLine("port value = " + line);
        outputdata = (outputdata + 1) % 4; // cycle ouput 0..3
        io.set_portState(outputdata); // We could have used set_bitState as well
        YAPI.Sleep(1000, ref errmsg);
      }
      YAPI.FreeAPI();
    }
  }
}
