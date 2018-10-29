/*********************************************************************
 *
 *  $Id: main.cs 32621 2018-10-10 13:10:25Z seb $
 *
 *  An example that show how to use a  Yocto-MaxiDisplay
 *
 *  You can find more information on our web site:
 *   Yocto-MaxiDisplay documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-maxidisplay/doc.html
 *   C# API Reference:
 *      https://www.yoctopuce.com/EN/doc/reference/yoctolib-cs-EN.html
 *
 *********************************************************************/

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
      YDisplayLayer l0, l1;
      int h, w, y, x, vx, vy;
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

      //clean up
      disp.resetAll();

      // retreive the display size
      w = disp.get_displayWidth();
      h = disp.get_displayHeight();

      // reteive the first layer
      l0 = disp.get_displayLayer(0);

      // display a text in the middle of the screen
      l0.drawText(w / 2, h / 2, YDisplayLayer.ALIGN.CENTER, "Hello world!");

      // visualize each corner
      l0.moveTo(0, 5);
      l0.lineTo(0, 0);
      l0.lineTo(5, 0);
      l0.moveTo(0, h - 6);
      l0.lineTo(0, h - 1);
      l0.lineTo(5, h - 1);
      l0.moveTo(w - 1, h - 6);
      l0.lineTo(w - 1, h - 1);
      l0.lineTo(w - 6, h - 1);
      l0.moveTo(w - 1, 5);
      l0.lineTo(w - 1, 0);
      l0.lineTo(w - 6, 0);

      // draw a circle in the top left corner of layer 1
      l1 = disp.get_displayLayer(1);
      l1.clear();
      l1.drawCircle(h / 8, h / 8, h / 8);

      // and animate the layer
      Console.WriteLine("Use Ctrl-C to stop");
      x = 0;
      y = 0;
      vx = 1;
      vy = 1;
      while (disp.isOnline()) {
        x += vx;
        y += vy;
        if ((x < 0) || (x > w - (h / 4))) vx = -vx;
        if ((y < 0) || (y > h - (h / 4))) vy = -vy;
        l1.setLayerPosition(x, y, 0);
        YAPI.Sleep(5, ref errmsg);
      }
      YAPI.FreeAPI();
    }
  }
}
