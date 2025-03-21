using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using OngekiMuseumApi.BackgroundServices;
using OngekiMuseumApi.Data;
using OngekiMuseumApi.Facades.Normalization;
using OngekiMuseumApi.Helpers;
using OngekiMuseumApi.Middlewares;
using OngekiMuseumApi.ServiceDefaults;
using OngekiMuseumApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

// Entity Framework Core の設定
var mySqlConnectionStringBuilder = new MySqlConnectionStringBuilder
{
    Server = builder.Configuration["Database:Server"],
    Port = Convert.ToUInt32(builder.Configuration["Database:Port"]),
    Database = builder.Configuration["Database:Database"],
    UserID = builder.Configuration["Database:UserID"],
    Password = builder.Configuration["Database:Password"]
};
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        mySqlConnectionStringBuilder.ConnectionString,
        ServerVersion.AutoDetect(mySqlConnectionStringBuilder.ConnectionString),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure()
    )
);

// HTTPクライアントファクトリの追加
builder.Services.AddHttpClient();

// サービスの登録
builder.Services.AddSingleton<ISlackLoggerService, SlackLoggerService>();

builder.Services.AddScoped<IOfficialMusicService, OfficialMusicService>();

// 正規化ファサードの登録
builder.Services.AddScoped<IChapterNormalizationFacade, ChapterNormalizationFacade>();
builder.Services.AddScoped<INormalizationFacade>(sp => sp.GetRequiredService<IChapterNormalizationFacade>());
builder.Services.AddScoped<ICategoryNormalizationFacade, CategoryNormalizationFacade>();
builder.Services.AddScoped<INormalizationFacade>(sp => sp.GetRequiredService<ICategoryNormalizationFacade>());
builder.Services.AddScoped<ISongNormalizationFacade, SongNormalizationFacade>();
builder.Services.AddScoped<INormalizationFacade>(sp => sp.GetRequiredService<ISongNormalizationFacade>());
builder.Services.AddScoped<IChartNormalizationFacade, ChartNormalizationFacade>();
builder.Services.AddScoped<INormalizationFacade>(sp => sp.GetRequiredService<IChartNormalizationFacade>());

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
