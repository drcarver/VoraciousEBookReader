using System.Runtime.CompilerServices;

using VoraciousEBookReader.EBookReader.Logging;

namespace Microsoft.Extensions.Logging;

/// <summary>
/// Customized ILoggerProvider, writes logs to text files 
/// </summary>
public class FileLogger : ILoggerProvider
{
    // The log file writer
    private readonly StreamWriter _logFileWriter;

    //Define the path to the text file
    public string LogFilePath { get; set; }
    
    /// <summary>
    /// The custom file logger for writing to a file
    /// </summary>
    /// <param name="fpath">The path to the file.</param>
    public FileLogger(string fpath)
    {
        // Create a StreamWriter to write logs to a text file
        FileInfo fileInfo = new FileInfo(fpath);
        
        LogFilePath = $"{fileInfo.DirectoryName}\\logfile.txt";
        _logFileWriter = new StreamWriter(LogFilePath, append: true);
    }

    /// <summary>
    /// The file to write to
    /// </summary>
    /// <param name="logFileWriter">The log file StreamWriter</param>
    /// <exception cref="ArgumentNullException">The StreamWriter is null</exception>
    public FileLogger(StreamWriter logFileWriter)
    {
        _logFileWriter = logFileWriter ?? throw new ArgumentNullException(nameof(logFileWriter));
    }

    /// <summary>
    /// Create a logger for the categoryName
    /// </summary>
    /// <param name="categoryName">The category name</param>
    /// <returns>The ILogger entry</returns>
    public ILogger CreateLogger(string categoryName)
    {
        return new CustomFileLogger(categoryName, _logFileWriter);
    }

    /// <summary>
    /// Dispose of the file.
    /// </summary>
    public void Dispose()
    {
        _logFileWriter.Dispose();
    }
}
