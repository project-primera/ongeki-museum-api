using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngekiMuseumApi.Models
public class OfficialMusic : ITimestamp
{
    /// <summary>
    /// ONGEKI公式サイトから取得した楽曲データを表すモデルクラス
    /// </summary>
    [Table("official-music")]
    {
        /// <summary>
        /// 主キー、自動採番
        /// </summary>
        [Key]
        public int Id { get; init; }

        /// <summary>
        /// 新曲フラグ
        /// NEW or 空文字列
        /// </summary>
        [MaxLength(3)]
        public string New { get; set; } = string.Empty;

        /// <summary>
        /// 追加日
        /// 20250220 など
        /// </summary>
        [MaxLength(8)]
        public string Date { get; set; } = string.Empty;

        /// <summary>
        /// 曲名
        /// </summary>
        [MaxLength(128)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// ソート用曲名
        /// </summary>
        [MaxLength(128)]
        public string TitleSort { get; set; } = string.Empty;

        /// <summary>
        /// アーティスト名
        /// </summary>
        [MaxLength(256)]
        public string Artist { get; set; } = string.Empty;

        /// <summary>
        /// 楽曲ID
        /// </summary>
        [MaxLength(6)]
        public string IdString { get; set; } = string.Empty;

        /// <summary>
        /// チャプターID
        /// </summary>
        [MaxLength(5)]
        public string ChapId { get; set; } = string.Empty;

        /// <summary>
        /// チャプター名
        /// </summary>
        [MaxLength(128)]
        public string Chapter { get; set; } = string.Empty;

        /// <summary>
        /// キャラクター名
        /// </summary>
        [MaxLength(64)]
        public string Character { get; set; } = string.Empty;

        /// <summary>
        /// キャラクターID
        /// </summary>
        [MaxLength(4)]
        public string CharaId { get; set; } = string.Empty;

        /// <summary>
        /// カテゴリー
        /// </summary>
        [MaxLength(16)]
        public string Category { get; set; } = string.Empty;

        /// <summary>
        /// カテゴリーID
        /// </summary>
        [MaxLength(2)]
        public string CategoryId { get; set; } = string.Empty;

        /// <summary>
        /// ルナティックフラグ
        /// 1 or 空文字列
        /// </summary>
        [MaxLength(1)]
        public string Lunatic { get; set; } = string.Empty;

        /// <summary>
        /// ボーナスフラグ
        /// 1 or 空文字列
        /// </summary>
        [MaxLength(1)]
        public string Bonus { get; set; } = string.Empty;

        /// <summary>
        /// 著作権情報
        /// </summary>
        [MaxLength(256)]
        public string Copyright1 { get; set; } = string.Empty;

        /// <summary>
        /// BASIC難易度
        /// </summary>
        [MaxLength(3)]
        public string LevBas { get; set; } = string.Empty;

        /// <summary>
        /// ADVANCED難易度
        /// </summary>
        [MaxLength(3)]
        public string LevAdv { get; set; } = string.Empty;

        /// <summary>
        /// EXPERT難易度
        /// </summary>
        [MaxLength(3)]
        public string LevExc { get; set; } = string.Empty;

        /// <summary>
        /// MASTER難易度
        /// </summary>
        [MaxLength(3)]
        public string LevMas { get; set; } = string.Empty;

        /// <summary>
        /// LUNATIC難易度
        /// </summary>
        [MaxLength(3)]
        public string LevLnt { get; set; } = string.Empty;

        /// <summary>
        /// 画像URL
        /// </summary>
        [MaxLength(32)]
        public string ImageUrl { get; set; } = string.Empty;

    /// <inheritdoc />
    public required DateTimeOffset CreatedAt { get; set; }

    /// <inheritdoc />
    public required DateTimeOffset UpdatedAt { get; set; }
}
