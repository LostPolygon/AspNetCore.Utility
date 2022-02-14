using System;
using Microsoft.Extensions.Logging;

namespace LostPolygon.AspNetCore.Utility; 

public class TaggedLoggerWrapper : ILogger {
    private readonly ILogger _loggerImplementation;
    private readonly string _prefix;

    public TaggedLoggerWrapper(ILogger loggerImplementation, string prefix) {
        _loggerImplementation = loggerImplementation;
        _prefix = prefix;
    }

    public IDisposable BeginScope<TState>(TState state) {
        return _loggerImplementation.BeginScope(state);
    }

    public bool IsEnabled(LogLevel logLevel) {
        return _loggerImplementation.IsEnabled(logLevel);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {
        string PrefixFormatter(TState sourceState, Exception? sourceException) => _prefix + formatter(sourceState, sourceException);

        _loggerImplementation.Log(logLevel, eventId, state, exception, PrefixFormatter);
    }
}