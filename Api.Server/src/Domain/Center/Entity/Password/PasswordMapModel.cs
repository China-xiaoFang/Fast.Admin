

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="PasswordMapModel"/> 密码映射表Model类
/// </summary>
[SugarTable("PasswordMap", "密码映射表")]
[SugarDbType(DatabaseTypeEnum.Center)]
public class PasswordMapModel : IDatabaseEntity
{
    /// <summary>
    /// 类型
    /// </summary>
    [SugarColumn(ColumnDescription = "类型", IsPrimaryKey = true)]
    public PasswordTypeEnum Type { get; set; }

    /// <summary>
    /// 明文
    /// </summary>
    [SugarSearchValue]
    [Required]
    [SugarColumn(ColumnDescription = "明文", Length = 50, IsPrimaryKey = true)]
    public string Plaintext { get; set; }

    /// <summary>
    /// 密文
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "密文", Length = 50)]
    public string Ciphertext { get; set; }
}