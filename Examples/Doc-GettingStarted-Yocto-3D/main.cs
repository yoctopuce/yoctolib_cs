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

            YTilt anytilt, tilt1, tilt2;
            YCompass compass;
            YAccelerometer accelerometer;
            YGyro gyro;

            target = args[0].ToUpper();

            // Setup the API to use local USB devices
            if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS)
            {
                Console.WriteLine("RegisterHub error: " + errmsg);
                Environment.Exit(0);
            }

            if (target == "ANY")
            {
                anytilt = YTilt.FirstTilt();
                if (anytilt == null)
                {
                    Console.WriteLine("No module connected (check USB cable)");
                    Environment.Exit(0);
                }
            }
            else
            {
                anytilt = YTilt.FindTilt(target + ".tilt1");
                if (!anytilt.isOnline())
                {
                    Console.WriteLine("Module not connected (check identification and USB cable)");
                    Environment.Exit(0);
                }
            }

            string serial = anytilt.get_module().get_serialNumber();
            tilt1 = YTilt.FindTilt(serial + ".tilt1");
            tilt2 = YTilt.FindTilt(serial + ".tilt2");
            compass = YCompass.FindCompass(serial + ".compass");
            accelerometer = YAccelerometer.FindAccelerometer(serial + ".accelerometer");
            gyro = YGyro.FindGyro(serial + ".gyro");
            int count = 0;

            while (true)
            {
                if (!tilt1.isOnline())
                {
                    Console.WriteLine("device disconnected");
                    Environment.Exit(0);
                }

                if (count % 10 == 0) Console.WriteLine("tilt1   tilt2   compass   acc   gyro");

                Console.Write(tilt1.get_currentValue().ToString() + "\t");
                Console.Write(tilt2.get_currentValue().ToString() + "\t");
                Console.Write(compass.get_currentValue().ToString() + "\t");
                Console.Write(accelerometer.get_currentValue().ToString() + "\t");
                Console.WriteLine(gyro.get_currentValue().ToString());

                YAPI.Sleep(250, ref errmsg);
            }
        }
    }
}