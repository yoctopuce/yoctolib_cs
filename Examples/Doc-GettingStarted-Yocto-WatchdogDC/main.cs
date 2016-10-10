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
            Console.WriteLine("demo  <serial_number>  [ on | off | reset]");
            Console.WriteLine("demo  <logical_name>  [ on | off | reset]");
            Console.WriteLine("demo  any [ on | off | reset]");
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            string errmsg = "";
            string target;
            YWatchdog watchdog;
            string state;

            if (args.Length < 2) usage();
            target = args[0].ToUpper();
            state = args[1].ToUpper();

            if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
                Console.WriteLine("RegisterHub error: " + errmsg);
                Environment.Exit(0);
            }

            if (target == "ANY") {
                watchdog = YWatchdog.FirstWatchdog();
                if (watchdog == null) {
                    Console.WriteLine("No module connected (check USB cable) ");
                    Environment.Exit(0);
                }
            } else watchdog = YWatchdog.FindWatchdog(target + ".watchdog1");

            if (watchdog.isOnline()) {
                if (state == "ON") watchdog.set_running(YWatchdog.RUNNING_ON);
                if (state == "OFF")  watchdog.set_running(YWatchdog.RUNNING_OFF);
                if (state == "RESET")  watchdog.resetWatchdog();

            } else {
                Console.WriteLine("Module not connected (check identification and USB cable)");
            }
            YAPI.FreeAPI();
        }
    }
}
