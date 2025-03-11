using Microsoft.Extensions.Logging;
using OngekiMuseumApi.Helpers;
using OngekiMuseumApi.Services;

namespace OngekiMuseumApi.Extensions;

/// <summary>
/// ILoggerの拡張メソッド
/// </summary>
public static class LoggerExtensions
{
    /// <summary>
    /// トレースログをSlackに送信する
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="message">メッセージ</param>
    /// <param name="args">メッセージのフォーマット引数</param>
    public static void LogTraceWithSlack(this ILogger logger, string message, params object[] args)
    {
        logger.LogTrace(message, args);
        var slackLoggerService = ServiceProviderStaticHelper.GetService<ISlackLoggerService>();
        slackLoggerService.SendLogToSlack(LogLevel.Trace, message, args);
    }

    /// <summary>
    /// デバッグログをSlackに送信する
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="message">メッセージ</param>
    /// <param name="args">メッセージのフォーマット引数</param>
    public static void LogDebugWithSlack(this ILogger logger, string message, params object[] args)
    {
        logger.LogDebug(message, args);
        var slackLoggerService = ServiceProviderStaticHelper.GetService<ISlackLoggerService>();
        slackLoggerService.SendLogToSlack(LogLevel.Debug, message, args);
    }

    /// <summary>
    /// 情報ログをSlackに送信する
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="message">メッセージ</param>
    /// <param name="args">メッセージのフォーマット引数</param>
    public static void LogInformationWithSlack(this ILogger logger, string message, params object[] args)
    {
        logger.LogInformation(message, args);
        var slackLoggerService = ServiceProviderStaticHelper.GetService<ISlackLoggerService>();
        slackLoggerService.SendLogToSlack(LogLevel.Information, message, args);
    }

    /// <summary>
    /// 警告ログをSlackに送信する
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="message">メッセージ</param>
    /// <param name="args">メッセージのフォーマット引数</param>
    public static void LogWarningWithSlack(this ILogger logger, string message, params object[] args)
    {
        logger.LogWarning(message, args);
        var slackLoggerService = ServiceProviderStaticHelper.GetService<ISlackLoggerService>();
        slackLoggerService.SendLogToSlack(LogLevel.Warning, message, args);
    }

    /// <summary>
    /// エラーログをSlackに送信する
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="message">メッセージ</param>
    /// <param name="args">メッセージのフォーマット引数</param>
    public static void LogErrorWithSlack(this ILogger logger, string message, params object[] args)
    {
        logger.LogError(message, args);
        var slackLoggerService = ServiceProviderStaticHelper.GetService<ISlackLoggerService>();
        slackLoggerService.SendLogToSlack(LogLevel.Error, message, args);
    }

    /// <summary>
    /// 例外情報を含むエラーログをSlackに送信する
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="exception">例外</param>
    /// <param name="message">メッセージ</param>
    /// <param name="args">メッセージのフォーマット引数</param>
    public static void LogErrorWithSlack(this ILogger logger, Exception exception, string message, params object[] args)
    {
        logger.LogError(exception, message, args);
        var slackLoggerService = ServiceProviderStaticHelper.GetService<ISlackLoggerService>();
        slackLoggerService.SendLogToSlack(LogLevel.Error, exception, message, args);
    }

    /// <summary>
    /// クリティカルログをSlackに送信する
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="message">メッセージ</param>
    /// <param name="args">メッセージのフォーマット引数</param>
    public static void LogCriticalWithSlack(this ILogger logger, string message, params object[] args)
    {
        logger.LogCritical(message, args);
        var slackLoggerService = ServiceProviderStaticHelper.GetService<ISlackLoggerService>();
        slackLoggerService.SendLogToSlack(LogLevel.Critical, message, args);
    }

    /// <summary>
    /// 例外情報を含むクリティカルログをSlackに送信する
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="exception">例外</param>
    /// <param name="message">メッセージ</param>
    /// <param name="args">メッセージのフォーマット引数</param>
    public static void LogCriticalWithSlack(this ILogger logger, Exception exception, string message, params object[] args)
    {
        logger.LogCritical(exception, message, args);
        var slackLoggerService = ServiceProviderStaticHelper.GetService<ISlackLoggerService>();
        slackLoggerService.SendLogToSlack(LogLevel.Critical, exception, message, args);
    }
}
