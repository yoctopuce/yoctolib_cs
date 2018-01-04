using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string errmsg = "";
            if (YAPI.RegisterHub("usb", ref errmsg) != YAPI.SUCCESS)
            {
                MessageBox.Show(errmsg);
                return;
            }


            YAPI.DisableExceptions();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
