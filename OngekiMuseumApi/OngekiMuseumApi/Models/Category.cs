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
    /// 主キー（自動採番）
    /// </summary>
    [Key]
    public int Id { get; init; }

    /// <summary>
    /// UUID
    /// UUIDv7形式の一意識別子
    /// </summary>
    public Guid Uuid { get; init; }

    /// <summary>
    /// 公式カテゴリID
    /// 公式サイトで使用されているカテゴリID
    /// </summary>
    public int OfficialId { get; set; }

    /// <summary>
    /// カテゴリ名
    /// </summary>
    [MaxLength(16)]
    public string Name { get; init; } = string.Empty;

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset UpdatedAt { get; set; }
}
