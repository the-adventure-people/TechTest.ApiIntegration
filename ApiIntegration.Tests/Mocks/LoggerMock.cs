namespace ApiIntegration.Tests
{
    using Microsoft.Extensions.Logging;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    public class LoggerMock : ILogger, IDisposable
    {
        private readonly Action<string> _printOutput = Console.WriteLine;
        public readonly List<string> Messages = new List<string>();

        public void Dispose()
        {
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var logMessage = formatter(state, exception);
            Messages.Add($"{logLevel}: {logMessage}");
            _printOutput(logMessage);
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => this;


        public void VerifyWasLogged(LogLevel logLevel, string logMessage)
        {
            Assert.That(Messages.Exists(msg => msg == $"{logLevel}: {logMessage}"), $"{logLevel} log message not found. Expected \"{logMessage}\".");
        }
    }
}