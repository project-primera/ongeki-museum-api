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
    /// 削除されていない公式楽曲データを取得
    /// </summary>
    /// <returns>削除されていない公式楽曲データのリスト</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var officialMusics = await _context.OfficialMusics
                .Where(m => !m.IsDeleted)
                .ToListAsync();

            var result = CreateMusicResponseList(officialMusics);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogErrorWithSlack($"公式楽曲データの取得中にエラーが発生しました: {ex.Message}");
            return StatusCode(500, "内部サーバーエラーが発生しました");
        }
    }

    /// <summary>
    /// すべての公式楽曲データを取得（削除楽曲を含む）
    /// </summary>
    /// <returns>すべての公式楽曲データのリスト</returns>
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformationWithSlack("公式楽曲データの全件取得APIが呼び出されました（削除楽曲を含む）");

        try
        {
            var officialMusics = await _context.OfficialMusics
                .ToListAsync();

            var result = CreateMusicResponseList(officialMusics);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogErrorWithSlack($"公式楽曲データの取得中にエラーが発生しました: {ex.Message}");
            return StatusCode(500, "内部サーバーエラーが発生しました");
        }
    }

    /// <summary>
    /// 楽曲データのレスポンスリストを作成
    /// </summary>
    /// <param name="officialMusics">楽曲データのリスト</param>
    /// <returns>レスポンス用の楽曲データリスト</returns>
    private static List<object> CreateMusicResponseList(IEnumerable<Models.OfficialMusic> officialMusics)
    {
        return officialMusics.Select(music => new
        {
            @new = music.New ?? "",
            date = music.Date ?? "",
            title = music.Title ?? "",
            title_sort = music.TitleSort ?? "",
            artist = music.Artist ?? "",
            id = music.IdString ?? "",
            chap_id = music.ChapId ?? "",
            chapter = music.Chapter ?? "",
            character = music.Character ?? "",
            chara_id = music.CharaId ?? "",
            category = music.Category ?? "",
            category_id = music.CategoryId ?? "",
            lunatic = music.Lunatic ?? "",
            bonus = music.Bonus ?? "",
            copyright1 = music.Copyright1 ?? "",
            lev_bas = music.LevBas ?? "",
            lev_adv = music.LevAdv ?? "",
            lev_exc = music.LevExc ?? "",
            lev_mas = music.LevMas ?? "",
            lev_lnt = music.LevLnt ?? "",
            image_url = music.ImageUrl ?? "",
            uuid = music.Uuid,
        }).OrderByDescending(music => music.date).ToList<object>();
    }
}
