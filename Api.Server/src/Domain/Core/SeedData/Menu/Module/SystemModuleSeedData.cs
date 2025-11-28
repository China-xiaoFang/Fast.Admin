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
    /// <param name="dateTime"><see cref="DateTime"/> 时间</param>
    /// <returns></returns>
    private static async Task SystemModuleSeedData(ISqlSugarClient db, ApplicationModel applicationModel, DateTime dateTime)
    {
        var systemModuleModel = new ModuleModel
        {
            ModuleId = YitIdHelper.NextId(),
            AppId = applicationModel.AppId,
            ModuleName = "系统管理",
            Icon = "fa-icon-SystemSetting",
            ViewType = ModuleViewTypeEnum.Admin,
            Sort = moduleSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        systemModuleModel = await db.Insertable(systemModuleModel)
            .ExecuteReturnEntityAsync();

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
            DesktopIcon = "desktop",
            HasWeb = true,
            WebIcon = "fa-icon-Desktop",
            WebRouter = "/system/app",
            WebComponent = "system/app/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/app.png",
            MobileRouter = "pages_system/app/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
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
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

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
            DesktopIcon = "tenant",
            HasWeb = true,
            WebIcon = "fa-icon-Tenant",
            WebRouter = "/system/tenant",
            WebComponent = "system/tenant/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/user.png",
            MobileRouter = "pages_system/tenant/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
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
            WebIcon = "fa-icon-Database",
            WebRouter = "/system/database",
            WebComponent = "system/database/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/database.png",
            MobileRouter = "pages_system/database/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
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
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

        #region 账号管理

        var accountMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Basic,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = PermissionConst.Account.Paged,
            MenuName = "账号管理",
            MenuTitle = "账号管理",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "idCard",
            HasWeb = true,
            WebIcon = "fa-icon-IdCard",
            WebRouter = "/system/account",
            WebComponent = "system/account/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/user.png",
            MobileRouter = "pages_system/account/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        accountMenuModel = await db.Insertable(accountMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Basic,
                    AppId = applicationModel.AppId,
                    MenuId = accountMenuModel.MenuId,
                    ButtonCode = PermissionConst.Account.Paged,
                    ButtonName = "列表",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Basic,
                    AppId = applicationModel.AppId,
                    MenuId = accountMenuModel.MenuId,
                    ButtonCode = PermissionConst.Account.Detail,
                    ButtonName = "详情",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Basic,
                    AppId = applicationModel.AppId,
                    MenuId = accountMenuModel.MenuId,
                    ButtonCode = PermissionConst.Account.Unlock,
                    ButtonName = "解除锁定",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Basic,
                    AppId = applicationModel.AppId,
                    MenuId = accountMenuModel.MenuId,
                    ButtonCode = PermissionConst.Account.ResetPassword,
                    ButtonName = "重置密码",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Basic,
                    AppId = applicationModel.AppId,
                    MenuId = accountMenuModel.MenuId,
                    ButtonCode = PermissionConst.Account.Status,
                    ButtonName = "状态更改",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

        #region 配置管理

        var configCLMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Basic,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = "Config:Catalog",
            MenuName = "配置管理",
            MenuTitle = "配置管理",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Catalog,
            HasDesktop = true,
            DesktopIcon = "systemSetting",
            HasWeb = true,
            WebIcon = "fa-icon-SystemSetting",
            WebRouter = null,
            WebComponent = null,
            HasMobile = false,
            MobileIcon = "fa-icon-organization",
            MobileRouter = null,
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        configCLMenuModel = await db.Insertable(configCLMenuModel)
            .ExecuteReturnEntityAsync();

        #region 单号配置

        var serialMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Basic,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = PermissionConst.Serial.Paged,
            MenuName = "单号配置",
            MenuTitle = "单号配置",
            ParentId = configCLMenuModel.MenuId,
            ParentIds = [0, configCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "adjust",
            HasWeb = true,
            WebIcon = "fa-icon-Adjust",
            WebRouter = "/system/serial",
            WebComponent = "system/serial/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/menu.png",
            MobileRouter = "pages_system/serial/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        serialMenuModel = await db.Insertable(serialMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Basic,
                    AppId = applicationModel.AppId,
                    MenuId = serialMenuModel.MenuId,
                    ButtonCode = PermissionConst.Serial.Paged,
                    ButtonName = "列表",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Basic,
                    AppId = applicationModel.AppId,
                    MenuId = serialMenuModel.MenuId,
                    ButtonCode = PermissionConst.Serial.Detail,
                    ButtonName = "详情",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Basic,
                    AppId = applicationModel.AppId,
                    MenuId = serialMenuModel.MenuId,
                    ButtonCode = PermissionConst.Serial.Add,
                    ButtonName = "新增",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Basic,
                    AppId = applicationModel.AppId,
                    MenuId = serialMenuModel.MenuId,
                    ButtonCode = PermissionConst.Serial.Edit,
                    ButtonName = "编辑",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

        #endregion

        #region 组织架构

        var orgCLMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.None,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = "Organization:Catalog",
            MenuName = "组织架构",
            MenuTitle = "组织架构",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Catalog,
            HasDesktop = true,
            DesktopIcon = "organization",
            HasWeb = true,
            WebIcon = "fa-icon-Organization",
            WebRouter = null,
            WebComponent = null,
            HasMobile = false,
            MobileIcon = "fa-icon-organization",
            MobileRouter = null,
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        orgCLMenuModel = await db.Insertable(orgCLMenuModel)
            .ExecuteReturnEntityAsync();

        #region 职位管理

        var positionMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.None,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = PermissionConst.Position.Paged,
            MenuName = "职位管理",
            MenuTitle = "职位管理",
            ParentId = orgCLMenuModel.MenuId,
            ParentIds = [0, orgCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "menu",
            HasWeb = true,
            WebIcon = null,
            WebRouter = "/system/position",
            WebComponent = "system/position/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/user.png",
            MobileRouter = "pages_system/position/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        positionMenuModel = await db.Insertable(positionMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = positionMenuModel.MenuId,
                    ButtonCode = PermissionConst.Position.Paged,
                    ButtonName = "列表",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = positionMenuModel.MenuId,
                    ButtonCode = PermissionConst.Position.Detail,
                    ButtonName = "详情",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = positionMenuModel.MenuId,
                    ButtonCode = PermissionConst.Position.Add,
                    ButtonName = "新增",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = positionMenuModel.MenuId,
                    ButtonCode = PermissionConst.Position.Edit,
                    ButtonName = "编辑",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = positionMenuModel.MenuId,
                    ButtonCode = PermissionConst.Position.Delete,
                    ButtonName = "删除",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

        #region 职级管理

        var jobLevelMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.None,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = PermissionConst.JobLevel.Paged,
            MenuName = "职级管理",
            MenuTitle = "职级管理",
            ParentId = orgCLMenuModel.MenuId,
            ParentIds = [0, orgCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "menu",
            HasWeb = true,
            WebIcon = null,
            WebRouter = "/system/jobLevel",
            WebComponent = "system/jobLevel/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/user.png",
            MobileRouter = "pages_system/jobLevel/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        jobLevelMenuModel = await db.Insertable(jobLevelMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = jobLevelMenuModel.MenuId,
                    ButtonCode = PermissionConst.JobLevel.Paged,
                    ButtonName = "列表",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = jobLevelMenuModel.MenuId,
                    ButtonCode = PermissionConst.JobLevel.Detail,
                    ButtonName = "详情",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = jobLevelMenuModel.MenuId,
                    ButtonCode = PermissionConst.JobLevel.Add,
                    ButtonName = "新增",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = jobLevelMenuModel.MenuId,
                    ButtonCode = PermissionConst.JobLevel.Edit,
                    ButtonName = "编辑",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = jobLevelMenuModel.MenuId,
                    ButtonCode = PermissionConst.JobLevel.Delete,
                    ButtonName = "删除",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

        #region 角色管理

        var roleMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.None,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = PermissionConst.Role.Paged,
            MenuName = "角色管理",
            MenuTitle = "角色管理",
            ParentId = orgCLMenuModel.MenuId,
            ParentIds = [0, orgCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "menu",
            HasWeb = true,
            WebIcon = null,
            WebRouter = "/system/role",
            WebComponent = "system/role/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/user.png",
            MobileRouter = "pages_system/role/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        roleMenuModel = await db.Insertable(roleMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = roleMenuModel.MenuId,
                    ButtonCode = PermissionConst.Role.Paged,
                    ButtonName = "列表",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = roleMenuModel.MenuId,
                    ButtonCode = PermissionConst.Role.Detail,
                    ButtonName = "详情",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = roleMenuModel.MenuId,
                    ButtonCode = PermissionConst.Role.Add,
                    ButtonName = "新增",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = roleMenuModel.MenuId,
                    ButtonCode = PermissionConst.Role.Edit,
                    ButtonName = "编辑",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = roleMenuModel.MenuId,
                    ButtonCode = PermissionConst.Role.Delete,
                    ButtonName = "删除",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

        #region 部门管理

        var departmentMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.None,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = PermissionConst.Department.Paged,
            MenuName = "部门管理",
            MenuTitle = "部门管理",
            ParentId = orgCLMenuModel.MenuId,
            ParentIds = [0, orgCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "menu",
            HasWeb = true,
            WebIcon = null,
            WebRouter = "/system/department",
            WebComponent = "system/department/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/user.png",
            MobileRouter = "pages_system/department/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        departmentMenuModel = await db.Insertable(departmentMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = departmentMenuModel.MenuId,
                    ButtonCode = PermissionConst.Department.Paged,
                    ButtonName = "列表",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = departmentMenuModel.MenuId,
                    ButtonCode = PermissionConst.Department.Detail,
                    ButtonName = "详情",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = departmentMenuModel.MenuId,
                    ButtonCode = PermissionConst.Department.Add,
                    ButtonName = "新增",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = departmentMenuModel.MenuId,
                    ButtonCode = PermissionConst.Department.Edit,
                    ButtonName = "编辑",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = departmentMenuModel.MenuId,
                    ButtonCode = PermissionConst.Department.Delete,
                    ButtonName = "删除",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

        #region 职员管理

        var employeeMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.None,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = PermissionConst.Employee.Paged,
            MenuName = "职员管理",
            MenuTitle = "职员管理",
            ParentId = orgCLMenuModel.MenuId,
            ParentIds = [0, orgCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "menu",
            HasWeb = true,
            WebIcon = null,
            WebRouter = "/system/employee",
            WebComponent = "system/employee/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/user.png",
            MobileRouter = "pages_system/employee/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        employeeMenuModel = await db.Insertable(employeeMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = employeeMenuModel.MenuId,
                    ButtonCode = PermissionConst.Employee.Paged,
                    ButtonName = "列表",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = employeeMenuModel.MenuId,
                    ButtonCode = PermissionConst.Employee.Detail,
                    ButtonName = "详情",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = employeeMenuModel.MenuId,
                    ButtonCode = PermissionConst.Employee.Add,
                    ButtonName = "新增",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.None,
                    AppId = applicationModel.AppId,
                    MenuId = employeeMenuModel.MenuId,
                    ButtonCode = PermissionConst.Employee.Edit,
                    ButtonName = "编辑",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

        #endregion

        #region 财务管理

        var financeCLMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Professional,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = "Finance:Catalog",
            MenuName = "财务管理",
            MenuTitle = "财务管理",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Catalog,
            HasDesktop = true,
            DesktopIcon = "money",
            HasWeb = true,
            WebIcon = "fa-icon-Money",
            WebRouter = null,
            WebComponent = null,
            HasMobile = false,
            MobileIcon = "fa-icon-money",
            MobileRouter = null,
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        financeCLMenuModel = await db.Insertable(financeCLMenuModel)
            .ExecuteReturnEntityAsync();

        #region 商户号

        var merchantMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Professional,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = PermissionConst.Merchant.Paged,
            MenuName = "商户号",
            MenuTitle = "商户号",
            ParentId = financeCLMenuModel.MenuId,
            ParentIds = [0, financeCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "menu",
            HasWeb = true,
            WebIcon = null,
            WebRouter = "/system/merchant",
            WebComponent = "system/merchant/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/user.png",
            MobileRouter = "pages_system/merchant/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        merchantMenuModel = await db.Insertable(merchantMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Professional,
                    AppId = applicationModel.AppId,
                    MenuId = merchantMenuModel.MenuId,
                    ButtonCode = PermissionConst.Merchant.Paged,
                    ButtonName = "列表",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Professional,
                    AppId = applicationModel.AppId,
                    MenuId = merchantMenuModel.MenuId,
                    ButtonCode = PermissionConst.Merchant.Detail,
                    ButtonName = "详情",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Professional,
                    AppId = applicationModel.AppId,
                    MenuId = merchantMenuModel.MenuId,
                    ButtonCode = PermissionConst.Merchant.Add,
                    ButtonName = "新增",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Professional,
                    AppId = applicationModel.AppId,
                    MenuId = merchantMenuModel.MenuId,
                    ButtonCode = PermissionConst.Merchant.Edit,
                    ButtonName = "编辑",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Professional,
                    AppId = applicationModel.AppId,
                    MenuId = merchantMenuModel.MenuId,
                    ButtonCode = PermissionConst.Merchant.Delete,
                    ButtonName = "删除",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

        #endregion

        #region 平台管理

        var platformCLMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Professional,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = "Platform:Catalog",
            MenuName = "平台管理",
            MenuTitle = "平台管理",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Catalog,
            HasDesktop = true,
            DesktopIcon = "desktop",
            HasWeb = true,
            WebIcon = "fa-icon-Desktop",
            WebRouter = null,
            WebComponent = null,
            HasMobile = false,
            MobileIcon = "fa-icon-desktop",
            MobileRouter = null,
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        platformCLMenuModel = await db.Insertable(platformCLMenuModel)
            .ExecuteReturnEntityAsync();

        #region 投诉工单

        var complaintMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = PermissionConst.Complaint.Paged,
            MenuName = "投诉工单",
            MenuTitle = "投诉工单",
            ParentId = platformCLMenuModel.MenuId,
            ParentIds = [0, platformCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "menu",
            HasWeb = true,
            WebIcon = null,
            WebRouter = "/system/complaint",
            WebComponent = "system/complaint/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/user.png",
            MobileRouter = "pages_system/complaint/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        complaintMenuModel = await db.Insertable(complaintMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = complaintMenuModel.MenuId,
                    ButtonCode = PermissionConst.Complaint.Paged,
                    ButtonName = "列表",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = complaintMenuModel.MenuId,
                    ButtonCode = PermissionConst.Complaint.Detail,
                    ButtonName = "详情",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

        #region 用户投诉

        var tenantComplaintMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Professional,
            AppId = applicationModel.AppId,
            ModuleId = systemModuleModel.ModuleId,
            MenuCode = PermissionConst.Complaint.Paged,
            MenuName = "用户投诉",
            MenuTitle = "用户投诉",
            ParentId = platformCLMenuModel.MenuId,
            ParentIds = [0, platformCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "menu",
            HasWeb = true,
            WebIcon = null,
            WebRouter = "/system/tenantComplaint",
            WebComponent = "system/tenantComplaint/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/user.png",
            MobileRouter = "pages_system/tenantComplaint/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        tenantComplaintMenuModel = await db.Insertable(tenantComplaintMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Professional,
                    AppId = applicationModel.AppId,
                    MenuId = tenantComplaintMenuModel.MenuId,
                    ButtonCode = PermissionConst.Complaint.TenantPaged,
                    ButtonName = "列表",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Professional,
                    AppId = applicationModel.AppId,
                    MenuId = tenantComplaintMenuModel.MenuId,
                    ButtonCode = PermissionConst.Complaint.TenantDetail,
                    ButtonName = "详情",
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = buttonSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

        #endregion
    }
}