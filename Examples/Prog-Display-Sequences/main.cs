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
            Console.WriteLine(execname + " <serial_number> ");
            Console.WriteLine(execname + " <logical_name>");
            Console.WriteLine(execname + "  any ");
            System.Threading.Thread.Sleep(2500);
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            string errmsg = "";
            string target;
            YDisplay disp;
            YDisplayLayer l0;
            int count = 8;

            if (args.Length < 1) usage();

            target = args[0].ToUpper();

            // API init
            if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
                Console.WriteLine("RegisterHub error: " + errmsg);
                Environment.Exit(0);
            }

            // find the display according to command line parameters
            if (target == "ANY") {
                disp = YDisplay.FirstDisplay();
                if (disp == null) {
                    Console.WriteLine("No module connected (check USB cable) ");
                    Environment.Exit(0);
                }
            } else disp = YDisplay.FindDisplay(target + ".display");


            if (!disp.isOnline()) {
                Console.WriteLine("Module not connected (check identification and USB cable) ");
                Environment.Exit(0);
            }

            disp.resetAll();
            // retreive the display size

            int w = disp.get_displayWidth();
            int h = disp.get_displayHeight();

            //reteive the first layer
            l0 = disp.get_displayLayer(0);

            int[] coord = new int[2 * count + 1];

            // precompute the "leds" position
            int ledwidth = (w / count);
            for (int i = 0; i < count; i++) {
                coord[i] = i * ledwidth;
                coord[2 * count - i - 2] = coord[i];
            }

            int framesCount = 2 * count - 2;

            // start recording
            disp.newSequence();

            // build one loop for recording
            for (int i = 0; i < framesCount ; i++) {
                l0.selectColorPen(0);
                l0.drawBar(coord[(i + framesCount - 1) % framesCount], h - 1, coord[(i + framesCount - 1) % framesCount] + ledwidth, h - 4);
                l0.selectColorPen(0xffffff);
                l0.drawBar(coord[i], h - 1, coord[i] + ledwidth, h - 4);
                disp.pauseSequence(100);  // records a 50ms pause.
            }
            // self-call : causes an endless looop
            disp.playSequence("K2000.seq");
            // stop recording and save to device filesystem
            disp.saveSequence("K2000.seq");

            // play the sequence
            disp.playSequence("K2000.seq");

            Console.WriteLine("This animation is running in background.");
        }
    }
}
