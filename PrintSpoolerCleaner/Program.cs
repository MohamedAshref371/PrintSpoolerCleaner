using System;
using System.IO;
using System.ServiceProcess;

namespace PrintSpoolerCleaner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"PrintSpoolerCleaner v1.1\n");
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
                    Console.WriteLine("Print Spooler stopped.");
                }
                else
                {
                    Console.WriteLine("Print Spooler is already stopped.");
                }

                // حذف الملفات
                Console.WriteLine("\nDeleting spool files...");
                if (Directory.Exists(spoolPath))
                {
                    DirectoryInfo di = new DirectoryInfo(spoolPath);
                    FileInfo[] files = di.GetFiles();

                    if (files.Length == 0)
                    {
                        Console.WriteLine("No files found in spool folder.");
                    }
                    else
                    {
                        foreach (FileInfo file in files)
                        {
                            try
                            {
                                file.Delete();
                                Console.WriteLine($"Deleted: {file.Name}");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error deleting file {file.Name}: {ex.Message}");
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Spool folder not found: {spoolPath}");
                }

                // تشغيل الخدمة مرة ثانية
                Console.WriteLine("\nStarting Print Spooler...");
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                Console.WriteLine("Print Spooler started.");

                Console.WriteLine("\nhttps://github.com/MohamedAshref371/PrintSpoolerCleaner");
                Console.WriteLine("Done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }
    }
}