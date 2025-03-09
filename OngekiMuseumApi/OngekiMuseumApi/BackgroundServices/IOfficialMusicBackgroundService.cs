namespace OngekiMuseumApi.BackgroundServices
{
    /// <summary>
    /// ONGEKI公式楽曲データを定期的に取得するバックグラウンドサービスのインターフェース
    /// </summary>
    public interface IOfficialMusicBackgroundService : IHostedService
    {

        /// <summary>
        /// バックグラウンドサービスを開始する
        /// </summary>
        /// <param name="cancellationToken">キャンセレーショントークン</param>
        /// <returns>タスク</returns>
        new Task StartAsync(CancellationToken cancellationToken);

        /// <summary>
        /// バックグラウンドサービスを停止する
        /// </summary>
        /// <param name="cancellationToken">キャンセレーショントークン</param>
        /// <returns>タスク</returns>
        new Task StopAsync(CancellationToken cancellationToken);
    }
}
