/*********************************************************************
 *
 *  $Id: main.cs 60197 2024-03-25 14:55:59Z seb $
 *
 *  Doc-Inventory example
 *
 *  You can find more information on our web site:
 *   C# API Reference:
 *      https://www.yoctopuce.com/EN/doc/reference/yoctolib-cs-EN.html
 *
 *********************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            YModule m;
            string errmsg = "";
            string username = "admin";
            string password = "1234";
            string host = "localhost";
            string url = "secure://" + username + ":" + password + "@" + host;

            // load known TLS certificate into the API
            string trusted_cert = load_cert_from_file(host);
            if (trusted_cert != "") {
                string error = YAPI.AddTrustedCertificates(trusted_cert);
                if (error != "") {
                    Console.WriteLine(error);
                    Environment.Exit(0);
                }
            }
            // test connection with VirtualHub
            int res = YAPI.TestHub(url, 1000, ref errmsg);
            if (res == YAPI.SSL_UNK_CERT) {
                // remote TLS certificate is unknown ask user what to do
                Console.WriteLine("Remote SSL/TLS certificate is unknown");
                Console.WriteLine("You can...");
                Console.WriteLine(" -(A)dd certificate to the API");
                Console.WriteLine(" -(I)gnore this error and continue");
                Console.WriteLine(" -(E)xit");
                Console.Write("Your choice: ");
                string line = Console.ReadLine().ToLower();
                if (line.StartsWith("a")) {
                    // download remote certificate and save it locally
                    trusted_cert = YAPI.DownloadHostCertificate(url, 5000);
                    if (trusted_cert.StartsWith("error")) {
                        Console.WriteLine(trusted_cert);
                        Environment.Exit(0);
                    }
                    save_cert_to_file(host, trusted_cert);
                    string error = YAPI.AddTrustedCertificates(trusted_cert);
                    if (error != "") {
                        Console.WriteLine(error);
                        Environment.Exit(0);
                    }
                } else if (line.StartsWith("i")) {
                    YAPI.SetNetworkSecurityOptions(YAPI.NO_HOSTNAME_CHECK | YAPI.NO_TRUSTED_CA_CHECK |
                                                   YAPI.NO_EXPIRATION_CHECK);
                } else {
                    Environment.Exit(0);
                }
            } else if (res != YAPI.SUCCESS) {
                Console.WriteLine("YAPI.TestHub failed:" + errmsg);
                Environment.Exit(0);
            }


            if (YAPI.RegisterHub(url, ref errmsg) != YAPI.SUCCESS) {
                Console.WriteLine("YAPI.RegisterHub failed:" + errmsg);
                Environment.Exit(0);
            }

            Console.WriteLine("Device list");
            m = YModule.FirstModule();
            while (m != null) {
                Console.WriteLine(m.get_serialNumber() + " (" + m.get_productName() + ")");
                m = m.nextModule();
            }
            YAPI.FreeAPI();
        }


        private static void save_cert_to_file(string url, string trustedCert)
        {
            string path = url.Replace('/', '_').Replace(':', '_') + ".crt";
            File.WriteAllText(path, trustedCert);
        }

        private static string load_cert_from_file(string url)
        {
            string path = url.Replace('/', '_').Replace(':', '_') + ".crt";
            if (File.Exists(path)) {
                return File.ReadAllText(path);
            }
            return "";
        }
    }
}