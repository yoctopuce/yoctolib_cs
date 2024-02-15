using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
  class Program
  {
    static void usage()
    {
      string execname = System.AppDomain.CurrentDomain.FriendlyName;
      Console.WriteLine("Usage:");
      Console.WriteLine(execname + "  <serial_number> ");
      Console.WriteLine(execname + "  <logical_name> ");
      Console.WriteLine(execname + "  any ");
      System.Threading.Thread.Sleep(2500);
      Environment.Exit(0);
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target, serial;
      YRfidReader reader;
      YBuzzer buzzer;
      YColorLedCluster led;
      YAnButton button1, button2;
      List<String> tagList;

      if (args.Length < 1) usage();
      target = args[0].ToUpper();

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS)
      {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      if (target == "ANY")
      {
        reader = YRfidReader.FirstRfidReader();
        if (reader == null)
        {
          Console.WriteLine("No module connected (check USB cable) ");
          Environment.Exit(0);
        }
      }
      else reader = YRfidReader.FindRfidReader(target + ".rfidReader");



      serial = reader.get_module().get_serialNumber();
      led = YColorLedCluster.FindColorLedCluster(serial + ".colorLedCluster");
      buzzer = YBuzzer.FindBuzzer(serial + ".buzzer");
      button1 = YAnButton.FindAnButton(serial + ".anButton1");
      button2 = YAnButton.FindAnButton(serial + ".anButton2");

      buzzer.set_volume(75);
      led.set_rgbColor(0, 1, 0x000000);

      Console.WriteLine("Place a RFID tag near the Antenna");
      do
      {
        tagList = reader.get_tagIdList();
        YAPI.Sleep(250, ref errmsg);
      } while (tagList.Count <= 0);


      string tagId = tagList[0];
      YRfidStatus opStatus = new YRfidStatus();
      YRfidOptions options = new YRfidOptions();
      YRfidTagInfo taginfo = reader.get_tagInfo(tagId, ref opStatus);
      int blocksize = taginfo.get_tagBlockSize();
      int firstBlock = taginfo.get_tagFirstBlock();
      Console.WriteLine("Tag ID          = " + taginfo.get_tagId());
      Console.WriteLine("Tag Memory size = " + taginfo.get_tagMemorySize().ToString() + " bytes");
      Console.WriteLine("Tag Block  size = " + taginfo.get_tagBlockSize().ToString() + " bytes");

      string data = reader.tagReadHex(tagId, firstBlock, 3 * blocksize, options, ref opStatus);
      if (opStatus.get_errorCode() == YRfidStatus.SUCCESS)
      {
        Console.WriteLine("First 3 blocks  = " + data);
        led.set_rgbColor(0, 1, 0x00FF00);
        buzzer.pulse(1000, 100);
      }
      else
      {
        Console.WriteLine("Cannot read tag contents (" + opStatus.get_errorMessage() + ")");
        led.set_rgbColor(0, 1, 0xFF0000);
      }

      led.rgb_move(0, 1, 0x000000, 200);
      YAPI.FreeAPI();

    }
  }
}
