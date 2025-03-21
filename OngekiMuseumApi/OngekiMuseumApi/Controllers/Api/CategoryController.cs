using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OngekiMuseumApi.Data;
using OngekiMuseumApi.Extensions;

namespace OngekiMuseumApi.Controllers.Api;

/// <summary>
/// カテゴリデータを提供するコントローラー
/// </summary>
[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CategoryController> _logger;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">データベースコンテキスト</param>
    /// <param name="logger">ロガー</param>
    public CategoryController(ApplicationDbContext context, ILogger<CategoryController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// すべてのカテゴリデータを取得
    /// </summary>
    /// <returns>カテゴリデータのリスト</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var categories = await _context.Categories.ToListAsync();

            // 匿名オブジェクトを作成してIdを除外し、nullの場合は空文字列に変換
            var result = categories.Select(category => new
            {
                category.Uuid,
                category.OfficialId,
                category.Name,
            }).OrderBy(category => category.OfficialId).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogErrorWithSlack($"カテゴリデータの取得中にエラーが発生しました: {ex.Message}");
            return StatusCode(500, "内部サーバーエラーが発生しました");
        }
    }
}
