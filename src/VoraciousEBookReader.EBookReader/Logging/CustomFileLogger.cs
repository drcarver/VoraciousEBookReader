using Microsoft.Extensions.Logging;

namespace VoraciousEBookReader.EBookReader.Logging;

/// <summary>
/// Customized ILogger, writes logs to text files
/// </summary>
public class CustomFileLogger : ILogger
{
    private readonly string _categoryName;
    private readonly StreamWriter _logFileWriter;

    /// <summary>
    /// Write a log statement to a file
    /// </summary>
    /// <param name="categoryName">The category name to log</param>
    /// <param name="logFileWriter">The log file stream to write to</param>
    public CustomFileLogger(string categoryName, StreamWriter logFileWriter)
    {
        _categoryName = categoryName;
        _logFileWriter = logFileWriter;
    }

    /// <summary>
    /// Begin a log file scope
    /// </summary>
    /// <typeparam name="TState">The state for the log file</typeparam>
    /// <param name="state">The scope of the file</param>
    /// <returns>A IDisposable for the stream writer</returns>
    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    /// <summary>
    /// Is the logging enabled for the log level
    /// </summary>
    /// <param name="logLevel">The log level</param>
    /// <returns>True if logging is enabled for the log level</returns>
    public bool IsEnabled(LogLevel logLevel)
    {
        // Ensure that only information level and higher logs are recorded
        return logLevel >= LogLevel.Information;
    }

    /// <summary>
    /// Log the entry to the file
    /// </summary>
    /// <typeparam name="TState">The state of the file</typeparam>
    /// <param name="logLevel">The log level for the file</param>
    /// <param name="eventId">The logging eventId</param>
    /// <param name="state">The state for the file</param>
    /// <param name="exception">The exception for the logging statement</param>
    /// <param name="formatter">The formatter for the log statement</param>
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter)
    {
        //Ensure that only information level and higher logs are recorded
        if (!IsEnabled(logLevel))
        {
            return;
        }

        //Get the formatted log message
        var message = formatter(state, exception);

        // Write log messages to text file
        _logFileWriter.WriteLine($"[{DateTime.Now}][{_categoryName}] {message}");
        _logFileWriter.Flush();
    }
}
