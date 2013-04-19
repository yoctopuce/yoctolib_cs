using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
  class Program
  {

    static void anButtonChangeCallBack(YFunction fct, string value)
    {
      Console.WriteLine("Position change         :" + fct.describe() + " = " + value);
    }

    static void temperatureChangeCallBack(YFunction fct, string value)
    {
      Console.WriteLine("Temperature change      :" + fct.describe() + " = " + value +"°C");
    }

    static void lightSensorChangeCallBack(YFunction fct, string value)
    {
      Console.WriteLine("Light change            :" + fct.describe() + " = " + value + "lx");
    }

    static void voltageSensorChangeCallBack(YFunction fct, string value)
    {
      Console.WriteLine("voltage change        :" + fct.describe() + " = " + value + "V");
    }

    static void deviceArrival(YModule m)
    {
      Console.WriteLine("Device arrival          : " + m.ToString());
      int fctcount = m.functionCount();
      string fctName, fctFullName;

      for (int i = 0; i < fctcount; i++)
      {
        fctName = m.functionId(i);
        fctFullName = m.get_serialNumber() + "." + fctName;

        // register call back for anbuttons
        if (fctName.IndexOf("anButton")==0)  
        { 
          YAnButton bt = YAnButton.FindAnButton(fctFullName);
          if(bt.isOnline()) bt.set_callback(anButtonChangeCallBack);
          Console.WriteLine("Callback registered for : " + fctFullName);
        }

        // register call back for temperature sensors
        if (fctName.IndexOf("temperature")==0)
        { 
          YTemperature t = YTemperature.FindTemperature(fctFullName);
          if (t.isOnline()) t.set_callback(temperatureChangeCallBack);
          Console.WriteLine("Callback registered for : " + fctFullName);
        }

        // register call back for light sensors
        if (fctName.IndexOf("lightSensor")==0)
        { 
          YLightSensor l = YLightSensor.FindLightSensor(fctFullName);
          if (l.isOnline()) l.set_callback(lightSensorChangeCallBack);
          Console.WriteLine("Callback registered for : " + fctFullName);
        }
 
        // register call back for voltage sensors
        if (fctName.IndexOf("voltage") == 0)
        {
          YVoltage v = YVoltage.FindVoltage(fctFullName);
          if (v.isOnline()) v.set_callback(voltageSensorChangeCallBack);
          Console.WriteLine("Callback registered for : " + fctFullName);
        }

        // and so on for other sensor type.....

      }
    }

    static void deviceRemoval(YModule m)
    {
      Console.WriteLine("Device removal          : " + m.get_serialNumber());
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
