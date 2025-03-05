using Microsoft.EntityFrameworkCore;
using OngekiMuseumApi.Data;
using OngekiMuseumApi.Services;

namespace OngekiMuseumApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.

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

        // 楽曲データサービスの登録
        builder.Services.AddScoped<IOfficialMusicService, OfficialMusicService>();

        // バックグラウンドサービスの登録
        builder.Services.AddHostedService<OfficialMusicBackgroundService>();

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
