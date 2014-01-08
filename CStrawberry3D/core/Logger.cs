using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CStrawberry3D.core
{
    public class Logger
    {
        [DllImport("Kernel32.dll")]
        static extern bool AllocConsole();
        [DllImport("Kernel32.dll")]
        static extern bool FreeConsole();

        private static Logger _singleton;
        public static Logger getSingleton()
        {
            if (_singleton == null)
            {
                _singleton = new Logger();
            }
            return _singleton;
        }
        private string _logFile = "log.txt";
        public string logFile
        {
            get
            {
                return _logFile;
            }
            set
            {
                _logFile = value;
            }
        }
        private bool _console = false;
        public bool console
        {
            get
            {
                return _console;
            }
            set
            {
                if (_console == false && value == true)
                    AllocConsole();
                else if (_console == true && value == false)
                    FreeConsole();
                _console = value;
            }
        }
        public void writeLog(string log)
        {
            if (_logFile != string.Empty)
            {
                StreamWriter fileWriter = new StreamWriter(_logFile, true);
                fileWriter.WriteLine(log);
                fileWriter.Close();
            }
        }
        public void info(string info)
        {
            string log = string.Format("<info>{0}: {1}", DateTime.Now, info);
            writeLog(log);
            Console.WriteLine(log);
        }
        public void error(string error)
        {
            string log = string.Format("<error>{0}: {1}", DateTime.Now, error);
            writeLog(log);
            Console.WriteLine(log);
        }
        ~Logger()
        {
            writeLog("<Application Exit>" + DateTime.Now);
        }

    }
}
