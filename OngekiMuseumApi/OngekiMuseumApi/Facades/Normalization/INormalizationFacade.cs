namespace OngekiMuseumApi.Facades.Normalization;

/// <summary>
/// 正規化サービスのファサードインターフェース
/// </summary>
public interface INormalizationFacade
{
    /// <summary>
    /// 実行順序（小さい値ほど先に実行される）
    /// </summary>
    int ExecutionOrder { get; }

    /// <summary>
    /// 正規化処理の名前
    /// </summary>
    string Name { get; }

    /// <summary>
    /// すべての正規化処理を実行する
    /// </summary>
    /// <returns>正規化処理の結果</returns>
    Task<int> NormalizeAsync();
}
