using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace OngekiMuseumApi.Services;

/// <summary>
/// ログメッセージをSlackに送信するサービス
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="configuration">設定</param>
/// <param name="slackWebhookService">Slack Webhookサービス</param>
public class SlackLoggerService(
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory
    ) : ISlackLoggerService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

    /// <inheritdoc />
    public void SendLogToSlack(LogLevel logLevel, string message, params object[] args)
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
    public void SendLogToSlack(LogLevel logLevel, Exception exception, string message, params object[] args)
    {
        var formattedMessage = string.Format(message, args);
        var webhookUrl = GetWebhookUrlForLogLevel(logLevel);

        if (string.IsNullOrEmpty(webhookUrl))
        {
            return;
        }

        var fullMessage = $"{formattedMessage}\n\nException: {exception.GetType().Name}\nMessage: {exception.Message}\nStackTrace: {exception.StackTrace}";
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
            LogLevel.Trace => _configuration["Slack:TraceWebhookUrl"] ?? string.Empty,
            LogLevel.Debug => _configuration["Slack:DebugWebhookUrl"] ?? string.Empty,
            LogLevel.Information => _configuration["Slack:InformationWebhookUrl"] ?? string.Empty,
            LogLevel.Warning => _configuration["Slack:WarningWebhookUrl"] ?? string.Empty,
            LogLevel.Error => _configuration["Slack:ErrorWebhookUrl"] ?? string.Empty,
            LogLevel.Critical => _configuration["Slack:CriticalWebhookUrl"] ?? string.Empty,
            _ => _configuration["Slack:CriticalWebhookUrl"] ?? string.Empty
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

        var client = _httpClientFactory.CreateClient();
        var content = new StringContent(JsonSerializer.Serialize(messageData), Encoding.UTF8, "application/json");

        client.PostAsync(url, content).ConfigureAwait(false);
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
