// ReSharper disable once CheckNamespace

namespace Fast.Center.Entity;

/// <summary>
/// <see cref="ApiInfoModel"/> 接口信息表Model类
/// </summary>
[SugarTable("ApiInfo", "接口信息表")]
[SugarDbType(DatabaseTypeEnum.Center)]
public class ApiInfoModel : IDatabaseEntity
{
    /// <summary>
    /// 接口Id
    /// </summary>
    [SugarColumn(ColumnDescription = "接口Id", IsPrimaryKey = true)]
    public long ApiId { get; set; }

    /// <summary>
    /// 服务名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "服务名称", Length = 20)]
    public string ServiceName { get; set; }

    /// <summary>
    /// 分组名称
    /// </summary>
    [Required]
    [SugarColumn(ColumnDescription = "分组名称", Length = 20)]
    public string GroupName { get; set; }

    /// <summary>
    /// 分组标题
    /// </summary>
    [SugarColumn(ColumnDescription = "分组标题", Length = 20)]
    public string GroupTitle { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    [SugarColumn(ColumnDescription = "版本", Length = 20)]
    public string Version { get; set; }

    /// <summary>
    /// 分组描述
    /// </summary>
    [SugarColumn(ColumnDescription = "分组描述", Length = 200)]
    public string Description { get; set; }

    /// <summary>
    /// 模块名称
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "模块名称", Length = 20)]
    public string ModuleName { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    [Required]
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "接口地址", Length = 200)]
    public string ApiUrl { get; set; }

    /// <summary>
    /// 接口名称
    /// </summary>
    [SugarSearchValue]
    [SugarColumn(ColumnDescription = "接口名称", Length = 50)]
    public string ApiName { get; set; }

    /// <summary>
    /// 请求方式
    /// </summary>
    [SugarColumn(ColumnDescription = "请求方式")]
    public HttpRequestMethodEnum Method { get; set; }

    /// <summary>
    /// 操作方式
    /// </summary>
    [SugarColumn(ColumnDescription = "操作方式")]
    public HttpRequestActionEnum Action { get; set; }

    /// <summary>
    /// 是否检测授权
    /// </summary>
    [SugarColumn(ColumnDescription = "是否检测授权")]
    public bool HasAuth { get; set; }

    /// <summary>
    /// 是否检测权限
    /// </summary>
    [SugarColumn(ColumnDescription = "是否检测权限")]
    public bool HasPermission { get; set; }

    /// <summary>
    /// 标签
    /// </summary>
    [SugarColumn(ColumnDescription = "标签", Length = 200, IsJson = true)]
    public List<string> Tags { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(ColumnDescription = "排序")]
    public int Sort { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Required, SugarSearchTime, SugarColumn(ColumnDescription = "创建时间", CreateTableFieldSort = 993)]
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [SugarColumn(ColumnDescription = "更新时间", CreateTableFieldSort = 996)]
    public DateTime? UpdatedTime { get; set; }

    /// <summary>Serves as the default hash function.</summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return ApiId.GetHashCode();
    }

    /// <summary>Determines whether the specified object is equal to the current object.</summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    /// <see langword="true" /> if the specified object  is equal to the current object; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object obj)
    {
        if (obj is not ApiInfoModel oldApiModel)
            return false;

        return ApiId == oldApiModel.ApiId
               && ServiceName == oldApiModel.ServiceName
               && GroupName == oldApiModel.GroupName
               && GroupTitle == oldApiModel.GroupTitle
               && Version == oldApiModel.Version
               && Description == oldApiModel.Description
               && ModuleName == oldApiModel.ModuleName
               && ApiUrl == oldApiModel.ApiUrl
               && ApiName == oldApiModel.ApiName
               && Method == oldApiModel.Method
               && Action == oldApiModel.Action
               && HasAuth == oldApiModel.HasAuth
               && HasPermission == oldApiModel.HasPermission
               && Tags.SequenceEqual(oldApiModel.Tags)
               && Sort == oldApiModel.Sort;
    }
}