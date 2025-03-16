namespace OngekiMuseumApi.Facades.Normalization;

/// <summary>
/// 譜面情報の正規化を行うファサードのインターフェース
/// </summary>
public interface IChartNormalizationFacade : INormalizationFacade
{
    /// <summary>
    /// 譜面情報の正規化を行います
    /// </summary>
    /// <returns>追加された譜面の数</returns>
    Task<int> NormalizeAsync();
}
