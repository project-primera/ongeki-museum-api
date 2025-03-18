using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OngekiMuseumApi.Data;
using OngekiMuseumApi.Extensions;

namespace OngekiMuseumApi.Controllers.Api;

/// <summary>
/// 楽曲データを提供するコントローラー
/// </summary>
[ApiController]
[Route("api/song")]
public class SongController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SongController> _logger;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">データベースコンテキスト</param>
    /// <param name="logger">ロガー</param>
    public SongController(ApplicationDbContext context, ILogger<SongController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// すべての楽曲データを取得
    /// </summary>
    /// <returns>楽曲データのリスト</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformationWithSlack("楽曲データの全件取得APIが呼び出されました");

        try
        {
            var songs = await _context.Songs.ToListAsync();

            // 匿名オブジェクトを作成してIdを除外し、nullの場合は空文字列に変換
            var result = songs.Select(song => new
            {
                song.Uuid,
                song.OfficialUuid,
                Title = song.Title,
                Artist = song.Artist,
                Copyright = song.Copyright ?? "",
                song.AddedAt,
            }).OrderByDescending(song => song.AddedAt).ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogErrorWithSlack($"楽曲データの取得中にエラーが発生しました: {ex.Message}");
            return StatusCode(500, "内部サーバーエラーが発生しました");
        }
    }
}
