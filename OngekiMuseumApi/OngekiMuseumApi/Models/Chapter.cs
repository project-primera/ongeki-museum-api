using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngekiMuseumApi.Models;

/// <summary>
/// ONGEKIのチャプター情報を表すモデルクラス
/// </summary>
[Table("chapter")]
public class Chapter : ITimestamp
{
    /// <summary>
    /// チャプターID（主キー）
    /// </summary>
    [Key]
    public int Id { get; init; }

    /// <summary>
    /// チャプター名
    /// </summary>
    [MaxLength(128)]
    public string ChapterName { get; set; } = string.Empty;

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset UpdatedAt { get; set; }
}
