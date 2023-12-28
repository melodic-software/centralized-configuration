using System.Text;
using Configuration.API.Tests.IntegrationTests.Loggers.Generic;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Configuration.API.Tests.IntegrationTests.Loggers;

internal class XUnitLogger : ILogger
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly string? _categoryName;
    private readonly LoggerExternalScopeProvider _scopeProvider;

    public static ILogger CreateLogger(ITestOutputHelper testOutputHelper) => 
        new XUnitLogger(testOutputHelper, new LoggerExternalScopeProvider(), string.Empty);

    public static ILogger<T> CreateLogger<T>(ITestOutputHelper testOutputHelper) => 
        new XUnitLogger<T>(testOutputHelper, new LoggerExternalScopeProvider());

    public XUnitLogger(ITestOutputHelper testOutputHelper, LoggerExternalScopeProvider scopeProvider, string? categoryName)
    {
        _testOutputHelper = testOutputHelper;
        _scopeProvider = scopeProvider;
        _categoryName = categoryName;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        string logLevelString = GetLogLevelString(logLevel);

        StringBuilder sb = new StringBuilder();

        sb.Append(logLevelString)
            .Append(" [").Append(_categoryName).Append("] ")
            .Append(formatter(state, exception));

        if (exception != null)
            sb.Append('\n').Append(exception);

        // Append scopes
        _scopeProvider.ForEachScope((scope, stringBuilder) =>
        {
            stringBuilder.Append("\n => ");
            stringBuilder.Append(scope);
        }, sb);

        _testOutputHelper.WriteLine(sb.ToString());
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel != LogLevel.None;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return _scopeProvider.Push(state);
    }

    private static string GetLogLevelString(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => "trce",
            LogLevel.Debug => "dbug",
            LogLevel.Information => "info",
            LogLevel.Warning => "warn",
            LogLevel.Error => "fail",
            LogLevel.Critical => "crit",
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel))
        };
    }
}