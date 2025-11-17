namespace Fast.Center.Service.Table.Dto;

/// <summary>
/// <see cref="FaTableColumnCtx"/> FastElementPlus FaTable 列上下文
/// </summary>
public class FaTableColumnCtx
{
    /// <summary>
    /// 表格列Id
    /// </summary>
    public long? ColumnId { get; set; }

    /// <summary>
    /// 绑定字段
    /// </summary>
    [StringRequired(ErrorMessage = "绑定字段不能为空")]
    public string Prop { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 固定
    /// </summary>
    /// <remarks>
    /// <para>left：左侧</para>
    /// <para>right：右侧</para>
    /// </remarks>
    public string Fixed { get; set; }

    /// <summary>
    /// 自动宽度
    /// </summary>
    public bool AutoWidth { get; set; }

    /// <summary>
    /// 宽度
    /// </summary>
    [IntRequired(ErrorMessage = "宽度不能为空")]
    public int Width { get; set; }

    /// <summary>
    /// 小的宽度
    /// </summary>
    [IntRequired(ErrorMessage = "小的宽度不能为空")]
    public int SmallWidth { get; set; }

    /// <summary>
    /// 顺序
    /// </summary>
    /// <remarks>从小到大</remarks>
    [IntRequired(ErrorMessage = "顺序不能为空")]
    public int Order { get; set; }

    /// <summary>
    /// 显示
    /// </summary>
    public bool Show { get; set; }

    /// <summary>
    /// 复制
    /// </summary>
    public bool Copy { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public bool Sortable { get; set; }

    /// <summary>
    /// 排序字段
    /// </summary>
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
    public string Type { get; set; }

    /// <summary>
    /// 链接
    /// </summary>
    public bool Link { get; set; }

    /// <summary>
    /// 点击事件名称
    /// </summary>
    public string ClickEmit { get; set; }

    /// <summary>
    /// 标签
    /// </summary>
    public bool Tag { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    public string Enum { get; set; }

    /// <summary>
    /// 日期格式化
    /// </summary>
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
    public string DateFormat { get; set; }

    /// <summary>
    /// 权限标识
    /// </summary>
    [SugarColumn(IsJson = true)]
    public List<string> AuthTag { get; set; }

    /// <summary>
    /// 数据删除
    /// </summary>
    public string DataDeleteField { get; set; }

    /// <summary>
    /// 插槽名称
    /// </summary>
    public string Slot { get; set; }

    /// <summary>
    /// 其他配置
    /// </summary>
    [SugarColumn(IsJson = true)]
    public List<FaTableColumnAdvancedCtx> OtherConfig { get; set; }

    /// <summary>
    /// 纯搜索
    /// </summary>
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
    public string SearchEl { get; set; }

    /// <summary>
    /// 搜索项Key
    /// </summary>
    public string SearchKey { get; set; }

    /// <summary>
    /// 搜索项名称
    /// </summary>
    public string SearchLabel { get; set; }

    /// <summary>
    /// 搜索项排序
    /// </summary>
    public int SearchOrder { get; set; }

    /// <summary>
    /// 搜索项插槽
    /// </summary>
    public string SearchSlot { get; set; }

    /// <summary>
    /// 搜索配置
    /// </summary>
    [SugarColumn(IsJson = true)]
    public List<FaTableColumnAdvancedCtx> SearchConfig { get; set; }
}