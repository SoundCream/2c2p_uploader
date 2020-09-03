using System;
using Microsoft.Extensions.Logging;

namespace _2C2P.FileUploader.UnitTest.Fakes
{
    public class FakeLogger<TCategoryName> : ILogger<TCategoryName>, IDisposable
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
        }
    }
}
