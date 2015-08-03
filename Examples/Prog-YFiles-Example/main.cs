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

    static void Main(string[] args)
    {
      string errmsg = "";
      string target;
      YFiles files;
  

      if (args.Length < 1) usage();

      target = args[0].ToUpper();

      // API init
      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS)
      {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      // find the display according to command line parameters
      if (target == "ANY")
      {
        files = YFiles.FirstFiles();
        if (files == null)
        {
          Console.WriteLine("No module with files features  (check USB cable) ");
          Environment.Exit(0);
        }
      }
      else files = YFiles.FindFiles(target + ".files");


      if (!files.isOnline())
      {
        Console.WriteLine("No module with files connected (check identification and USB cable) ");
        Environment.Exit(0);
      }

  Console.WriteLine();
  Console.WriteLine("Using " + files.get_friendlyName());
  Console.WriteLine();

  byte[] binaryData;
 
  // create text files and upload them to the device
  for (int i=1 ; i<= 5;i++)
  {
     string contents="This is file "+i;
     
    // convert the string to binary data
     binaryData= Encoding.ASCII.GetBytes(contents);
     // upload the file to the device
     files.upload("file"+i+".txt",binaryData );

  }

  // list files found on the device
  Console.WriteLine("Files on device:");
  List<YFileRecord>   filelist=files.get_list("*");
  for (int i=0 ; i< filelist.Count(); i++) 
  {
      string filename = filelist[i].get_name();
      Console.Write(filename);
      Console.Write(new string(' ', 40-filename.Length));  // align
      Console.Write(filelist[i].get_crc().ToString("X"));
      Console.Write("    ");
      Console.WriteLine(filelist[i].get_size()+" bytes");
  }

  //download a file
 binaryData=files.download("file1.txt");

  // convert to string
 string st = System.Text.Encoding.Default.GetString(binaryData);
  

  // and display
  Console.WriteLine("");
  Console.WriteLine("contents of file1.txt:");
  Console.WriteLine(st);
      
    }
  }
}
