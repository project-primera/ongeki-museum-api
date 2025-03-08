namespace OngekiMuseumApi.Services;

/// <summary>
/// ONGEKI公式楽曲データの取得と保存を行うサービスのインターフェース
/// </summary>
public interface IOfficialMusicService
{
    /// <summary>
    /// 公式サイトから楽曲データを取得し、データベースに保存する
    /// </summary>
    /// <returns>非同期タスク</returns>
    Task FetchAndSaveOfficialMusicAsync();
}