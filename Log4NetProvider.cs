using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Logging;

namespace ConsoleApp2;

public class Log4NetProvider: ILoggerProvider
{
    private readonly string _configFile;
    public Log4NetProvider(string configFile)
    {
        _configFile = configFile;
        ConfigureLog4Net();
    }

    private void ConfigureLog4Net()
    {
        var assembly = Assembly.GetEntryAssembly();
        if (assembly is null) throw new InvalidOperationException("No entry assembly found.");
        var repo = LogManager.GetRepository(assembly);
        XmlConfigurator.Configure(repo, new FileInfo(_configFile));
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new Log4NetLogger(LogManager.GetLogger(categoryName));
    }

    public void Dispose() { }
}

public class Log4NetLogger(ILog log) : ILogger
{
    public bool IsEnabled(LogLevel logLevel) => log.IsInfoEnabled;
    
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }
    public void Log<TState>(LogLevel logLevel, EventId eventId,
        TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!log.Logger.IsEnabledFor(ConvertLevel(logLevel)))
            return;

        var message = formatter(state, exception);
        if (exception != null)
            message += $" Exception: {exception}";

        log.Logger.Log(typeof(Log4NetLogger), ConvertLevel(logLevel), message, exception);
    }

    private static log4net.Core.Level ConvertLevel(LogLevel logLevel) =>
        logLevel switch
        {
            LogLevel.Trace => log4net.Core.Level.Trace,
            LogLevel.Debug => log4net.Core.Level.Debug,
            LogLevel.Information => log4net.Core.Level.Info,
            LogLevel.Warning => log4net.Core.Level.Warn,
            LogLevel.Error => log4net.Core.Level.Error,
            LogLevel.Critical => log4net.Core.Level.Fatal,
            _ => log4net.Core.Level.Off
        };
}