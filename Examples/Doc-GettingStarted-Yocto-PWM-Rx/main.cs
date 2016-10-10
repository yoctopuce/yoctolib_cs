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
            YPwmInput pwm;
            YPwmInput pwm1 = null;
            YPwmInput pwm2 = null;
            YModule m = null;

            if (args.Length < 1) usage();
            target = args[0].ToUpper();

            // Setup the API to use local USB devices
            if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
                Console.WriteLine("RegisterHub error: " + errmsg);
                Environment.Exit(0);
            }

            if (target == "ANY") {
                // retreive any pwm input available
                pwm = YPwmInput.FirstPwmInput();
                if (pwm == null) die("No module connected");

            } else {
                // retreive the first pwm input from the device given on command line
                pwm = YPwmInput.FindPwmInput(target + ".pwmInput1");
            }

            // we need to retreive both channels from the device.
            if (pwm.isOnline()) {
                m = pwm.get_module();
                pwm1 = YPwmInput.FindPwmInput(m.get_serialNumber() + ".pwmInput1");
                pwm2 = YPwmInput.FindPwmInput(m.get_serialNumber() + ".pwmInput2");
            } else die("Module not connected");

            while (m.isOnline()) {
                Console.WriteLine("PWM1: " + pwm1.get_frequency().ToString() + " Hz "
                                  + pwm1.get_dutyCycle().ToString() + " % "
                                  + pwm1.get_pulseCounter().ToString() + " pulse edges ");
                Console.WriteLine("PWM2: " + pwm2.get_frequency().ToString() + " Hz "
                                  + pwm2.get_dutyCycle().ToString() + " % "
                                  + pwm2.get_pulseCounter().ToString() + " pulse edges ");
                Console.WriteLine("  (press Ctrl-C to exit)");
                YAPI.Sleep(1000, ref errmsg);
            }
            YAPI.FreeAPI();
            die("Module not connected");
        }
    }
}