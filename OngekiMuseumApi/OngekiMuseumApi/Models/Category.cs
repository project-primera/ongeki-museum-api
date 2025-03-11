using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngekiMuseumApi.Models;

/// <summary>
/// ONGEKIのカテゴリ情報を表すモデルクラス
/// </summary>
[Table("category")]
public class Category : ITimestamp
{
    /// <summary>
    /// カテゴリID（主キー）
    /// </summary>
    [Key]
    public int Id { get; init; }

    /// <summary>
    /// カテゴリ名
    /// </summary>
    [MaxLength(16)]
    public string Name { get; set; } = string.Empty;

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset UpdatedAt { get; set; }
}
