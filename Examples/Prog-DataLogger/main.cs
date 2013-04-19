using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
  class Program
  {
    static string UnixTimestampToDateTime(long UnixTimeStamp)
    {
      return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(UnixTimeStamp).ToString();
    }


    static void Main(string[] args)
    {
      string errmsg = "";
      YDataLogger logger;
      List<YDataStream> dataStreams = new List<YDataStream>();
      int i;

      // No exception please
      YAPI.DisableExceptions();

      if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS)
      {
        Console.WriteLine("RegisterHub error: " + errmsg);
        Environment.Exit(0);
      }

      logger = YDataLogger.FirstDataLogger();
      if (logger == null)
      {
        Console.WriteLine("No module with data logger found");
        Console.WriteLine("(Device not connected or firmware too old)");
        Environment.Exit(0);
      }

      Console.WriteLine("Using DataLogger of " + logger.get_module().get_serialNumber());

      if (logger.get_dataStreams(dataStreams) != YAPI.SUCCESS)
      {
        Console.WriteLine("get_dataStreams failed: " + errmsg);
        Environment.Exit(0);
      }


      Console.WriteLine("found: " + dataStreams.Count.ToString() + " stream(s) of data.");

      for (i = 0; i < dataStreams.Count; i++)
      {
        YDataStream s = dataStreams[i];
        long nrows, ncols;
        Console.WriteLine("Data stream  " + i.ToString());
        Console.Write("- Run #" + s.get_runIndex().ToString());
        Console.Write(", time=" + s.get_startTime().ToString());
        if (s.get_startTimeUTC() > 0)
          Console.Write(", UTC =" + UnixTimestampToDateTime(s.get_startTimeUTC()));
        Console.WriteLine();

        nrows = s.get_rowCount();
        ncols = s.get_columnCount();

        if (nrows > 0)
        {
          Console.Write(" " + nrows.ToString() + " samples taken every ");
          Console.WriteLine(s.get_dataSamplesInterval().ToString() + " [s]");
        }


        List<String> names = s.get_columnNames();
        Console.Write("   ");
        for (int c = 0; c < names.Count; c++)
        {
          Console.Write(names[c] + "\t");
        }
        Console.WriteLine();

        double[,] datatable;
        datatable = s.get_dataRows();


        for (int r = 0; r < nrows; r++)
        {
          Console.Write("   ");
          for (int c = 0; c < ncols; c++)
            Console.Write(datatable[r, c].ToString("0.#") + '\t');
          Console.WriteLine();
        }

       

      }
      Console.WriteLine("Done. Have a nice day :)");
    }
  }
}
