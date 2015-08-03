using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// hub discovery, method 2 :  this example
// will register any and all hubs found.


namespace ConsoleApplication1
{
    class Program
    {
        // called each time a new device (networked or not) is detected
        static void arrivalCallback(YModule dev)
        {
            bool isAHub = false;
            // iterate on all functions on the module and find the ports
            int fctCount =  dev.functionCount();
            for (int i = 0; i < fctCount; i++) {
                // retreive the hardware name of the ith function
                string fctHwdName = dev.functionId(i);
                if (fctHwdName.Length > 7)
                    if (fctHwdName.Substring(0, 7) == "hubPort") {
                        // the device contains a  hubPortx function, so it's a hub
                        if (!isAHub) {
                            Console.WriteLine("hub found : " + dev.get_friendlyName());
                            isAHub = true;
                        }
                        // The port logical name is always the serial#
                        // of the connected device
                        string deviceid = dev.functionName(i);
                        Console.WriteLine(" " + fctHwdName + " : " + deviceid);
                    }

            }
        }

        static void Main(string[] args)
        {
            
            string errmsg = "";
      
            Console.WriteLine("Waiting for hubs to signal themselves..." );

            // configure the API to contact any networked device
            if (YAPI.RegisterHub("net", ref errmsg) != YAPI.SUCCESS)
            {
                Console.WriteLine("RegisterHub error: " + errmsg);
                Environment.Exit(0);
            }

            // each time a new device is connected/discovered
            // arrivalCallback will be called.
            YAPI.RegisterDeviceArrivalCallback(arrivalCallback);

            // wait for 30 seconds, doing nothing.
            for (int i = 0; i < 30; i++)
            {
                YAPI.UpdateDeviceList(ref errmsg);
                YAPI.Sleep(1000, ref errmsg);
            }

        }
    }
}
