namespace Fast.Center.Service.Table.Dto;

/// <summary>
/// <see cref="SaveUserTableConfigInput"/> 保存用户表格配置输入
/// </summary>
public class SaveUserTableConfigInput
{
    /// <summary>
    /// 表格Key
    /// </summary>
    [StringRequired(ErrorMessage = "表格Key不能为空")]
    public string TableKey { get; set; }

    /// <summary>
    /// 表格列
    /// </summary>
    public List<SaveUserTableColumnConfigDto> Columns { get; set; }

    /// <summary>
    /// 保存用户表格列配置Dto
    /// </summary>
    public class SaveUserTableColumnConfigDto
    {
        /// <summary>
        /// 表格列Id
        /// </summary>
        [LongRequired(ErrorMessage = "表格列Id不能为空")]
        public long ColumnId { get; set; }

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
        public int? Width { get; set; }

        /// <summary>
        /// 小的宽度
        /// </summary>
        public int? SmallWidth { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        /// <remarks>从小到大</remarks>
        public int? Order { get; set; }

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
        /// 搜索项名称
        /// </summary>
        public string SearchLabel { get; set; }

        /// <summary>
        /// 搜索项排序
        /// </summary>
        public int? SearchOrder { get; set; }
    }
}