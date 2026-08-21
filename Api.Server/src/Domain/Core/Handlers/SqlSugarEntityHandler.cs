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
using Fast.CenterLog.Enum;
using Fast.SqlSugar;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using Yitter.IdGenerator;

namespace Fast.Core;

/// <summary>
/// Sugar实体处理
/// </summary>
public class SqlSugarEntityHandler : ISqlSugarEntityHandler
{
    /// <summary>
    /// 授权用户
    /// </summary>
    private readonly IUser _user;

    /// <summary>
    /// SqlSugar 实体服务
    /// </summary>
    private readonly ISqlSugarEntityService _sqlSugarEntityService;

    /// <summary>
    /// 请求上下文
    /// </summary>
    private readonly HttpContext _httpContext;

    /// <summary>
    /// SQL 日志专用通道
    /// </summary>
    private readonly SqlSugarLogChannel _sqlSugarLogChannel;

    /// <summary>
    /// 初始化 SqlSugar 实体处理器
    /// </summary>
    public SqlSugarEntityHandler(IUser user, ISqlSugarEntityService sqlSugarEntityService,
        IHttpContextAccessor httpContextAccessor, SqlSugarLogChannel sqlSugarLogChannel)
    {
        _user = user;
        _sqlSugarEntityService = sqlSugarEntityService;
        _httpContext = httpContextAccessor.HttpContext;
        _sqlSugarLogChannel = sqlSugarLogChannel;
    }

    /// <inheritdoc />
    public async Task<ConnectionSettingsOptions> GetConnectionSettings<TEntity>(ISqlSugarClient sqlSugarClient,
        SugarDbTypeAttribute sugarDbType, Type entityType)
    {
        var databaseTypeStr = sugarDbType.Type?.ToString();
        if (string.IsNullOrWhiteSpace(databaseTypeStr))
            return null;

        var databaseType = Enum.Parse<DatabaseTypeEnum>(databaseTypeStr, true);

        switch (databaseType)
        {
            case DatabaseTypeEnum.Center:
                return SqlSugarContext.ConnectionSettings;
            case DatabaseTypeEnum.CenterLog:
            case DatabaseTypeEnum.Gateway:
            case DatabaseTypeEnum.Deploy:
                return await _sqlSugarEntityService.GetConnectionSetting(CommonConst.Default.TenantId,
                    CommonConst.Default.TenantNo, databaseType);
            case DatabaseTypeEnum.Admin:
            case DatabaseTypeEnum.AdminLog:
                return await _sqlSugarEntityService.GetConnectionSetting(_user.TenantId, _user.TenantNo, databaseType);
            default:
                throw new SqlSugarException("未知的 Database 类型！");
        }
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(string rawSql, SugarParameter[] parameters, TimeSpan executeTime, string handlerSql)
    {
        // 获取 CenterLog 库的连接字符串配置
        var connectionSetting = await _sqlSugarEntityService.GetConnectionSetting(CommonConst.Default.TenantId,
            CommonConst.Default.TenantNo, DatabaseTypeEnum.CenterLog);
        var connectionConfig = SqlSugarContext.GetConnectionConfig(connectionSetting);

        // 组装数据
        var sqlExecutionLogModel = new SqlExecutionLogModel
        {
            RecordId = YitIdHelper.NextId(),
            AccountId = _user.AccountId,
            Mobile = _user.Mobile,
            NickName = _user.NickName,
            ExecuteSeconds = executeTime.TotalSeconds,
            PureSql = handlerSql,
            DepartmentId = _user.DepartmentId,
            DepartmentName = _user.DepartmentName,
            CreatedUserId = _user.EmployeeId,
            CreatedUserName = _user.EmployeeName,
            CreatedTime = DateTime.Now,
            TenantId = _user.TenantId,
            TenantName = _user.TenantName
        };
        sqlExecutionLogModel.RecordCreate(_httpContext);

        // 只等待日志进入有界通道；通道满时自然施加背压，不在业务请求中等待日志数据库写入
        await _sqlSugarLogChannel.WriteAsync(connectionConfig, sqlExecutionLogModel);
    }

    /// <inheritdoc />
    public async Task ExecuteTimeoutAsync(string fileName, int fileLine, string methodName, string rawSql,
        SugarParameter[] parameters, TimeSpan executeTime, string handlerSql, string message)
    {
        // 获取 CenterLog 库的连接字符串配置
        var connectionSetting = await _sqlSugarEntityService.GetConnectionSetting(CommonConst.Default.TenantId,
            CommonConst.Default.TenantNo, DatabaseTypeEnum.CenterLog);
        var connectionConfig = SqlSugarContext.GetConnectionConfig(connectionSetting);

        // 组装数据
        var sqlTimeoutLogModel = new SqlTimeoutLogModel
        {
            RecordId = YitIdHelper.NextId(),
            AccountId = _user.AccountId,
            Mobile = _user.Mobile,
            NickName = _user.NickName,
            FileName = fileName,
            FileLine = fileLine,
            MethodName = methodName,
            TimeoutSeconds = executeTime.TotalSeconds,
            PureSql = handlerSql,
            DepartmentId = _user.DepartmentId,
            DepartmentName = _user.DepartmentName,
            CreatedUserId = _user.EmployeeId,
            CreatedUserName = _user.EmployeeName,
            CreatedTime = DateTime.Now,
            TenantId = _user.TenantId,
            TenantName = _user.TenantName
        };
        sqlTimeoutLogModel.RecordCreate(_httpContext);

        // 只等待日志进入有界通道；通道满时自然施加背压，不在业务请求中等待日志数据库写入
        await _sqlSugarLogChannel.WriteAsync(connectionConfig, sqlTimeoutLogModel);
    }

    /// <inheritdoc />
    public async Task ExecuteDiffLogAsync(DiffType diffType, string tableName, string tableDescription, object businessData,
        List<List<DiffLogColumnInfo>> beforeColumnList, List<List<DiffLogColumnInfo>> afterColumnList, string rawSql,
        SugarParameter[] parameters, TimeSpan? executeTime, string handlerSql)
    {
        // 获取 CenterLog 库的连接字符串配置
        var connectionSetting = await _sqlSugarEntityService.GetConnectionSetting(CommonConst.Default.TenantId,
            CommonConst.Default.TenantNo, DatabaseTypeEnum.CenterLog);
        var connectionConfig = SqlSugarContext.GetConnectionConfig(connectionSetting);

        var diffLogType = diffType switch
        {
            DiffType.insert => DiffLogTypeEnum.Insert,
            DiffType.update => DiffLogTypeEnum.Update,
            DiffType.delete => DiffLogTypeEnum.Delete,
            _ => DiffLogTypeEnum.Unknown
        };

        // 组装数据
        var sqlDiffLogModel = new SqlDiffLogModel
        {
            RecordId = YitIdHelper.NextId(),
            AccountId = _user.AccountId,
            Mobile = _user.Mobile,
            NickName = _user.NickName,
            DiffType = diffLogType,
            TableName = tableName,
            TableDescription = tableDescription,
            BeforeColumnList = beforeColumnList,
            AfterColumnList = afterColumnList,
            ExecuteSeconds = executeTime?.TotalSeconds,
            PureSql = handlerSql,
            DepartmentId = _user.DepartmentId,
            DepartmentName = _user.DepartmentName,
            CreatedUserId = _user.EmployeeId,
            CreatedUserName = _user.EmployeeName,
            CreatedTime = DateTime.Now,
            TenantId = _user.TenantId,
            TenantName = _user.TenantName
        };
        sqlDiffLogModel.RecordCreate(_httpContext);

        // 只等待日志进入有界通道；通道满时自然施加背压，不在业务请求中等待日志数据库写入
        await _sqlSugarLogChannel.WriteAsync(connectionConfig, sqlDiffLogModel);
    }

    /// <inheritdoc />
    public async Task ExecuteErrorAsync(string fileName, int fileLine, string methodName, string rawSql,
        SugarParameter[] parameters, string handlerSql, SqlSugarException exception)
    {
        // 获取 CenterLog 库的连接字符串配置
        var connectionSetting = await _sqlSugarEntityService.GetConnectionSetting(CommonConst.Default.TenantId,
            CommonConst.Default.TenantNo, DatabaseTypeEnum.CenterLog);
        var connectionConfig = SqlSugarContext.GetConnectionConfig(connectionSetting);

        // 组装数据
        var sqlExceptionLogModel = new SqlExceptionLogModel
        {
            RecordId = YitIdHelper.NextId(),
            AccountId = _user.AccountId,
            Mobile = _user.Mobile,
            NickName = _user.NickName,
            FileName = fileName,
            FileLine = fileLine,
            MethodName = methodName,
            Message = exception.Message,
            Source = exception.Source,
            StackTrace = exception.StackTrace,
            PureSql = handlerSql,
            DepartmentId = _user.DepartmentId,
            DepartmentName = _user.DepartmentName,
            CreatedUserId = _user.EmployeeId,
            CreatedUserName = _user.EmployeeName,
            CreatedTime = DateTime.Now,
            TenantId = _user.TenantId,
            TenantName = _user.TenantName
        };
        sqlExceptionLogModel.RecordCreate(_httpContext);

        // 只等待日志进入有界通道；通道满时自然施加背压，不在业务请求中等待日志数据库写入
        await _sqlSugarLogChannel.WriteAsync(connectionConfig, sqlExceptionLogModel);
    }

    /// <inheritdoc />
    public bool IsSuperAdmin()
    {
        return _user.IsSuperAdmin;
    }

    /// <inheritdoc />
    public bool IsAdmin()
    {
        return _user.IsAdmin;
    }

    /// <inheritdoc />
    public long? AssignTenantId()
    {
        return _user.TenantId;
    }

    /// <inheritdoc />
    public long? AssignDepartmentId()
    {
        return _user.DepartmentId;
    }

    /// <inheritdoc />
    public string AssignDepartmentName()
    {
        return _user.DepartmentName;
    }

    /// <inheritdoc />
    public long? AssignUserId()
    {
        return _user.EmployeeId;
    }

    /// <inheritdoc />
    public string AssignUserName()
    {
        return string.IsNullOrWhiteSpace(_user.EmployeeName) ? _user.NickName : _user.EmployeeName;
    }
}