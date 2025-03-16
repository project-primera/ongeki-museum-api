using Microsoft.EntityFrameworkCore;
using OngekiMuseumApi.Data;
using OngekiMuseumApi.Extensions;
using OngekiMuseumApi.Models;

namespace OngekiMuseumApi.Facades.Normalization;

/// <summary>
/// チャプター情報の正規化を行うファサード
/// </summary>
public class ChapterNormalizationFacade : IChapterNormalizationFacade
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ChapterNormalizationFacade> _logger;

    /// <inheritdoc />
    public int ExecutionOrder => 20;

    /// <inheritdoc />
    public string Name => "チャプター情報の正規化";

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">データベースコンテキスト</param>
    /// <param name="logger">ロガー</param>
    public ChapterNormalizationFacade(ApplicationDbContext context, ILogger<ChapterNormalizationFacade> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<int> NormalizeAsync()
    {
        try
        {
            _logger.LogInformationWithSlack("チャプター情報の正規化を開始します");

            // OfficialMusicテーブルから一意のChapIdとChapter名を抽出
            var chapters = await _context.OfficialMusics
                .Where(m => m.ChapId != null && m.Chapter != null)
                .Select(m => new { m.ChapId, m.Chapter })
                .Distinct()
                .ToListAsync();

            if (chapters.Count == 0)
            {
                _logger.LogWarningWithSlack("抽出可能なチャプター情報がありません");
                return 0;
            }

            _logger.LogInformationWithSlack($"{chapters.Count}件のチャプター情報を抽出しました");

            int addedCount = 0;

            foreach (var chapterInfo in chapters)
            {
                // nullチェック（念のため）
                if (string.IsNullOrEmpty(chapterInfo.ChapId) || string.IsNullOrEmpty(chapterInfo.Chapter))
                {
                    continue;
                }

                // ChapIdをintに変換
                if (!int.TryParse(chapterInfo.ChapId, out var chapId))
                {
                    _logger.LogWarningWithSlack($"ChapId '{chapterInfo.ChapId}' をint型に変換できませんでした");
                    continue;
                }

                // 既存のチャプターを検索
                var existingChapter = await _context.Chapters
                    .FirstOrDefaultAsync(c => c.Name == chapterInfo.Chapter);

                if (existingChapter is null)
                {
                    // 新規データを追加
                    var newChapter = new Chapter
                    {
                        OfficialId = chapId,
                        Name = chapterInfo.Chapter,
                        Uuid = Guid.CreateVersion7(),
                    };

                    await _context.Chapters.AddAsync(newChapter);
                    addedCount++;
                }
                else
                {
                    // 既存データを更新（名前が変わっている可能性があるため）
                    if (existingChapter.Name != chapterInfo.Chapter)
                    {
                        existingChapter.OfficialId = chapId;
                        existingChapter.Name = chapterInfo.Chapter;
                        _context.Chapters.Update(existingChapter);
                    }
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformationWithSlack($"{addedCount}件の新規チャプターデータを保存しました");

            return addedCount;
        }
        catch (Exception ex)
        {
            _logger.LogErrorWithSlack(ex, "チャプター情報の正規化・保存中にエラーが発生しました");
            throw;
        }
    }
}
