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
        private static readonly List<string> MusicJsonUrls = [
            "https://ongeki.sega.jp/assets/json/music/music.json"
        ];

        /// <summary>
        /// 公式サイトから楽曲データを取得し、データベースに保存する
        /// </summary>
        /// <returns>非同期タスク</returns>
        public async Task FetchAndSaveOfficialMusicAsync()
        {
            logger.LogInformationWithSlack("公式楽曲データの取得を開始します");

            foreach (var musicJsonUrl in MusicJsonUrls)
            {
                try
                {
                    logger.LogInformationWithSlack($"{musicJsonUrl} の楽曲データを取得します");

                    // JSONデータを取得
                    var client = httpClientFactory.CreateClient();
                    var response = await client.GetAsync(musicJsonUrl);
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
                        logger.LogWarningWithSlack("取得した楽曲データが空です");
                        return;
                    }

                    logger.LogInformationWithSlack($"{musicList.Count}件の楽曲データを取得しました");

                    // データベースに保存
                    var newCount = 0;
                    var updateCount = 0;

                    foreach (var musicJson in musicList)
                    {
                        // 既存のデータを検索（楽曲名、アーティスト名、チャプター名で一致するものを検索）
                        var existingMusic = await context.OfficialMusics
                            .FirstOrDefaultAsync(m =>
                                m.Title == NullIfEmpty(musicJson.title) &&
                                m.Artist == NullIfEmpty(musicJson.artist) &&
                                m.Chapter == NullIfEmpty(musicJson.chapter) &&
                                m.Lunatic == NullIfEmpty(musicJson.lunatic) &&
                                m.Bonus == NullIfEmpty(musicJson.bonus)
                            );

                        if (existingMusic is null)
                        {
                            // 新規データを追加
                            var newMusic = new OfficialMusic {
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
                            logger.LogInformationWithSlack($"OfficialMusic 新規: {musicJson.title}");
                            await context.OfficialMusics.AddAsync(newMusic);
                            newCount++;
                        }
                        else
                        {
                            string updateLog = "";

                            // 各フィールドを比較して変更があるか確認
                            if (!string.Equals(existingMusic.New, NullIfEmpty(musicJson.@new)))
                            {
                                updateLog += $"\nNew: {existingMusic.New} → {musicJson.@new}";
                                existingMusic.New = NullIfEmpty(musicJson.@new);
                            }
                            if (!string.Equals(existingMusic.Date, NullIfEmpty(musicJson.date)))
                            {
                                updateLog += $"\nDate: {existingMusic.Date} → {musicJson.date}";
                                existingMusic.Date = NullIfEmpty(musicJson.date);
                            }
                            if (!string.Equals(existingMusic.Title, NullIfEmpty(musicJson.title)))
                            {
                                updateLog += $"\nTitle: {existingMusic.Title} → {musicJson.title}";
                                existingMusic.Title = NullIfEmpty(musicJson.title);
                            }
                            if (!string.Equals(existingMusic.TitleSort, NullIfEmpty(musicJson.title_sort)))
                            {
                                updateLog += $"\nTitleSort: {existingMusic.TitleSort} → {musicJson.title_sort}";
                                existingMusic.TitleSort = NullIfEmpty(musicJson.title_sort);
                            }
                            if (!string.Equals(existingMusic.Artist, NullIfEmpty(musicJson.artist)))
                            {
                                updateLog += $"\nArtist: {existingMusic.Artist} → {musicJson.artist}";
                                existingMusic.Artist = NullIfEmpty(musicJson.artist);
                            }
                            if (!string.Equals(existingMusic.ChapId, NullIfEmpty(musicJson.chap_id)))
                            {
                                updateLog += $"\nChapId: {existingMusic.ChapId} → {musicJson.chap_id}";
                                existingMusic.ChapId = NullIfEmpty(musicJson.chap_id);
                            }
                            if (!string.Equals(existingMusic.Chapter, NullIfEmpty(musicJson.chapter)))
                            {
                                updateLog += $"\nChapter: {existingMusic.Chapter} → {musicJson.chapter}";
                                existingMusic.Chapter = NullIfEmpty(musicJson.chapter);
                            }
                            if (!string.Equals(existingMusic.Character, NullIfEmpty(musicJson.character)))
                            {
                                updateLog += $"\nCharacter: {existingMusic.Character} → {musicJson.character}";
                                existingMusic.Character = NullIfEmpty(musicJson.character);
                            }
                            if (!string.Equals(existingMusic.CharaId, NullIfEmpty(musicJson.chara_id)))
                            {
                                updateLog += $"\nCharaId: {existingMusic.CharaId} → {musicJson.chara_id}";
                                existingMusic.CharaId = NullIfEmpty(musicJson.chara_id);
                            }
                            if (!string.Equals(existingMusic.Category, NullIfEmpty(musicJson.category)))
                            {
                                updateLog += $"\nCategory: {existingMusic.Category} → {musicJson.category}";
                                existingMusic.Category = NullIfEmpty(musicJson.category);
                            }
                            if (!string.Equals(existingMusic.CategoryId, NullIfEmpty(musicJson.category_id)))
                            {
                                updateLog += $"\nCategoryId: {existingMusic.CategoryId} → {musicJson.category_id}";
                                existingMusic.CategoryId = NullIfEmpty(musicJson.category_id);
                            }
                            if (!string.Equals(existingMusic.Lunatic, NullIfEmpty(musicJson.lunatic)))
                            {
                                updateLog += $"\nLunatic: {existingMusic.Lunatic} → {musicJson.lunatic}";
                                existingMusic.Lunatic = NullIfEmpty(musicJson.lunatic);
                            }
                            if (!string.Equals(existingMusic.Bonus, NullIfEmpty(musicJson.bonus)))
                            {
                                updateLog += $"\nBonus: {existingMusic.Bonus} → {musicJson.bonus}";
                                existingMusic.Bonus = NullIfEmpty(musicJson.bonus);
                            }
                            if (!string.Equals(existingMusic.Copyright1, NullIfEmpty(musicJson.copyright1)))
                            {
                                updateLog += $"\nCopyright1: {existingMusic.Copyright1} → {musicJson.copyright1}";
                                existingMusic.Copyright1 = NullIfEmpty(musicJson.copyright1);
                            }
                            if (!string.Equals(existingMusic.LevBas, NullIfEmpty(musicJson.lev_bas)))
                            {
                                updateLog += $"\nLevBas: {existingMusic.LevBas} → {musicJson.lev_bas}";
                                existingMusic.LevBas = NullIfEmpty(musicJson.lev_bas);
                            }
                            if (!string.Equals(existingMusic.LevAdv, NullIfEmpty(musicJson.lev_adv)))
                            {
                                updateLog += $"\nLevAdv: {existingMusic.LevAdv} → {musicJson.lev_adv}";
                                existingMusic.LevAdv = NullIfEmpty(musicJson.lev_adv);
                            }
                            if (!string.Equals(existingMusic.LevExc, NullIfEmpty(musicJson.lev_exc)))
                            {
                                updateLog += $"\nLevExc: {existingMusic.LevExc} → {musicJson.lev_exc}";
                                existingMusic.LevExc = NullIfEmpty(musicJson.lev_exc);
                            }
                            if (!string.Equals(existingMusic.LevMas, NullIfEmpty(musicJson.lev_mas)))
                            {
                                updateLog += $"\nLevMas: {existingMusic.LevMas} → {musicJson.lev_mas}";
                                existingMusic.LevMas = NullIfEmpty(musicJson.lev_mas);
                            }
                            if (!string.Equals(existingMusic.LevLnt, NullIfEmpty(musicJson.lev_lnt)))
                            {
                                updateLog += $"\nLevLnt: {existingMusic.LevLnt} → {musicJson.lev_lnt}";
                                existingMusic.LevLnt = NullIfEmpty(musicJson.lev_lnt);
                            }
                            if (!string.Equals(existingMusic.ImageUrl, NullIfEmpty(musicJson.image_url)))
                            {
                                updateLog += $"\nImageUrl: {existingMusic.ImageUrl} → {musicJson.image_url}";
                                existingMusic.ImageUrl = NullIfEmpty(musicJson.image_url);
                            }

                            // 更新があった場合のみ保存
                            if (updateLog != "")
                            {
                                logger.LogInformationWithSlack($"OfficialMusic 更新: {existingMusic.Title} ({existingMusic.Id})\n```\n{updateLog}\n```");
                                context.Update(existingMusic);
                                updateCount++;
                            }
                        }
                    }

                    await context.SaveChangesAsync();
                    logger.LogInformationWithSlack($"{newCount}件の新規楽曲データを保存しました。\n{updateCount}件の既存楽曲データを更新しました。");
                }
                catch (Exception ex)
                {
                    logger.LogErrorWithSlack(ex, $"楽曲データの取得・保存中にエラーが発生しました。 url: {musicJsonUrl}");
                    throw new InvalidOperationException("Failed to fetch and save official music data", ex);
                }
                // ループごとにsleepする
                await Task.Delay(5000);
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
