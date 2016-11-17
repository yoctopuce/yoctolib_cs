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



    // this is the recusive function to draw 1/3nd of the Von Koch flake
    static void recursiveLine(YDisplayLayer layer, double x0, double y0, double x1, double y1, int deep)
    {
      double dx, dy, mx, my;
      if (deep <= 0) {
        layer.moveTo((int)(x0 + 0.5), (int)(y0 + 0.5));
        layer.lineTo((int)(x1 + 0.5), (int)(y1 + 0.5));
      } else {
        dx = (x1 - x0) / 3;
        dy = (y1 - y0) / 3;
        mx = ((x0 + x1) / 2) + (0.87 * (y1 - y0) / 3);
        my = ((y0 + y1) / 2) - (0.87 * (x1 - x0) / 3);
        recursiveLine(layer, x0, y0, x0 + dx, y0 + dy, deep - 1);
        recursiveLine(layer, x0 + dx, y0 + dy, mx, my, deep - 1);
        recursiveLine(layer, mx, my, x1 - dx, y1 - dy, deep - 1);
        recursiveLine(layer, x1 - dx, y1 - dy, x1, y1, deep - 1);
      }
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YDisplay disp;
      YDisplayLayer l1, l2;

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

      // display clean up
      disp.resetAll();

      l1 = disp.get_displayLayer(1);
      l2 = disp.get_displayLayer(2);
      l1.hide();    // L1 is hidden, l2 stay visible
      double centerX = disp.get_displayWidth() / 2;
      double centerY = disp.get_displayHeight() / 2;
      double radius = disp.get_displayHeight() / 2;
      double a = 0;

      while (true) {
        // we draw in the hidden layer
        l1.clear();
        for (int i = 0; i < 3; i++)
          recursiveLine(l1, centerX + radius * Math.Cos(a + i * 2.094),
                        centerY + radius * Math.Sin(a + i * 2.094),
                        centerX + radius * Math.Cos(a + (i + 1) * 2.094),
                        centerY + radius * Math.Sin(a + (i + 1) * 2.094), 2);
        // then we swap contents with the visible layer

        disp.swapLayerContent(1, 2);
        // change the flake angle
        a += 0.1257;
      }
    }
  }
}
