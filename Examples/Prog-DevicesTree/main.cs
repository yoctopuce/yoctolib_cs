using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


// This example will found all hubs on the local network,
// connect to them one by one, retreive information, and
// then disconnect.

namespace ConsoleApplication1
{

    class YoctoShield
    {
        private string _serial;
        private List<string> _subdevices;

        public YoctoShield(string serial)
        {
            this._serial = serial;
            this._subdevices = new List<string>();
        }

        public string getSerial()
        {
            return _serial;
        }

        public bool addSubdevice(string serial)
        {
            for (int i = 1; i <= 4; i++) {
                YHubPort p = YHubPort.FindHubPort(this._serial + ".hubPort" + i.ToString());
                if (p.get_logicalName() == serial) {
                    _subdevices.Add(serial);
                    return true;
                }
            }
            return false;

        }

        public void removeSubDevice(string serial)
        {
            _subdevices.Remove(serial);
        }

        public void describe()
        {
            Console.WriteLine("  " + this._serial);
            for (int i = 0; i < _subdevices.Count; i++) {
                Console.WriteLine("    " + _subdevices[i]);
            }
        }

    }

    class RootDevice
    {
        private string _serial;
        private string _url;
        private List<YoctoShield> _shields;
        private List<string> _subdevices;

        public RootDevice(string serialnumber, string url)
        {
            this._serial = serialnumber;
            this._url = url;
            this._shields = new List<YoctoShield>();
            this._subdevices = new List<string>();
        }

        public string getSerial()
        {
            return _serial;
        }

        public void addSubDevice(string serial)
        {
            if (serial.Substring(0, 7) == "YHUBSHL") {
                _shields.Add(new YoctoShield(serial));
            } else {
                // Device to plug look if the device is plugged on a shield
                foreach (YoctoShield shield in _shields) {
                    if (shield.addSubdevice(serial))
                        return;
                }
                _subdevices.Add(serial);
            }
        }


        public void removeSubDevice(string serial)
        {
            _subdevices.Remove(serial);
            for (int i = _shields.Count() - 1; i >= 0; i--) {
                if (_shields[i].getSerial() == serial) {
                    _shields.RemoveAt(i);
                } else {
                    _shields[i].removeSubDevice(serial);
                }
            }
        }


        public void describe()
        {
            Console.WriteLine(this._serial + " (" + _url + ")");
            for (int i = 0; i < _subdevices.Count; i++)
                Console.WriteLine("  " + _subdevices[i]);
            for (int i = 0; i < _shields.Count; i++)
                _shields[i].describe();
        }
    }


    class Program
    {

        static List<RootDevice> __rootDevices = new List<RootDevice>();

        static RootDevice getYoctoHub(string serial)
        {
            foreach (RootDevice rootDevice in __rootDevices) {
                if (rootDevice.getSerial() == serial) {
                    return rootDevice;
                }
            }
            return null;
        }

        static RootDevice addRootDevice(string serial, string url)
        {
            foreach (RootDevice root in __rootDevices) {
                if (root.getSerial() == serial) {
                    return root;
                }
            }
            RootDevice rootDevice = new RootDevice(serial, url);
            __rootDevices.Add(rootDevice);
            return rootDevice;

        }

        static void showNetwork()
        {
            Console.WriteLine("**** device inventory *****");
            foreach (RootDevice root in __rootDevices) {
                root.describe();
            }
        }


        static void removalCallback(YModule module)
        {
            string serial = module.get_serialNumber();
            for (int i = __rootDevices.Count() - 1; i >= 0; i--) {
                __rootDevices[i].removeSubDevice(serial);
                if (__rootDevices[i].getSerial() == serial) {
                    __rootDevices.RemoveAt(i);
                }
            }
        }

        static void arrivalCallback(YModule module)
        {
            string serial = module.get_serialNumber();
            string parentHub = module.get_parentHub();
            if (parentHub == "") {
                // root device (
                string url = module.get_url();
                addRootDevice(serial, url);
            } else {
                RootDevice hub = getYoctoHub(parentHub);
                if (hub != null) {
                    hub.addSubDevice(serial);
                }
            }
        }


        static void Main(string[] args)
        {
            string errmsg = "";
            // configure the API to contact any networked device
            if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
                Console.WriteLine("RegisterHub error: " + errmsg);
                Environment.Exit(0);
            }

            if (YAPI.RegisterHub("net", ref errmsg) != YAPI.SUCCESS) {
                Console.WriteLine("RegisterHub error: " + errmsg);
                Environment.Exit(0);
            }

            // each time a new device is connected/discovered
            // arrivalCallback will be called.
            YAPI.RegisterDeviceArrivalCallback(arrivalCallback);
            // each time a device is disconnected/removed
            // removalCallback will be called.
            YAPI.RegisterDeviceRemovalCallback(removalCallback);

            Console.WriteLine("Waiting for hubs to signal themselves...");
            while (true) {
                YAPI.UpdateDeviceList(ref errmsg);
                YAPI.Sleep(1000, ref errmsg);
                showNetwork();
            }
        }
    }
}