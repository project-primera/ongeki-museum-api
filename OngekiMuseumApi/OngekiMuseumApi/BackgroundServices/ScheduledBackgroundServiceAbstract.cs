using OngekiMuseumApi.Extensions;

namespace OngekiMuseumApi.BackgroundServices
{
    /// <summary>
    /// スケジュールされたバックグラウンドサービスの抽象基底クラス
    /// </summary>
    public abstract class ScheduledBackgroundServiceAbstract : BackgroundService
    {
        protected readonly ILogger _logger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger">ロガー</param>
        protected ScheduledBackgroundServiceAbstract(ILogger logger)
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
            var firstLogString = "";
            // 開始したこと、初回実行をするか、する場合は何分遅れで実行するかをログに出力
            firstLogString = $"[バックグラウンドサービス] {GetServiceName()}が開始されました";

            // 初回実行するかどうかをログに出力
            if (IsFirstRun())
            {
                var delayTime = GetFirstDelayTime();
                firstLogString += $"\n初回実行されます / 遅延時間: {delayTime}";
            }
            else
            {
                firstLogString += "\n初回実行されません";
            }

            _logger.LogInformationWithSlack(firstLogString);

            // 初回実行（オプション）
            if (IsFirstRun())
            {
                await Task.Delay(GetFirstDelayTime(), stoppingToken);
                _logger.LogInformationWithSlack($"[バックグラウンドサービス] {GetServiceName()}を初回実行します");
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

                    _logger.LogInformationWithSlack(
                        $"[バックグラウンドサービス] 次回の{GetServiceName()} は {nextExecutionTime:yyyy/MM/dd HH:mm:ss} に実行されます ({delay:dd\\.hh\\:mm\\:ss}後)",
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
                    _logger.LogErrorWithSlack(ex, $"[バックグラウンドサービス] {GetServiceName()}でエラーが発生しました");

                    // エラー発生時の待機時間
                    await Task.Delay(GetErrorRetryDelayTime(), stoppingToken);
                }
            }

            _logger.LogInformationWithSlack($"[バックグラウンドサービス] {GetServiceName()}が停止しました");
        }

        /// <summary>
        /// 次の待機時間を計算する
        /// </summary>
        /// <returns>待機時間</returns>
        private TimeSpan CalculateNextExecutionTime()
        {
// #if DEBUG
            // return (TimeSpan.FromMinutes(1));
// #endif

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
            _logger.LogInformationWithSlack($"[バックグラウンドサービス] {GetServiceName()} を実行します");

            try
            {
                await TaskAsync();
            }
            catch (Exception ex)
            {
                _logger.LogErrorWithSlack(ex, $"[バックグラウンドサービス] {GetServiceName()} でエラーが発生しました");
            }

            _logger.LogInformationWithSlack($"[バックグラウンドサービス] {GetServiceName()} が完了しました");
        }

        /// <summary>
        /// バックグラウンドサービスの実装
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
