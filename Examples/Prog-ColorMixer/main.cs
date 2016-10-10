using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    public class ColorMixer
    {
        // current Color
        public System.Int32 _color;
        // vector that contain all registered leds
        private List<YColorLed> _leds;

        // internal method that will update the color on all registered leds
        private void refreshColor()
        {
            System.Int32 i;
            for (i = 0; i < _leds.Count; i++)
                _leds[i].set_rgbColor(_color);
        }

        static void redCallback(YAnButton button, String calibratedValue)
        {
            // calculate the red component by scaling the calibratedValue
            // from 0..1000 to 0..255

            uint value = uint.Parse(calibratedValue);
            byte red = (byte)((value * 255 / 1000) & 0xff);
            // we used the userData to get the pointer to the instance of ColorMixer
            ColorMixer mixer = (ColorMixer) button.get_userData();
            // update the color
            mixer.changeRed(red);
        }

        static void greenCallback(YAnButton button, String calibratedValue)
        {
            // calculate the green component by scaling the calibratedValue
            // from 0..1000 to 0..255
            uint value = uint.Parse(calibratedValue);
            byte green = (byte)((value * 255 / 1000) & 0xff);
            // we used the userData to get the pointer to the instance of ColorMixer
            ColorMixer mixer = (ColorMixer) button.get_userData();
            // update the color
            mixer.changeGreen(green);
        }

        static void blueCallback(YAnButton button, String calibratedValue)
        {
            // calculate the blue component by scaling the calibratedValue
            // from 0..1000 to 0..255
            uint value = uint.Parse(calibratedValue);
            byte blue = (byte)((value * 255 / 1000) & 0xff);
            // we used the userData to get the pointer to the instance of ColorMixer
            ColorMixer mixer = (ColorMixer) button.get_userData();
            // update the color
            mixer.changeBlue(blue);
        }

        // constructor
        public ColorMixer()
        {
            _color = 0;
            _leds = new List<YColorLed>();
        }

        // register a YoctoLed
        public void addLED(YColorLed led)
        {
            _leds.Add(led);
        }

        // update only red component
        public void changeRed(byte red)
        {
            _color = (_color & 0xffff) | (red << 16);
            refreshColor();
        }

        // update only geen component
        public void changeGreen(byte green)
        {
            _color = (_color & 0xff00ff) | (green << 8);
            refreshColor();
        }

        // update only blue component
        public void changeBlue(byte blue)
        {
            _color = (_color & 0xffff00) | blue;
            refreshColor();
        }

        public void assignRedButton(YAnButton button)
        {
            // we store a pointer to the current instance of ColorMixer into
            // the userData Field
            button.set_userData(this);
            // and we register our static method to change red color as callback
            button.registerValueCallback(redCallback);
        }

        public void assignGreenButton(YAnButton button)
        {
            // we store a pointer to the current instance of ColorMixer into
            // the userData Field
            button.set_userData(this);
            // and we register our static method to change green color as callback
            button.registerValueCallback(greenCallback);

        }
        public void assignBlueButton(YAnButton button)
        {
            // we store a pointer to the current instance of ColorMixer into
            // the userData Field
            button.set_userData(this);
            // and we register our static method to change blue color as callback
            button.registerValueCallback(blueCallback);
        }
    }





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

        static int Main(string[] args)
        {
            string errmsg = "";
            int i;
            int nbled = 0;

            Console.WriteLine("Yoctopuce Library v" + YAPI.GetAPIVersion());
            Console.WriteLine("ColorMixer");
            if (args.Length < 1) {
                Console.WriteLine("usage: demo [usb | ip_address]");
                return 1;
            }

            for (i = 0; i < args.Length; i++) {
                // Setup the API to use local USB devices
                if (YAPI.RegisterHub(args[i], ref errmsg) != YAPI.SUCCESS) {
                    Console.WriteLine("Unable to get acces to devices on " + args[i]);
                    Console.WriteLine("error: " + errmsg);
                    return 1;
                }
            }

            // create our ColorMixer Object
            ColorMixer mixer = new ColorMixer();

            // get our pointer on our 3 knob
            // we use will reference the 3 knob by the logical name
            // that we have configured using the VirtualHub
            YAnButton knobRed = YAnButton.FindAnButton("Red");
            YAnButton knobGreen = YAnButton.FindAnButton("Green");
            YAnButton knobBlue = YAnButton.FindAnButton("Blue");

            // register these 3 knob to the mixer
            mixer.assignRedButton(knobRed);
            mixer.assignGreenButton(knobGreen);
            mixer.assignBlueButton(knobBlue);

            // display a warning if we miss a knob
            if (!knobRed.isOnline())
                Console.WriteLine("Warning: knob \"" + knobRed + "\" is not connected");
            if (!knobGreen.isOnline())
                Console.WriteLine("Warning: knob \"" + knobGreen + "\" is not connected" );
            if (!knobBlue.isOnline())
                Console.WriteLine("Warning: knob \"" + knobBlue + "\" is not connected" );

            // register all led that is connected to our "network"
            YColorLed led = YColorLed.FirstColorLed();
            while (led != null) {
                mixer.addLED(led);
                nbled++;
                led = led.nextColorLed();
            }
            Console.WriteLine(nbled + " Color Led detected", nbled);
            // never hanling loop that will..
            while (true) {
                // ... handle all event durring 5000ms without using lots of CPU ...
                YAPI.Sleep(1000, ref errmsg);
                // ... and check for device plug/unplug
                YAPI.UpdateDeviceList(ref errmsg);
            }
        }
    }
}