using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OngekiMuseumApi.Data;
using OngekiMuseumApi.Models;

namespace OngekiMuseumApi.Services
{
    /// <summary>
    /// ONGEKI公式楽曲データの取得と保存を行うサービス
    /// </summary>
    public class OfficialMusicService : IOfficialMusicService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<OfficialMusicService> _logger;
        private const string MusicJsonUrl = "https://ongeki.sega.jp/assets/json/music/music.json";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="context">データベースコンテキスト</param>
        /// <param name="httpClientFactory">HTTPクライアントファクトリ</param>
        /// <param name="logger">ロガー</param>
        public OfficialMusicService(ApplicationDbContext context, IHttpClientFactory httpClientFactory, ILogger<OfficialMusicService> logger)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        /// <summary>
        /// 公式サイトから楽曲データを取得し、データベースに保存する
        /// </summary>
        /// <returns>非同期タスク</returns>
        public async Task FetchAndSaveOfficialMusicAsync()
        {
            try
            {
                _logger.LogInformation("公式楽曲データの取得を開始します");

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
                    _logger.LogWarning("取得した楽曲データが空です");
                    return;
                }

                _logger.LogInformation($"{musicList.Count}件の楽曲データを取得しました");

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
                        existingMusic.New = musicJson.@new;
                        existingMusic.Date = musicJson.date;
                        existingMusic.Title = musicJson.title;
                        existingMusic.TitleSort = musicJson.title_sort;
                        existingMusic.Artist = musicJson.artist;
                        existingMusic.ChapId = musicJson.chap_id;
                        existingMusic.Chapter = musicJson.chapter;
                        existingMusic.Character = musicJson.character;
                        existingMusic.CharaId = musicJson.chara_id;
                        existingMusic.Category = musicJson.category;
                        existingMusic.CategoryId = musicJson.category_id;
                        existingMusic.Lunatic = musicJson.lunatic;
                        existingMusic.Bonus = musicJson.bonus;
                        existingMusic.Copyright1 = musicJson.copyright1;
                        existingMusic.LevBas = musicJson.lev_bas;
                        existingMusic.LevAdv = musicJson.lev_adv;
                        existingMusic.LevExc = musicJson.lev_exc;
                        existingMusic.LevMas = musicJson.lev_mas;
                        existingMusic.LevLnt = musicJson.lev_lnt;
                        existingMusic.ImageUrl = musicJson.image_url;
                        existingMusic.UpdatedAt = now;
                    }
                    else
                    {
                        // 新規データを追加
                        var newMusic = new OfficialMusic
                        {
                            New = musicJson.@new,
                            Date = musicJson.date,
                            Title = musicJson.title,
                            TitleSort = musicJson.title_sort,
                            Artist = musicJson.artist,
                            IdString = musicJson.id,
                            ChapId = musicJson.chap_id,
                            Chapter = musicJson.chapter,
                            Character = musicJson.character,
                            CharaId = musicJson.chara_id,
                            Category = musicJson.category,
                            CategoryId = musicJson.category_id,
                            Lunatic = musicJson.lunatic,
                            Bonus = musicJson.bonus,
                            Copyright1 = musicJson.copyright1,
                            LevBas = musicJson.lev_bas,
                            LevAdv = musicJson.lev_adv,
                            LevExc = musicJson.lev_exc,
                            LevMas = musicJson.lev_mas,
                            LevLnt = musicJson.lev_lnt,
                            ImageUrl = musicJson.image_url,
                            CreatedAt = now,
                            UpdatedAt = now
                        };

                        await _context.OfficialMusics.AddAsync(newMusic);
                        count++;
                    }
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation($"{count}件の新規楽曲データを保存しました");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "楽曲データの取得・保存中にエラーが発生しました");
                throw;
            }
        }
    }

    /// <summary>
    /// ONGEKI公式楽曲データの取得と保存を行うサービスのインターフェース
    /// </summary>
    public interface IOfficialMusicService
    {
        /// <summary>
        /// 公式サイトから楽曲データを取得し、データベースに保存する
        /// </summary>
        /// <returns>非同期タスク</returns>
        Task FetchAndSaveOfficialMusicAsync();
    }

    /// <summary>
    /// JSONデータをデシリアライズするためのクラス
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class OfficialMusicJson
    {
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
    }
}
