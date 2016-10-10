using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {

        static void logfun(YModule m, string value)
        {
            Console.WriteLine(m.get_serialNumber() + ": " + value );
        }

        static void deviceArrival(YModule m)
        {
            string serial = m.get_serialNumber();
            Console.WriteLine("Device arrival : " + serial);
            m.registerLogCallback(logfun);
        }


        static void Main(string[] args)
        {

            string errmsg = "";

            if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
                Console.WriteLine("RegisterHub error : " + errmsg);
                Environment.Exit(0);
            }

            YAPI.RegisterDeviceArrivalCallback(deviceArrival);

            Console.WriteLine("Hit Ctrl-C to Stop ");
            while (true) {
                YAPI.UpdateDeviceList(ref errmsg); // traps plug/unplug events
                YAPI.Sleep(500, ref errmsg);   // traps others events
            }

        }
    }
}
