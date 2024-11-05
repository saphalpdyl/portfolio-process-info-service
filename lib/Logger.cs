﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_PingProcessInformationToWebsiteService.lib
{
    internal class Logger
    {
        private static readonly Logger _instance = new Logger();
        private static bool _isConfiguredLogger = false;

        private string _logFolderPath;

        private Logger() { }

        public static Logger GetInstance() { return _instance; }

        public static Logger SetupLogger(string logFolderPath)
        {
            if (_isConfiguredLogger)
            {
                throw new Exception("[Exception] Logger has already been configured");
            }

            _isConfiguredLogger = true;
            _instance._logFolderPath = logFolderPath;
            return Logger.GetInstance();
        }

        public bool LogEvent(string data)
        {
            try
            {
                if (!_isConfiguredLogger) 
                    throw new Exception("[Exception] Logger hasn't been configured");

                string dateAsFileName = DateTime.Now.ToString()
                    .Replace("/", "_")
                    .Replace(":","_")
                    .Replace(" ","_")
                    .ToLowerInvariant();
                string finalPath = Path.Combine(this._logFolderPath, dateAsFileName + ".log");
                File.WriteAllText(finalPath, data);
                return true;
            } catch (Exception e)
            {
                Console.WriteLine($"[Exception] {e.Message}");
                return false;
            }

        }
    }
}
