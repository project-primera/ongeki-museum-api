namespace OngekiMuseumApi.BackgroundServices
{
    /// <summary>
    /// スケジュールされたバックグラウンドサービスの抽象基底クラス
    /// </summary>
    public abstract class AbstractScheduledBackgroundService : BackgroundService
    {
        protected readonly ILogger _logger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger">ロガー</param>
        protected AbstractScheduledBackgroundService(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// バックグラウンドサービスの実行メソッド
        /// </summary>
        /// <param name="stoppingToken">キャンセレーショントークン</param>
        /// <returns>非同期タスク</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"[定期実行タスク] {GetServiceName()}が開始されました");

            // 初回実行（オプション）
            if (IsFirstRun())
            {
                _logger.LogInformation($"[定期実行タスク] {GetServiceName()}を初回実行します");
                await Task.Delay(GetFirstDelayTime(), stoppingToken);
                await ExecuteScheduledTaskAsync();
            }

            // 定期実行のループ
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // 次の実行時間を計算
                    var delay = CalculateNextExecutionTime();

                    // 次回実行時間を絶対時間で取得
                    var nextExecutionTime = DateTimeOffset.Now.Add(delay);

                    _logger.LogInformation(
                        $"[定期実行タスク] 次回の{GetServiceName()} は {nextExecutionTime:yyyy/MM/dd HH:mm:ss} に実行されます ({delay:dd\\.hh\\:mm\\:ss}後)",
                        nextExecutionTime, delay
                    );

                    // 次の実行時間まで待機
                    await Task.Delay(delay, stoppingToken);

                    // キャンセルされていなければ実行
                    if (!stoppingToken.IsCancellationRequested)
                    {
                        await ExecuteScheduledTaskAsync();
                    }
                }
                catch (OperationCanceledException)
                {
                    // キャンセル要求による例外は正常終了として扱う
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"[定期実行タスク] {GetServiceName()}でエラーが発生しました");

                    // エラー発生時の待機時間
                    await Task.Delay(GetErrorRetryDelayTime(), stoppingToken);
                }
            }

            _logger.LogInformation($"[定期実行タスク] {GetServiceName()}が停止しました");
        }

        /// <summary>
        /// 次の待機時間を計算する
        /// </summary>
        /// <returns>待機時間</returns>
        private TimeSpan CalculateNextExecutionTime()
        {
#if DEBUG
            return (TimeSpan.FromMinutes(5));
#endif

            var (hour, minute, second) = GetExecutionTime();

            var jstNow = TimeZoneInfo.ConvertTime(DateTimeOffset.Now, TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time"));
            var jstTarget = new DateTime(jstNow.Year, jstNow.Month, jstNow.Day, hour, minute, second);
            if (jstNow > jstTarget)
            {
                jstTarget = jstTarget.AddDays(1);
            }

            var delay = jstTarget - jstNow;
            return delay;
        }

        /// <summary>
        /// スケジュールされたタスクを実行する
        /// </summary>
        /// <returns>非同期タスク</returns>
        private async Task ExecuteScheduledTaskAsync()
        {
            _logger.LogInformation("[定期実行タスク] {ServiceName} を実行します", GetServiceName());

            try
            {
                await TaskAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[定期実行タスク] {ServiceName} でエラーが発生しました", GetServiceName());
            }

            _logger.LogInformation("[定期実行タスク] {ServiceName} が完了しました", GetServiceName());
        }

        /// <summary>
        /// 定期実行タスクの実装
        /// </summary>
        /// <returns></returns>
        protected abstract Task TaskAsync();

        /// <summary>
        /// サービス名を取得する
        /// </summary>
        /// <returns>サービス名</returns>
        protected abstract string GetServiceName();

        /// <summary>
        /// 実行時間（時・分・秒）を取得する
        /// </summary>
        /// <returns>実行時間（時・分・秒）のタプル</returns>
        protected abstract (int Hour, int Minute, int Second) GetExecutionTime();

        /// <summary>
        /// エラー発生時の再試行待機時間を取得する
        /// </summary>
        /// <returns>再試行までの待機時間</returns>
        protected virtual TimeSpan GetErrorRetryDelayTime() => TimeSpan.FromMinutes(10);

        /// <summary>
        /// 起動時に即座に実行するかどうか
        /// </summary>
        /// <returns>即座に実行する場合はtrue、そうでない場合はfalse</returns>
        protected virtual bool IsFirstRun() => false;

        /// <summary>
        /// エラー発生時の再試行待機時間を取得する
        /// </summary>
        /// <returns>再試行までの待機時間</returns>
        protected virtual TimeSpan GetFirstDelayTime() => TimeSpan.FromMinutes(0);
    }
}
