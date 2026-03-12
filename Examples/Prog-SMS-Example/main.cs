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

    static void smsCallback(YMessageBox mbox, YSms sms)
    {
      Console.WriteLine("- dated " + sms.get_timestamp());
      Console.WriteLine("  from " + sms.get_sender());
      Console.WriteLine("  '" + sms.get_textData() + "'");
      sms.deleteFromSIM();
    }

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YMessageBox mbox;

      if (args.Length < 1) usage();
      target = args[0].ToUpper();

      // API init
      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
        Console.WriteLine("RegisterHub error: " + errmsg);
        System.Threading.Thread.Sleep(2500);
        Environment.Exit(0);
      }

      // find the GSM hub according to command line parameters
      if (target == "ANY") {
        mbox = YMessageBox.FirstMessageBox();
        if (mbox == null) {
          Console.WriteLine("No module with SMS features  (check USB cable) ");
          System.Threading.Thread.Sleep(2500);
          Environment.Exit(0);
        }
      } else mbox = YMessageBox.FindMessageBox(target + ".messageBox");

      if (!mbox.isOnline()) {
        Console.WriteLine("Module not found (check identification and USB cable) ");
        System.Threading.Thread.Sleep(2500);
        Environment.Exit(0);
      }

      Console.WriteLine();
      Console.WriteLine("Using " + mbox.get_friendlyName());
      Console.WriteLine();

      // list messages found on the device
      Console.WriteLine("Messages found on the SIM card:");
      List<YSms> messages = mbox.get_messages();
      if (messages.Count() == 0) {
        Console.WriteLine("* None");
      }
      for (int i = 0 ; i < messages.Count(); i++) {
        YSms sms = messages[i];
        Console.WriteLine("- dated " + sms.get_timestamp());
        Console.WriteLine("  from " + sms.get_sender());
        Console.WriteLine("  '" + sms.get_textData() + "'");
      }

      // register a callback to receive any new message
      mbox.registerSmsCallback(smsCallback);

      // offer to send a new message
      Console.WriteLine("To test sending SMS, provide message recipient (+xxxxxxx).");
      Console.WriteLine("To skip sending, leave empty and press Enter.");
      string number = Console.ReadLine();
      if (number != "") {
        // if that call fails, make sure that your SIM operator
        // allows you to send SMS given your current contract
        mbox.sendTextMessage(number, "Hello from YoctoHub-GSM !");
      }

      Console.WriteLine("Waiting to receive SMS, press Ctrl-C to quit");
      while (true) {
        YAPI.Sleep(3000, ref errmsg);
      }
    }
  }
}
