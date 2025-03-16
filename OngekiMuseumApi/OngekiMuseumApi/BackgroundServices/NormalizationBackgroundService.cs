using OngekiMuseumApi.Extensions;
using OngekiMuseumApi.Facades.Normalization;

namespace OngekiMuseumApi.BackgroundServices
{
    /// <summary>
    /// 正規化処理を定期的に実行するバックグラウンドサービス
    /// </summary>
    public class NormalizationBackgroundService : ScheduledBackgroundServiceAbstract, INormalizationBackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="serviceScopeFactory">サービススコープファクトリ</param>
        /// <param name="logger">ロガー</param>
        public NormalizationBackgroundService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<NormalizationBackgroundService> logger)
            : base(logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <inheritdoc />
        protected override string GetServiceName() => "公式データの正規化";

        /// <inheritdoc />
        protected override (int Hour, int Minute, int Second) GetExecutionTime() => (7, 30, 0);

        /// <inheritdoc />
        protected override bool IsFirstRun() => true;

        /// <inheritdoc />
        protected override TimeSpan GetFirstDelayTime() => TimeSpan.FromMinutes(3);

        /// <inheritdoc />
        protected override async Task TaskAsync()
        {
            // スコープを作成して、ファサードを取得
            using var scope = _serviceScopeFactory.CreateScope();
            var facades = scope.ServiceProvider.GetServices<INormalizationFacade>()
                .OrderBy(f => f.ExecutionOrder)
                .ToList();

            _logger.LogInformationWithSlack($"正規化処理を実行します。実行順序: {string.Join(" -> ", facades.Select(f => f.Name))}");

            // 各正規化ファサードを実行
            foreach (var facade in facades)
            {
                try
                {
                    await facade.NormalizeAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogErrorWithSlack(ex, $"正規化処理中にエラーが発生しました");
                }
            }
        }
    }
}
