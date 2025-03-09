using OngekiMuseumApi.Services;

namespace OngekiMuseumApi.BackgroundServices
{
    /// <summary>
    /// ONGEKI公式楽曲データを定期的に取得するバックグラウンドサービス
    /// </summary>
    public class OfficialMusicBackgroundService : AbstractScheduledBackgroundService, IOfficialMusicBackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="serviceProvider">サービスプロバイダ</param>
        /// <param name="logger">ロガー</param>
        public OfficialMusicBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<OfficialMusicBackgroundService> logger)
            : base(logger)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        protected override string GetServiceName() => "公式楽曲データ取得";

        /// <inheritdoc />
        protected override (int Hour, int Minute, int Second) GetExecutionTime() => (7, 0, 0);

        /// <inheritdoc />
        protected override bool IsFirstRun() => true;

        /// <inheritdoc />
        protected override async Task TaskAsync()
        {
            // スコープを作成してサービスを取得
            using var scope = _serviceProvider.CreateScope();
            var officialMusicService = scope.ServiceProvider.GetRequiredService<IOfficialMusicService>();

            // 楽曲データを取得・保存
            await officialMusicService.FetchAndSaveOfficialMusicAsync();
        }
    }
}
