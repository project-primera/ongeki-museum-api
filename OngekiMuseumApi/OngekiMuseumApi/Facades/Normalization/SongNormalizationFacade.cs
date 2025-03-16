using Microsoft.EntityFrameworkCore;
using OngekiMuseumApi.Data;
using OngekiMuseumApi.Extensions;
using OngekiMuseumApi.Models;
using System.Globalization;

namespace OngekiMuseumApi.Facades.Normalization;

/// <summary>
/// 楽曲情報の正規化を行うファサード
/// </summary>
public class SongNormalizationFacade : ISongNormalizationFacade
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SongNormalizationFacade> _logger;
    private static readonly TimeZoneInfo JstTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">データベースコンテキスト</param>
    /// <param name="logger">ロガー</param>
    public SongNormalizationFacade(ApplicationDbContext context, ILogger<SongNormalizationFacade> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<int> NormalizeAsync()
    {
        _logger.LogInformationWithSlack("楽曲情報の正規化を開始します");

        // 公式楽曲データを取得（非削除のみ）
        var officialMusics = await _context.OfficialMusics
            .Where(m => !m.IsDeleted)
            .ToListAsync();

        _logger.LogInformationWithSlack($"正規化対象の公式楽曲データ: {officialMusics.Count}件");

        int newCount = 0;
        int updateCount = 0;

        foreach (var officialMusic in officialMusics)
        {
            // 曲名とアーティスト名で既存の楽曲を検索
            var existingSong = await _context.Songs
                .FirstOrDefaultAsync(s =>
                    s.Title == officialMusic.Title &&
                    s.Artist == officialMusic.Artist);

            if (existingSong is null)
            {
                // 新規作成
                var newSong = new Song
                {
                    Uuid = Guid.NewGuid(), // 新しいUUIDを生成
                    OfficialUuid = officialMusic.Uuid,
                    Title = officialMusic.Title ?? string.Empty,
                    Artist = officialMusic.Artist ?? string.Empty,
                    Copyright = officialMusic.Copyright1,
                    AddedAt = GetAddedAtFromDateString(officialMusic.Date),
                };

                await _context.Songs.AddAsync(newSong);
                newCount++;

                _logger.LogDebugWithSlack($"楽曲を新規作成しました: {newSong.Title} - {newSong.Artist}");
            }
            else
            {
                // 更新が必要かどうかを確認
                bool needsUpdate = false;

                // OfficialUuidの確認
                if (existingSong.OfficialUuid != officialMusic.Uuid)
                {
                    existingSong.OfficialUuid = officialMusic.Uuid;
                    needsUpdate = true;
                }

                // Copyrightの確認
                if (existingSong.Copyright != officialMusic.Copyright1)
                {
                    existingSong.Copyright = officialMusic.Copyright1;
                    needsUpdate = true;
                }

                // AddedAtの確認
                var newAddedAt = GetAddedAtFromDateString(officialMusic.Date);
                if (existingSong.AddedAt != newAddedAt)
                {
                    existingSong.AddedAt = newAddedAt;
                    needsUpdate = true;
                }

                if (needsUpdate)
                {
                    existingSong.UpdatedAt = DateTimeOffset.UtcNow;
                    _context.Songs.Update(existingSong);
                    updateCount++;

                    _logger.LogDebugWithSlack($"楽曲を更新しました: {existingSong.Title} - {existingSong.Artist}");
                }
            }
        }

        // 変更をデータベースに保存
        await _context.SaveChangesAsync();

        _logger.LogInformationWithSlack($"楽曲情報の正規化が完了しました: 新規作成 {newCount}件, 更新 {updateCount}件");

        return newCount + updateCount;
    }

    /// <summary>
    /// 日付文字列からAddedAt（JST 7:00）を取得する
    /// </summary>
    /// <param name="dateString">日付文字列（yyyyMMdd形式）</param>
    /// <returns>AddedAtの値</returns>
    private DateTimeOffset GetAddedAtFromDateString(string? dateString)
    {
        if (string.IsNullOrEmpty(dateString) || dateString.Length != 8 || !DateTime.TryParseExact(
                dateString, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            // デフォルト値として現在時刻を返す
            return TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, JstTimeZone);
        }

        // その日の7時（JST）を設定
        var jstDate = new DateTime(date.Year, date.Month, date.Day, 7, 0, 0);
        return new DateTimeOffset(jstDate, TimeSpan.FromHours(9)); // JSTはUTC+9
    }
}
