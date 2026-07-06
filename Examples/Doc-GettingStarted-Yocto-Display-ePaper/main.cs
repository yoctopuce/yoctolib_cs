/*********************************************************************
 *
 *  $Id: main.cs 75050 2026-07-02 09:49:34Z seb $
 *
 *  An example that shows how to use a  Yocto-Display-ePaper
 *
 *  You can find more information on our web site:
 *   Yocto-Display-ePaper documentation:
 *      https://www.yoctopuce.com/EN/products/yocto-display-epaper/doc.html
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

      if (args.Length < 1) usage();

      target = args[0].ToUpper();

      // API init
      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS)
      {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      // find the display according to command line parameters
      if (target == "ANY")
      {
        disp = YDisplay.FirstDisplay();
        if (disp == null)
        {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
      }
      else disp = YDisplay.FindDisplay(target + ".display");

      if (!disp.isOnline())
      {
        Console.WriteLine("Module not connected (check identification and USB cable) ");
        Environment.Exit(0);
      }

      int[] colors = { 0xFFFFFF, 0x000000, 0xFF0000, 0xFFFF00 };

      // Makes sure the Panel type is set
      string paneltype = disp.get_displayPanel();
      if (paneltype == "NOT_SET")
      {
        Console.WriteLine("Use the virtual to Configure the panel first");
        YAPI.Sleep(3000, ref errmsg);
        Environment.Exit(0);
      }

      // retrieve the display size
      int w = disp.get_displayWidth();
      int h = disp.get_displayHeight();
      int middleX = (int)(w / 2);
      int middleY = (int)(h / 2);
      Console.WriteLine("Using device " + disp.get_serialNumber() + " (panel: " + paneltype + " " + w + "x" + h + "pixels)\n");
      disp.resetAll();

      // retrieve the first layer
      YDisplayLayer l0 = disp.get_displayLayer(0);
      l0.selectFont("medium.yfm");
      int interations = 0;
      bool animation = true;
      Random rnd = new Random();

      while (animation)
      {
        interations++;
        // prevent refreshing for 2 sec
        disp.postponeRefresh(2000);
        l0.clear();
        // draw a few circle
        for (int i = 0; i < 15; i++)
        {
          int cx = rnd.Next(w);
          int cy = rnd.Next(h);
          int r = rnd.Next((h / 20), (h / 10));
          l0.selectFillColor(colors[rnd.Next(4)]);
          l0.drawDisc(cx, cy, r);
          l0.drawCircle(cx, cy, r);
        }
        // draw a rectangle with panel type in it
        l0.selectFillColor(0xffffff);
        l0.drawBar(middleX - 75, middleY - 10, middleX + 75, middleY + 12);
        l0.drawRect(middleX - 75, middleY - 10, middleX + 75, middleY + 12);
        l0.drawText(middleX, middleY, YDisplayLayer.ALIGN.CENTER, paneltype);
        // forces a full refresh only the 1rst time
        if (interations == 1) disp.regenerateDisplay();
        disp.triggerRefresh(); // display is allowed to refresh  again
        YAPI.Sleep(1000, ref errmsg);
        // if no fast refresh available, don't even try to run animations
        if (paneltype.IndexOf("KS") < 0) animation = false;
      }

      YAPI.FreeAPI();
    }
  }
}