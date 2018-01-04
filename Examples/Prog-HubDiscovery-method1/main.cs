using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


// This example will found all hubs on the local network,
// connect to them one by one, retreive information, and
// then disconnect.

namespace ConsoleApplication1
{
  class Program
  {

    static List<string> KnownHubs;

    static void HubDiscovered(string serial, string url)
    {
      // The call-back can be called several times for the same hub
      // (the discovery technique is based on a periodic broadcast)
      // So we use a dictionnary to avoid duplicates
      if (KnownHubs.Contains(serial)) return;

      Console.WriteLine("hub found: " + serial + " (" + url + ")");

      // connect to the hub
      string msg = "";
      YAPI.RegisterHub(url, ref msg);

      //  find the hub module
      YModule hub = YModule.FindModule(serial);

      // iterate on all functions on the module and find the ports
      int fctCount =  hub.functionCount();
      for (int i = 0; i < fctCount; i++) {
        // retreive the hardware name of the ith function
        string fctHwdName = hub.functionId(i);
        if (fctHwdName.Length > 7)
          if (fctHwdName.Substring(0, 7) == "hubPort") {
            // The port logical name is always the serial#
            // of the connected device
            string deviceid =  hub.functionName(i);
            Console.WriteLine("  " + fctHwdName + " : " + deviceid);
          }
      }
      // add the hub to the dictionnary so we won't have to
      // process is again.
      KnownHubs.Add(serial);

      // disconnect from the hub
      YAPI.UnregisterHub(url);
    }

    static void Main(string[] args)
    {
      // create a dictionnary
      string errmsg = "";
      KnownHubs = new List<string>();

      Console.WriteLine("Waiting for hubs to signal themselves...");

      // register the callback: HubDiscovered will be
      // invoked each time a hub signals its presence
      YAPI.RegisterHubDiscoveryCallback(HubDiscovered);

      // wait for 30 seconds, doing nothing.
      for (int i = 0 ; i < 30; i++) {
        YAPI.UpdateDeviceList(ref errmsg);
        YAPI.Sleep(1000, ref errmsg);
      }
    }
  }
}
