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

using Fast.Common;
using Fast.FastCloud.Entity;
using Fast.FastCloud.Enum;
using Fast.IaaS;
using Fast.NET.Core;
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
[Order(1)]
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

            // 创建核心库
            db.DbMaintenance.CreateDatabase();

            // 查询核心表是否存在
            var sql =
                $"SELECT COUNT(*) FROM [information_schema].[TABLES] WHERE [TABLE_NAME] = '{typeof(UserModel).GetSugarTableName()}'";
            if (await db.Ado.GetIntAsync(sql) > 0)
                return;

            // 加载Aop
            SugarEntityFilter.LoadSugarAop(FastContext.HostEnvironment.IsDevelopment(), db);

            _logger.LogInformation("开始初始化数据库...");

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

            var dateTime = new DateTime(2025, 01, 01);

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
                CreatedTime = dateTime
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
                    CreatedTime = dateTime
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
                CreatedTime = dateTime
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
                        CreatedTime = dateTime
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
                        StartTime = dateTime,
                        EndTime = new DateTime(2099, 12, 31),
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = dateTime
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
                        StartTime = dateTime,
                        EndTime = new DateTime(2099, 12, 31),
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = dateTime
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
                ActivationTime = dateTime,
                Edition = EditionEnum.Flagship,
                AutoRenewal = true,
                RenewalExpiryTime = dateTime.AddDays(1080),
                IsTrial = true,
                IsInitialized = false,
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = dateTime
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
                RenewalTime = dateTime,
                RenewalExpiryTime = dateTime.AddDays(1080),
                Amount = 0,
                Remark = "内部版，不对外出售",
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = dateTime
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
                    // 初始化日志库
                    new()
                    {
                        PlatformId = platformModel.Id,
                        DatabaseType = DatabaseTypeEnum.FastCloudLog,
                        DbType = DbType.SqlServer,
                        Status = CommonStatusEnum.Enable,
                        PublicIp = SqlSugarContext.ConnectionSettings.ServiceIp,
                        IntranetIp = "127.0.0.1",
                        Port = SqlSugarContext.ConnectionSettings.Port,
                        DbName = "FastCloudLog",
                        DbUser = SqlSugarContext.ConnectionSettings.DbUser,
                        DbPwd = SqlSugarContext.ConnectionSettings.DbPwd,
                        CommandTimeOut = SqlSugarContext.ConnectionSettings.CommandTimeOut!.Value,
                        SugarSqlExecMaxSeconds = SqlSugarContext.ConnectionSettings.SugarSqlExecMaxSeconds!.Value,
                        DiffLog = false,
                        DisableAop = true,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = dateTime
                    },
                    // 初始化部署库
                    new()
                    {
                        PlatformId = platformModel.Id,
                        DatabaseType = DatabaseTypeEnum.Deploy,
                        DbType = DbType.SqlServer,
                        Status = CommonStatusEnum.Enable,
                        PublicIp = SqlSugarContext.ConnectionSettings.ServiceIp,
                        IntranetIp = "127.0.0.1",
                        Port = SqlSugarContext.ConnectionSettings.Port,
                        DbName = "Deploy",
                        DbUser = SqlSugarContext.ConnectionSettings.DbUser,
                        DbPwd = SqlSugarContext.ConnectionSettings.DbPwd,
                        CommandTimeOut = SqlSugarContext.ConnectionSettings.CommandTimeOut!.Value,
                        SugarSqlExecMaxSeconds = SqlSugarContext.ConnectionSettings.SugarSqlExecMaxSeconds!.Value,
                        DiffLog = true,
                        DisableAop = false,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = dateTime
                    },
                    // 初始化网关库
                    new()
                    {
                        PlatformId = platformModel.Id,
                        DatabaseType = DatabaseTypeEnum.Gateway,
                        DbType = DbType.SqlServer,
                        Status = CommonStatusEnum.Enable,
                        PublicIp = SqlSugarContext.ConnectionSettings.ServiceIp,
                        IntranetIp = "127.0.0.1",
                        Port = SqlSugarContext.ConnectionSettings.Port,
                        DbName = "Gateway",
                        DbUser = SqlSugarContext.ConnectionSettings.DbUser,
                        DbPwd = SqlSugarContext.ConnectionSettings.DbPwd,
                        CommandTimeOut = SqlSugarContext.ConnectionSettings.CommandTimeOut!.Value,
                        SugarSqlExecMaxSeconds = SqlSugarContext.ConnectionSettings.SugarSqlExecMaxSeconds!.Value,
                        DiffLog = true,
                        DisableAop = false,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = dateTime
                    },
                    // 初始化平台核心库
                    new()
                    {
                        PlatformId = platformModel.Id,
                        DatabaseType = DatabaseTypeEnum.Center,
                        DbType = DbType.SqlServer,
                        Status = CommonStatusEnum.Enable,
                        PublicIp = SqlSugarContext.ConnectionSettings.ServiceIp,
                        IntranetIp = "127.0.0.1",
                        Port = SqlSugarContext.ConnectionSettings.Port,
                        DbName = "Center",
                        DbUser = SqlSugarContext.ConnectionSettings.DbUser,
                        DbPwd = SqlSugarContext.ConnectionSettings.DbPwd,
                        CommandTimeOut = SqlSugarContext.ConnectionSettings.CommandTimeOut!.Value,
                        SugarSqlExecMaxSeconds = SqlSugarContext.ConnectionSettings.SugarSqlExecMaxSeconds!.Value,
                        DiffLog = true,
                        DisableAop = false,
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = dateTime
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            #region Config

            // 系统配置
            await ConfigSeedData.SystemConfigSeedData(db, adminUserModel.Id, adminUserModel.NickName, dateTime);

            #endregion

            #region platform.fastdotnet.com

            var platformApplicationModel = new ApplicationModel
            {
                Id = CommonConst.DefaultAppId,
                Edition = EditionEnum.Internal,
                AppNo = "Fast000001",
                AppName = CommonConst.DefaultAppName,
                ServiceStartTime = dateTime,
                ServiceEndTime = new DateTime(2099, 12, 31),
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = dateTime
            };
            platformApplicationModel = await db.Insertable(platformApplicationModel)
                .ExecuteReturnEntityAsync();
            await db.Insertable(new List<ApplicationOpenIdModel>
                {
                    new()
                    {
                        AppId = platformApplicationModel.Id,
                        OpenId = "platform.fastdotnet.com",
                        AppType = AppEnvironmentEnum.Web,
                        ApiUrl = "https://api.fastdotnet.com",
                        ApiBaseUrl = "",
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = dateTime
                    },
                    new()
                    {
                        AppId = platformApplicationModel.Id,
                        OpenId = "DesktopOpenId",
                        AppType = AppEnvironmentEnum.Desktop,
                        ApiUrl = "https://api.fastdotnet.com",
                        ApiBaseUrl = "",
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = dateTime
                    },
                    new()
                    {
                        AppId = platformApplicationModel.Id,
                        OpenId = "WeChatOpenId",
                        AppType = AppEnvironmentEnum.WeChatMiniProgram,
                        ApiUrl = "https://api.fastdotnet.com",
                        ApiBaseUrl = "",
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = dateTime
                    },
                    new()
                    {
                        AppId = platformApplicationModel.Id,
                        OpenId = "AndroidOpenId",
                        AppType = AppEnvironmentEnum.Android,
                        ApiUrl = "https://api.fastdotnet.com",
                        ApiBaseUrl = "",
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = dateTime
                    },
                    new()
                    {
                        AppId = platformApplicationModel.Id,
                        OpenId = "IOSOpenId",
                        AppType = AppEnvironmentEnum.IOS,
                        ApiUrl = "https://api.fastdotnet.com",
                        ApiBaseUrl = "",
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = dateTime
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            // 菜单
            await MenuSeedData.PlatformMenuSeedData(db, platformApplicationModel, adminUserModel.Id, adminUserModel.NickName, dateTime);

            #endregion

            #region admin.fastdotnet.com

            var adminApplicationModel = new ApplicationModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Flagship,
                AppNo = "Fast000002",
                AppName = "Fast.Admin",
                ServiceStartTime = dateTime,
                ServiceEndTime = new DateTime(2099, 12, 31),
                CreatedUserId = adminUserModel.Id,
                CreatedUserName = adminUserModel.NickName,
                CreatedTime = dateTime
            };
            adminApplicationModel = await db.Insertable(adminApplicationModel)
                .ExecuteReturnEntityAsync();
            await db.Insertable(new List<ApplicationOpenIdModel>
                {
                    new()
                    {
                        AppId = adminApplicationModel.Id,
                        OpenId = "admin.fastdotnet.com",
                        AppType = AppEnvironmentEnum.Web,
                        ApiUrl = "https://api.fastdotnet.com",
                        ApiBaseUrl = "",
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = dateTime
                    },
                    new()
                    {
                        AppId = adminApplicationModel.Id,
                        OpenId = "DesktopOpenId",
                        AppType = AppEnvironmentEnum.Desktop,
                        ApiUrl = "https://api.fastdotnet.com",
                        ApiBaseUrl = "",
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = dateTime
                    },
                    new()
                    {
                        AppId = adminApplicationModel.Id,
                        OpenId = "WeChatOpenId",
                        AppType = AppEnvironmentEnum.WeChatMiniProgram,
                        ApiUrl = "https://api.fastdotnet.com",
                        ApiBaseUrl = "",
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = dateTime
                    },
                    new()
                    {
                        AppId = adminApplicationModel.Id,
                        OpenId = "AndroidOpenId",
                        AppType = AppEnvironmentEnum.Android,
                        ApiUrl = "https://api.fastdotnet.com",
                        ApiBaseUrl = "",
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = dateTime
                    },
                    new()
                    {
                        AppId = adminApplicationModel.Id,
                        OpenId = "IOSOpenId",
                        AppType = AppEnvironmentEnum.IOS,
                        ApiUrl = "https://api.fastdotnet.com",
                        ApiBaseUrl = "",
                        CreatedUserId = adminUserModel.Id,
                        CreatedUserName = adminUserModel.NickName,
                        CreatedTime = dateTime
                    }
                })
                .ExecuteCommandAsync(cancellationToken);

            #endregion

            _logger.LogInformation("初始化数据库成功。");
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