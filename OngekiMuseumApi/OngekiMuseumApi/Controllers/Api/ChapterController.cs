using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OngekiMuseumApi.Data;
using OngekiMuseumApi.Extensions;

namespace OngekiMuseumApi.Controllers.Api;

/// <summary>
/// チャプターデータを提供するコントローラー
/// </summary>
[ApiController]
[Route("api/chapter")]
public class ChapterController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ChapterController> _logger;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">データベースコンテキスト</param>
    /// <param name="logger">ロガー</param>
    public ChapterController(ApplicationDbContext context, ILogger<ChapterController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// すべてのチャプターデータを取得
    /// </summary>
    /// <returns>チャプターデータのリスト</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var chapters = await _context.Chapters.ToListAsync();

            // 匿名オブジェクトを作成してIdを除外し、nullの場合は空文字列に変換
            var result = chapters.Select(chapter => new
            {
                chapter.Uuid,
                chapter.OfficialId,
                Name = chapter.Name ?? "",
            }).OrderBy(chapter => chapter.OfficialId).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogErrorWithSlack($"チャプターデータの取得中にエラーが発生しました: {ex.Message}");
            return StatusCode(500, "内部サーバーエラーが発生しました");
        }
    }
}
