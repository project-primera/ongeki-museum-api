namespace OngekiMuseumApi.Facades.Normalization;

/// <summary>
/// 正規化サービスのファサードインターフェース
/// </summary>
public interface INormalizationFacade
{
    /// <summary>
    /// すべての正規化処理を実行する
    /// </summary>
    /// <returns>正規化処理の結果</returns>
    Task<int> NormalizeAsync();
}
