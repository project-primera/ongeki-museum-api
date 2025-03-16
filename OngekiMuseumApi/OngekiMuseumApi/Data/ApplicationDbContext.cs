using Microsoft.EntityFrameworkCore;
using OngekiMuseumApi.Models;

namespace OngekiMuseumApi.Data
{
    /// <summary>
    /// アプリケーションのデータベースコンテキスト
    /// </summary>
    public class ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        TimeProvider timeProvider
    ) : DbContext(options) {
        /// <summary>
        /// オンゲキ公式楽曲データ
        /// </summary>
        public DbSet<OfficialMusic> OfficialMusics => Set<OfficialMusic>();

        /// <summary>
        /// オンゲキチャプターデータ
        /// </summary>
        public DbSet<Chapter> Chapters => Set<Chapter>();

        /// <summary>
        /// オンゲキカテゴリデータ
        /// </summary>
        public DbSet<Category> Categories => Set<Category>();

        /// <summary>
        /// オンゲキ楽曲データ
        /// </summary>
        public DbSet<Song> Songs => Set<Song>();

        /// <summary>
        /// オンゲキ譜面データ
        /// </summary>
        public DbSet<Chart> Charts => Set<Chart>();

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSnakeCaseNamingConvention();

        /// <inheritdoc />
        public override int SaveChanges() {
            AddTimestamps();
            return base.SaveChanges();
        }

        /// <inheritdoc />
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
            AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// エンティティにタイムスタンプを追加する
        /// </summary>
        private void AddTimestamps() {
            var entities = ChangeTracker.Entries()
                .Where(x => x is { Entity: ITimestamp, State: EntityState.Added or EntityState.Modified });

            foreach (var entity in entities) {
                var now = timeProvider.GetUtcNow();

                if (entity.State == EntityState.Added) {
                    ((ITimestamp)entity.Entity).CreatedAt = now;
                }
                ((ITimestamp)entity.Entity).UpdatedAt = now;
            }
        }
    }
}
