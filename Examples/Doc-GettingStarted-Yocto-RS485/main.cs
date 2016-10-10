using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ConsoleApplication1
{
    class Program
    {

        static void Main(string[] args)
        {
            string errmsg = "";

            if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS) {
                Console.WriteLine("RegisterHub error: " + errmsg);
                Environment.Exit(1);
            }


            YSerialPort serialPort;
            if (args.Length > 0 && args[0] != "any") {
                serialPort = YSerialPort.FindSerialPort(args[0]);
            } else {
                serialPort = YSerialPort.FirstSerialPort();
                if (serialPort == null) {
                    Console.WriteLine("No module connected (check USB cable)");
                    Environment.Exit(1);
                }
            }
            int slave, reg, val;
            String cmd;
            do {
                Console.WriteLine("Please enter the MODBUS slave address (1...255)");
                Console.Write("Slave: ");
                slave = Convert.ToInt32(Console.ReadLine());
            } while (slave < 1 || slave > 255);
            do {
                Console.WriteLine("Please select a Coil No (>=1), Input Bit No (>=10001+),");
                Console.WriteLine("       Input Register No (>=30001) or Register No (>=40001)");
                Console.Write("No: ");
                reg = Convert.ToInt32(Console.ReadLine());
            } while (reg < 1 || reg >= 50000 || (reg % 10000) == 0);
            while (true) {
                if (reg >= 40001) {
                    val = serialPort.modbusReadRegisters(slave, reg - 40001, 1)[0];
                } else if (reg >= 30001) {
                    val = serialPort.modbusReadInputRegisters(slave, reg - 30001, 1)[0];
                } else if (reg >= 10001) {
                    val = serialPort.modbusReadInputBits(slave, reg - 10001, 1)[0];
                } else {
                    val = serialPort.modbusReadBits(slave, reg - 1, 1)[0];
                }
                Console.WriteLine("Current value: " + val.ToString());
                Console.Write("Press ENTER to read again, Q to quit");
                if ((reg % 30000) < 10000) {
                    Console.Write(" or enter a new value");
                }
                Console.Write(": ");
                cmd = Console.ReadLine();
                if (cmd == "q" || cmd == "Q") break;
                if (cmd != "" && (reg % 30000) < 10000) {
                    val = Convert.ToInt32(cmd);
                    if (reg >= 30001) {
                        serialPort.modbusWriteRegister(slave, reg - 30001, val);
                    } else {
                        serialPort.modbusWriteBit(slave, reg - 1, val);
                    }
                }
            }
            YAPI.FreeAPI();
        }
    }
}
