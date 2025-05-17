using System;
using System.Threading;

namespace RPCYoinker
{
    public class Logger
    {
        public enum LogLevel
        {
            Info,
            Success,
            Error
        }

        public static void Log(string message, LogLevel logLevel = LogLevel.Info)
        {
            switch (logLevel)
            {
                case LogLevel.Info:
                    Console.WriteLine($"[~] {message}");
                    break;
                case LogLevel.Success:
                    Console.WriteLine($"[+] {message}");
                    break;
                case LogLevel.Error:
                    Console.WriteLine($"[-] {message}");
                    break;
            }
        }

        public static string ReadLine(string message, string defaultValue = "")
        {
            Console.Write($"[?] {message}: ");

            string result = Console.ReadLine();

            if (string.IsNullOrEmpty(result))
                return defaultValue;

            return result;
        }

        public static void Exit(LogLevel logLevel = LogLevel.Info)
        {
            Log("Closing in 5 seconds...", logLevel);
            Thread.Sleep(5000);
        }
    }
}