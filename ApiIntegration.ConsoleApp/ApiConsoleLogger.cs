namespace ApiIntegration.ConsoleApp
{
    using Microsoft.Extensions.Logging;
    using System;

    public static class ApiConsoleLogger
    {
        public static ILogger Create<T>()
        {
            var logger = new ConsoleLogger<T>();
            return logger;
        }

        public class ConsoleLogger<T> : ILogger<T>, IDisposable
        {
            private readonly Action<string> _output = Console.WriteLine;

            void IDisposable.Dispose()
            {
            }

            public void Log<TState>(
                LogLevel logLevel,
                EventId eventId, TState state,
                Exception exception,
                Func<TState, Exception, string> formatter) => _output(formatter(state, exception));

            public bool IsEnabled(LogLevel logLevel) => true;

            public IDisposable BeginScope<TState>(TState state) => this;
        }
    }
}
