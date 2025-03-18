using Microsoft.AspNetCore.Mvc;
using OngekiMuseumApi.Extensions;

namespace OngekiMuseumApi.Controllers;

/// <summary>
/// トップページを提供するコントローラー
/// </summary>
public class TopController : ControllerBase
{
    private readonly ILogger<TopController> _logger;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="logger">ロガー</param>
    public TopController(ILogger<TopController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// トップページを取得
    /// </summary>
    /// <returns>HTMLコンテンツ</returns>
    [HttpGet("/")]
    public ContentResult Index()
    {
        // 通常のログとSlackへのログを同時に送信
        _logger.LogInformationWithSlack("トップページにアクセスがありました");

        string html = """
                      <!DOCTYPE html>
                      <html lang="ja">
                      <head>
                          <meta charset="UTF-8">
                          <meta name="viewport" content="width=device-width, initial-scale=1.0">
                          <title>project-primera/ongeki-museum-api</title>
                          <style>
                              body {
                                  line-height: 1.6;
                                  color: #333;
                                  max-width: 800px;
                                  margin: 0 auto;
                                  padding: 20px;
                              }
                              h1 {
                                  color: #0066cc;
                                  border-bottom: 2px solid #0066cc;
                                  padding-bottom: 10px;
                              }
                              h2 {
                                  color: #0099cc;
                                  margin-top: 30px;
                              }
                              a {
                                  color: #0066cc;
                                  text-decoration: none;
                              }
                              a:hover {
                                  text-decoration: underline;
                              }
                              .footer {
                                  margin-top: 50px;
                                  padding-top: 20px;
                                  border-top: 1px solid #ddd;
                                  font-size: 0.9em;
                                  color: #666;
                              }
                          </style>
                      </head>
                      <body>
                          <h1>project-primera/ongeki-museum-api</h1>
                          <p>
                              オンゲキの公式楽曲データを正規化して提供するAPIです。<br>
                              利用に際して、各権利者様の権利を尊重し、個人利用の範囲内でご利用ください。
                          </p>

                          <h2>API一覧</h2>
                          <p>
                              <a href="/api/official-music">/api/official-music</a><br>
                              公式の楽曲データをそのまま返します。削除された楽曲などの情報も含みます。
                          </p>
                          <p>
                              <a href="/api/chapter">/api/chapter</a><br>
                              チャプター情報を返します。
                          </p>
                          <p>
                              <a href="/api/category">/api/category</a><br>
                              カテゴリ情報を返します。
                          </p>
                          <p>
                              <a href="/api/song">/api/song</a><br>
                              楽曲データを返します。削除された楽曲を含みます。
                          </p>
                          <p>
                              <a href="/api/chart">/api/chart</a><br>
                              譜面データを返します。削除された譜面を含みます。
                          </p>
                          <p>
                              <a href="/api/chart/join">/api/chart/join</a><br>
                              譜面データと楽曲データを結合して返します。削除された楽曲・譜面を含みます。
                          </p>

                          <h2>このサイトのリポジトリ</h2>
                          <p>
                              <a href="https://github.com/project-primera/ongeki-museum-api" target="_blank">project-primera/ongeki-museum-api</a><br>
                              不具合等はissueへご報告いただけますと幸いです。
                          </p>
                      </body>
                      </html>
                      """;

        return Content(html, "text/html");
    }
}
