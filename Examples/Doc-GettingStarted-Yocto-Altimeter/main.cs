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
            Console.WriteLine(execname + " <serial_number>");
            Console.WriteLine(execname + " <logical_name>");
            Console.WriteLine(execname + " any  ");
            System.Threading.Thread.Sleep(2500);
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            string errmsg = "";
            string target;

            YAltitude asensor;
            YTemperature tsensor;
            YPressure psensor;

            if (args.Length < 1) usage();
            target = args[0].ToUpper();

            // Setup the API to use local USB devices
            if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
                Console.WriteLine("RegisterHub error: " + errmsg);
                Environment.Exit(0);
            }

            if (target == "ANY") {
                asensor = YAltitude.FirstAltitude();
                tsensor = YTemperature.FirstTemperature();
                psensor = YPressure.FirstPressure();

                if ((asensor == null) || (tsensor == null) || (psensor == null)) {
                    Console.WriteLine("No module connected (check USB cable) ");
                    Environment.Exit(0);
                }
            } else {
                asensor = YAltitude.FindAltitude(target + ".altitude");
                tsensor = YTemperature.FindTemperature(target + ".temperature");
                psensor = YPressure.FindPressure(target + ".pressure");
            }


            if (!asensor.isOnline()) {
                Console.WriteLine("Module not connected (check identification and USB cable)");
                Environment.Exit(0);
            }
            while (asensor.isOnline()) {
                Console.WriteLine("Current altitude:    " + asensor.get_currentValue().ToString() + " m "
                                  + "(QNH=" + asensor.get_qnh().ToString() + " hPa)");
                Console.WriteLine("Current temperature: " + tsensor.get_currentValue().ToString() + " °C");
                Console.WriteLine("Current pressure:    " + psensor.get_currentValue().ToString() + " hPa");
                Console.WriteLine("  (press Ctrl-C to exit)");

                YAPI.Sleep(1000, ref errmsg);
            }
            YAPI.FreeAPI();
        }
    }
}