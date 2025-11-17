// ReSharper disable once CheckNamespace

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="TableColumnConfigModel"/> 表格列配置表Model类
/// </summary>
[SugarTable("TableColumnConfig", "表格列配置表")]
[SugarDbType(DatabaseTypeEnum.Center)]
[SugarIndex($"IX_{{table}}_{nameof(TableId)}", nameof(TableId), OrderByType.Asc)]
public class TableColumnConfigModel : IDatabaseEntity
{
    /// <summary>
    /// 列Id
    /// </summary>
    [SugarColumn(ColumnDescription = "列Id", IsPrimaryKey = true)]
    public long ColumnId { get; set; }

    /// <summary>
    /// 表格Id
    /// </summary>
    [SugarColumn(ColumnDescription = "表格Id")]
    public long TableId { get; set; }

    /// <summary>
    /// 绑定字段
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "绑定字段", Length = 50)]
    public string Prop { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(ColumnDescription = "名称", Length = 50)]
    public string Label { get; set; }

    /// <summary>
    /// 固定
    /// </summary>
    /// <remarks>
    /// <para>left：左侧</para>
    /// <para>right：右侧</para>
    /// </remarks>
    [SugarColumn(ColumnDescription = "固定", Length = 5)]
    public string Fixed { get; set; }

    /// <summary>
    /// 自动宽度
    /// </summary>
    [SugarColumn(ColumnDescription = "自动宽度")]
    public bool AutoWidth { get; set; }

    /// <summary>
    /// 宽度
    /// </summary>
    [SugarColumn(ColumnDescription = "宽度")]
    public int Width { get; set; }

    /// <summary>
    /// 小的宽度
    /// </summary>
    [SugarColumn(ColumnDescription = "小的宽度")]
    public int SmallWidth { get; set; }

    /// <summary>
    /// 顺序
    /// </summary>
    /// <remarks>从小到大</remarks>
    [SugarColumn(ColumnDescription = "顺序")]
    public int Order { get; set; }

    /// <summary>
    /// 显示
    /// </summary>
    [SugarColumn(ColumnDescription = "显示")]
    public bool Show { get; set; }

    /// <summary>
    /// 复制
    /// </summary>
    [SugarColumn(ColumnDescription = "复制")]
    public bool Copy { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(ColumnDescription = "排序")]
    public bool Sortable { get; set; }

    /// <summary>
    /// 排序字段
    /// </summary>
    [SugarColumn(ColumnDescription = "排序字段", Length = 50)]
    public string SortableField { get; set; }

    /// <summary>
    /// 列类型
    /// </summary>
    /// <remarks>
    /// <para>expand：可展开按钮列</para>
    /// <para>image：图片列</para>
    /// <para>date：日期显示（格式 "YYYY-MM-DD"）</para>
    /// <para>time：时间显示（格式 "HH:mm:ss"）</para>
    /// <para>dateTime：日期时间显示（格式 "YYYY-MM-DD HH:mm:ss"）</para>
    /// <para>d2：数值列，保留 2 位小数，不带千分位</para>
    /// <para>d4：数值列，保留 4 位小数，不带千分位</para>
    /// <para>d6：数值列，保留 6 位小数，不带千分位</para>
    /// <para>gd2：数值列，保留 2 位小数，带千分位</para>
    /// <para>gd4：数值列，保留 4 位小数，带千分位</para>
    /// <para>gd6：数值列，保留 6 位小数，带千分位</para>
    /// <para>submitInfo：提交信息列</para>
    /// </remarks>
    [SugarColumn(ColumnDescription = "列类型", Length = 10)]
    public string Type { get; set; }

    /// <summary>
    /// 链接
    /// </summary>
    [SugarColumn(ColumnDescription = "链接")]
    public bool Link { get; set; }

    /// <summary>
    /// 点击事件名称
    /// </summary>
    [SugarColumn(ColumnDescription = "点击事件名称", Length = 50)]
    public string ClickEmit { get; set; }

    /// <summary>
    /// 标签
    /// </summary>
    [SugarColumn(ColumnDescription = "标签")]
    public bool Tag { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    [SugarColumn(ColumnDescription = "字典名称", Length = 50)]
    public string Enum { get; set; }

    /// <summary>
    /// 日期格式化
    /// </summary>
    [SugarColumn(ColumnDescription = "日期格式化")]
    public bool DateFix { get; set; }

    /// <summary>
    /// 日期格式化
    /// </summary>
    /// <remarks>
    /// <para>YYYY-MM-DD HH:mm:ss</para>
    /// <para>YYYY-MM-DD HH:mm</para>
    /// <para>YYYY-MM-DD</para>
    /// <para>YYYY-MM</para>
    /// <para>YYYY</para>
    /// <para>MM</para>
    /// <para>DD</para>
    /// <para>MM-DD</para>
    /// <para>HH:mm:ss</para>
    /// <para>HH:mm</para>
    /// <para>HH</para>
    /// <para>mm:ss</para>
    /// <para>mm</para>
    /// <para>ss</para>
    /// </remarks>
    [SugarColumn(ColumnDescription = "日期格式化", Length = 19)]
    public string DateFormat { get; set; }

    /// <summary>
    /// 权限标识
    /// </summary>
    [SugarColumn(ColumnDescription = "权限标识", Length = 200, IsJson = true)]
    public List<string> AuthTag { get; set; }

    /// <summary>
    /// 数据删除
    /// </summary>
    [SugarColumn(ColumnDescription = "数据删除", Length = 50)]
    public string DataDeleteField { get; set; }

    /// <summary>
    /// 插槽名称
    /// </summary>
    [SugarColumn(ColumnDescription = "插槽名称", Length = 50)]
    public string Slot { get; set; }

    /// <summary>
    /// 其他配置
    /// </summary>
    [SugarColumn(ColumnDescription = "其他配置", ColumnDataType = StaticConfig.CodeFirst_BigString, IsJson = true)]
    public List<FaTableColumnAdvancedCtx> OtherConfig { get; set; }

    /// <summary>
    /// 纯搜索
    /// </summary>
    [SugarColumn(ColumnDescription = "纯搜索")]
    public bool PureSearch { get; set; }

    /// <summary>
    /// 搜索项
    /// </summary>
    /// <remarks>
    /// <para>el-input</para>
    /// <para>el-input-number</para>
    /// <para>el-select</para>
    /// <para>el-select-v2</para>
    /// <para>el-tree-select</para>
    /// <para>el-cascader</para>
    /// <para>el-date-picker</para>
    /// <para>el-time-picker</para>
    /// <para>el-time-select</para>
    /// <para>el-switch</para>
    /// <para>el-slider</para>
    /// <para>slot</para>
    /// </remarks>
    [SugarColumn(ColumnDescription = "搜索项", Length = 50)]
    public string SearchEl { get; set; }

    /// <summary>
    /// 搜索项Key
    /// </summary>
    [SugarColumn(ColumnDescription = "搜索项Key", Length = 50)]
    public string SearchKey { get; set; }

    /// <summary>
    /// 搜索项名称
    /// </summary>
    [SugarColumn(ColumnDescription = "搜索项名称", Length = 50)]
    public string SearchLabel { get; set; }

    /// <summary>
    /// 搜索项排序
    /// </summary>
    [SugarColumn(ColumnDescription = "搜索项排序")]
    public int SearchOrder { get; set; }

    /// <summary>
    /// 搜索项插槽
    /// </summary>
    [SugarColumn(ColumnDescription = "搜索项插槽", Length = 50)]
    public string SearchSlot { get; set; }

    /// <summary>
    /// 搜索配置
    /// </summary>
    [SugarColumn(ColumnDescription = "搜索配置", ColumnDataType = StaticConfig.CodeFirst_BigString, IsJson = true)]
    public List<FaTableColumnAdvancedCtx> SearchConfig { get; set; }
}