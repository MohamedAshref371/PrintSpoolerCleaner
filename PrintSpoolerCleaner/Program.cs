using System;
using System.IO;
using System.ServiceProcess;

namespace PrintSpoolerCleaner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PrintSpoolerCleaner v1.0");
            string spoolPath = Path.Combine(Environment.SystemDirectory, "spool", "PRINTERS");
            
            try
            {
                ServiceController sc = new ServiceController("Spooler");

                // إيقاف الخدمة
                if (sc.Status != ServiceControllerStatus.Stopped)
                {
                    Console.WriteLine("Stopping Print Spooler...");
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                }

                // حذف الملفات
                Console.WriteLine("Deleting spool files...");
                if (Directory.Exists(spoolPath))
                {
                    DirectoryInfo di = new DirectoryInfo(spoolPath);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error deleting file {file.Name}: {ex.Message}");
                        }
                    }
                }

                // تشغيل الخدمة مرة تانية
                Console.WriteLine("Starting Print Spooler...");
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));

                Console.WriteLine("https://github.com/MohamedAshref371/PrintSpoolerCleaner");
                Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.ReadLine();
        }
    }
}
