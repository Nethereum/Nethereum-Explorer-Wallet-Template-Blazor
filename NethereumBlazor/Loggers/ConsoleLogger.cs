using Splat;
using System;
using System.ComponentModel;

namespace NethereumBlazor.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public LogLevel Level => LogLevel.Info;

        public void Write([Localizable(false)] string message, LogLevel logLevel)
        {
            Console.WriteLine(message);
        }

        public void Write(Exception exception, [Localizable(false)] string message, LogLevel logLevel)
        {
            Console.WriteLine(message);
        }

        public void Write([Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
        {
            Console.WriteLine(message);
        }

        public void Write(Exception exception, [Localizable(false)] string message, [Localizable(false)] Type type, LogLevel logLevel)
        {
            Console.WriteLine(message);
        }
    }
}
