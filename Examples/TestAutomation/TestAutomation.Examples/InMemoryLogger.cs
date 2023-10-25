using Microsoft.Extensions.Logging;

namespace TestAutomation.Examples;

public class InMemoryLogger<T> : ILogger<T>
{
    public List<string> Logs { get; } = new List<string>();

    public IDisposable BeginScope<TState>(TState state) => null;
    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        Logs.Add(formatter(state, exception));
    }
}