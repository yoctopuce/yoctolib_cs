using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {

        static int upgradeSerialList(List<string> allserials)
        {
            string errmsg = "";
            foreach (string serial in allserials) {
                YModule module = YModule.FindModule(serial);
                string product = module.get_productName();
                string current = module.get_firmwareRelease();

                // check if a new firmare is available on yoctopuce.com
                string newfirm = module.checkFirmware("www.yoctopuce.com", true);
                if (newfirm == "") {
                    Console.WriteLine(product + " " + serial + "(rev=" + current + ") is up to date");
                } else {
                    Console.WriteLine(product + " " + serial + "(rev=" + current + ") need be updated with firmare : ");
                    Console.WriteLine("    " + newfirm);
                    // execute the firmware upgrade
                    YFirmwareUpdate update = module.updateFirmware(newfirm);
                    int status = update.startUpdate();
                    do {
                        int newstatus = update.get_progress();
                        if (newstatus != status)
                            Console.WriteLine(newstatus + "% " + update.get_progressMessage());
                        YAPI.Sleep(500, ref errmsg);
                        status = newstatus;
                    } while (status < 100 && status >= 0);
                    if (status < 0) {
                        Console.WriteLine("Firmware Update failed: " + update.get_progressMessage());
                        Environment.Exit(1);
                    } else {
                        if (module.isOnline()) {
                            Console.WriteLine(status + "% Firmware Updated Successfully!");
                        } else {
                            Console.WriteLine(status + " Firmware Update failed: module " + serial + "is not online");
                            Environment.Exit(1);
                        }
                    }
                }
            }
            return 0;
        }

        static void Main(string[] args)
        {
            int i;
            List<string> hubs = new List<string>();
            List<string> shield = new List<string>();
            List<string> devices = new List<string>();

            string errmsg = "";


            if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
                Console.WriteLine("RegisterHub error : " + errmsg);
                Environment.Exit(0);
            }

            for (i = 0; i < args.Length; i++) {
                Console.WriteLine("Update module connected to hub " + args[i]);
                if (YAPI.RegisterHub(args[i], ref errmsg) != YAPI.SUCCESS) {
                    Console.WriteLine("RegisterHub error: " + errmsg);
                    Environment.Exit(1);
                }
            }
            //fist step construct the list of all hub /shield and devices connected
            YModule module = YModule.FirstModule();
            while (module != null) {
                string product = module.get_productName();
                string serial = module.get_serialNumber();
                if (product == "YoctoHub-Shield") {
                    shield.Add(serial);
                } else if (product.StartsWith("YoctoHub")) {
                    hubs.Add(serial);
                } else if (product != "VirtualHub") {
                    devices.Add(serial);
                }
                module = module.nextModule();
            }
            // fist upgrades all Hubs...
            upgradeSerialList(hubs);
            // ... then all shield..
            upgradeSerialList(shield);
            // ... and finaly all devices
            upgradeSerialList(devices);
            Console.WriteLine("All devices are now up to date");
            YAPI.FreeAPI();
        }
    }
}
