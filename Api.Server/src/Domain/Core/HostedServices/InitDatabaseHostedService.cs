using System.Text;
using Fast.Center.Entity;
using Fast.Center.Enum;
using Fast.SqlSugar;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;
using Yitter.IdGenerator;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="InitDatabaseHostedService"/> 初始化 Database 托管服务
/// </summary>
[Order(101)]
public class InitDatabaseHostedService : IHostedService
{
    /// <summary>
    /// <see cref="IServiceProvider"/> 服务提供者
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// <see cref="InitDatabaseHostedService"/> 初始化 Database 托管服务
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> 服务提供者</param>
    /// <param name="logger"><see cref="ILogger"/> 日志</param>
    public InitDatabaseHostedService(IServiceProvider serviceProvider, ILogger<InitDatabaseHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous Start operation.</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var db = new SqlSugarClient(SqlSugarContext.GetConnectionConfig(SqlSugarContext.ConnectionSettings));

            // 创建库
            db.DbMaintenance.CreateDatabase();

            // 查询核心表是否存在
            var sql =
                $"SELECT COUNT(*) FROM [information_schema].[TABLES] WHERE [TABLE_NAME] = '{typeof(AccountModel).GetSugarTableName()}'";
            if (await db.Ado.GetIntAsync(sql) > 0)
                return;

            // 加载Aop
            SugarEntityFilter.LoadSugarAop(FastContext.HostEnvironment.IsDevelopment(), db);

            {
                var logSb = new StringBuilder();
                logSb.Append("\u001b[40m\u001b[1m\u001b[32m");
                logSb.Append("info");
                logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
                logSb.Append(": ");
                logSb.Append($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffffff zzz dddd}");
                logSb.Append(Environment.NewLine);
                logSb.Append("\u001b[40m\u001b[90m");
                logSb.Append("      ");
                logSb.Append("开始初始化数据库...");
                logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
                Console.WriteLine(logSb.ToString());
            }

            // 获取所有不分表的Model类型
            var tableTypes = SqlSugarContext.SqlSugarEntityList.Where(wh => !wh.IsSplitTable)
                .Where(wh => wh.SugarDbType == null || (DatabaseTypeEnum)wh.SugarDbType == DatabaseTypeEnum.Center)
                .Select(sl => sl.EntityType)
                .ToArray();
            // 获取所有分表的Model类型
            var splitTableTypes = SqlSugarContext.SqlSugarEntityList.Where(wh => wh.IsSplitTable)
                .Where(wh => wh.SugarDbType == null || (DatabaseTypeEnum)wh.SugarDbType == DatabaseTypeEnum.Center)
                .Select(sl => sl.EntityType)
                .ToArray();

            // 创建表
            db.CodeFirst.InitTables(tableTypes);
            db.CodeFirst.SplitTables()
                .InitTables(splitTableTypes);

            var dateTime = new DateTime(2025, 01, 01);

            // 初始化系统租户
            var systemTenantModel = new TenantModel
            {
                TenantId = CommonConst.Default.TenantId,
                TenantNo = CommonConst.Default.TenantNo,
                TenantCode = "Fast",
                Status = CommonStatusEnum.Enable,
                TenantName = "FastDotNet工作室",
                ShortName = "Fast",
                SpellName = "fast dotnet gong zuo shi",
                Edition = EditionEnum.Internal,
                AdminAccountId = CommonConst.Default.SuperAdminAccountId,
                AdminName = "超级管理员",
                AdminMobile = "15580001115",
                AdminEmail = "2875616188@qq.com",
                AdminPhone = null,
                RobotName = "机器人",
                TenantType = TenantTypeEnum.System,
                LogoUrl = "https://gitee.com/China-xiaoFang/fast.admin/raw/master/Fast.png",
                AllowDeleteData = true,
                DatabaseInitialized = false,
                CreatedTime = dateTime
            };
            systemTenantModel = await db.Insertable(systemTenantModel)
                .ExecuteReturnEntityAsync();

            #region 超级管理员

            var superAdminAccountModel = new AccountModel
            {
                AccountId = CommonConst.Default.SuperAdminAccountId,
                AccountKey = VerificationUtil.IdToCodeByLong(CommonConst.Default.SuperAdminAccountId),
                Mobile = "15580001115",
                Email = "2875616188@qq.com",
                Password = CryptoUtil.SHA1Encrypt(CommonConst.Default.AdminPassword)
                    .ToUpper(),
                NickName = "超级管理员",
                Avatar = "https://gitee.com/China-xiaoFang/fast.admin/raw/master/Fast.png",
                Status = CommonStatusEnum.Enable,
                Sex = GenderEnum.Unknown,
                Birthday = new DateTime(1998, 01, 01),
                CreatedTime = dateTime
            };
            superAdminAccountModel = await db.Insertable(superAdminAccountModel)
                .ExecuteReturnEntityAsync();

            var superAdminUserId = YitIdHelper.NextId();
            var robotUserId = YitIdHelper.NextId();
            await db.Insertable(new List<TenantUserModel>
                {
                    new()
                    {
                        UserId = superAdminUserId,
                        UserKey = VerificationUtil.IdToCodeByLong(superAdminUserId),
                        AccountId = superAdminAccountModel.AccountId,
                        Account = "SuperAdmin",
                        EmployeeNo = "",
                        EmployeeName = "超级管理员",
                        IdPhoto = "https://gitee.com/China-xiaoFang/fast.admin/raw/master/Fast.png",
                        DepartmentId = null,
                        DepartmentName = null,
                        UserType = UserTypeEnum.SuperAdmin,
                        Status = CommonStatusEnum.Enable,
                        CreatedTime = dateTime,
                        TenantId = systemTenantModel.TenantId
                    },
                    new()
                    {
                        UserId = robotUserId,
                        UserKey = VerificationUtil.IdToCodeByLong(robotUserId),
                        AccountId = -99,
                        Account = $"{systemTenantModel.TenantCode}_Robot",
                        EmployeeNo = "",
                        EmployeeName = systemTenantModel.RobotName,
                        UserType = UserTypeEnum.Robot,
                        Status = CommonStatusEnum.Disable,
                        CreatedTime = dateTime,
                        TenantId = systemTenantModel.TenantId
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            #region PasswordRecordModel

            // 初始化密码记录表
            await db.Insertable(new List<PasswordRecordModel>
                {
                    new()
                    {
                        AccountId = superAdminAccountModel.AccountId,
                        OperationType = PasswordOperationTypeEnum.Create,
                        Type = PasswordTypeEnum.SHA1,
                        Password = CryptoUtil.SHA1Encrypt(CommonConst.Default.AdminPassword)
                            .ToUpper(),
                        CreatedTime = dateTime
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            #region PasswordMap

            // 初始化密码映射表
            await db.Insertable(new List<PasswordMapModel>
                {
                    new()
                    {
                        Type = PasswordTypeEnum.MD5,
                        Plaintext = CommonConst.Default.AdminPassword,
                        Ciphertext = CryptoUtil.MD5Encrypt(CommonConst.Default.AdminPassword)
                    },
                    new()
                    {
                        Type = PasswordTypeEnum.SHA1,
                        Plaintext = CommonConst.Default.AdminPassword,
                        Ciphertext = CryptoUtil.SHA1Encrypt(CommonConst.Default.AdminPassword)
                    },
                    new()
                    {
                        Type = PasswordTypeEnum.MD5,
                        Plaintext = CommonConst.Default.Password,
                        Ciphertext = CryptoUtil.MD5Encrypt(CommonConst.Default.Password)
                    },
                    new()
                    {
                        Type = PasswordTypeEnum.SHA1,
                        Plaintext = CommonConst.Default.Password,
                        Ciphertext = CryptoUtil.SHA1Encrypt(CommonConst.Default.Password)
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            // 系统数据库
            await DatabaseSeedData.SystemDatabaseSeedData(db, systemTenantModel.TenantId, systemTenantModel.TenantCode,
                dateTime);

            // 配置
            await ConfigSeedData.SystemConfigSeedData(db, dateTime);

            // 系统序号规则
            await SysSerialSeedData.SeedData(db);

            // 应用
            var applicationModel = await ApplicationSeedData.SeedData(db, dateTime);

            // 菜单
            await MenuSeedData.DefaultMenuSeedData(db, applicationModel, dateTime);

            // 初始化普通租户
            TenantModel defaultTenantModel;
            // 开启事务
            await db.Ado.BeginTranAsync();
            try
            {
                var tenantNo = SysSerialContext.GenTenantNo(db);
                defaultTenantModel = new TenantModel
                {
                    TenantId = YitIdHelper.NextId(),
                    TenantNo = tenantNo,
                    TenantCode = "XiaoF",
                    Status = CommonStatusEnum.Enable,
                    TenantName = "小方科技有限公司",
                    ShortName = "小方科技",
                    SpellName = "xiao fang ke ji you xian gong si",
                    Edition = EditionEnum.Flagship,
                    AdminName = "管理员",
                    AdminMobile = "15580001115",
                    AdminEmail = "2875616188@qq.com",
                    AdminPhone = null,
                    RobotName = "小方机器人",
                    TenantType = TenantTypeEnum.Common,
                    LogoUrl = "https://gitee.com/China-xiaoFang/fast.admin/raw/master/Fast.png",
                    AllowDeleteData = true,
                    DatabaseInitialized = false,
                    CreatedTime = dateTime
                };
                await db.Insertable(defaultTenantModel)
                    .ExecuteCommandAsync(cancellationToken);

                // 提交事务
                await db.Ado.CommitTranAsync();
            }
            catch
            {
                // 回滚事务
                await db.Ado.RollbackTranAsync();
                throw;
            }

            // 租户数据库
            await DatabaseSeedData.SystemDatabaseSeedData(db, defaultTenantModel.TenantId,
                defaultTenantModel.TenantCode,
                dateTime);

            // 创建请求作用域
            using var scope = _serviceProvider.CreateScope();
            // 初始化租户数据库
            var tenantDatabaseService = scope.ServiceProvider.GetService<ITenantDatabaseService>();
            await tenantDatabaseService.InitTenantDatabase(systemTenantModel.TenantId);
            await tenantDatabaseService.InitTenantDatabase(defaultTenantModel.TenantId);

            {
                var logSb = new StringBuilder();
                logSb.Append("\u001b[40m\u001b[1m\u001b[32m");
                logSb.Append("info");
                logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
                logSb.Append(": ");
                logSb.Append($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffffff zzz dddd}");
                logSb.Append(Environment.NewLine);
                logSb.Append("\u001b[40m\u001b[90m");
                logSb.Append("      ");
                logSb.Append("初始化数据库成功。");
                logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
                Console.WriteLine(logSb.ToString());
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Init database error...");
        }
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous Stop operation.</returns>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}