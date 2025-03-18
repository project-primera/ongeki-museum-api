using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OngekiMuseumApi.Data;
using OngekiMuseumApi.Extensions;

namespace OngekiMuseumApi.Controllers.Api;

/// <summary>
/// 譜面データを提供するコントローラー
/// </summary>
[ApiController]
[Route("api/chart")]
public class ChartController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ChartController> _logger;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">データベースコンテキスト</param>
    /// <param name="logger">ロガー</param>
    public ChartController(ApplicationDbContext context, ILogger<ChartController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// すべての譜面データを取得
    /// </summary>
    /// <returns>譜面データのリスト</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformationWithSlack("譜面データの全件取得APIが呼び出されました");

        try
        {
            var charts = await _context.Charts.ToListAsync();

            // 匿名オブジェクトを作成してIdを除外
            var result = charts.Select(chart => new
            {
                chart.Uuid,
                chart.SongUuid,
                Difficulty = chart.Difficulty.ToString(),
                chart.Level,
                chart.IsBonus,
            }).OrderByDescending(chart => chart.SongUuid)
              .ThenBy(chart => chart.Difficulty)
              .ToList();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogErrorWithSlack($"譜面データの取得中にエラーが発生しました: {ex.Message}");
            return StatusCode(500, "内部サーバーエラーが発生しました");
        }
    }

    /// <summary>
    /// 譜面データと楽曲データを結合して取得
    /// </summary>
    /// <returns>譜面データと楽曲データを結合したリスト</returns>
    [HttpGet("join")]
    public async Task<IActionResult> GetJoin()
    {
        _logger.LogInformationWithSlack("譜面データと楽曲データの結合取得APIが呼び出されました");

        try
        {
            var joinedData = await _context.Charts
                .Join(
                    _context.Songs,
                    chart => chart.SongUuid,
                    song => song.Uuid,
                    (chart, song) => new
                    {
                        ChartUuid = chart.Uuid,
                        SongUuid = song.Uuid,
                        Difficulty = chart.Difficulty.ToString(),
                        chart.Level,
                        chart.IsBonus,
                        song.Title,
                        song.Artist,
                        Copyright = song.Copyright ?? "",
                        song.AddedAt,
                    })
                .OrderByDescending(data => data.AddedAt)
                .ThenBy(data => data.ChartUuid)
                .ToListAsync();

            return Ok(joinedData);
        }
        catch (Exception ex)
        {
            _logger.LogErrorWithSlack($"譜面データと楽曲データの結合取得中にエラーが発生しました: {ex.Message}");
            return StatusCode(500, "内部サーバーエラーが発生しました");
        }
    }
}
