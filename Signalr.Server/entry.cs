using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace Signalr.Server
{
    class entry
    {
        static void Main(string[] args)
        {
            AddLocalhostCertificateToTrustedRootIfNotAlreadyAdded();
            using (WebApp.Start("https://localhost:8089"))
            {
                Console.WriteLine("web api has been hosted");
                Console.ReadKey();
            }
        }

        static void AddLocalhostCertificateToTrustedRootIfNotAlreadyAdded()
        {
            Console.WriteLine("Checking for localhost certificate...");
            var localhostCert = new X509Certificate2("localhost.pfx", "Six7ate9!", X509KeyStorageFlags.MachineKeySet);
            if (localhostCert != null)
            {
                var store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadWrite);
                if (!store.Certificates.Contains(localhostCert))
                {
                    store.Add(localhostCert);
                    Console.WriteLine("Added localhost certificate to local machine/trusted root");
                }
                else
                {
                    Console.WriteLine("Localhost certificate already added to local machine/trusted root");
                }
                store.Close();
            }
            else
            {
                throw new Exception("Could not load locahost.pfx.");
            }

            //
            // Also bind the local port to this cert for everyone
            //

            //
            // Check to see if the URL ACL is registered already
            //
            ProcessStartInfo psi = new ProcessStartInfo("netsh", "http show urlacl url=https://localhost:8089/");
            psi.CreateNoWindow = false;
            psi.ErrorDialog = true;
            psi.WindowStyle = ProcessWindowStyle.Normal;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            var p1 = Process.Start(psi);
            if (!p1.WaitForExit(5000))
            {
                throw new Exception("Error issuing urlacl command.  Are you in Administrative mode??");
            }
            var output = p1.StandardOutput.ReadToEnd();
            var exists = output.Contains("https://localhost:8089/");

            if (!exists)
            {

                psi = new ProcessStartInfo("netsh", "http add urlacl url=https://localhost:8089/ user=Everyone");
                psi.CreateNoWindow = false;
                psi.ErrorDialog = true;
                psi.WindowStyle = ProcessWindowStyle.Normal;
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                p1 = Process.Start(psi);
                if (!p1.WaitForExit(5000))
                {
                    throw new Exception("Error issuing urlacl command.  Are you in Administrative mode??");
                }
                Console.WriteLine("Issued urlacl command.");
                Console.WriteLine(p1.StandardOutput.ReadToEnd());
            }


            //
            // Now check to see if the sslcert is already bound
            //
            psi = new ProcessStartInfo("netsh", "http show sslcert ipport=0.0.0.0:8089");
            psi.CreateNoWindow = false;
            psi.ErrorDialog = true;
            psi.WindowStyle = ProcessWindowStyle.Normal;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            p1 = Process.Start(psi);
            if (!p1.WaitForExit(5000))
            {
                throw new Exception("Error issuing urlacl command.  Are you in Administrative mode??");
            }
            output = p1.StandardOutput.ReadToEnd();
            exists = output.Contains("0.0.0.0:8089");

            if (!exists)
            {
                psi = new ProcessStartInfo("netsh", "http add sslcert ipport=0.0.0.0:8089 certhash=8d74c0be39e1b31faa50e5f12d18f6ff06e63fea appid={623C12E4-4BF1-4FA8-B434-65210EFECE2A} CertStoreName=Root");
                psi.CreateNoWindow = false;
                psi.ErrorDialog = true;
                psi.WindowStyle = ProcessWindowStyle.Normal;
                psi.UseShellExecute = false;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardError = true;
                p1 = Process.Start(psi);
                if (!p1.WaitForExit(5000))
                {
                    throw new Exception("Error issuing add sslcert command.  Are you in Administrative mode??");
                }
                Console.WriteLine("Issued urlacl command.");
                Console.WriteLine(p1.StandardOutput.ReadToEnd());
            }

        }
    }
}
