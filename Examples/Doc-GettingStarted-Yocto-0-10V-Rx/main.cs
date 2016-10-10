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

            YGenericSensor tsensor;

            if (args.Length < 1) usage();
            target = args[0].ToUpper();

            // Setup the API to use local USB devices
            if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
                Console.WriteLine("RegisterHub error: " + errmsg);
                Environment.Exit(0);
            }

            if (target == "ANY") {
                tsensor = YGenericSensor.FirstGenericSensor();

                if (tsensor == null) {
                    Console.WriteLine("No module connected (check USB cable) ");
                    Environment.Exit(0);
                }
                Console.WriteLine("Using: " + tsensor.get_module().get_serialNumber());
            } else {
                tsensor = YGenericSensor.FindGenericSensor(target + ".genericSensor1");
            }

            // retreive module serial
            string serial = tsensor.get_module().get_serialNumber();

            // retreive both channels
            YGenericSensor ch1 = YGenericSensor.FindGenericSensor(serial + ".genericSensor1");
            YGenericSensor ch2 = YGenericSensor.FindGenericSensor(serial + ".genericSensor2");

            string unitSensor1 = "", unitSensor2 = "";
            if (ch1.isOnline()) unitSensor1  = ch1.get_unit();
            if (ch2.isOnline()) unitSensor2 = ch2.get_unit();


            while (ch1.isOnline() && ch2.isOnline()) {
                Console.Write("Channel 1 : " + ch1.get_currentValue().ToString() + unitSensor1);
                Console.Write("   Channel 2 : " + ch2.get_currentValue().ToString() + unitSensor2);
                Console.WriteLine("  (press Ctrl-C to exit)");
                YAPI.Sleep(1000, ref errmsg);
            }

            Console.WriteLine("Module not connected (check identification and USB cable)");
            YAPI.FreeAPI();
        }
    }
}