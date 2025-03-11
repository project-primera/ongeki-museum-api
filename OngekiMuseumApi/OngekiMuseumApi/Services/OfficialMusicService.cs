using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OngekiMuseumApi.Data;
using OngekiMuseumApi.Extensions;
using OngekiMuseumApi.Models;

namespace OngekiMuseumApi.Services
{
    /// <summary>
    /// ONGEKI公式楽曲データの取得と保存を行うサービス
    /// </summary>
    /// <remarks>
    /// コンストラクタ
    /// </remarks>
    /// <param name="context">データベースコンテキスト</param>
    /// <param name="httpClientFactory">HTTPクライアントファクトリ</param>
    /// <param name="logger">ロガー</param>
    public class OfficialMusicService(
        ApplicationDbContext context,
        IHttpClientFactory httpClientFactory,
        ILogger<OfficialMusicService> logger
        ) : IOfficialMusicService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly ILogger<OfficialMusicService> _logger = logger;
        private const string MusicJsonUrl = "https://ongeki.sega.jp/assets/json/music/music.json";

        /// <summary>
        /// 公式サイトから楽曲データを取得し、データベースに保存する
        /// </summary>
        /// <returns>非同期タスク</returns>
        public async Task FetchAndSaveOfficialMusicAsync()
        {
            try
            {
                _logger.LogInformationWithSlack("公式楽曲データの取得を開始します");

                // HTTPクライアントを作成
                var client = _httpClientFactory.CreateClient();

                // JSONデータを取得
                var response = await client.GetAsync(MusicJsonUrl);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();

                // JSONデータをパース
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var musicList = JsonSerializer.Deserialize<List<OfficialMusicJson>>(jsonString, options);

                if (musicList == null || musicList.Count == 0)
                {
                    _logger.LogWarningWithSlack("取得した楽曲データが空です");
                    return;
                }

                _logger.LogInformationWithSlack($"{musicList.Count}件の楽曲データを取得しました");

                // データベースに保存
                var now = DateTime.UtcNow;
                var count = 0;

                foreach (var musicJson in musicList)
                {
                    // 既存のデータを検索
                    var existingMusic = await _context.OfficialMusics
                        .FirstOrDefaultAsync(m => m.IdString == musicJson.id);

                    if (existingMusic != null)
                    {
                        // 既存データを更新
                        existingMusic.New = NullIfEmpty(musicJson.@new);
                        existingMusic.Date = NullIfEmpty(musicJson.date);
                        existingMusic.Title = NullIfEmpty(musicJson.title);
                        existingMusic.TitleSort = NullIfEmpty(musicJson.title_sort);
                        existingMusic.Artist = NullIfEmpty(musicJson.artist);
                        existingMusic.ChapId = NullIfEmpty(musicJson.chap_id);
                        existingMusic.Chapter = NullIfEmpty(musicJson.chapter);
                        existingMusic.Character = NullIfEmpty(musicJson.character);
                        existingMusic.CharaId = NullIfEmpty(musicJson.chara_id);
                        existingMusic.Category = NullIfEmpty(musicJson.category);
                        existingMusic.CategoryId = NullIfEmpty(musicJson.category_id);
                        existingMusic.Lunatic = NullIfEmpty(musicJson.lunatic);
                        existingMusic.Bonus = NullIfEmpty(musicJson.bonus);
                        existingMusic.Copyright1 = NullIfEmpty(musicJson.copyright1);
                        existingMusic.LevBas = NullIfEmpty(musicJson.lev_bas);
                        existingMusic.LevAdv = NullIfEmpty(musicJson.lev_adv);
                        existingMusic.LevExc = NullIfEmpty(musicJson.lev_exc);
                        existingMusic.LevMas = NullIfEmpty(musicJson.lev_mas);
                        existingMusic.LevLnt = NullIfEmpty(musicJson.lev_lnt);
                        existingMusic.ImageUrl = NullIfEmpty(musicJson.image_url);
                    }
                    else
                    {
                        // 新規データを追加
                        var newMusic = new OfficialMusic
                        {
                            New = NullIfEmpty(musicJson.@new),
                            Date = NullIfEmpty(musicJson.date),
                            Title = NullIfEmpty(musicJson.title),
                            TitleSort = NullIfEmpty(musicJson.title_sort),
                            Artist = NullIfEmpty(musicJson.artist),
                            IdString = NullIfEmpty(musicJson.id),
                            ChapId = NullIfEmpty(musicJson.chap_id),
                            Chapter = NullIfEmpty(musicJson.chapter),
                            Character = NullIfEmpty(musicJson.character),
                            CharaId = NullIfEmpty(musicJson.chara_id),
                            Category = NullIfEmpty(musicJson.category),
                            CategoryId = NullIfEmpty(musicJson.category_id),
                            Lunatic = NullIfEmpty(musicJson.lunatic),
                            Bonus = NullIfEmpty(musicJson.bonus),
                            Copyright1 = NullIfEmpty(musicJson.copyright1),
                            LevBas = NullIfEmpty(musicJson.lev_bas),
                            LevAdv = NullIfEmpty(musicJson.lev_adv),
                            LevExc = NullIfEmpty(musicJson.lev_exc),
                            LevMas = NullIfEmpty(musicJson.lev_mas),
                            LevLnt = NullIfEmpty(musicJson.lev_lnt),
                            ImageUrl = NullIfEmpty(musicJson.image_url),
                        };

                        await _context.OfficialMusics.AddAsync(newMusic);
                        count++;
                    }
                }

                await _context.SaveChangesAsync();
                _logger.LogInformationWithSlack($"{count}件の新規楽曲データを保存しました");
            }
            catch (Exception ex)
            {
                _logger.LogErrorWithSlack(ex, "楽曲データの取得・保存中にエラーが発生しました");
                throw new InvalidOperationException("Failed to fetch and save official music data", ex);
            }
        }

        /// <summary>
        /// 文字列が空の場合はnullを返し、それ以外は元の値を返す
        /// </summary>
        /// <param name="value">チェックする文字列</param>
        /// <returns>空の場合はnull、それ以外は元の文字列</returns>
        private static string? NullIfEmpty(string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }

    /// <summary>
    /// JSONデータをデシリアライズするためのクラス
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class OfficialMusicJson
    {
#pragma warning disable IDE1006 // 命名スタイル
        // ReSharper disable once InconsistentNaming
        public string @new { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string date { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string title { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string title_sort { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string artist { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string id { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string chap_id { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string chapter { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string character { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string chara_id { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string category { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string category_id { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string lunatic { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string bonus { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string copyright1 { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string lev_bas { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string lev_adv { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string lev_exc { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string lev_mas { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string lev_lnt { get; set; } = string.Empty;

        // ReSharper disable once InconsistentNaming
        public string image_url { get; set; } = string.Empty;
#pragma warning restore IDE1006 // 命名スタイル
    }
}
