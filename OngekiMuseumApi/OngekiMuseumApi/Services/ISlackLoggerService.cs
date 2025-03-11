using Microsoft.Extensions.Logging;

namespace OngekiMuseumApi.Services;

/// <summary>
/// ログメッセージをSlackに送信するサービスのインターフェース
/// </summary>
public interface ISlackLoggerService
{
    /// <summary>
    /// ログメッセージをSlackに送信する
    /// </summary>
    /// <param name="logLevel">ログレベル</param>
    /// <param name="message">メッセージ</param>
    /// <param name="args">メッセージのフォーマット引数</param>
    void SendLogToSlack(LogLevel logLevel, string message, params object?[] args);

    /// <summary>
    /// 例外情報を含むログメッセージをSlackに送信する
    /// </summary>
    /// <param name="logLevel">ログレベル</param>
    /// <param name="exception">例外</param>
    /// <param name="message">メッセージ</param>
    /// <param name="args">メッセージのフォーマット引数</param>
    void SendLogToSlack(LogLevel logLevel, Exception exception, string message, params object?[] args);
}
