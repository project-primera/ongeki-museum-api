using Microsoft.EntityFrameworkCore;
using OngekiMuseumApi.BackgroundServices;
using OngekiMuseumApi.Data;
using OngekiMuseumApi.Facades.Normalization;
using OngekiMuseumApi.Helpers;
using OngekiMuseumApi.Middlewares;
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
            options.UseMySql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
                mySqlOptions => mySqlOptions.EnableRetryOnFailure()
            )
        );

        // HTTPクライアントファクトリの追加
        builder.Services.AddHttpClient();

        // サービスの登録
        builder.Services.AddSingleton<ISlackLoggerService, SlackLoggerService>();

        builder.Services.AddScoped<IOfficialMusicService, OfficialMusicService>();
        builder.Services.AddScoped<IChapterService, ChapterService>();

        // 正規化ファサードの登録
        builder.Services.AddScoped<IChapterNormalizationFacade, ChapterNormalizationFacade>();
        builder.Services.AddScoped<INormalizationFacade>(sp => sp.GetRequiredService<IChapterNormalizationFacade>());
        builder.Services.AddScoped<ICategoryNormalizationFacade, CategoryNormalizationFacade>();
        builder.Services.AddScoped<INormalizationFacade>(sp => sp.GetRequiredService<ICategoryNormalizationFacade>());

        // バックグラウンドサービスの登録
        builder.Services.AddHostedService<OfficialMusicBackgroundService>();
        builder.Services.AddHostedService<NormalizationBackgroundService>();

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // ServiceProviderHelperにIServiceProviderを設定
        ServiceProviderStaticHelper.SetServiceProvider(app.Services);

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseMiddleware<ExceptionHandlerMiddleware>();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
