using Microsoft.EntityFrameworkCore;
using OngekiMuseumApi.Models;

namespace OngekiMuseumApi.Data
{
    /// <summary>
    /// アプリケーションのデータベースコンテキスト
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// ONGEKI公式楽曲データ
        /// </summary>
        public DbSet<OfficialMusic> OfficialMusics { get; set; } = null!;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="options">DbContextオプション</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //
        }

        /// <summary>
        /// モデル構成時に呼び出されるメソッド
        /// </summary>
        /// <param name="modelBuilder">モデルビルダー</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // DB関連の設定
            modelBuilder
                .UseCollation("utf8mb4_bin")
                .HasCharSet("utf8mb4");

            // インデックスなどの設定
            modelBuilder.Entity<OfficialMusic>()
                .HasIndex((i => i.IdString));
        }
    }
}
