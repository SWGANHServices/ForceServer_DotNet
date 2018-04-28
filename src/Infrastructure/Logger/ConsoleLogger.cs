using System;
using SwgAnh.Docker.Contracts;

namespace SwgAnh.Docker.Infrastructure.Logger 
{
    public class ConsoleLogger : ILogger
    {
        private readonly ConsoleColor DefaultColor = Console.ForegroundColor;
        private const string Error = "ERROR:";
        private const string Debug = "DEBUG:";
        private const string Warning = "WARNING:";

        public void LogDebug(string message)
        {
            Console.WriteLine($"{Debug} {message}");
        }

        public void Log(string message)
        {
            Console.WriteLine($"{message}");
        }

        public void LogError(string message)
        {
            SetColor(ConsoleColor.Red);
            Console.WriteLine($"{Error} {message}");
            ResetColor();
        }

        public void LogWarning(string message)
        {
            
            SetColor(ConsoleColor.Yellow);
            Console.WriteLine($"{Warning}: {message}");
            ResetColor();
        }

        private static void SetColor(ConsoleColor color) 
        {
            Console.ForegroundColor = color;
        }

        
        private void ResetColor() 
        {
            Console.ForegroundColor = DefaultColor;
        }
    }

}