using System;
using System.IO;
namespace Assignment1
{
    public class Logger
    {
        public Logger()
        {

            string rootfolder = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.IndexOf("bin"));
            Console.WriteLine("root folder is :" + rootfolder);
            string logDir = rootfolder + "logs/";
            string logFilePath = logDir + "/log.txt";

            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }
            if (File.Exists(logFilePath))
            {
                File.Delete(logFilePath);
            }

        }



        public  void Log(string logMessage, TextWriter w)
        {
            try
            {
                w.Write("\r\nLog Entry : ");
                w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                w.WriteLine("  :");
                w.WriteLine($"  :{logMessage}");
            }
            catch (Exception) {
                Console.WriteLine("exception while writing log");
            }
           
        }

       

    }
}
