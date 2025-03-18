using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngekiMuseumApi.Models;

/// <summary>
/// ONGEKI楽曲の正規化データを表すモデルクラス
/// </summary>
[Table("song")]
public class Song : ITimestamp
{
    /// <summary>
    /// 楽曲ID（主キー）
    /// </summary>
    [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    public Guid OfficialUuid { get; set; }

    /// <summary>
    /// 曲名
    /// </summary>
    [MaxLength(128)]
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// アーティスト名
    /// </summary>
    [MaxLength(256)]
    public string Artist { get; init; } = string.Empty;

    /// <summary>
    /// 著作権情報
    /// </summary>
    [MaxLength(256)]
    public string? Copyright { get; set; }

    /// <summary>
    /// 楽曲追加日時（JSTでその日の7時）
    /// </summary>
    public DateTimeOffset AddedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset UpdatedAt { get; set; }
}
