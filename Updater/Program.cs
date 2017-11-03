using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Xml;
using System.Net;
using System.IO.Compression;
using System.Windows.Forms;

namespace Updater
{
    class Program
    {
        static void Main(string[] args)
        {
             Update(args[0]);
        }
           
           
        static void Update(string programName)
        {
            try
            {
                string process = programName.Replace(".exe", "");
                Console.WriteLine("Terminating process");
                while (Process.GetProcessesByName(process).Length > 0)
                {
                    Process[] myProcesses2 = Process.GetProcessesByName(process);
                    for (int i = 1; i < myProcesses2.Length; i++) { myProcesses2[i].Kill(); }

                    Thread.Sleep(300);
                }
                Console.WriteLine("Success");
                

                DirectoryInfo directoryinfo = new DirectoryInfo("tmp");
                if (directoryinfo.Exists)
                {
                    using (ZipArchive archive = ZipFile.OpenRead(@"tmp\update.zip"))
                    {
                        archive.ExtractToDirectory(Directory.GetCurrentDirectory());
                        DeleteFromFile(@"_DELETE_.txt");
                    }


                    directoryinfo.Delete(true);
                    Console.WriteLine("Success");
                    Process.Start(programName + ".exe");
                }

            }
            catch (Exception e) { Console.WriteLine(e.Message);}
        }

        static void DeleteFromFile(string filename)
        {
            FileInfo file = new FileInfo(filename);
            if (file.Exists)
            {
                string[] filesToDelete = File.ReadAllLines(filename);
                foreach (var delete in filesToDelete)
                {
                    FileInfo f = new FileInfo(delete);
                    if (f.Exists) try { f.Delete(); } catch { };
                }
                file.Delete();
            }
            
        }
        
    }
}
