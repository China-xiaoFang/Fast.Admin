//// ------------------------------------------------------------------------
//// Apache开源许可证
//// 
//// 版权所有 © 2018-Now 小方
//// 
//// 许可授权：
//// 本协议授予任何获得本软件及其相关文档（以下简称“软件”）副本的个人或组织。
//// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
//// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
//// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
//// 3.修改或衍生作品须明确标注原作者及原软件出处。
//// 
//// 特别声明：
//// - 本软件按“原样”提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
//// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
//// - 包括但不限于数据丢失、业务中断等情况。
//// 
//// 免责条款：
//// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
//// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
//// ------------------------------------------------------------------------

//using Fast.Common;
//using Fast.FastCloud.Entity;
//using Fast.SqlSugar;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using SqlSugar;

//// ReSharper disable once CheckNamespace
//namespace Fast.FastCloud.Core;

///// <summary>
///// <see cref="PlatformInitService"/> 平台初始化服务
///// </summary>
//public class PlatformInitService
//{
//    /// <summary>
//    /// 数据库
//    /// </summary>
//    private readonly ISqlSugarClient _repository;

//    /// <summary>
//    /// 运行环境
//    /// </summary>
//    private readonly IHostEnvironment _hostEnvironment;

//    /// <summary>
//    /// 日志
//    /// </summary>
//    private readonly ILogger _logger;

//    public PlatformInitService(ISqlSugarClient repository, IHostEnvironment hostEnvironment, ILogger<IPlatformInitService> logger)
//    {
//        _repository = repository;
//        _hostEnvironment = hostEnvironment;
//        _logger = logger;
//    }

//    public async Task InitPlatformDatabase(long platformId)
//    {
//        // 查询平台信息
//        var platformModel = await _repository.Queryable<PlatformModel>()
//            .Where(wh => wh.Id == platformId)
//            .SingleAsync();

//        if (platformModel == null)
//        {
//            throw new UserFriendlyException("平台信息不存在！");
//        }

//        // 判断平台是否为试用平台
//        if (!platformModel.IsTrial)
//        {
//            throw new UserFriendlyException("平台已正式上线，禁止初始化操作！");
//        }

//        // 查询平台对应的数据库信息
//        var centerDatabaseModel = await _repository.Queryable<DatabaseModel>()
//            .Where(wh => wh.Status == CommonStatusEnum.Enable)
//            .Where(wh => wh.PlatformId == platformId)
//            .Where(wh => wh.DatabaseType == DatabaseTypeEnum.Center)
//            .Select(sl => new ConnectionSettingsOptions
//                {
//                    ServiceIp = _hostEnvironment.IsDevelopment()
//                        // 开发环境使用公网地址
//                        ? sl.PublicIp
//                        // 生产环境使用内网地址
//                        : sl.IntranetIp
//                },
//                true)
//            .SingleAsync();

//        if (centerDatabaseModel == null)
//        {
//            throw new UserFriendlyException("平台数据库信息不存在！");
//        }

//        var db = new SqlSugarClient(SqlSugarContext.GetConnectionConfig(centerDatabaseModel));
//        // 加载Aop
//        SugarEntityFilter.LoadSugarAop(_hostEnvironment.IsDevelopment(),
//            db,
//            centerDatabaseModel.SugarSqlExecMaxSeconds,
//            false,
//            true,
//            null);

//        // 创建核心库
//        db.DbMaintenance.CreateDatabase();

//        // 这里可能会存在表，安全期间，先执行一次删除
//        var delTableSql = """
//                          EXEC [sp_msforeachtable] 'ALTER TABLE ? NOCHECK CONSTRAINT all';
//                          EXEC sp_msforeachtable 'DROP TABLE IF EXISTS ?';
//                          """;
//        await db.Ado.ExecuteCommandAsync(delTableSql);

//        _logger.LogInformation("开始初始化平台数据库...");
//    }
//}