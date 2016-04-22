using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
  class Program
  {

    static void anButtonValueChangeCallBack(YFunction fct, string value)
    {
      Console.WriteLine(fct.get_hardwareId() + ": " + value + " (new value)");
    }

    static void sensorValueChangeCallBack(YSensor fct, string value)
    {
      Console.WriteLine(fct.get_hardwareId() + ": " + value + " (new value)");
    }

    static void sensorTimedReportCallBack(YSensor fct, YMeasure measure)
    {
      Console.WriteLine(fct.get_hardwareId() + ": " + measure.get_averageValue() + " " + fct.get_unit() + " (timed report)");
    }

    static void deviceLog(YModule module, string logline)
    {
        Console.WriteLine("log:" + module.get_hardwareId() + ":" + logline);
    }

    static void deviceArrival(YModule m)
    {
      string serial = m.get_serialNumber();
      Console.WriteLine("Device arrival : " + serial);
        m.registerLogCallback(deviceLog);

      // First solution: look for a specific type of function (eg. anButton)
      int fctcount = m.functionCount();
      for (int i = 0; i < fctcount; i++)
      {
          string hardwareId = serial + "." + m.functionId(i);
          if (hardwareId.IndexOf(".anButton") >= 0)  
          {
              Console.WriteLine("- " + hardwareId);
              YAnButton anButton = YAnButton.FindAnButton(hardwareId);
              anButton.registerValueCallback(anButtonValueChangeCallBack);
          }
      }

      // Alternate solution: register any kind of sensor on the device
      YSensor sensor = YSensor.FirstSensor();
      while(sensor != null) {
          if(sensor.get_module().get_serialNumber() == serial) {
              string hardwareId = sensor.get_hardwareId();
              Console.WriteLine("- " + hardwareId);
              sensor.registerValueCallback(sensorValueChangeCallBack);
              sensor.registerTimedReportCallback(sensorTimedReportCallBack);
          }
          sensor = sensor.nextSensor();
      }
    }

    static void deviceRemoval(YModule m)
    {
      Console.WriteLine("Device removal : " + m.get_serialNumber());
    }

    static void Main(string[] args)
    {

      string errmsg = "";

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS)
      {
        Console.WriteLine("RegisterHub error : " + errmsg);
        Environment.Exit(0);
      }

      YAPI.UpdateDeviceList(ref errmsg); // traps plug/unplug events
      YAPI.Sleep(2000, ref errmsg);   // traps others events

      YAPI.RegisterDeviceArrivalCallback(deviceArrival);
      YAPI.RegisterDeviceRemovalCallback(deviceRemoval);

      Console.WriteLine("Hit Ctrl-C to Stop ");

      while (true)
      {
        YAPI.UpdateDeviceList(ref errmsg); // traps plug/unplug events
        YAPI.Sleep(500, ref errmsg);   // traps others events
      }

    }
  }
}
