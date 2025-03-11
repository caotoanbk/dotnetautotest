using System;
using System.IO;
using System.Threading;

namespace LoggingLibrary
{
    public class Logger
    {
        private readonly string _filePath;
        private static readonly object _lock = new object();

        public Logger(string filePath)
        {
            _filePath = filePath;
        }

        public void Log(string message)
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}";
            WriteToFile(logMessage);
        }

        private void WriteToFile(string message)
        {
            lock (_lock)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(_filePath, true))
                    {
                        writer.WriteLine(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to write to log file: {ex.Message}");
                }
            }
        }
    }
}