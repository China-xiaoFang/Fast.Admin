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
using Fast.Common;
using Fast.FastCloud.Entity;
using Fast.FastCloud.Enum;
using Fast.SqlSugar;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;
using Yitter.IdGenerator;

// ReSharper disable once CheckNamespace
namespace Fast.FastCloud.Core;

/// <summary>
/// <see cref="InitDatabaseHostedService"/> 初始化 Database 托管服务
/// </summary>
[SuppressSniffer]
public class InitDatabaseHostedService : IHostedService
{
    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    public InitDatabaseHostedService(ILogger<InitDatabaseHostedService> logger)
    {
        _logger = logger;
    }

    private int _moduleSort = 1000;

    private int moduleSort
    {
        get
        {
            _moduleSort--;
            return _moduleSort;
        }
    }

    private int _menuSort = 1000;

    private int menuSort
    {
        get
        {
            _menuSort--;
            return _menuSort;
        }
    }

    private int _buttonSort = 1000;

    private int buttonSort
    {
        get
        {
            _buttonSort--;
            return _buttonSort;
        }
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
            // 这里不能使用Aop
            var db = new SqlSugarClient(SqlSugarContext.DefaultConnectionConfig);
            // 执行超时时间
            db.Ado.CommandTimeOut = SqlSugarContext.ConnectionSettings.CommandTimeOut;
            SugarEntityFilter.LoadSugarAop(FastContext.HostEnvironment.IsDevelopment(),
                db,
                SqlSugarContext.ConnectionSettings.SugarSqlExecMaxSeconds,
                false,
                true,
                null);

            // 创建核心库
            db.DbMaintenance.CreateDatabase();

            // 查询核心表是否存在
            var sql
                = $"SELECT COUNT(*) FROM [information_schema].[TABLES] WHERE [TABLE_NAME] = '{typeof(UserModel).GetSugarTableName()}'";
            if (await db.Ado.GetIntAsync(sql) > 0)
                return;

            var logSb1 = new StringBuilder();
            logSb1.Append("\u001b[40m\u001b[1m\u001b[32m");
            logSb1.Append("system_notify");
            logSb1.Append("\u001b[39m\u001b[22m\u001b[49m");
            logSb1.Append(": ");
            logSb1.Append($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffffff zzz dddd}");
            logSb1.Append(Environment.NewLine);
            logSb1.Append("\u001b[40m\u001b[37m");
            logSb1.Append("               ");
            logSb1.Append("开始初始化数据库...");
            logSb1.Append("\u001b[39m\u001b[22m\u001b[49m");
            Console.WriteLine(logSb1.ToString());

            // 获取所有不分表的Model类型
            var tableTypes = SqlSugarContext.SqlSugarEntityList.Where(wh => !wh.IsSplitTable)
                .Where(wh => wh.SugarDbType == null || (DatabaseTypeEnum) wh.SugarDbType == DatabaseTypeEnum.FastCloud)
                .Select(sl => sl.EntityType)
                .ToArray();
            // 获取所有分表的Model类型
            var splitTableTypes = SqlSugarContext.SqlSugarEntityList.Where(wh => wh.IsSplitTable)
                .Where(wh => wh.SugarDbType == null || (DatabaseTypeEnum) wh.SugarDbType == DatabaseTypeEnum.FastCloud)
                .Select(sl => sl.EntityType)
                .ToArray();
            // 创建表
            db.CodeFirst.InitTables(tableTypes);
            db.CodeFirst.SplitTables()
                .InitTables(splitTableTypes);

            #region User

            // 初始化管理员
            var adminUserModel = new UserModel
            {
                Id = CommonConst.DefaultUserId,
                Mobile = "15580001115",
                Password = CryptoUtil.SHA1Encrypt(CommonConst.DefaultPassword),
                UserType = UserTypeEnum.Admin,
                Status = CommonStatusEnum.Enable,
                NickName = "管理员",
                Avatar = "https://gitee.com/China-xiaoFang/fast.admin/raw/master/Fast.png",
                CreatedUserId = CommonConst.DefaultUserId,
                CreatedUserName = "管理员",
                CreatedTime = new DateTime(2025, 01, 01)
            };
            adminUserModel = await db.Insertable(adminUserModel)
                .ExecuteReturnEntityAsync();
            // 初始化机器人
            await db.Insertable(new UserModel
                {
                    Id = YitIdHelper.NextId(),
                    Mobile = "15580001110",
                    Password = CryptoUtil.SHA1Encrypt(CommonConst.DefaultPassword),
                    UserType = UserTypeEnum.Robot,
                    Status = CommonStatusEnum.Enable,
                    NickName = "机器人",
                    Avatar = "https://gitee.com/China-xiaoFang/fast.admin/raw/master/Fast.png",
                    CreatedUserId = adminUserModel.Id,
                    CreatedUserName = adminUserModel.NickName,
                    CreatedTime = new DateTime(2025, 01, 01)
                })
                .ExecuteCommandAsync(cancellationToken);
            // 初始化普通用户
            var noneUserModel = new UserModel
            {
                Id = YitIdHelper.NextId(),
                Mobile = "15580001111",
                Password = CryptoUtil.SHA1Encrypt(CommonConst.DefaultPassword),
                UserType = UserTypeEnum.None,
                Status = CommonStatusEnum.Enable,
                NickName = "小方",
                Avatar = "https://gitee.com/China-xiaoFang/fast.admin/raw/master/Fast.png",
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            noneUserModel = await db.Insertable(noneUserModel)
                .ExecuteReturnEntityAsync();

            #endregion

            #region PasswordMap

            // 初始化密码映射表
            await db.Insertable(new List<PasswordMapModel>
                {
                    new()
                    {
                        Type = PasswordTypeEnum.MD5,
                        Plaintext = CommonConst.DefaultPassword,
                        Ciphertext = CryptoUtil.MD5Encrypt(CommonConst.DefaultPassword)
                    },
                    new() {Type = PasswordTypeEnum.MD5, Plaintext = "123456", Ciphertext = CryptoUtil.MD5Encrypt("123456")},
                    new()
                    {
                        Type = PasswordTypeEnum.SHA1,
                        Plaintext = CommonConst.DefaultPassword,
                        Ciphertext = CryptoUtil.SHA1Encrypt(CommonConst.DefaultPassword)
                    },
                    new() {Type = PasswordTypeEnum.SHA1, Plaintext = "123456", Ciphertext = CryptoUtil.SHA1Encrypt("123456")}
                })
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            #region Edition

            // 初始化版本信息
            var editionPriceList = new List<EditionPriceModel>();
            var editionDiscountList = new List<EditionDiscountModel>();
            foreach (var editionItem in typeof(EditionEnum).EnumToList<EditionEnum>()
                         .Where(wh => wh.Value != EditionEnum.None)
                         .Where(wh => wh.Value != EditionEnum.Internal)
                         .Select((sl, index) => new {sl.Value, Index = index})
                         .ToList())
            {
                var basePrice = 2999M + editionItem.Index * 2000;
                foreach (var durationItem in typeof(RenewalDurationEnum).EnumToList<RenewalDurationEnum>()
                             .Select((sl, index) => new {sl.Value, Index = index})
                             .ToList())
                {
                    var price = basePrice + durationItem.Index * 500;
                    editionPriceList.Add(new EditionPriceModel
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = editionItem.Value,
                        Status = CommonStatusEnum.Enable,
                        Duration = durationItem.Value,
                        Price = price,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    });
                    // 首购
                    editionDiscountList.Add(new EditionDiscountModel
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = editionItem.Value,
                        Status = CommonStatusEnum.Enable,
                        DiscountType = DiscountTypeEnum.FirstBuyFixedPrice,
                        Duration = durationItem.Value,
                        FixedPrice = price - 500,
                        StartTime = new DateTime(2025, 01, 01),
                        EndTime = new DateTime(2099, 12, 31),
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    });
                    // 打折
                    editionDiscountList.Add(new EditionDiscountModel
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = editionItem.Value,
                        Status = CommonStatusEnum.Enable,
                        DiscountType = DiscountTypeEnum.Percentage,
                        Duration = durationItem.Value,
                        DiscountRate = 90,
                        StartTime = new DateTime(2025, 01, 01),
                        EndTime = new DateTime(2099, 12, 31),
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    });
                }
            }

            await db.Insertable(editionPriceList)
                .ExecuteCommandAsync(cancellationToken);
            await db.Insertable(editionDiscountList)
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            #region Platform

            // 初始化平台信息
            var platformModel = new PlatformModel
            {
                Id = CommonConst.DefaultPlatformId,
                PlatformNo = CommonConst.DefaultPlatformNo,
                PlatformName = CommonConst.DefaultPlatformName,
                ShortName = CommonConst.DefaultPlatformName,
                Status = CommonStatusEnum.Enable,
                AdminName = "小方",
                AdminMobile = "15580001115",
                AdminEmail = "2875616188@qq.com",
                LogoUrl = "https://gitee.com/China-xiaoFang/fast.admin/raw/master/Fast.png",
                ActivationTime = new DateTime(2025, 01, 01),
                Edition = EditionEnum.Flagship,
                AutoRenewal = true,
                RenewalExpiryTime = new DateTime(2028, 01, 01),
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            platformModel = await db.Insertable(platformModel)
                .ExecuteReturnEntityAsync();
            // 初始化平台续费记录
            var platformRenewalRecordModel = new PlatformRenewalRecordModel
            {
                Id = YitIdHelper.NextId(),
                PlatformId = platformModel.Id,
                FromEdition = EditionEnum.None,
                ToEdition = EditionEnum.Internal,
                RenewalType = RenewalTypeEnum.Activation,
                Duration = RenewalDurationEnum.ThreeYear,
                RenewalTime = new DateTime(2025, 01, 01),
                RenewalExpiryTime = new DateTime(2028, 01, 01),
                Amount = 0,
                Remark = "内部版，不对外出售",
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            await db.Insertable(platformRenewalRecordModel)
                .ExecuteReturnEntityAsync();

            #endregion

            #region UserPlatform

            // 初始化用户平台权限
            await db.Insertable(new UserPlatformModel {UserId = noneUserModel.Id, PlatformId = platformModel.Id})
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            #region Database

            await db.Insertable(new List<DatabaseModel>
                {
                    // 初始化平台日志库
                    new()
                    {
                        PlatformId = platformModel.Id,
                        DatabaseType = DatabaseTypeEnum.FastCloudLog,
                        DbType = DbType.SqlServer,
                        Status = CommonStatusEnum.Enable,
                        PublicIp = "192.168.10.69",
                        IntranetIp = "127.0.0.1",
                        Port = 3389,
                        DbName = "FastCloudLog",
                        DbUser = "admin",
                        DbPwd = "123456",
                        CommandTimeOut = 60,
                        SugarSqlExecMaxSeconds = 30,
                        DiffLog = false,
                        DisableAop = true,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    // 初始化平台部署库
                    new()
                    {
                        PlatformId = platformModel.Id,
                        DatabaseType = DatabaseTypeEnum.Deploy,
                        DbType = DbType.SqlServer,
                        Status = CommonStatusEnum.Enable,
                        PublicIp = "192.168.10.69",
                        IntranetIp = "127.0.0.1",
                        Port = 3389,
                        DbName = "Deploy",
                        DbUser = "admin",
                        DbPwd = "123456",
                        CommandTimeOut = 60,
                        SugarSqlExecMaxSeconds = 30,
                        DiffLog = true,
                        DisableAop = false,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    // 初始化平台网关库
                    new()
                    {
                        PlatformId = platformModel.Id,
                        DatabaseType = DatabaseTypeEnum.Gateway,
                        DbType = DbType.SqlServer,
                        Status = CommonStatusEnum.Enable,
                        PublicIp = "192.168.10.69",
                        IntranetIp = "127.0.0.1",
                        Port = 3389,
                        DbName = "Gateway",
                        DbUser = "admin",
                        DbPwd = "123456",
                        CommandTimeOut = 60,
                        SugarSqlExecMaxSeconds = 30,
                        DiffLog = true,
                        DisableAop = false,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    // 初始化平台核心库
                    new()
                    {
                        PlatformId = platformModel.Id,
                        DatabaseType = DatabaseTypeEnum.Center,
                        DbType = DbType.SqlServer,
                        Status = CommonStatusEnum.Enable,
                        PublicIp = "192.168.10.69",
                        IntranetIp = "127.0.0.1",
                        Port = 3389,
                        DbName = "Center",
                        DbUser = "admin",
                        DbPwd = "123456",
                        CommandTimeOut = 60,
                        SugarSqlExecMaxSeconds = 30,
                        DiffLog = true,
                        DisableAop = false,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            #region Application

            // 初始化应用信息
            var applicationModel = new ApplicationModel
            {
                Id = CommonConst.DefaultAppId,
                Edition = EditionEnum.Internal,
                AppNo = "Fast000001",
                AppName = CommonConst.DefaultAppName,
                OpenId = "https://admin.fastdotnet.com",
                AppType = AppEnvironmentEnum.Web,
                ServiceStartTime = new DateTime(2025, 01, 01),
                ServiceEndTime = new DateTime(2099, 12, 31),
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            applicationModel = await db.Insertable(applicationModel)
                .ExecuteReturnEntityAsync();

            #endregion

            #region Menu

            #region 系统管理

            var systemModuleModel = new ModuleModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleName = "系统管理",
                Icon = "system",
                ViewType = ModuleViewTypeEnum.Admin,
                Sort = moduleSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            systemModuleModel = await db.Insertable(systemModuleModel)
                .ExecuteReturnEntityAsync();

            #region 用户管理

            var userMenuModel = new MenuModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleId = systemModuleModel.Id,
                MenuCode = "User:Page",
                MenuName = "用户管理",
                MenuTitle = "用户管理",
                MenuType = MenuTypeEnum.Menu,
                HasDesktop = false,
                HasWeb = true,
                WebIcon = "app",
                WebRouter = "/system/user/page",
                WebComponent = "system/user/index",
                HasMobile = false,
                Visible = YesOrNotEnum.Y,
                Sort = menuSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            userMenuModel = await db.Insertable(userMenuModel)
                .ExecuteReturnEntityAsync();
            await db.Insertable(new List<ButtonModel>
                {
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = userMenuModel.Id,
                        ButtonCode = "User:Page",
                        ButtonName = "列表",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = userMenuModel.Id,
                        ButtonCode = "User:Detail",
                        ButtonName = "详情",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = userMenuModel.Id,
                        ButtonCode = "User:Add",
                        ButtonName = "新增",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = userMenuModel.Id,
                        ButtonCode = "User:Edit",
                        ButtonName = "编辑",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            #region 版本管理

            var editionMenuModel = new MenuModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleId = systemModuleModel.Id,
                MenuCode = "Edition:Page",
                MenuName = "版本管理",
                MenuTitle = "版本管理",
                MenuType = MenuTypeEnum.Menu,
                HasDesktop = false,
                HasWeb = true,
                WebIcon = "edition",
                WebRouter = "/system/edition/page",
                WebComponent = "system/edition/index",
                HasMobile = false,
                Visible = YesOrNotEnum.Y,
                Sort = menuSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            editionMenuModel = await db.Insertable(editionMenuModel)
                .ExecuteReturnEntityAsync();
            await db.Insertable(new List<ButtonModel>
                {
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = editionMenuModel.Id,
                        ButtonCode = "Edition:Page",
                        ButtonName = "列表",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = editionMenuModel.Id,
                        ButtonCode = "Edition:Detail",
                        ButtonName = "详情",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = editionMenuModel.Id,
                        ButtonCode = "Edition:Add",
                        ButtonName = "新增",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = editionMenuModel.Id,
                        ButtonCode = "Edition:Edit",
                        ButtonName = "编辑",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = editionMenuModel.Id,
                        ButtonCode = "Edition:Delete",
                        ButtonName = "删除",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            #region Database

            var dbMenuModel = new MenuModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleId = systemModuleModel.Id,
                MenuCode = "Database:Page",
                MenuName = "数据库配置",
                MenuTitle = "数据库配置",
                MenuType = MenuTypeEnum.Menu,
                HasDesktop = false,
                HasWeb = true,
                WebIcon = "database",
                WebRouter = "/system/database/page",
                WebComponent = "system/database/index",
                HasMobile = false,
                Visible = YesOrNotEnum.Y,
                Sort = menuSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            dbMenuModel = await db.Insertable(dbMenuModel)
                .ExecuteReturnEntityAsync();
            await db.Insertable(new List<ButtonModel>
                {
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = dbMenuModel.Id,
                        ButtonCode = "Database:Page",
                        ButtonName = "列表",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = dbMenuModel.Id,
                        ButtonCode = "Database:Detail",
                        ButtonName = "详情",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = dbMenuModel.Id,
                        ButtonCode = "Database:Add",
                        ButtonName = "新增",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = dbMenuModel.Id,
                        ButtonCode = "Database:Edit",
                        ButtonName = "编辑",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = dbMenuModel.Id,
                        ButtonCode = "Database:Delete",
                        ButtonName = "删除",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            #region 应用管理

            var appMenuModel = new MenuModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleId = systemModuleModel.Id,
                MenuCode = "App:Page",
                MenuName = "应用管理",
                MenuTitle = "应用管理",
                MenuType = MenuTypeEnum.Menu,
                HasDesktop = false,
                HasWeb = true,
                WebIcon = "app",
                WebRouter = "/system/app/page",
                WebComponent = "system/app/index",
                HasMobile = false,
                Visible = YesOrNotEnum.Y,
                Sort = menuSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            appMenuModel = await db.Insertable(appMenuModel)
                .ExecuteReturnEntityAsync();
            await db.Insertable(new List<ButtonModel>
                {
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = appMenuModel.Id,
                        ButtonCode = "App:Page",
                        ButtonName = "列表",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = appMenuModel.Id,
                        ButtonCode = "App:Detail",
                        ButtonName = "详情",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = appMenuModel.Id,
                        ButtonCode = "App:Add",
                        ButtonName = "新增",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = appMenuModel.Id,
                        ButtonCode = "App:Edit",
                        ButtonName = "编辑",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = appMenuModel.Id,
                        ButtonCode = "App:Delete",
                        ButtonName = "删除",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            #endregion

            #region 平台管理

            var platformModuleModel = new ModuleModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleName = "平台管理",
                Icon = "platform",
                ViewType = ModuleViewTypeEnum.All,
                Sort = moduleSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            platformModuleModel = await db.Insertable(platformModuleModel)
                .ExecuteReturnEntityAsync();

            #region 系统日志

            var sysLogCLMenuModel = new MenuModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleId = platformModuleModel.Id,
                MenuCode = "SystemLog:Catalog",
                MenuName = "系统日志",
                MenuTitle = "系统日志",
                MenuType = MenuTypeEnum.Catalog,
                HasDesktop = false,
                HasWeb = true,
                WebIcon = "api",
                HasMobile = false,
                Visible = YesOrNotEnum.Y,
                Sort = menuSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            sysLogCLMenuModel = await db.Insertable(sysLogCLMenuModel)
                .ExecuteReturnEntityAsync();

            var exLogMenuModel = new MenuModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleId = platformModuleModel.Id,
                MenuCode = "ExceptionLog:Page",
                MenuName = "异常日志",
                MenuTitle = "异常日志",
                ParentId = sysLogCLMenuModel.Id,
                MenuType = MenuTypeEnum.Menu,
                HasDesktop = false,
                HasWeb = true,
                WebIcon = "api",
                WebRouter = "/platform/log/exception/page",
                WebComponent = "platform/log/exception/index",
                HasMobile = false,
                Visible = YesOrNotEnum.Y,
                Sort = menuSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            exLogMenuModel = await db.Insertable(exLogMenuModel)
                .ExecuteReturnEntityAsync();
            await db.Insertable(new List<ButtonModel>
                {
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = exLogMenuModel.Id,
                        ButtonCode = "ExceptionLog:Page",
                        ButtonName = "列表",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = exLogMenuModel.Id,
                        ButtonCode = "ExceptionLog:Delete",
                        ButtonName = "删除",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            var sqlExLogMenuModel = new MenuModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleId = platformModuleModel.Id,
                MenuCode = "SqlExceptionLog:Page",
                MenuName = "Sql异常",
                MenuTitle = "Sql异常",
                ParentId = sysLogCLMenuModel.Id,
                MenuType = MenuTypeEnum.Menu,
                HasDesktop = false,
                HasWeb = true,
                WebIcon = "api",
                WebRouter = "/platform/log/sql/exception/page",
                WebComponent = "platform/log/sql/exception/index",
                HasMobile = false,
                Visible = YesOrNotEnum.Y,
                Sort = menuSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            sqlExLogMenuModel = await db.Insertable(sqlExLogMenuModel)
                .ExecuteReturnEntityAsync();
            await db.Insertable(new List<ButtonModel>
                {
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = sqlExLogMenuModel.Id,
                        ButtonCode = "SqlExceptionLog:Page",
                        ButtonName = "列表",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = sqlExLogMenuModel.Id,
                        ButtonCode = "SqlExceptionLog:Delete",
                        ButtonName = "删除",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            var sqlTimeoutLogMenuModel = new MenuModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleId = platformModuleModel.Id,
                MenuCode = "SqlTimeoutLog:Page",
                MenuName = "Sql超时",
                MenuTitle = "Sql超时",
                ParentId = sysLogCLMenuModel.Id,
                MenuType = MenuTypeEnum.Menu,
                HasDesktop = false,
                HasWeb = true,
                WebIcon = "api",
                WebRouter = "/platform/log/sql/timeout/page",
                WebComponent = "platform/log/sql/timeout/index",
                HasMobile = false,
                Visible = YesOrNotEnum.Y,
                Sort = menuSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            sqlTimeoutLogMenuModel = await db.Insertable(sqlTimeoutLogMenuModel)
                .ExecuteReturnEntityAsync();
            await db.Insertable(new List<ButtonModel>
                {
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = sqlTimeoutLogMenuModel.Id,
                        ButtonCode = "SqlTimeoutLog:Page",
                        ButtonName = "列表",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = sqlTimeoutLogMenuModel.Id,
                        ButtonCode = "SqlTimeoutLog:Delete",
                        ButtonName = "删除",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            #region 平台日志

            var platformLogCLMenuModel = new MenuModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleId = platformModuleModel.Id,
                MenuCode = "PlatformLog:Catalog",
                MenuName = "平台日志",
                MenuTitle = "平台日志",
                MenuType = MenuTypeEnum.Catalog,
                HasDesktop = false,
                HasWeb = true,
                WebIcon = "log",
                HasMobile = false,
                Visible = YesOrNotEnum.Y,
                Sort = menuSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            platformLogCLMenuModel = await db.Insertable(platformLogCLMenuModel)
                .ExecuteReturnEntityAsync();

            var visitLogMenuModel = new MenuModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleId = platformModuleModel.Id,
                MenuCode = "VisitLog:Page",
                MenuName = "访问日志",
                MenuTitle = "访问日志",
                ParentId = platformLogCLMenuModel.Id,
                MenuType = MenuTypeEnum.Menu,
                HasDesktop = false,
                HasWeb = true,
                WebIcon = "api",
                WebRouter = "/platform/log/visit/page",
                WebComponent = "platform/log/visit/index",
                HasMobile = false,
                Visible = YesOrNotEnum.Y,
                Sort = menuSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            visitLogMenuModel = await db.Insertable(visitLogMenuModel)
                .ExecuteReturnEntityAsync();
            await db.Insertable(new List<ButtonModel>
                {
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = visitLogMenuModel.Id,
                        ButtonCode = "VisitLog:Page",
                        ButtonName = "列表",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = visitLogMenuModel.Id,
                        ButtonCode = "VisitLog:Delete",
                        ButtonName = "删除",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            var operateLogMenuModel = new MenuModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleId = platformModuleModel.Id,
                MenuCode = "OperateLog:Page",
                MenuName = "操作日志",
                MenuTitle = "操作日志",
                ParentId = platformLogCLMenuModel.Id,
                MenuType = MenuTypeEnum.Menu,
                HasDesktop = false,
                HasWeb = true,
                WebIcon = "api",
                WebRouter = "/platform/log/operate/page",
                WebComponent = "platform/log/operate/index",
                HasMobile = false,
                Visible = YesOrNotEnum.Y,
                Sort = menuSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            operateLogMenuModel = await db.Insertable(operateLogMenuModel)
                .ExecuteReturnEntityAsync();
            await db.Insertable(new List<ButtonModel>
                {
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = operateLogMenuModel.Id,
                        ButtonCode = "OperateLog:Page",
                        ButtonName = "列表",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = operateLogMenuModel.Id,
                        ButtonCode = "OperateLog:Delete",
                        ButtonName = "删除",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            var sqlExecLogMenuModel = new MenuModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleId = platformModuleModel.Id,
                MenuCode = "SqlExecutionLog:Page",
                MenuName = "Sql日志",
                MenuTitle = "Sql日志",
                ParentId = sysLogCLMenuModel.Id,
                MenuType = MenuTypeEnum.Menu,
                HasDesktop = false,
                HasWeb = true,
                WebIcon = "api",
                WebRouter = "/platform/log/sql/execution/page",
                WebComponent = "platform/log/sql/execution/index",
                HasMobile = false,
                Visible = YesOrNotEnum.Y,
                Sort = menuSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            sqlExecLogMenuModel = await db.Insertable(sqlExecLogMenuModel)
                .ExecuteReturnEntityAsync();
            await db.Insertable(new List<ButtonModel>
                {
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = sqlExecLogMenuModel.Id,
                        ButtonCode = "SqlExecutionLog:Page",
                        ButtonName = "列表",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = sqlExecLogMenuModel.Id,
                        ButtonCode = "SqlExecutionLog:Delete",
                        ButtonName = "删除",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            var sqlDiffLogMenuModel = new MenuModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleId = platformModuleModel.Id,
                MenuCode = "SqlDiffLog:Page",
                MenuName = "Sql差异",
                MenuTitle = "Sql差异",
                ParentId = sysLogCLMenuModel.Id,
                MenuType = MenuTypeEnum.Menu,
                HasDesktop = false,
                HasWeb = true,
                WebIcon = "api",
                WebRouter = "/platform/log/sql/diff/page",
                WebComponent = "platform/log/sql/diff/index",
                HasMobile = false,
                Visible = YesOrNotEnum.Y,
                Sort = menuSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            sqlDiffLogMenuModel = await db.Insertable(sqlDiffLogMenuModel)
                .ExecuteReturnEntityAsync();
            await db.Insertable(new List<ButtonModel>
                {
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = sqlDiffLogMenuModel.Id,
                        ButtonCode = "SqlDiffLog:Page",
                        ButtonName = "列表",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = sqlDiffLogMenuModel.Id,
                        ButtonCode = "SqlDiffLog:Delete",
                        ButtonName = "删除",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            #endregion

            #region 开发应用

            var devModuleModel = new ModuleModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleName = "开发应用",
                Icon = "dev",
                ViewType = ModuleViewTypeEnum.All,
                Sort = moduleSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            devModuleModel = await db.Insertable(devModuleModel)
                .ExecuteReturnEntityAsync();

            #region Api

            var apiCLMenuModel = new MenuModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleId = devModuleModel.Id,
                MenuCode = "Api:Catalog",
                MenuName = "Api",
                MenuTitle = "Api",
                MenuType = MenuTypeEnum.Catalog,
                HasDesktop = false,
                HasWeb = true,
                WebIcon = "api",
                HasMobile = false,
                Visible = YesOrNotEnum.Y,
                Sort = menuSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            apiCLMenuModel = await db.Insertable(apiCLMenuModel)
                .ExecuteReturnEntityAsync();

            var apiMenuModel = new MenuModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleId = devModuleModel.Id,
                MenuCode = "Api:Page",
                MenuName = "接口管理",
                MenuTitle = "接口管理",
                ParentId = apiCLMenuModel.Id,
                MenuType = MenuTypeEnum.Menu,
                HasDesktop = false,
                HasWeb = true,
                WebIcon = "api",
                WebRouter = "/dev/api/page",
                WebComponent = "dev/api/index",
                HasMobile = false,
                Visible = YesOrNotEnum.Y,
                Sort = menuSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            apiMenuModel = await db.Insertable(apiMenuModel)
                .ExecuteReturnEntityAsync();
            await db.Insertable(new ButtonModel
                {
                    Id = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.Id,
                    MenuId = apiMenuModel.Id,
                    ButtonCode = "Api:Page",
                    ButtonName = "列表",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = adminUserModel.Id,
                    CreatedUserName = adminUserModel.NickName,
                    CreatedTime = new DateTime(2025, 01, 01)
                })
                .ExecuteCommandAsync(cancellationToken);

            await db.Insertable(new List<MenuModel>
                {
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        ModuleId = devModuleModel.Id,
                        MenuCode = "Api:Swagger",
                        MenuName = "Swagger",
                        MenuTitle = "Swagger",
                        ParentId = apiCLMenuModel.Id,
                        MenuType = MenuTypeEnum.Internal,
                        HasDesktop = false,
                        HasWeb = true,
                        WebIcon = "api",
                        HasMobile = false,
                        Link = "http://127.0.0.1:38081",
                        Visible = YesOrNotEnum.Y,
                        Sort = menuSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        ModuleId = devModuleModel.Id,
                        MenuCode = "Api:Knife4j",
                        MenuName = "Knife4j",
                        MenuTitle = "Knife4j",
                        ParentId = apiCLMenuModel.Id,
                        MenuType = MenuTypeEnum.Internal,
                        HasDesktop = false,
                        HasWeb = true,
                        WebIcon = "api",
                        HasMobile = false,
                        Link = "http://127.0.0.1:38081/knife4j",
                        Visible = YesOrNotEnum.Y,
                        Sort = menuSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            #region 菜单管理

            var menuModel = new MenuModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleId = devModuleModel.Id,
                MenuCode = "Menu:Page",
                MenuName = "菜单管理",
                MenuTitle = "菜单管理",
                MenuType = MenuTypeEnum.Menu,
                HasDesktop = false,
                HasWeb = true,
                WebIcon = "api",
                WebRouter = "/dev/menu/page",
                WebComponent = "dev/menu/index",
                HasMobile = false,
                Visible = YesOrNotEnum.Y,
                Sort = menuSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            menuModel = await db.Insertable(menuModel)
                .ExecuteReturnEntityAsync();
            await db.Insertable(new List<ButtonModel>
                {
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = menuModel.Id,
                        ButtonCode = "Menu:Page",
                        ButtonName = "列表",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = menuModel.Id,
                        ButtonCode = "Menu:Detail",
                        ButtonName = "详情",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = menuModel.Id,
                        ButtonCode = "Menu:Add",
                        ButtonName = "新增",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = menuModel.Id,
                        ButtonCode = "Menu:Edit",
                        ButtonName = "编辑",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    },
                    new()
                    {
                        Id = YitIdHelper.NextId(),
                        Edition = EditionEnum.Internal,
                        AppId = applicationModel.Id,
                        MenuId = menuModel.Id,
                        ButtonCode = "Menu:Delete",
                        ButtonName = "删除",
                        HasDesktop = true,
                        HasWeb = true,
                        HasMobile = true,
                        Sort = buttonSort,
                        Status = CommonStatusEnum.Enable,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = new DateTime(2025, 01, 01)
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            #region 密码映射

            var passwordMapModel = new MenuModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                ModuleId = devModuleModel.Id,
                MenuCode = "PasswordMap:Page",
                MenuName = "密码映射",
                MenuTitle = "密码映射",
                MenuType = MenuTypeEnum.Menu,
                HasDesktop = false,
                HasWeb = true,
                WebIcon = "api",
                WebRouter = "/dev/passwordMap/page",
                WebComponent = "dev/passwordMap/index",
                HasMobile = false,
                Visible = YesOrNotEnum.Y,
                Sort = menuSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = new DateTime(2025, 01, 01)
            };
            passwordMapModel = await db.Insertable(passwordMapModel)
                .ExecuteReturnEntityAsync();
            await db.Insertable(new ButtonModel
                {
                    Id = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.Id,
                    MenuId = passwordMapModel.Id,
                    ButtonCode = "PasswordMap:Page",
                    ButtonName = "列表",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = adminUserModel.Id,
                    CreatedUserName = adminUserModel.NickName,
                    CreatedTime = new DateTime(2025, 01, 01)
                })
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            #endregion

            #endregion
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Init database error...");
        }

        var logSb = new StringBuilder();
        logSb.Append("\u001b[40m\u001b[1m\u001b[32m");
        logSb.Append("system_notify");
        logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
        logSb.Append(": ");
        logSb.Append($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffffff zzz dddd}");
        logSb.Append(Environment.NewLine);
        logSb.Append("\u001b[40m\u001b[1m\u001b[32m");
        logSb.Append("               ");
        logSb.Append("初始化数据库成功。");
        logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
        Console.WriteLine(logSb.ToString());
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