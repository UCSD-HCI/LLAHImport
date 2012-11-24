using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Runtime.InteropServices;
using System.IO;

namespace LLAHImport
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string[] mArgs;
        LLAHDocumentRepo db;

//        [DllImport("kernel32", SetLastError = true)]
//        private static extern bool AttachConsole(int dwProcessId);
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int pid);

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if ( e.Args.Length > 0 )
            {
                mArgs = e.Args;
                // Command line given, display console
                if (!AttachConsole(-1))
                { // Attach to an parent process console
                    AllocConsole(); // Alloc a new console
                    Console.WriteLine("Had to allocate new console");
                }
                string dbPath = mArgs[0];
                if (File.Exists(Path.Combine(dbPath, LLAHDocumentRepo.fileListName)))
                {
                    db = new LLAHDocumentRepo(dbPath);
                    Console.WriteLine("Database Found. " + db.GetNumberOfDocuments() + " documents loaded");
                }
                else
                {
                    db = new LLAHDocumentRepo(dbPath);
                    Console.WriteLine("Database created.");
                }
                int idx = 1;
                int numFiles = 0;
                for (idx = 1; idx < mArgs.Length; idx++)
                {
                    string filename = mArgs[idx];
                    if (File.Exists(filename))
                    {
                        db.AddPDF(filename);
                        Console.WriteLine(filename + " added.");
                        numFiles++;
                    }
                    else
                    {
                        Console.WriteLine(filename + " not found. Skipped");
                    }

                }
                Console.WriteLine(numFiles + " files added! Press enter to exit.");

                       
            }
            else
            {
                new MainWindow().ShowDialog();
            }
            this.Shutdown();
        }
    }
}
