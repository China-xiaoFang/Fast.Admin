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
using Fast.Admin.Entity;
using Fast.Admin.Enum;
using Fast.Admin.Service.Database.Dto;
using Fast.Center.Entity;
using Fast.Center.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Yitter.IdGenerator;

namespace Fast.Admin.Service.Database;

/// <summary>
/// <see cref="DatabaseService"/> Database 服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Auth, Name = "database")]
public class DatabaseService : ITenantDatabaseService, ITransientDependency, IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarEntityService _sqlSugarEntityService;

    public DatabaseService(IUser user, ISqlSugarEntityService sqlSugarEntityService)
    {
        _user = user;
        _sqlSugarEntityService = sqlSugarEntityService;
    }

    /// <summary>
    /// 初始化租户数据库
    /// </summary>
    /// <param name="tenantId"><see cref="long"/> 租户Id</param>
    /// <returns></returns>
    [NonAction]
    public async Task InitTenantDatabase(long tenantId)
    {
        var db = new SqlSugarClient(SqlSugarContext.GetConnectionConfig(SqlSugarContext.ConnectionSettings));
        // 加载Aop
        SugarEntityFilter.LoadSugarAop(FastContext.HostEnvironment.IsDevelopment(), db);

        var tenantModel = await db.Queryable<TenantModel>()
            .Where(wh => wh.TenantId == tenantId)
            .SingleAsync();

        if (tenantModel == null)
        {
            throw new UserFriendlyException("租户不存在！");
        }

        if (tenantModel.DatabaseInitialized)
            return;

        var adminConnectionSettings =
            await _sqlSugarEntityService.GetConnectionSetting(tenantModel.TenantId, tenantModel.TenantNo, DatabaseTypeEnum.Admin);
        var adminDb = new SqlSugarClient(SqlSugarContext.GetConnectionConfig(adminConnectionSettings));

        var adminLogConnectionSettings =
            await _sqlSugarEntityService.GetConnectionSetting(tenantModel.TenantId, tenantModel.TenantNo,
                DatabaseTypeEnum.AdminLog);
        var logDb = new SqlSugarClient(SqlSugarContext.GetConnectionConfig(adminLogConnectionSettings));

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
            logSb.Append($"开始同步租户【{tenantModel.TenantName}】数据库...");
            logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
            Console.WriteLine(logSb.ToString());
        }

        // 创建库
        adminDb.DbMaintenance.CreateDatabase();

        // 加载Aop
        SugarEntityFilter.LoadSugarAop(FastContext.HostEnvironment.IsDevelopment(), adminDb);

        // 获取所有不分表的Model类型
        var adminTableTypes = SqlSugarContext.SqlSugarEntityList.Where(wh => !wh.IsSplitTable)
            .Where(wh => wh.SugarDbType == null || (DatabaseTypeEnum) wh.SugarDbType == DatabaseTypeEnum.Admin)
            .Select(sl => sl.EntityType)
            .ToArray();
        // 获取所有分表的Model类型
        var adminSplitTableTypes = SqlSugarContext.SqlSugarEntityList.Where(wh => wh.IsSplitTable)
            .Where(wh => wh.SugarDbType == null || (DatabaseTypeEnum) wh.SugarDbType == DatabaseTypeEnum.Admin)
            .Select(sl => sl.EntityType)
            .ToArray();

        // 同步表
        adminDb.CodeFirst.InitTables(adminTableTypes);
        adminDb.CodeFirst.SplitTables()
            .InitTables(adminSplitTableTypes);

        // 创建库
        logDb.DbMaintenance.CreateDatabase();

        // 加载Aop
        SugarEntityFilter.LoadSugarAop(FastContext.HostEnvironment.IsDevelopment(), logDb);

        // 获取所有不分表的Model类型
        var logTableTypes = SqlSugarContext.SqlSugarEntityList.Where(wh => !wh.IsSplitTable)
            .Where(wh => wh.SugarDbType == null || (DatabaseTypeEnum) wh.SugarDbType == DatabaseTypeEnum.AdminLog)
            .Select(sl => sl.EntityType)
            .ToArray();
        // 获取所有分表的Model类型
        var logSplitTableTypes = SqlSugarContext.SqlSugarEntityList.Where(wh => wh.IsSplitTable)
            .Where(wh => wh.SugarDbType == null || (DatabaseTypeEnum) wh.SugarDbType == DatabaseTypeEnum.AdminLog)
            .Select(sl => sl.EntityType)
            .ToArray();

        // 同步表
        logDb.CodeFirst.InitTables(logTableTypes);
        logDb.CodeFirst.SplitTables()
            .InitTables(logSplitTableTypes);

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
            logSb.Append($"同步租户【{tenantModel.TenantName}】数据库成功。");
            logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
            Console.WriteLine(logSb.ToString());
        }

        if (tenantModel.DatabaseInitialized)
        {
            return;
        }

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
            logSb.Append($"开始初始化租户【{tenantModel.TenantName}】数据库...");
            logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
            Console.WriteLine(logSb.ToString());
        }

        // 查询所有系统规则序号
        var serialRuleList = await adminDb.Queryable<SerialRuleModel>()
            .Where(wh => wh.SerialType == SerialTypeEnum.AutoSerial)
            .ToListAsync();

        // 初始化工号序号
        if (serialRuleList.All(a => a.RuleType != SerialRuleTypeEnum.EmployeeNo))
        {
            await adminDb.Insertable(new SerialRuleModel
                {
                    SerialRuleId = YitIdHelper.NextId(),
                    RuleType = SerialRuleTypeEnum.EmployeeNo,
                    SerialType = SerialTypeEnum.AutoSerial,
                    Prefix = null,
                    DateType = SerialDateTypeEnum.Day,
                    Spacer = SerialSpacerEnum.None,
                    Length = 2
                })
                .ExecuteCommandAsync();
        }

        // 初始化公司（组织架构）
        await adminDb.Insertable(new OrganizationModel
            {
                OrgId = YitIdHelper.NextId(),
                ParentId = 0,
                ParentIds = [0],
                OrgName = tenantModel.TenantName,
                OrgCode = $"{tenantModel.TenantCode.ToLower()}_hq",
                Contacts = tenantModel.AdminName,
                Phone = tenantModel.AdminMobile,
                Sort = 1,
                Remark = null
            })
            .ExecuteCommandAsync();

        // 初始化租户管理员角色
        await adminDb.Insertable(new RoleModel
            {
                RoleId = YitIdHelper.NextId(),
                RoleType = RoleTypeEnum.Admin,
                RoleName = "管理员",
                RoleCode = "manager_role",
                Sort = 1,
                DataScopeType = DataScopeTypeEnum.All,
                Remark = null
            })
            .ExecuteCommandAsync();

        // 判断是否为普通租户
        if (tenantModel.TenantType == TenantTypeEnum.Common)
        {
            var accountModel = await db.Queryable<AccountModel>()
                .Where(wh => wh.Mobile == tenantModel.AdminMobile)
                .SingleAsync();
            if (accountModel == null)
            {
                var accountId = YitIdHelper.NextId();
                accountModel = new AccountModel
                {
                    AccountId = accountId,
                    AccountKey = NumberUtil.IdToCodeByLong(accountId),
                    Mobile = tenantModel.AdminMobile,
                    Email = tenantModel.AdminEmail,
                    Password = CryptoUtil.SHA1Encrypt(CommonConst.Default.Password)
                        .ToUpper(),
                    NickName = tenantModel.AdminName,
                    Avatar = "https://gitee.com/China-xiaoFang/fast.admin/raw/master/Fast.png",
                    Status = CommonStatusEnum.Enable,
                    Sex = GenderEnum.Unknown
                };
                accountModel = await db.Insertable(accountModel)
                    .ExecuteReturnEntityAsync();
            }

            // 回填管理员账号Id
            tenantModel.AdminAccountId = accountModel.AccountId;

            // 初始化租户管理员用户
            var userId = YitIdHelper.NextId();
            var robotUserId = YitIdHelper.NextId();
            await db.Insertable(new List<TenantUserModel>
                {
                    new()
                    {
                        UserId = userId,
                        UserKey = NumberUtil.IdToCodeByLong(userId),
                        AccountId = accountModel.AccountId,
                        Account = $"{tenantModel.TenantCode}_Admin",
                        EmployeeNo = "",
                        EmployeeName = tenantModel.AdminName,
                        IdPhoto = "https://gitee.com/China-xiaoFang/fast.admin/raw/master/Fast.png",
                        DepartmentId = null,
                        DepartmentName = null,
                        UserType = UserTypeEnum.Admin,
                        Status = CommonStatusEnum.Enable,
                        TenantId = tenantModel.TenantId
                    },
                    new()
                    {
                        UserId = robotUserId,
                        UserKey = NumberUtil.IdToCodeByLong(robotUserId),
                        AccountId = -99,
                        Account = $"{tenantModel.TenantCode}_Robot",
                        EmployeeNo = "",
                        EmployeeName = tenantModel.RobotName,
                        UserType = UserTypeEnum.Robot,
                        Status = CommonStatusEnum.Disable,
                        TenantId = tenantModel.TenantId
                    }
                })
                .ExecuteCommandAsync();

            #region PasswordRecordModel

            // 初始化密码记录表
            await db.Insertable(new List<PasswordRecordModel>
                {
                    new()
                    {
                        AccountId = accountModel.AccountId,
                        OperationType = PasswordOperationTypeEnum.Create,
                        Type = PasswordTypeEnum.SHA1,
                        Password = CryptoUtil.SHA1Encrypt(CommonConst.Default.AdminPassword)
                            .ToUpper()
                    }
                })
                .ExecuteCommandAsync();

            #endregion

            // 回填管理员账号Id
            tenantModel.AdminAccountId = accountModel.AccountId;
        }

        tenantModel.DatabaseInitialized = true;
        await db.Updateable(tenantModel)
            .ExecuteCommandAsync();

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
            logSb.Append($"初始化租户【{tenantModel.TenantName}】数据库成功。");
            logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
            Console.WriteLine(logSb.ToString());
        }
    }

    /// <summary>
    /// 同步数据库
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("同步数据库", HttpRequestActionEnum.Submit)]
    public async Task SyncDatabase(SyncDatabaseInput input)
    {
        if (_user?.IsSuperAdmin == false)
            throw new UserFriendlyException("非超级管理员禁止操作！");

        await InitTenantDatabase(input.TenantId);
    }
}