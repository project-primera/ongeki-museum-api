namespace OngekiMuseumApi.Helpers;

/// <summary>
/// サービスプロバイダーを保持するヘルパークラス
/// </summary>
public static class ServiceProviderStaticHelper
{
    private static IServiceProvider? _serviceProvider;

    /// <summary>
    /// サービスプロバイダーを設定する
    /// </summary>
    /// <param name="serviceProvider">サービスプロバイダー</param>
    public static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// サービスプロバイダーを取得する
    /// </summary>
    /// <returns>サービスプロバイダー</returns>
    /// <exception cref="InvalidOperationException">サービスプロバイダーが設定されていない場合</exception>
    private static IServiceProvider GetServiceProvider()
    {
        if (_serviceProvider == null)
        {
            throw new InvalidOperationException("サービスプロバイダーが設定されていません。Program.csでSetServiceProviderを呼び出してください。");
        }

        return _serviceProvider;
    }

    /// <summary>
    /// サービスを取得する
    /// </summary>
    /// <typeparam name="T">サービスの型</typeparam>
    /// <returns>サービス</returns>
    public static T GetService<T>() where T : notnull
    {
        var serviceProvider = GetServiceProvider();
        var s = serviceProvider.GetService<T>()!;
        return s;
    }
}
