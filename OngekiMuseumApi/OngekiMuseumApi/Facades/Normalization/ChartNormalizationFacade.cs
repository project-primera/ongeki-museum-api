using Microsoft.EntityFrameworkCore;
using OngekiMuseumApi.Data;
using OngekiMuseumApi.Extensions;
using OngekiMuseumApi.Models;

namespace OngekiMuseumApi.Facades.Normalization;

/// <summary>
/// 譜面情報の正規化を行うファサード
/// </summary>
public class ChartNormalizationFacade : IChartNormalizationFacade
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ChartNormalizationFacade> _logger;

    /// <inheritdoc />
    public int ExecutionOrder => 40;

    /// <inheritdoc />
    public string Name => "譜面情報の正規化";

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">データベースコンテキスト</param>
    /// <param name="logger">ロガー</param>
    public ChartNormalizationFacade(ApplicationDbContext context, ILogger<ChartNormalizationFacade> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// 譜面情報の正規化を行います
    /// </summary>
    public async Task<int> NormalizeAsync()
    {
        try
        {
            _logger.LogInformationWithSlack("譜面情報の正規化を開始します");

            // 楽曲データを取得
            var songs = await _context.Songs.ToListAsync();
            if (songs.Count == 0)
            {
                _logger.LogWarningWithSlack("正規化対象の楽曲データがありません");
                return 0;
            }

            // OfficialMusicテーブルからデータを取得
            var officialMusics = await _context.OfficialMusics.ToListAsync();
            if (officialMusics.Count == 0)
            {
                _logger.LogWarningWithSlack("正規化対象の公式楽曲データがありません");
                return 0;
            }

            int addedCount = 0;

            // 楽曲ごとに処理
            foreach (var song in songs)
            {
                // 対応するOfficialMusicを検索
                var officialMusic = officialMusics.FirstOrDefault(m => m.Uuid == song.OfficialUuid);
                if (officialMusic == null)
                {
                    _logger.LogWarningWithSlack($"楽曲UUID '{song.Uuid}' に対応する公式楽曲データが見つかりませんでした");
                    continue;
                }

                // 難易度ごとに譜面データを作成
                addedCount += await ProcessDifficultyAsync(officialMusic, Difficulty.Basic, officialMusic.LevBas, song.Uuid);
                addedCount += await ProcessDifficultyAsync(officialMusic, Difficulty.Advanced, officialMusic.LevAdv, song.Uuid);
                addedCount += await ProcessDifficultyAsync(officialMusic, Difficulty.Expert, officialMusic.LevExc, song.Uuid);
                addedCount += await ProcessDifficultyAsync(officialMusic, Difficulty.Master, officialMusic.LevMas, song.Uuid);
                addedCount += await ProcessDifficultyAsync(officialMusic, Difficulty.Lunatic, officialMusic.LevLnt, song.Uuid);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformationWithSlack($"{addedCount}件の新規譜面データを保存しました");

            return addedCount;
        }
        catch (Exception ex)
        {
            _logger.LogErrorWithSlack(ex, "譜面情報の正規化・保存中にエラーが発生しました");
            throw;
        }
    }

    /// <summary>
    /// 難易度ごとの譜面データを処理します
    /// </summary>
    /// <param name="music">公式楽曲データ</param>
    /// <param name="difficulty">難易度</param>
    /// <param name="levelString">難易度レベル文字列</param>
    /// <param name="songUuid">楽曲UUID</param>
    /// <returns>追加された譜面の数</returns>
    private async Task<int> ProcessDifficultyAsync(OfficialMusic music, Difficulty difficulty, string? levelString, Guid songUuid)
    {
        // レベルが設定されていない場合はスキップ
        if (string.IsNullOrEmpty(levelString))
        {
            return 0;
        }

        // レベル値を解析（10.0なら100、10+なら105として格納）
        int levelValue = ParseLevelString(levelString);
        if (levelValue < 0)
        {
            _logger.LogWarningWithSlack($"難易度レベル '{levelString}' を解析できませんでした");
            return 0;
        }

        // ボーナスフラグを解析
        bool isBonus = !string.IsNullOrEmpty(music.Bonus) && music.Bonus == "1";

        // 既存の譜面を検索
        var existingChart = await _context.Set<Chart>()
            .FirstOrDefaultAsync(c => c.SongUuid == songUuid && c.Difficulty == difficulty);

        if (existingChart is null)
        {
            // 新規データを追加
            var newChart = new Chart
            {
                Uuid = Guid.CreateVersion7(),
                SongUuid = songUuid,
                Difficulty = difficulty,
                Level = levelValue,
                IsBonus = isBonus,
                IsDeleted = music.IsDeleted
            };

            await _context.Set<Chart>().AddAsync(newChart);
            return 1;
        }
        else
        {
            // 既存データを更新
            bool updated = false;

            if (existingChart.Level != levelValue)
            {
                existingChart.Level = levelValue;
                updated = true;
            }

            if (existingChart.IsBonus != isBonus)
            {
                existingChart.IsBonus = isBonus;
                updated = true;
            }

            if (existingChart.IsDeleted != music.IsDeleted)
            {
                existingChart.IsDeleted = music.IsDeleted;
                updated = true;
            }

            if (updated)
            {
                _context.Set<Chart>().Update(existingChart);
            }

            return 0;
        }
    }

    /// <summary>
    /// 難易度レベル文字列を解析して数値に変換します
    /// </summary>
    /// <param name="levelString">難易度レベル文字列（例: "10.0", "10+"）</param>
    /// <returns>変換後の数値（10.0なら100、10+なら105）</returns>
    private int ParseLevelString(string levelString)
    {
        // "10+"形式の場合
        if (levelString.EndsWith("+"))
        {
            if (int.TryParse(levelString.TrimEnd('+'), out var baseLevel))
            {
                return baseLevel * 10 + 5; // 10+なら105
            }
        }
        // "10.0"形式の場合
        else if (decimal.TryParse(levelString, out var decimalLevel))
        {
            return (int)(decimalLevel * 10); // 10なら100
        }

        return -1; // 解析失敗
    }
}
