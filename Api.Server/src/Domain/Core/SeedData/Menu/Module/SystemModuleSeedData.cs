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

using Fast.Center.Entity;
using Fast.Center.Enum;
using SqlSugar;
using Yitter.IdGenerator;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="MenuSeedData"/> 系统模块种子数据
/// </summary>
internal static partial class MenuSeedData
{
    /// <summary>
    /// 系统模块种子数据
    /// </summary>
    /// <param name="db"></param>
    /// <param name="applicationModel"><see cref="ApplicationModel"/> 应用</param>
    /// <param name="userId"><see cref="long"/> 用户Id</param>
    /// <param name="userName"><see cref="string"/> 用户名称</param>
    /// <param name="dateTime"><see cref="DateTime"/> 时间</param>
    /// <returns></returns>
    private static async Task SystemModuleSeedData(ISqlSugarClient db, ApplicationModel applicationModel, long userId,
        string userName, DateTime dateTime)
    {
        var systemModuleModel = new ModuleModel
        {
            ModuleId = YitIdHelper.NextId(),
            AppId = applicationModel.AppId,
            ModuleName = "系统管理",
            Icon = "system",
            ViewType = ModuleViewTypeEnum.Admin,
            Sort = moduleSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
        };
        systemModuleModel = await db.Insertable(systemModuleModel)
            .ExecuteReturnEntityAsync();

        #region 租户管理

        var tenantMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = PermissionConst.Tenant.Paged,
            MenuName = "租户管理",
            MenuTitle = "租户管理",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "user",
            HasWeb = true,
            WebIcon = "user",
            WebRouter = "/system/tenant",
            WebComponent = "system/tenant/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/user.png",
            MobileRouter = "pages_system/tenant/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
        };
        tenantMenuModel = await db.Insertable(tenantMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = tenantMenuModel.MenuId,
                    ButtonCode = PermissionConst.Tenant.Paged,
                    ButtonName = "列表",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = tenantMenuModel.MenuId,
                    ButtonCode = PermissionConst.Tenant.Detail,
                    ButtonName = "详情",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = tenantMenuModel.MenuId,
                    ButtonCode = PermissionConst.Tenant.Add,
                    ButtonName = "新增",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = tenantMenuModel.MenuId,
                    ButtonCode = PermissionConst.Tenant.Edit,
                    ButtonName = "编辑",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = tenantMenuModel.MenuId,
                    ButtonCode = PermissionConst.Tenant.Status,
                    ButtonName = "状态更改",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

        #region 账号管理

        var accountMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = PermissionConst.Account.Paged,
            MenuName = "账号管理",
            MenuTitle = "账号管理",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "user",
            HasWeb = true,
            WebIcon = "user",
            WebRouter = "/system/account",
            WebComponent = "system/account/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/user.png",
            MobileRouter = "pages_system/account/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
        };
        accountMenuModel = await db.Insertable(accountMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = accountMenuModel.MenuId,
                    ButtonCode = PermissionConst.Account.Paged,
                    ButtonName = "列表",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = accountMenuModel.MenuId,
                    ButtonCode = PermissionConst.Account.Detail,
                    ButtonName = "详情",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = accountMenuModel.MenuId,
                    ButtonCode = PermissionConst.Account.Unlock,
                    ButtonName = "解除锁定",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = accountMenuModel.MenuId,
                    ButtonCode = PermissionConst.Account.ResetPassword,
                    ButtonName = "重置密码",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = accountMenuModel.MenuId,
                    ButtonCode = PermissionConst.Account.Status,
                    ButtonName = "状态更改",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

        #region 应用管理

        var appMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = PermissionConst.App.Paged,
            MenuName = "应用管理",
            MenuTitle = "应用管理",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "app",
            HasWeb = true,
            WebIcon = "app",
            WebRouter = "/system/app",
            WebComponent = "system/app/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/app.png",
            MobileRouter = "pages_system/app/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
        };
        appMenuModel = await db.Insertable(appMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = appMenuModel.MenuId,
                    ButtonCode = PermissionConst.App.Paged,
                    ButtonName = "列表",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = appMenuModel.MenuId,
                    ButtonCode = PermissionConst.App.Detail,
                    ButtonName = "详情",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = appMenuModel.MenuId,
                    ButtonCode = PermissionConst.App.Add,
                    ButtonName = "新增",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = appMenuModel.MenuId,
                    ButtonCode = PermissionConst.App.Edit,
                    ButtonName = "编辑",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = appMenuModel.MenuId,
                    ButtonCode = PermissionConst.App.Delete,
                    ButtonName = "删除",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

        #region 数据库配置

        var dbMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = PermissionConst.Database.Paged,
            MenuName = "数据库配置",
            MenuTitle = "数据库配置",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "database",
            HasWeb = true,
            WebIcon = "database",
            WebRouter = "/system/database",
            WebComponent = "system/database/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/database.png",
            MobileRouter = "pages_system/database/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
        };
        dbMenuModel = await db.Insertable(dbMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = dbMenuModel.MenuId,
                    ButtonCode = PermissionConst.Database.Paged,
                    ButtonName = "列表",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = dbMenuModel.MenuId,
                    ButtonCode = PermissionConst.Database.Detail,
                    ButtonName = "详情",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = dbMenuModel.MenuId,
                    ButtonCode = PermissionConst.Database.Add,
                    ButtonName = "新增",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = dbMenuModel.MenuId,
                    ButtonCode = PermissionConst.Database.Edit,
                    ButtonName = "编辑",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = dbMenuModel.MenuId,
                    ButtonCode = PermissionConst.Database.Delete,
                    ButtonName = "删除",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

        #region 系统配置

        var configMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = PermissionConst.Config.Paged,
            MenuName = "系统配置",
            MenuTitle = "系统配置",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "database",
            HasWeb = true,
            WebIcon = "database",
            WebRouter = "/system/config",
            WebComponent = "system/config/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/database.png",
            MobileRouter = "pages_system/config/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
        };
        configMenuModel = await db.Insertable(configMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = configMenuModel.MenuId,
                    ButtonCode = PermissionConst.Config.Paged,
                    ButtonName = "列表",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = configMenuModel.MenuId,
                    ButtonCode = PermissionConst.Config.Detail,
                    ButtonName = "详情",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = configMenuModel.MenuId,
                    ButtonCode = PermissionConst.Config.Add,
                    ButtonName = "新增",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = configMenuModel.MenuId,
                    ButtonCode = PermissionConst.Config.Edit,
                    ButtonName = "编辑",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion
    }
}