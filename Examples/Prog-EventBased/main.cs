using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void anButtonValueChangeCallBack(YFunction fct, string value)
        {
            Console.WriteLine(fct.get_hardwareId() + ": " + value + " (new value)");
        }

        static void sensorValueChangeCallBack(YSensor fct, string value)
        {
            Console.WriteLine(fct.get_hardwareId() + ": " + value + " " + fct.get_userData() + " (new value)");
        }

        static void sensorTimedReportCallBack(YSensor fct, YMeasure measure)
        {
            Console.WriteLine(fct.get_hardwareId() + ": " + measure.get_averageValue() + " " + fct.get_userData() + " (timed report)");
        }

        static void deviceLog(YModule module, string logline)
        {
            Console.WriteLine("log: " + module.get_hardwareId() + ":" + logline);
        }

        static void configChange(YModule m)
        {
            Console.WriteLine("config change: " + m.get_serialNumber());
        }


        static void beaconChange(YModule m, int beacon)
        {
            Console.WriteLine("Beacon of " + m.get_serialNumber() + " changed to " + beacon);
        }


        static void deviceArrival(YModule m)
        {
            string serial = m.get_serialNumber();
            Console.WriteLine("Device arrival : " + serial);
            m.registerLogCallback(deviceLog);
            m.registerConfigChangeCallback(configChange);
            m.registerBeaconCallback(beaconChange);

            // First solution: look for a specific type of function (eg. anButton)
            int fctcount = m.functionCount();
            for (int i = 0; i < fctcount; i++) {
                string hardwareId = serial + "." + m.functionId(i);
                if (hardwareId.IndexOf(".anButton") >= 0) {
                    Console.WriteLine("- " + hardwareId);
                    YAnButton anButton = YAnButton.FindAnButton(hardwareId);
                    anButton.registerValueCallback(anButtonValueChangeCallBack);
                }
            }

            // Alternate solution: register any kind of sensor on the device
            YSensor sensor = YSensor.FirstSensor();
            while (sensor != null) {
                if (sensor.get_module().get_serialNumber() == serial) {
                    string hardwareId = sensor.get_hardwareId();
                    Console.WriteLine("- " + hardwareId);
                    string unit = sensor.get_unit();
                    sensor.set_userData(unit);
                    sensor.registerValueCallback(sensorValueChangeCallBack);
                    sensor.registerTimedReportCallback(sensorTimedReportCallBack);
                }

                sensor = sensor.nextSensor();
            }
        }

        static void deviceRemoval(YModule m)
        {
            Console.WriteLine("Device removal : " + m.get_serialNumber());
        }

        private static void log(string line)
        {
            Console.Write("LOG : " + line);
        }

        static void Main(string[] args)
        {
            string errmsg = "";

            YAPI.RegisterLogFunction(log);

            if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
                Console.WriteLine("RegisterHub error : " + errmsg);
                Environment.Exit(0);
            }

            YAPI.RegisterDeviceArrivalCallback(deviceArrival);
            YAPI.RegisterDeviceRemovalCallback(deviceRemoval);

            Console.WriteLine("Hit Ctrl-C to Stop ");

            while (true) {
                YAPI.UpdateDeviceList(ref errmsg); // traps plug/unplug events
                YAPI.Sleep(500, ref errmsg); // traps others events
            }
        }
    }
}