using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace CStrawberry3D.Platform
{
    public static class Logger
    {
        [DllImport("Kernel32.dll")]
        static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("Kernel32.dll")]
        static extern bool AllocConsole();
        [DllImport("Kernel32.dll")]
        static extern bool FreeConsole();

        public static string LogFile { get; set; }
        public static bool PendingOnError { get; set; }
        static bool _showConsole;

        public static bool ShowConsole
        {
            get
            {
                return _showConsole;
            }
            set
            {
                if (_showConsole != value)
                {
                    if (value)
                    {
                        //AllocConsole();
                        //Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
                    }
                    else
                    {
                        //FreeConsole();
                    }
                }
                _showConsole = value;
            }
        }
        static Logger()
        {
            PendingOnError = true;
            ShowConsole = true;
            LogFile = "log.txt";
            WriteLog("<Application Start>" + DateTime.Now);
        }
        public static void WriteLog(string log)
        {
            if (!string.IsNullOrEmpty(LogFile))
            {
                StreamWriter fileWriter = new StreamWriter(LogFile, true);
                fileWriter.WriteLine(log);
                fileWriter.Close();
            }
        }
        public static void Info(string info)
        {
            string log = string.Format("<info>{0}: {1}", DateTime.Now, info);
            WriteLog(log);
            Console.WriteLine(log);
        }
        public static void Error(string error)
        {
            string log = string.Format("<error>{0}: {1}", DateTime.Now, error);
            WriteLog(log);
            Console.WriteLine(log);
            if (PendingOnError)
            {
                MessageBox.Show(log, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
