using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OngekiMuseumApi.Data;
using OngekiMuseumApi.Extensions;
using OngekiMuseumApi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OngekiMuseumApi.Facades.Normalization;

/// <summary>
/// カテゴリ情報の正規化を行うファサード
/// </summary>
public class CategoryNormalizationFacade : ICategoryNormalizationFacade
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CategoryNormalizationFacade> _logger;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">データベースコンテキスト</param>
    /// <param name="logger">ロガー</param>
    public CategoryNormalizationFacade(ApplicationDbContext context, ILogger<CategoryNormalizationFacade> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<int> NormalizeAsync()
    {
        try
        {
            _logger.LogInformationWithSlack("カテゴリ情報の正規化を開始します");

            // OfficialMusicテーブルから一意のCategoryIdとCategory名を抽出
            var categories = await _context.OfficialMusics
                .Where(m => m.CategoryId != null && m.Category != null)
                .Select(m => new { m.CategoryId, m.Category })
                .Distinct()
                .ToListAsync();

            if (categories.Count == 0)
            {
                _logger.LogWarningWithSlack("抽出可能なカテゴリ情報がありません");
                return 0;
            }

            _logger.LogInformationWithSlack($"{categories.Count}件のカテゴリ情報を抽出しました");

            int addedCount = 0;

            foreach (var categoryInfo in categories)
            {
                // nullチェック（念のため）
                if (string.IsNullOrEmpty(categoryInfo.CategoryId) || string.IsNullOrEmpty(categoryInfo.Category))
                {
                    continue;
                }

                // CategoryIdをintに変換
                if (!int.TryParse(categoryInfo.CategoryId, out var officialCategoryId))
                {
                    _logger.LogWarningWithSlack($"CategoryId '{categoryInfo.CategoryId}' をint型に変換できませんでした");
                    continue;
                }

                // 既存のカテゴリを検索（名前で検索）
                var existingCategory = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == categoryInfo.Category);

                if (existingCategory is null)
                {
                    // 新規データを追加
                    var newCategory = new Category
                    {
                        OfficialId = officialCategoryId,
                        Uuid = Guid.CreateVersion7(),
                        Name = categoryInfo.Category
                    };

                    await _context.Categories.AddAsync(newCategory);
                    addedCount++;
                }
                else
                {
                    // 既存データを更新（OfficialIdが変わっている可能性があるため）
                    if (existingCategory.Name != categoryInfo.Category)
                    {
                        existingCategory.OfficialId = officialCategoryId;
                        _context.Categories.Update(existingCategory);
                    }
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformationWithSlack($"{addedCount}件の新規カテゴリデータを保存しました");

            return addedCount;
        }
        catch (Exception ex)
        {
            _logger.LogErrorWithSlack(ex, "カテゴリ情報の正規化・保存中にエラーが発生しました");
            throw;
        }
    }
}
