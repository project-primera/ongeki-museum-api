using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OngekiMuseumApi.Services
{
    /// <summary>
    /// ONGEKI公式楽曲データを定期的に取得するバックグラウンドサービス
    /// </summary>
    public class OfficialMusicBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OfficialMusicBackgroundService> _logger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="serviceProvider">サービスプロバイダ</param>
        /// <param name="logger">ロガー</param>
        public OfficialMusicBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<OfficialMusicBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        /// <summary>
        /// バックグラウンドサービスの実行メソッド
        /// </summary>
        /// <param name="stoppingToken">キャンセレーショントークン</param>
        /// <returns>非同期タスク</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ONGEKI公式楽曲データ取得バックグラウンドサービスが開始されました");

            // 初回実行
            await FetchOfficialMusicDataAsync();

            // 定期実行のループ
            while (!stoppingToken.IsCancellationRequested)
            {
                // 次の実行時間（朝7時JST）を計算
                var now = DateTime.UtcNow;
                var jstNow = TimeZoneInfo.ConvertTimeFromUtc(now, TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time"));
                var jstTarget = new DateTime(jstNow.Year, jstNow.Month, jstNow.Day, 7, 0, 0);

                // 今日の朝7時を過ぎていたら、明日の朝7時に設定
                if (jstNow.TimeOfDay > jstTarget.TimeOfDay)
                {
                    jstTarget = jstTarget.AddDays(1);
                }

                // 次の実行時間までの待機時間を計算
                var utcTarget = TimeZoneInfo.ConvertTimeToUtc(jstTarget);
                var delay = utcTarget - now;

                _logger.LogInformation($"次回の楽曲データ取得は {jstTarget:yyyy/MM/dd HH:mm:ss} (JST) に実行されます");

                // 次の実行時間まで待機
                await Task.Delay(delay, stoppingToken);

                // キャンセルされていなければ実行
                if (!stoppingToken.IsCancellationRequested)
                {
                    await FetchOfficialMusicDataAsync();
                }
            }
        }

        /// <summary>
        /// 公式楽曲データを取得するメソッド
        /// </summary>
        /// <returns>非同期タスク</returns>
        private async Task FetchOfficialMusicDataAsync()
        {
            try
            {
                _logger.LogInformation("定期実行: 公式楽曲データの取得を開始します");

                // スコープを作成してサービスを取得
                using var scope = _serviceProvider.CreateScope();
                var officialMusicService = scope.ServiceProvider.GetRequiredService<IOfficialMusicService>();

                // 楽曲データを取得・保存
                await officialMusicService.FetchAndSaveOfficialMusicAsync();

                _logger.LogInformation("定期実行: 公式楽曲データの取得が完了しました");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "定期実行: 公式楽曲データの取得中にエラーが発生しました");
            }
        }
    }
}
