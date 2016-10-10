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
            Console.WriteLine(execname + " <serial_number>");
            Console.WriteLine(execname + " <logical_name>");
            Console.WriteLine(execname + " any  ");
            System.Threading.Thread.Sleep(2500);
            Environment.Exit(0);
        }

        static void die(string msg)
        {
            Console.WriteLine(msg + " (check USB cable) ");
            Environment.Exit(0);

        }

        static void Main(string[] args)
        {
            string errmsg = "";
            string target;
            YVoltage sensor;
            YVoltage sensorDC = null;
            YVoltage sensorAC = null;
            YModule m = null;

            if (args.Length < 1) usage();
            target = args[0].ToUpper();

            // Setup the API to use local USB devices
            if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
                Console.WriteLine("RegisterHub error: " + errmsg);
                Environment.Exit(0);
            }

            if (target == "ANY") {
                // retreive any voltage sensor (can be AC or DC)
                sensor = YVoltage.FirstVoltage();
                if (sensor == null) die("No module connected");

            } else sensor = YVoltage.FindVoltage(target + ".voltage1");

            // we need to retreive both DC and AC voltage from the device.
            if (sensor.isOnline()) {
                m = sensor.get_module();
                sensorDC = YVoltage.FindVoltage(m.get_serialNumber() + ".voltage1");
                sensorAC = YVoltage.FindVoltage(m.get_serialNumber() + ".voltage2");
            } else {
                die("Module not connected");
            }

            while (m.isOnline()) {
                Console.Write("DC: " + sensorDC.get_currentValue().ToString() + " v ");
                Console.Write("AC: " + sensorAC.get_currentValue().ToString() + " v ");

                Console.WriteLine("  (press Ctrl-C to exit)");

                YAPI.Sleep(1000, ref errmsg);
            }
            YAPI.FreeAPI();
        }
    }
}