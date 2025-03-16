using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OngekiMuseumApi.Data;
using OngekiMuseumApi.Extensions;

namespace OngekiMuseumApi.Controllers.Api;

/// <summary>
/// 公式楽曲データを提供するコントローラー
/// </summary>
[ApiController]
[Route("api/official-music")]
public class OfficialMusicController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OfficialMusicController> _logger;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">データベースコンテキスト</param>
    /// <param name="logger">ロガー</param>
    public OfficialMusicController(ApplicationDbContext context, ILogger<OfficialMusicController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// すべての公式楽曲データを取得
    /// </summary>
    /// <returns>公式楽曲データのリスト</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformationWithSlack("公式楽曲データの全件取得APIが呼び出されました");

        try
        {
            var officialMusics = await _context.OfficialMusics
                .Where(m => !m.IsDeleted)
                .ToListAsync();

            // 匿名オブジェクトを作成してIdを除外し、nullの場合は空文字列に変換
            var result = officialMusics.Select(music => new
            {
                New = music.New ?? "",
                Date = music.Date ?? "",
                Title = music.Title ?? "",
                TitleSort = music.TitleSort ?? "",
                Artist = music.Artist ?? "",
                IdString = music.IdString ?? "",
                ChapId = music.ChapId ?? "",
                Chapter = music.Chapter ?? "",
                Character = music.Character ?? "",
                CharaId = music.CharaId ?? "",
                Category = music.Category ?? "",
                CategoryId = music.CategoryId ?? "",
                Lunatic = music.Lunatic ?? "",
                Bonus = music.Bonus ?? "",
                Copyright1 = music.Copyright1 ?? "",
                LevBas = music.LevBas ?? "",
                LevAdv = music.LevAdv ?? "",
                LevExc = music.LevExc ?? "",
                LevMas = music.LevMas ?? "",
                LevLnt = music.LevLnt ?? "",
                ImageUrl = music.ImageUrl ?? ""
            }).OrderByDescending(music => music.Date).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogErrorWithSlack($"公式楽曲データの取得中にエラーが発生しました: {ex.Message}");
            return StatusCode(500, "内部サーバーエラーが発生しました");
        }
    }
}
