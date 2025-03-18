using OngekiMuseumApi.Extensions;

namespace OngekiMuseumApi.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next) {

    /// <summary>
    /// ハンドルされていない例外があればキャッチしてSlackに通知する
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    public async Task InvokeAsync(
        HttpContext context,
        ILogger<ExceptionHandlerMiddleware> logger
    ) {
        try {
            await next(context).ConfigureAwait(false);
        } catch (Exception e) {
            logger.LogCriticalWithSlack(e, "ハンドルされていない例外が発生しました。");
        }
    }
}
