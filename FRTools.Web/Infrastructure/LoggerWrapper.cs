using System;
using Microsoft.Extensions.Logging;

namespace FRTools.Web.Infrastructure
{
    public class LoggerWrapper : ILogger, IDisposable
    {
        private readonly ILogger _logger;

        public LoggerWrapper(ILogger logger) => _logger = logger;

        public IDisposable BeginScope<TState>(TState state) => _logger?.BeginScope(state);

        public bool IsEnabled(LogLevel logLevel) => _logger?.IsEnabled(logLevel) ?? false;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) => _logger?.Log(logLevel, eventId, state, exception, formatter);

        public void Dispose()
        {
        }
    }
}