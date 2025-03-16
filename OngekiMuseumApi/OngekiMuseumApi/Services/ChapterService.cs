using Microsoft.EntityFrameworkCore;
using OngekiMuseumApi.Data;
using OngekiMuseumApi.Extensions;
using OngekiMuseumApi.Models;

namespace OngekiMuseumApi.Services;

/// <summary>
/// ONGEKIのチャプター情報を管理するサービスの実装
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="context">データベースコンテキスト</param>
/// <param name="logger">ロガー</param>
public class ChapterService(
    ApplicationDbContext context,
    ILogger<ChapterService> logger
    ) : IChapterService {
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<ChapterService> _logger = logger;

    /// <inheritdoc />
    public async Task<int> NormalizeAndSaveChaptersAsync()
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
                _logger.LogWarning("抽出可能なチャプター情報がありません");
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

                // ChapIdをintに
                if (!int.TryParse(chapterInfo.ChapId, out _))
                {
                    _logger.LogWarningWithSlack($"ChapIdが数値ではありません: {chapterInfo.ChapId}");
                    continue;
                }
                var intChapId = int.Parse(chapterInfo.ChapId);

                // 既存のチャプターを検索
                var existingChapter = await _context.Chapters
                    .FirstOrDefaultAsync(c => c.Id == intChapId);

                if (existingChapter != null)
                {
                    // 既存データを更新（名前が変わっている可能性があるため）
                    if (existingChapter.Name != chapterInfo.Chapter)
                    {
                        existingChapter.Name = chapterInfo.Chapter;
                        _context.Chapters.Update(existingChapter);
                    }
                }
                else
                {
                    // 新規データを追加
                    var newChapter = new Chapter
                    {
                        Id = intChapId,
                        Name = chapterInfo.Chapter
                    };

                    await _context.Chapters.AddAsync(newChapter);
                    addedCount++;
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformationWithSlack($"{addedCount}件の新規チャプターデータを保存しました");

            return addedCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "チャプター情報の正規化・保存中にエラーが発生しました");
            throw new InvalidOperationException("チャプター情報の処理中にエラーが発生しました", ex);
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Chapter>> GetAllChaptersAsync()
    {
        return await _context.Chapters
            .OrderBy(c => c.Id)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Chapter?> GetChapterByIdAsync(string chapId)
    {
        // ChapIdをintに
        if (!int.TryParse(chapId, out _))
        {
            _logger.LogWarningWithSlack($"ChapIdが数値ではありません: {chapId}");
        }
        var intChapId = int.Parse(chapId);

        return await _context.Chapters
            .FirstOrDefaultAsync(c => c.Id == intChapId);
    }
}
