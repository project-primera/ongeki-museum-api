using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngekiMuseumApi.Models;

/// <summary>
/// ONGEKI公式サイトから取得した楽曲データを表すモデルクラス
/// </summary>
[Table("official_music")]
public class OfficialMusic : ITimestamp
{
    /// <summary>
    /// 主キー、自動採番
    /// </summary>
    [Key]
    public int Id { get; init; }

    /// <summary>
    /// UUID
    /// UUIDv7形式の一意識別子
    /// </summary>
    public Guid Uuid { get; set; }

    /// <summary>
    /// 新曲フラグ
    /// NEW or 空文字列
    /// </summary>
    [MaxLength(3)]
    public string? New { get; set; }

    /// <summary>
    /// 追加日
    /// 20250220 など
    /// </summary>
    [MaxLength(8)]
    public string? Date { get; set; }

    /// <summary>
    /// 曲名
    /// </summary>
    [MaxLength(128)]
    public string? Title { get; set; }

    /// <summary>
    /// ソート用曲名
    /// </summary>
    [MaxLength(128)]
    public string? TitleSort { get; set; }

    /// <summary>
    /// アーティスト名
    /// </summary>
    [MaxLength(256)]
    public string? Artist { get; set; }

    /// <summary>
    /// 楽曲ID
    /// </summary>
    [MaxLength(6)]
    public string? IdString { get; set; }

    /// <summary>
    /// チャプターID
    /// </summary>
    [MaxLength(5)]
    public string? ChapId { get; set; }

    /// <summary>
    /// チャプター名
    /// </summary>
    [MaxLength(128)]
    public string? Chapter { get; set; }

    /// <summary>
    /// キャラクター名
    /// </summary>
    [MaxLength(64)]
    public string? Character { get; set; }

    /// <summary>
    /// キャラクターID
    /// </summary>
    [MaxLength(4)]
    public string? CharaId { get; set; }

    /// <summary>
    /// カテゴリー
    /// </summary>
    [MaxLength(16)]
    public string? Category { get; set; }

    /// <summary>
    /// カテゴリーID
    /// </summary>
    [MaxLength(2)]
    public string? CategoryId { get; set; }

    /// <summary>
    /// ルナティックフラグ
    /// 1 or 空文字列
    /// </summary>
    [MaxLength(1)]
    public string? Lunatic { get; set; }

    /// <summary>
    /// ボーナスフラグ
    /// 1 or 空文字列
    /// </summary>
    [MaxLength(1)]
    public string? Bonus { get; set; }

    /// <summary>
    /// 著作権情報
    /// </summary>
    [MaxLength(256)]
    public string? Copyright1 { get; set; }

    /// <summary>
    /// BASIC難易度
    /// </summary>
    [MaxLength(3)]
    public string? LevBas { get; set; }

    /// <summary>
    /// ADVANCED難易度
    /// </summary>
    [MaxLength(3)]
    public string? LevAdv { get; set; }

    /// <summary>
    /// EXPERT難易度
    /// </summary>
    [MaxLength(3)]
    public string? LevExc { get; set; }

    /// <summary>
    /// MASTER難易度
    /// </summary>
    [MaxLength(3)]
    public string? LevMas { get; set; }

    /// <summary>
    /// LUNATIC難易度
    /// </summary>
    [MaxLength(3)]
    public string? LevLnt { get; set; }

    /// <summary>
    /// 画像URL
    /// </summary>
    [MaxLength(32)]
    public string? ImageUrl { get; set; }

    /// <summary>
    /// 削除フラグ
    /// 公式サイトから削除された楽曲の場合はtrue
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset UpdatedAt { get; set; }
}
