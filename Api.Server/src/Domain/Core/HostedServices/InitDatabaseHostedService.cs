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

using System.Text;
using Fast.Center.Entity;
using Fast.Center.Enum;
using Fast.SqlSugar;
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
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// <see cref="InitDatabaseHostedService"/> 初始化 Database 托管服务
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/> 日志</param>
    public InitDatabaseHostedService(ILogger<InitDatabaseHostedService> logger)
    {
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
                .Where(wh => wh.SugarDbType == null || (DatabaseTypeEnum) wh.SugarDbType == DatabaseTypeEnum.Center)
                .Select(sl => sl.EntityType)
                .ToArray();
            // 获取所有分表的Model类型
            var splitTableTypes = SqlSugarContext.SqlSugarEntityList.Where(wh => wh.IsSplitTable)
                .Where(wh => wh.SugarDbType == null || (DatabaseTypeEnum) wh.SugarDbType == DatabaseTypeEnum.Center)
                .Select(sl => sl.EntityType)
                .ToArray();

            // 创建表
            db.CodeFirst.InitTables(tableTypes);
            db.CodeFirst.SplitTables()
                .InitTables(splitTableTypes);

            var dateTime = new DateTime(2025, 01, 01);

            #region 超级管理员

            var superAdminAccountModel = new AccountModel
            {
                Id = CommonConst.Default.SuperAdminAccountId,
                AccountKey = VerificationUtil.IdToCodeByLong(CommonConst.Default.SuperAdminAccountId),
                Mobile = "15580001115",
                Email = "2875616188@qq.com",
                Password = CryptoUtil.SHA1Encrypt(CommonConst.Default.AdminPassword)
                    .ToUpper(),
                NickName = "超级管理员",
                Avatar = "https://gitee.com/China-xiaoFang/fast.admin/raw/master/Fast.png",
                Status = CommonStatusEnum.Enable,
                Phone = null,
                Sex = GenderEnum.Unknown,
                Birthday = null,
                CreatedUserId = null,
                CreatedUserName = "超级管理员",
                CreatedTime = dateTime
            };
            superAdminAccountModel = await db.Insertable(superAdminAccountModel)
                .ExecuteReturnEntityAsync();
            var superAdminUserId = YitIdHelper.NextId();
            var superAdminUserModel = new TenantUserModel
            {
                Id = superAdminUserId,
                UserKey = VerificationUtil.IdToCodeByLong(superAdminUserId),
                AccountId = CommonConst.Default.SuperAdminAccountId,
                Account = "SuperAdmin",
                LoginEmployeeNo = "",
                EmployeeNo = "",
                EmployeeName = "超级管理员",
                IdPhoto = "https://gitee.com/China-xiaoFang/fast.admin/raw/master/Fast.png",
                DeptId = null,
                DeptName = null,
                UserType = UserTypeEnum.SuperAdmin,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = null,
                CreatedUserName = "超级管理员",
                CreatedTime = dateTime,
                TenantId = CommonConst.Default.TenantId
            };
            superAdminUserModel = await db.Insertable(superAdminUserModel)
                .ExecuteReturnEntityAsync();

            #endregion

            // 初始化系统租户
            var systemTenantModel = new TenantModel
            {
                Id = CommonConst.Default.TenantId,
                TenantNo = CommonConst.Default.TenantNo,
                TenantCode = "Fast",
                Status = CommonStatusEnum.Enable,
                TenantName = "FastDotNet工作室",
                ShortName = "Fast",
                SpellName = "FastDotNet gong zuo shi",
                Edition = EditionEnum.Internal,
                AdminAccountId = CommonConst.Default.SuperAdminAccountId,
                AdminName = "超级管理员",
                AdminMobile = "15580001115",
                AdminEmail = "2875616188@qq.com",
                AdminPhone = null,
                TenantType = TenantTypeEnum.System,
                LogoUrl = "https://gitee.com/China-xiaoFang/fast.admin/raw/master/Fast.png",
                AllowDeleteData = true,
                DatabaseInitialized = false,
                CreatedUserId = superAdminUserModel.Id,
                CreatedUserName = superAdminAccountModel.NickName,
                CreatedTime = dateTime
            };
            systemTenantModel = await db.Insertable(systemTenantModel)
                .ExecuteReturnEntityAsync();

            await db.Insertable(new TenantUserModel
                {
                    Id = YitIdHelper.NextId(),
                    UserKey = "",
                    AccountId = -99,
                    Account = $"{systemTenantModel.TenantCode}_Robot",
                    LoginEmployeeNo = "",
                    EmployeeNo = "",
                    EmployeeName = "机器人",
                    IdPhoto = null,
                    DeptId = null,
                    DeptName = null,
                    UserType = UserTypeEnum.Robot,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = superAdminUserModel.Id,
                    CreatedUserName = superAdminAccountModel.NickName,
                    CreatedTime = dateTime,
                    TenantId = systemTenantModel.Id
                })
                .ExecuteCommandAsync(cancellationToken);

            #region PasswordRecordModel

            // 初始化密码记录表
            await db.Insertable(new List<PasswordRecordModel>
                {
                    new()
                    {
                        AccountId = superAdminAccountModel.Id,
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

            // 配置
            await ConfigSeedData.SystemConfigSeedData(db, superAdminUserModel.Id, superAdminAccountModel.NickName, dateTime);

            // 系统数据库
            await DatabaseSeedData.SystemDatabaseSeedData(db, systemTenantModel.Id, superAdminUserModel.Id,
                superAdminAccountModel.NickName, dateTime);

            // 系统序号规则
            await SysSerialSeedData.SeedData(db);

            // 应用
            var applicationModel = await ApplicationSeedData.SeedData(db, superAdminUserModel.Id,
                superAdminAccountModel.NickName, dateTime);

            // 菜单
            await MenuSeedData.DefaultMenuSeedData(db, applicationModel, superAdminUserModel.Id, superAdminAccountModel.NickName,
                dateTime);

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