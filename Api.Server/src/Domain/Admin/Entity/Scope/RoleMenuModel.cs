

namespace Fast.Admin.Entity;

/// <summary>
/// <see cref="RoleMenuModel"/> 角色菜单表Model类
/// </summary>
[SugarTable("RoleMenu", "角色菜单表")]
[SugarDbType(DatabaseTypeEnum.Admin)]
public class RoleMenuModel : IDatabaseEntity
{
    /// <summary>
    /// 角色Id
    /// </summary>
    [SugarColumn(ColumnDescription = "角色Id", IsPrimaryKey = true)]
    public long RoleId { get; set; }

    /// <summary>
    /// 菜单Id
    /// </summary>
    [SugarColumn(ColumnDescription = "菜单Id", IsPrimaryKey = true)]
    public long MenuId { get; set; }

    /// <summary>
    /// 创建者用户Id
    /// </summary>
    [SugarColumn(ColumnDescription = "创建者用户Id", CreateTableFieldSort = 991)]
    public long? CreatedUserId { get; set; }

    /// <summary>
    /// 创建者用户名称
    /// </summary>
    [SugarColumn(ColumnDescription = "创建者用户名称", Length = 20, CreateTableFieldSort = 992)]
    public string CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Required, SugarSearchTime, SugarColumn(ColumnDescription = "创建时间", CreateTableFieldSort = 993)]
    public DateTime? CreatedTime { get; set; }
}