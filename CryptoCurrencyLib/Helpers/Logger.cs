using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace CryptoCurrency.Helpers
{
    public class Logger
    {
        public string LogFileName { get; set; }
        public string ExcelFilePath { get; set; }
        public string DestinationPath { get; set; }

        private string _excelFilename = "home_auto.xls";

        public Logger()
        {
            GetDest();
        }
        public string GetDest()
        {
            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string DestinationPath = System.IO.Path.GetDirectoryName(strExeFilePath) + "/Temp/";
            if (!Directory.Exists(DestinationPath))
            {
                Directory.CreateDirectory(DestinationPath);
            }
            LogFileName = DestinationPath + "log.txt";
            ExcelFilePath = DestinationPath + _excelFilename;
            return DestinationPath;
        }
        public void LogFile(string newMessage)
        {
            var Line = DateTime.Now.ToString() + " : " + newMessage;
            File.AppendAllText(LogFileName, Line + Environment.NewLine);
        }
    }
}