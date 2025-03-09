namespace OngekiMuseumApi.Services;

/// <summary>
/// ONGEKIのチャプター情報を管理するサービスのインターフェース
/// </summary>
public interface IChapterService
{
    /// <summary>
    /// OfficialMusicテーブルからチャプター情報を抽出し、Chapterテーブルに正規化して保存する
    /// </summary>
    /// <returns>正規化して保存したチャプター数</returns>
    Task<int> NormalizeAndSaveChaptersAsync();

    /// <summary>
    /// すべてのチャプター情報を取得する
    /// </summary>
    /// <returns>チャプター情報のリスト</returns>
    Task<IEnumerable<Models.Chapter>> GetAllChaptersAsync();

    /// <summary>
    /// 指定したChapIdのチャプター情報を取得する
    /// </summary>
    /// <param name="chapId">チャプターID</param>
    /// <returns>チャプター情報、存在しない場合はnull</returns>
    Task<Models.Chapter?> GetChapterByIdAsync(string chapId);
}
