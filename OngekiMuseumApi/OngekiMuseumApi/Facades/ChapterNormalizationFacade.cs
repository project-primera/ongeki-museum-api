using Microsoft.EntityFrameworkCore;
using OngekiMuseumApi.Data;
using OngekiMuseumApi.Models;

namespace OngekiMuseumApi.Facade;

/// <summary>
/// チャプター情報の正規化を行うファサード
/// </summary>
public class ChapterNormalizationFacade : IChapterNormalizationFacade
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ChapterNormalizationFacade> _logger;

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
            _logger.LogInformation("チャプター情報の正規化を開始します");

            // OfficialMusicテーブルから一意のChapIdとChapter名を抽出
            var chapters = await _context.OfficialMusics
                .Where(m => m.ChapId != null && m.Chapter != null)
                .Select(m => new { m.ChapId, m.Chapter })
                .Distinct()
                .ToListAsync();

            if (chapters.Count == 0)
            {
                _logger.LogWarning("抽出可能なチャプター情報がありません");
                return 0;
            }

            _logger.LogInformation($"{chapters.Count}件のチャプター情報を抽出しました");

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
                    _logger.LogWarning($"ChapId '{chapterInfo.ChapId}' をint型に変換できませんでした");
                    continue;
                }

                // 既存のチャプターを検索
                var existingChapter = await _context.Chapters
                    .FirstOrDefaultAsync(c => c.Id == chapId);

                if (existingChapter != null)
                {
                    // 既存データを更新（名前が変わっている可能性があるため）
                    if (existingChapter.ChapterName != chapterInfo.Chapter)
                    {
                        existingChapter.ChapterName = chapterInfo.Chapter;
                        _context.Chapters.Update(existingChapter);
                    }
                }
                else
                {
                    // 新規データを追加
                    var newChapter = new Chapter
                    {
                        Id = chapId,
                        ChapterName = chapterInfo.Chapter
                    };

                    await _context.Chapters.AddAsync(newChapter);
                    addedCount++;
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation($"{addedCount}件の新規チャプターデータを保存しました");

            return addedCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "チャプター情報の正規化・保存中にエラーが発生しました");
            throw;
        }
    }
}
