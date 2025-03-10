using Microsoft.EntityFrameworkCore;
using OngekiMuseumApi.BackgroundServices;
using OngekiMuseumApi.Data;
using OngekiMuseumApi.Facades.Normalization;
using OngekiMuseumApi.Services;

namespace OngekiMuseumApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Entity Framework Core の設定
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions => npgsqlOptions.EnableRetryOnFailure()
            )
        );

        // HTTPクライアントファクトリの追加
        builder.Services.AddHttpClient();

        // 楽曲データサービスの登録
        builder.Services.AddScoped<IOfficialMusicService, OfficialMusicService>();
        builder.Services.AddScoped<IChapterService, ChapterService>();

        // 正規化ファサードの登録
        builder.Services.AddScoped<IChapterNormalizationFacade, ChapterNormalizationFacade>();
        builder.Services.AddScoped<INormalizationFacade>(sp => sp.GetRequiredService<IChapterNormalizationFacade>());

        // バックグラウンドサービスの登録
        builder.Services.AddHostedService<OfficialMusicBackgroundService>();
        builder.Services.AddHostedService<NormalizationBackgroundService>();

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
