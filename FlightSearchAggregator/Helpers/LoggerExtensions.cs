using System.Runtime.CompilerServices;

namespace FlightSearchAggregator.Helpers;

public static class LoggerExtensions
{
    public static void LogInformationWithMethod(this ILogger logger, string message, [CallerMemberName] string methodName = "")
    {
        logger.LogInformation($"{methodName}. {message}");
    }

    public static void LogErrorWithMethod(this ILogger logger, Exception ex, string message, [CallerMemberName] string methodName = "")
    {
        logger.LogError(ex, $"{methodName}. {message}");
    }

}