using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
  class Program
  {
    

    static void Main(string[] args)
    {
      string errmsg = "";
    
      YColorLed led;
     

      // API init
      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS)
      {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      
    led = YColorLed.FirstColorLed();
	if (led == null)
		{  Console.WriteLine( "no color led found (check USB cable)");
           Environment.Exit(0);
        }
  
    led.resetBlinkSeq();                       // cleans the sequence
    led.addRgbMoveToBlinkSeq(0x00FF00,500);     // move to green in 500 ms
    led.addRgbMoveToBlinkSeq(0x000000,   0);    // switch to black instantaneously
    led.addRgbMoveToBlinkSeq(0x000000,  250);   // stays black for 250ms
    led.addRgbMoveToBlinkSeq(0x0000FF,    0);   // switch to blue instantaneously
    led.addRgbMoveToBlinkSeq(0x0000FF,  100);   // stays blue for 100ms
    led.addRgbMoveToBlinkSeq(0x000000,   0);    // switch to black instantaneously
    led.addRgbMoveToBlinkSeq(0x000000,  250);   // stays black for 250ms
    led.addRgbMoveToBlinkSeq(0xFF0000,    0);   // switch to red instantaneously
    led.addRgbMoveToBlinkSeq(0xFF0000,  100);   // stays red for 100ms
    led.addRgbMoveToBlinkSeq(0x000000,    0);   // switch to black instantaneously
    led.addRgbMoveToBlinkSeq(0x000000, 1000);   // stays black for 1s
    led.startBlinkSeq();                       // starts sequence 
    Console.WriteLine("done");
    


   
    }
  }
}
