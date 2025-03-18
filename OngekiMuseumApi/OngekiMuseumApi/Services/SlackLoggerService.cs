using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OngekiMuseumApi.Services;

/// <summary>
/// ログメッセージをSlackに送信するサービス
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="configuration">設定</param>
public class SlackLoggerService(
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory
    ) : ISlackLoggerService
{
    /// <inheritdoc />
    public void SendLogToSlack(LogLevel logLevel, string message, params object?[] args)
    {
        var formattedMessage = string.Format(message, args);
        var webhookUrl = GetWebhookUrlForLogLevel(logLevel);

        if (string.IsNullOrEmpty(webhookUrl))
        {
            return;
        }

        SendWebhook(formattedMessage, webhookUrl);
    }

    /// <inheritdoc />
    public void SendLogToSlack(LogLevel logLevel, Exception exception, string message, params object?[] args)
    {
        var formattedMessage = string.Format(message, args);
        var webhookUrl = GetWebhookUrlForLogLevel(logLevel);

        if (string.IsNullOrEmpty(webhookUrl))
        {
            return;
        }

        var fullMessage = $"{formattedMessage}\n```\nException: {exception.GetType().Name}\nMessage: {exception.Message}\nStackTrace\n{exception.StackTrace}\n```";
        SendWebhook(fullMessage, webhookUrl);
    }

    /// <summary>
    /// ログレベルに応じたWebhook URLを取得する
    /// </summary>
    /// <param name="logLevel">ログレベル</param>
    /// <returns>Webhook URL</returns>
    private string GetWebhookUrlForLogLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => configuration["Slack:TraceWebhookUrl"] ?? string.Empty,
            LogLevel.Debug => configuration["Slack:DebugWebhookUrl"] ?? string.Empty,
            LogLevel.Information => configuration["Slack:InformationWebhookUrl"] ?? string.Empty,
            LogLevel.Warning => configuration["Slack:WarningWebhookUrl"] ?? string.Empty,
            LogLevel.Error => configuration["Slack:ErrorWebhookUrl"] ?? string.Empty,
            LogLevel.Critical => configuration["Slack:CriticalWebhookUrl"] ?? string.Empty,
            _ => configuration["Slack:CriticalWebhookUrl"] ?? string.Empty
        };
    }

    /// <summary>
    /// Webhookにメッセージを送信する
    /// </summary>
    /// <param name="message"></param>
    /// <param name="url"></param>
    private void SendWebhook(string message, string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return;
        }

        var messageData = new MessageData
        {
            Text = message
        };

        var client = httpClientFactory.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(messageData), Encoding.UTF8, "application/json");

        try
        {
            client.PostAsync(url, content).ConfigureAwait(false);
        }
        catch (Exception)
        {
            // Webhookはエラーが発生しても処理を続行する
        }
    }

    /// <summary>
    /// SlackのWebhookに送信するメッセージデータ
    /// </summary>
    private sealed class MessageData
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
}
