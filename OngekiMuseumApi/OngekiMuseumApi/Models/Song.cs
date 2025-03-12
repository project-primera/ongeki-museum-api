using System;
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
    public int Id { get; init; }

    /// <summary>
    /// 楽曲のUUID（曲名とアーティストから生成したUUIDv7）
    /// </summary>
    [MaxLength(36)]
    public string Uuid { get; set; } = string.Empty;

    /// <summary>
    /// 曲名
    /// </summary>
    [MaxLength(128)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// アーティスト名
    /// </summary>
    [MaxLength(256)]
    public string Artist { get; set; } = string.Empty;

    /// <summary>
    /// ボーナス楽曲フラグ
    /// </summary>
    public bool IsBonusSong { get; set; }

    /// <summary>
    /// 楽曲追加日時（JSTでその日の7時）
    /// </summary>
    public DateTimeOffset AddedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset UpdatedAt { get; set; }
}
