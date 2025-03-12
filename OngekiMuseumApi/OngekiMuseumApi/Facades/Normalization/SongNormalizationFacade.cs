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
        try
        {
            _logger.LogInformationWithSlack("楽曲情報の正規化を開始します");

            // OfficialMusicテーブルから楽曲情報を抽出（dateの値で古い順にソート）
            // ルナティック楽曲とボーナス楽曲は除外
            var officialMusics = await _context.OfficialMusics
                .Where(m => m.Title != null && m.Artist != null && m.IdString != null)
                .Where(m => m.Lunatic != "1") // ルナティック楽曲を除外
                .Where(m => m.Bonus != "1")   // ボーナス楽曲を除外
                .OrderBy(m => m.Date) // 追加日の古い順に処理
                .OrderBy(m => m.IdString) // 追加日の古い順に処理
                .ToListAsync();

            if (officialMusics.Count == 0)
            {
                _logger.LogWarningWithSlack("抽出可能な楽曲情報がありません");
                return 0;
            }

            _logger.LogInformationWithSlack($"{officialMusics.Count}件の楽曲情報を抽出しました");

            int addedCount = 0;

            // 追加日の古い順に処理
            foreach (var music in officialMusics)
            {
                // nullチェック（念のため）
                if (string.IsNullOrEmpty(music.Title) || string.IsNullOrEmpty(music.Artist) || string.IsNullOrEmpty(music.IdString))
                {
                    continue;
                }

                // IdStringをintに変換
                if (!int.TryParse(music.IdString, out var songId))
                {
                    _logger.LogWarningWithSlack($"IdString '{music.IdString}' をint型に変換できませんでした");
                    continue;
                }

                // 既存の楽曲を検索（楽曲名とアーティスト名で検索）
                var existingSong = await _context.Set<Song>()
                    .FirstOrDefaultAsync(s => s.Title == music.Title && s.Artist == music.Artist);

                // 追加日時の設定
                DateTimeOffset addedAt = GetAddedAtFromDateString(music.Date);

                // 著作権情報の設定（"-"の場合はnull）
                string? copyright = music.Copyright1 == "-" ? null : music.Copyright1;

                if (existingSong != null)
                {
                    // 既存データを更新
                    bool isUpdated = false;

                    if (existingSong.Title != music.Title)
                    {
                        existingSong.Title = music.Title;
                        isUpdated = true;
                    }

                    if (existingSong.Artist != music.Artist)
                    {
                        existingSong.Artist = music.Artist;
                        isUpdated = true;
                    }

                    if (existingSong.Copyright != copyright)
                    {
                        existingSong.Copyright = copyright;
                        isUpdated = true;
                    }

                    if (existingSong.AddedAt != addedAt)
                    {
                        existingSong.AddedAt = addedAt;
                        isUpdated = true;
                    }

                    if (isUpdated)
                    {
                        _context.Update(existingSong);
                    }
                }
                else
                {
                    // 新規データを追加
                    var newSong = new Song
                    {
                        Id = songId,
                        Title = music.Title,
                        Artist = music.Artist,
                        Copyright = copyright,
                        AddedAt = addedAt
                    };

                    await _context.AddAsync(newSong);
                    addedCount++;
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformationWithSlack($"{addedCount}件の新規楽曲データを保存しました");

            return addedCount;
        }
        catch (Exception ex)
        {
            _logger.LogErrorWithSlack(ex, "楽曲情報の正規化・保存中にエラーが発生しました");
            throw;
        }
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
