using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace ConsoleApplication1
{
    class Program
    {
        static void usage()
        {
            string errmsg = "";
            string exe = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
            Console.WriteLine("Bad command line arguments");
            Console.WriteLine(exe + " <serial_number>  value");
            Console.WriteLine(exe + " <logical_name> value");
            Console.WriteLine(exe + "   any value ");
            Console.WriteLine("Eg.");
            Console.WriteLine(exe + " any 15 ");
            Console.WriteLine(exe + " YRGBHI01-123456 4");
            YAPI.Sleep(2500, ref errmsg);
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            string errmsg = "";
            string target;
            YCurrentLoopOutput loop;
            double value;

          

            if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS)
            {
                Console.WriteLine("RegisterHub error: " + errmsg);
                Environment.Exit(0);
            }

            if (args.Length < 2) usage();

            target = args[0].ToUpper();
            value = Convert.ToDouble(args[1]);

           

            if (target == "ANY")
            {
                loop = YCurrentLoopOutput.FirstCurrentLoopOutput();
                if (loop == null)
                {
                    Console.WriteLine("No module connected (check USB cable) ");
                    Environment.Exit(0);
                }
            }
            else
            {
                loop = YCurrentLoopOutput.FindCurrentLoopOutput(target + ".currentLoopOutput");
            }


            if (loop.isOnline())
            {
                loop.set_current(value);
                int loopPower = loop.get_loopPower();
                if (loopPower == YCurrentLoopOutput.LOOPPOWER_NOPWR)
                {
                    Console.WriteLine("Current loop not powered");
                    Environment.Exit(0);
                }
                if (loopPower == YCurrentLoopOutput.LOOPPOWER_LOWPWR)
                {
                    Console.WriteLine("Insufficient voltage on current loop");
                    Environment.Exit(0);
                }

                Console.WriteLine("current loop set to " + value.ToString() + " mA");
            }
            else
                Console.WriteLine("Module not connected (check identification and USB cable)");

        }
    }
}
