using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngekiMuseumApi.Models;

/// <summary>
/// ONGEKIの譜面情報を表すモデルクラス
/// </summary>
[Table("chart")]
public class Chart : ITimestamp
{
    /// <summary>
    /// 譜面ID（主キー）
    /// </summary>
    [Key]
    public int Id { get; init; }

    /// <summary>
    /// UUID
    /// UUIDv7形式の一意識別子
    /// </summary>
    public Guid Uuid { get; init; }

    /// <summary>
    /// 楽曲UUID
    /// 関連する楽曲のUUID
    /// </summary>
    public Guid SongUuid { get; init; }

    /// <summary>
    /// 難易度
    /// </summary>
    public Difficulty Difficulty { get; init; }

    /// <summary>
    /// レベル値
    /// 10.0なら100、10.5なら105として格納
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// ボーナストラックフラグ
    /// </summary>
    public bool IsBonus { get; set; }

    /// <summary>
    /// 削除フラグ
    /// 公式サイトから削除された譜面の場合はtrue
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTimeOffset UpdatedAt { get; set; }
}

/// <summary>
/// 譜面の難易度を表す列挙型
/// </summary>
public enum Difficulty
{
    /// <summary>
    /// BASIC難易度
    /// </summary>
    Basic = 1,

    /// <summary>
    /// ADVANCED難易度
    /// </summary>
    Advanced = 2,

    /// <summary>
    /// EXPERT難易度
    /// </summary>
    Expert = 3,

    /// <summary>
    /// MASTER難易度
    /// </summary>
    Master = 4,

    /// <summary>
    /// LUNATIC難易度
    /// </summary>
    Lunatic = 5,

    /// <summary>
    /// REMASTER難易度（予約のみ / 未使用）
    /// </summary>
    Remaster = 6
}
