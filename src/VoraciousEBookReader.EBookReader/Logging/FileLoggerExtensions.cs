
namespace Microsoft.Extensions.Logging;

public static class FileLoggerExtension
{
    public static ILoggerFactory AddFile(this ILoggerFactory factory, string fileName)
    {
        factory.AddProvider(new FileLogger(fileName));
        return factory;
    }
}

