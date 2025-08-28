// ------------------------------------------------------------------------
// Apache开源许可证
// 
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本协议授予任何获得本软件及其相关文档（以下简称“软件”）副本的个人或组织。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
// 3.修改或衍生作品须明确标注原作者及原软件出处。
// 
// 特别声明：
// - 本软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
// - 包括但不限于数据丢失、业务中断等情况。
// 
// 免责条款：
// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
// ------------------------------------------------------------------------

using Fast.CenterLog.Entity;
using Fast.Common;
using Fast.SqlSugar;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SqlSugar;
using Yitter.IdGenerator;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="SqlSugarEntityHandler"/> Sugar实体处理
/// </summary>
public class SqlSugarEntityHandler : ISqlSugarEntityHandler
{
    /// <summary>
    /// 授权用户
    /// </summary>
    private readonly IUser _user;

    /// <summary>
    /// SqlSugar实体服务
    /// </summary>
    private readonly ISqlSugarEntityService _sqlSugarEntityService;

    /// <summary>
    /// 请求上下文
    /// </summary>
    private readonly HttpContext _httpContext;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    public SqlSugarEntityHandler(IUser user, ISqlSugarEntityService sqlSugarEntityService,
        IHttpContextAccessor httpContextAccessor, ILogger<ISqlSugarEntityHandler> logger)
    {
        _user = user;
        _sqlSugarEntityService = sqlSugarEntityService;
        _httpContext = httpContextAccessor.HttpContext;
        _logger = logger;
    }

    /// <summary>根据实体类型获取连接字符串</summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="sqlSugarClient"><see cref="T:SqlSugar.ISqlSugarClient" /> 默认库SqlSugar客户端</param>
    /// <param name="sugarDbType">实体类头部的 <see cref="T:Fast.SqlSugar.SugarDbTypeAttribute" /> 特性，如果不存在可能为空</param>
    /// <param name="entityType"><see cref="T:System.Type" /> 实体类型</param>
    /// <returns></returns>
    public async Task<ConnectionSettingsOptions> GetConnectionSettings<TEntity>(ISqlSugarClient sqlSugarClient,
        SugarDbTypeAttribute sugarDbType, Type entityType)
    {
        var databaseTypeStr = sugarDbType.Type?.ToString();
        if (string.IsNullOrWhiteSpace(databaseTypeStr))
            return null;

        var databaseType = Enum.Parse<DatabaseTypeEnum>(databaseTypeStr, true);

        switch (databaseType)
        {
            // 按理说这里不应该存在这些类型的，如果存在则返回默认的
            case DatabaseTypeEnum.FastCloud:
            case DatabaseTypeEnum.FastCloudLog:
            case DatabaseTypeEnum.Deploy:
            case DatabaseTypeEnum.Gateway:
            case DatabaseTypeEnum.Center:
            default:
                return SqlSugarContext.ConnectionSettings;
            case DatabaseTypeEnum.CenterLog:
            case DatabaseTypeEnum.Admin:
                return await _sqlSugarEntityService.GetConnectionSetting(_user.TenantId, _user.TenantNo, databaseType);
        }
    }

    /// <summary>执行Sql</summary>
    /// <param name="rawSql"><see cref="T:System.String" /> 原始Sql语句</param>
    /// <param name="parameters"><see cref="T:SqlSugar.SugarParameter" /> Sql参数</param>
    /// <param name="executeTime"><see cref="T:System.TimeSpan" /> 执行时间</param>
    /// <param name="handlerSql"><see cref="T:System.String" /> 参数化处理后的Sql语句</param>
    /// <returns></returns>
    public async Task ExecuteAsync(string rawSql, SugarParameter[] parameters, TimeSpan executeTime, string handlerSql)
    {
        // 获取 CenterLog 库的连接字符串配置
        var connectionSetting =
            await _sqlSugarEntityService.GetConnectionSetting(_user.TenantId, _user.TenantNo, DatabaseTypeEnum.CenterLog);
        var connectionConfig = SqlSugarContext.GetConnectionConfig(connectionSetting);

        var tenantId = _user.TenantId;
        var tenantNo = _user.TenantNo;

        // 组装数据
        var sqlExecutionLogModel = new SqlExecutionLogModel
        {
            Id = YitIdHelper.NextId(),
            DepartmentId = _user.DepartmentId,
            DepartmentName = _user.DepartmentName,
            // 这里用 Center 库的
            CreatedUserId = _user.UserId,
            CreatedUserName = string.IsNullOrWhiteSpace(_user.NickName) ? _user.EmployeeName : _user.NickName,
            CreatedTime = DateTime.Now,
            TenantId = tenantId,
            AppId = _user.AppId,
            AppName = _user.AppName,
            Mobile = _user.Mobile,
            RawSql = rawSql,
            Parameters = parameters,
            PureSql = handlerSql
        };
        sqlExecutionLogModel.RecordCreate(_httpContext);

        _ = Task.Run(async () =>
        {
            try
            {
                // 这里不能使用AOP
                var db = new SqlSugarClient(connectionConfig);

                // 异步不等待
                await db.Insertable(sqlExecutionLogModel)
                    .SplitTable()
                    .ExecuteCommandAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"TenantId：{tenantId}；TenantNo：{tenantNo}；SqlSugar Aop 执行Sql，保存失败；{ex.Message}");
            }
        });
    }

    /// <summary>执行Sql超时</summary>
    /// <param name="fileName"><see cref="T:System.String" /> 文件名称</param>
    /// <param name="fileLine"><see cref="T:System.Int32" /> 文件行数</param>
    /// <param name="methodName"><see cref="T:System.String" /> 方法名称</param>
    /// <param name="rawSql"><see cref="T:System.String" /> 未处理的Sql语句</param>
    /// <param name="parameters"><see cref="T:SqlSugar.SugarParameter" /> Sql参数</param>
    /// <param name="executeTime"><see cref="T:System.TimeSpan" /> 执行时间</param>
    /// <param name="handlerSql"><see cref="T:System.String" /> 参数化处理后的Sql语句</param>
    /// <param name="message"><see cref="T:System.String" /></param>
    /// <returns></returns>
    public async Task ExecuteTimeoutAsync(string fileName, int fileLine, string methodName, string rawSql,
        SugarParameter[] parameters, TimeSpan executeTime, string handlerSql, string message)
    {
        // TODO:这里本意是放在 FastCloud 库，单是好像会暴漏此库的连接字符串
    }

    /// <summary>执行Sql差异</summary>
    /// <param name="diffType"><see cref="T:SqlSugar.DiffType" /> 差异类型</param>
    /// <param name="tableName"><see cref="T:System.String" /> 表名称</param>
    /// <param name="tableDescription"><see cref="T:System.String" /> 表描述</param>
    /// <param name="diffDescription"><see cref="T:System.String" /> 差异描述</param>
    /// <param name="beforeColumnList"><see cref="T:System.String" /> 执行前列信息</param>
    /// <param name="afterColumnList"><see cref="T:System.String" /> 执行后列信息</param>
    /// <param name="rawSql"><see cref="T:System.String" /> 原始Sql语句</param>
    /// <param name="parameters"><see cref="T:SqlSugar.SugarParameter" /> Sql参数</param>
    /// <param name="executeTime"><see cref="T:System.TimeSpan" /> 执行时间</param>
    /// <param name="handlerSql"><see cref="T:System.String" /> 参数化处理后的Sql语句</param>
    /// <returns></returns>
    public async Task ExecuteDiffLogAsync(DiffType diffType, string tableName, string tableDescription, string diffDescription,
        List<List<DiffLogColumnInfo>> beforeColumnList, List<List<DiffLogColumnInfo>> afterColumnList, string rawSql,
        SugarParameter[] parameters, TimeSpan? executeTime, string handlerSql)
    {
        // 获取 CenterLog 库的连接字符串配置
        var connectionSetting =
            await _sqlSugarEntityService.GetConnectionSetting(_user.TenantId, _user.TenantNo, DatabaseTypeEnum.CenterLog);
        var connectionConfig = SqlSugarContext.GetConnectionConfig(connectionSetting);

        var tenantId = _user.TenantId;
        var tenantNo = _user.TenantNo;

        var diffLogType = diffType switch
        {
            DiffType.insert => DiffLogTypeEnum.Insert,
            DiffType.update => DiffLogTypeEnum.Update,
            DiffType.delete => DiffLogTypeEnum.Delete,
            _ => DiffLogTypeEnum.None
        };

        // 组装数据
        var sqlDiffLogModel = new SqlDiffLogModel
        {
            Id = YitIdHelper.NextId(),
            DepartmentId = _user.DepartmentId,
            DepartmentName = _user.DepartmentName,
            // 这里用 Center 库的
            CreatedUserId = _user.UserId,
            CreatedUserName = string.IsNullOrWhiteSpace(_user.NickName) ? _user.EmployeeName : _user.NickName,
            CreatedTime = DateTime.Now,
            TenantId = tenantId,
            AppId = _user.AppId,
            AppName = _user.AppName,
            Mobile = _user.Mobile,
            DiffType = diffLogType,
            TableName = tableName,
            TableDescription = tableDescription,
            DiffDescription = diffDescription,
            BeforeColumnList = beforeColumnList,
            AfterColumnList = afterColumnList,
            ExecuteSeconds = executeTime?.TotalSeconds,
            RawSql = rawSql,
            Parameters = parameters,
            PureSql = handlerSql
        };
        sqlDiffLogModel.RecordCreate(_httpContext);

        _ = Task.Run(async () =>
        {
            try
            {
                // 这里不能使用AOP
                var db = new SqlSugarClient(connectionConfig);

                // 异步不等待
                await db.Insertable(sqlDiffLogModel)
                    .SplitTable()
                    .ExecuteCommandAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"TenantId：{tenantId}；TenantNo：{tenantNo}；SqlSugar Aop 执行Sql差异，保存失败；{ex.Message}");
            }
        });
    }

    /// <summary>执行Sql错误</summary>
    /// <param name="fileName"><see cref="T:System.String" /> 文件名称</param>
    /// <param name="fileLine"><see cref="T:System.Int32" /> 文件行数</param>
    /// <param name="methodName"><see cref="T:System.String" /> 方法名称</param>
    /// <param name="rawSql"><see cref="T:System.String" /> 原始Sql语句</param>
    /// <param name="parameters"><see cref="T:SqlSugar.SugarParameter" /> Sql参数</param>
    /// <param name="handlerSql"><see cref="T:System.String" /> 参数化处理后的Sql语句</param>
    /// <param name="message"><see cref="T:System.String" /></param>
    /// <returns></returns>
    public async Task ExecuteErrorAsync(string fileName, int fileLine, string methodName, string rawSql,
        SugarParameter[] parameters, string handlerSql, string message)
    {
        // TODO:这里本意是放在 FastCloud 库，单是好像会暴漏此库的连接字符串
    }

    /// <summary>指派租户Id</summary>
    /// <returns></returns>
    public long? AssignTenantId()
    {
        return _user?.TenantId;
    }

    /// <summary>指派部门Id</summary>
    /// <returns></returns>
    public long? AssignDepartmentId()
    {
        return _user?.DepartmentId;
    }

    /// <summary>指派部门名称</summary>
    /// <returns></returns>
    public string AssignDepartmentName()
    {
        return _user?.DepartmentName;
    }

    /// <summary>指派用户Id</summary>
    /// <returns></returns>
    public long? AssignUserId()
    {
        // 这里默认是职员信息，Center库的需要手动赋值
        return _user?.EmployeeId;
    }

    /// <summary>指派用户名称</summary>
    /// <returns></returns>
    public string AssignUserName()
    {
        // 这里默认是职员信息，Center库的需要手动赋值
        return string.IsNullOrWhiteSpace(_user.NickName) ? _user.EmployeeName : _user.NickName;
    }

    /// <summary>是否为超级管理员</summary>
    /// <returns></returns>
    public bool IsSuperAdmin()
    {
        return _user?.IsSuperAdmin ?? false;
    }

    /// <summary>是否为管理员</summary>
    /// <returns></returns>
    public bool IsAdmin()
    {
        return _user?.IsAdmin ?? false;
    }
}