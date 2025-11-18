using Fast.Center.Entity;
using Fast.Center.Enum;
using SqlSugar;
using Yitter.IdGenerator;


namespace Fast.Core;

/// <summary>
/// <see cref="MenuSeedData"/> 开发模块种子数据
/// </summary>
internal static partial class MenuSeedData
{
    /// <summary>
    /// 开发模块种子数据
    /// </summary>
    /// <param name="db"></param>
    /// <param name="applicationModel"><see cref="ApplicationModel"/> 应用</param>
    /// <param name="dateTime"><see cref="DateTime"/> 时间</param>
    /// <returns></returns>
    private static async Task DevModuleSeedData(ISqlSugarClient db, ApplicationModel applicationModel,
        DateTime dateTime)
    {
        var devModuleModel = new ModuleModel
        {
            ModuleId = YitIdHelper.NextId(),
            AppId = applicationModel.AppId,
            ModuleName = "开发应用",
            Icon = "fa-icon-Terminal",
            Color = "#AD6DEF",
            ViewType = ModuleViewTypeEnum.All,
            Sort = moduleSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        devModuleModel = await db.Insertable(devModuleModel)
            .ExecuteReturnEntityAsync();

        var apiCLMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            ModuleId = devModuleModel.ModuleId,
            MenuCode = "Api:Catalog",
            MenuName = "Api",
            MenuTitle = "Api",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Catalog,
            HasDesktop = true,
            DesktopIcon = "api",
            HasWeb = true,
            WebIcon = "fa-icon-Api",
            WebRouter = null,
            WebComponent = null,
            HasMobile = false,
            MobileIcon = "fa-icon-api",
            MobileRouter = null,
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        apiCLMenuModel = await db.Insertable(apiCLMenuModel)
            .ExecuteReturnEntityAsync();

        #region Api

        var apiMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            ModuleId = devModuleModel.ModuleId,
            MenuCode = PermissionConst.ApiPaged,
            MenuName = "接口管理",
            MenuTitle = "接口管理",
            ParentId = apiCLMenuModel.MenuId,
            ParentIds = [0, apiCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopRouter = null,
            DesktopIcon = "api",
            HasWeb = true,
            WebIcon = "fa-icon-Gateway",
            WebRouter = "/dev/api",
            WebComponent = "dev/api/index",
            HasMobile = false,
            MobileIcon = null,
            MobileRouter = null,
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        apiMenuModel = await db.Insertable(apiMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new ButtonModel
            {
                ButtonId = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.AppId,
                MenuId = apiMenuModel.MenuId,
                ButtonCode = PermissionConst.ApiPaged,
                ButtonName = "列表",
                HasDesktop = true,
                HasWeb = true,
                HasMobile = false,
                Sort = buttonSort,
                Status = CommonStatusEnum.Enable,
                CreatedTime = dateTime
            })
            .ExecuteCommandAsync();

        await db.Insertable(new List<MenuModel>
            {
                new()
                {
                    MenuId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    ModuleId = devModuleModel.ModuleId,
                    MenuCode = PermissionConst.ApiSwagger,
                    MenuName = "Swagger",
                    MenuTitle = "Swagger",
                    ParentId = apiCLMenuModel.MenuId,
                    ParentIds = [0, apiCLMenuModel.MenuId],
                    MenuType = MenuTypeEnum.Internal,
                    HasDesktop = true,
                    DesktopIcon = "api",
                    DesktopRouter = null,
                    HasWeb = true,
                    WebIcon = "fa-icon-Swagger",
                    WebRouter = null,
                    WebComponent = null,
                    HasMobile = false,
                    MobileIcon = null,
                    MobileRouter = null,
                    Link = "http://127.0.0.1:38081",
                    Visible = YesOrNotEnum.Y,
                    Sort = menuSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    MenuId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    ModuleId = devModuleModel.ModuleId,
                    MenuCode = PermissionConst.ApiKnife4j,
                    MenuName = "Knife4j",
                    MenuTitle = "Knife4j",
                    ParentId = apiCLMenuModel.MenuId,
                    ParentIds = [0, apiCLMenuModel.MenuId],
                    MenuType = MenuTypeEnum.Internal,
                    HasDesktop = true,
                    DesktopIcon = "api",
                    DesktopRouter = null,
                    HasWeb = true,
                    WebIcon = "fa-icon-Swagger",
                    WebRouter = null,
                    WebComponent = null,
                    HasMobile = false,
                    MobileIcon = null,
                    MobileRouter = null,
                    Link = "http://127.0.0.1:38081/knife4j",
                    Visible = YesOrNotEnum.Y,
                    Sort = menuSort,
                    Status = CommonStatusEnum.Enable,
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
            ModuleId = devModuleModel.ModuleId,
            MenuCode = PermissionConst.Config.Paged,
            MenuName = "系统配置",
            MenuTitle = "系统配置",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "chrome",
            HasWeb = true,
            WebIcon = "fa-icon-Chrome",
            WebRouter = "/dev/config",
            WebComponent = "dev/config/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/database.png",
            MobileRouter = "pages_dev/config/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
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
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

        var devCLMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            ModuleId = devModuleModel.ModuleId,
            MenuCode = "Dev:Catalog",
            MenuName = "开发配置",
            MenuTitle = "开发配置",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Catalog,
            HasDesktop = true,
            DesktopIcon = "terminal",
            HasWeb = true,
            WebIcon = "fa-icon-Terminal",
            WebRouter = null,
            WebComponent = null,
            HasMobile = true,
            MobileIcon = "fa-icon-terminal",
            MobileRouter = null,
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        devCLMenuModel = await db.Insertable(devCLMenuModel)
            .ExecuteReturnEntityAsync();

        #region 菜单管理

        var menuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            ModuleId = devModuleModel.ModuleId,
            MenuCode = PermissionConst.Menu.Paged,
            MenuName = "菜单管理",
            MenuTitle = "菜单管理",
            ParentId = devCLMenuModel.MenuId,
            ParentIds = [0, devCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "menu",
            HasWeb = true,
            WebIcon = "fa-icon-Menu",
            WebRouter = "/dev/menu",
            WebComponent = "dev/menu/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/menu.png",
            MobileRouter = "pages_dev/menu/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        menuModel = await db.Insertable(menuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = menuModel.MenuId,
                    ButtonCode = PermissionConst.Menu.Paged,
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
                    MenuId = menuModel.MenuId,
                    ButtonCode = PermissionConst.Menu.Detail,
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
                    MenuId = menuModel.MenuId,
                    ButtonCode = PermissionConst.Menu.Add,
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
                    MenuId = menuModel.MenuId,
                    ButtonCode = PermissionConst.Menu.Edit,
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
                    MenuId = menuModel.MenuId,
                    ButtonCode = PermissionConst.Menu.Delete,
                    ButtonName = "删除",
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
                    MenuId = menuModel.MenuId,
                    ButtonCode = PermissionConst.Menu.Status,
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

        #region 序号配置

        var serialMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            ModuleId = devModuleModel.ModuleId,
            MenuCode = PermissionConst.SysSerial.Paged,
            MenuName = "序号配置",
            MenuTitle = "序号配置",
            ParentId = devCLMenuModel.MenuId,
            ParentIds = [0, devCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "adjust",
            HasWeb = true,
            WebIcon = "fa-icon-Adjust",
            WebRouter = "/dev/sysSerial",
            WebComponent = "dev/sysSerial/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/menu.png",
            MobileRouter = "pages_dev/sysSerial/page/index",
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
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = serialMenuModel.MenuId,
                    ButtonCode = PermissionConst.SysSerial.Paged,
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
                    MenuId = serialMenuModel.MenuId,
                    ButtonCode = PermissionConst.SysSerial.Detail,
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
                    MenuId = serialMenuModel.MenuId,
                    ButtonCode = PermissionConst.SysSerial.Add,
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
                    MenuId = serialMenuModel.MenuId,
                    ButtonCode = PermissionConst.SysSerial.Edit,
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

        #region 数据字典

        var dictionaryMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            ModuleId = devModuleModel.ModuleId,
            MenuCode = PermissionConst.Dictionary.Paged,
            MenuName = "数据字典",
            MenuTitle = "数据字典",
            ParentId = devCLMenuModel.MenuId,
            ParentIds = [0, devCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "dictionary",
            HasWeb = true,
            WebIcon = "fa-icon-Dictionary",
            WebRouter = "/dev/dictionary",
            WebComponent = "dev/dictionary/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/menu.png",
            MobileRouter = "pages_dev/dictionary/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        dictionaryMenuModel = await db.Insertable(dictionaryMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = dictionaryMenuModel.MenuId,
                    ButtonCode = PermissionConst.Dictionary.Paged,
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
                    MenuId = dictionaryMenuModel.MenuId,
                    ButtonCode = PermissionConst.Dictionary.Detail,
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
                    MenuId = dictionaryMenuModel.MenuId,
                    ButtonCode = PermissionConst.Dictionary.Add,
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
                    MenuId = dictionaryMenuModel.MenuId,
                    ButtonCode = PermissionConst.Dictionary.Edit,
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
                    MenuId = dictionaryMenuModel.MenuId,
                    ButtonCode = PermissionConst.Dictionary.Delete,
                    ButtonName = "删除",
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
                    MenuId = dictionaryMenuModel.MenuId,
                    ButtonCode = PermissionConst.Dictionary.Status,
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

        #region 表格配置

        var tableConfigMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            ModuleId = devModuleModel.ModuleId,
            MenuCode = PermissionConst.Table.Paged,
            MenuName = "表格配置",
            MenuTitle = "表格配置",
            ParentId = devCLMenuModel.MenuId,
            ParentIds = [0, devCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "tableConfig",
            HasWeb = true,
            WebIcon = "fa-icon-TableConfig",
            WebRouter = "/dev/tableConfig",
            WebComponent = "dev/tableConfig/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/menu.png",
            MobileRouter = "pages_dev/tableConfig/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        tableConfigMenuModel = await db.Insertable(tableConfigMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = tableConfigMenuModel.MenuId,
                    ButtonCode = PermissionConst.Table.Paged,
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
                    MenuId = tableConfigMenuModel.MenuId,
                    ButtonCode = PermissionConst.Table.Detail,
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
                    MenuId = tableConfigMenuModel.MenuId,
                    ButtonCode = PermissionConst.Table.Add,
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
                    MenuId = tableConfigMenuModel.MenuId,
                    ButtonCode = PermissionConst.Table.Edit,
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
                    MenuId = tableConfigMenuModel.MenuId,
                    ButtonCode = PermissionConst.Table.Delete,
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

        #region 调度程序

        var schedulerMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            ModuleId = devModuleModel.ModuleId,
            MenuCode = PermissionConst.Scheduler.Paged,
            MenuName = "调度程序",
            MenuTitle = "调度程序",
            ParentId = devCLMenuModel.MenuId,
            ParentIds = [0, devCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "task",
            HasWeb = true,
            WebIcon = "fa-icon-Task",
            WebRouter = "/dev/scheduler",
            WebComponent = "dev/scheduler/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/menu.png",
            MobileRouter = "pages_dev/scheduler/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        schedulerMenuModel = await db.Insertable(schedulerMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    ButtonId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuId = schedulerMenuModel.MenuId,
                    ButtonCode = PermissionConst.Scheduler.Paged,
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
                    MenuId = schedulerMenuModel.MenuId,
                    ButtonCode = PermissionConst.Scheduler.Detail,
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
                    MenuId = schedulerMenuModel.MenuId,
                    ButtonCode = PermissionConst.Scheduler.Add,
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
                    MenuId = schedulerMenuModel.MenuId,
                    ButtonCode = PermissionConst.Scheduler.Edit,
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
                    MenuId = schedulerMenuModel.MenuId,
                    ButtonCode = PermissionConst.Scheduler.Delete,
                    ButtonName = "删除",
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
                    MenuId = schedulerMenuModel.MenuId,
                    ButtonCode = PermissionConst.Scheduler.Start,
                    ButtonName = "启动",
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
                    MenuId = schedulerMenuModel.MenuId,
                    ButtonCode = PermissionConst.Scheduler.Stop,
                    ButtonName = "暂停",
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
                    MenuId = schedulerMenuModel.MenuId,
                    ButtonCode = PermissionConst.Scheduler.Trigger,
                    ButtonName = "执行作业",
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
                    MenuId = schedulerMenuModel.MenuId,
                    ButtonCode = PermissionConst.Scheduler.ResumeJob,
                    ButtonName = "恢复作业",
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
                    MenuId = schedulerMenuModel.MenuId,
                    ButtonCode = PermissionConst.Scheduler.StopJob,
                    ButtonName = "暂停作业",
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

        var sensitiveCLMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            ModuleId = devModuleModel.ModuleId,
            MenuCode = "Sensitive:Catalog",
            MenuName = "敏感数据",
            MenuTitle = "敏感数据",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Catalog,
            HasDesktop = true,
            DesktopIcon = "accountSafe",
            HasWeb = true,
            WebIcon = "fa-icon-AccountSafe",
            WebRouter = null,
            WebComponent = null,
            HasMobile = true,
            MobileIcon = "fa-icon-accountSafe",
            MobileRouter = null,
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        sensitiveCLMenuModel = await db.Insertable(sensitiveCLMenuModel)
            .ExecuteReturnEntityAsync();

        #region 密码映射

        var passwordMapModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            ModuleId = devModuleModel.ModuleId,
            MenuCode = PermissionConst.PasswordMapPaged,
            MenuName = "密码映射",
            MenuTitle = "密码映射",
            ParentId = sensitiveCLMenuModel.MenuId,
            ParentIds = [0, sensitiveCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "lock",
            HasWeb = true,
            WebIcon = "fa-icon-Lock",
            WebRouter = "/dev/passwordMap",
            WebComponent = "dev/passwordMap/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/passwordMap.png",
            MobileRouter = "pages_dev/passwordMap/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        passwordMapModel = await db.Insertable(passwordMapModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new ButtonModel
            {
                ButtonId = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.AppId,
                MenuId = passwordMapModel.MenuId,
                ButtonCode = PermissionConst.PasswordMapPaged,
                ButtonName = "列表",
                HasDesktop = true,
                HasWeb = true,
                HasMobile = true,
                Sort = buttonSort,
                Status = CommonStatusEnum.Enable,
                CreatedTime = dateTime
            })
            .ExecuteCommandAsync();

        #endregion

        #region 密码记录

        var passwordRecordModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            ModuleId = devModuleModel.ModuleId,
            MenuCode = PermissionConst.PasswordRecordPaged,
            MenuName = "密码记录",
            MenuTitle = "密码记录",
            ParentId = sensitiveCLMenuModel.MenuId,
            ParentIds = [0, sensitiveCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "lock",
            HasWeb = true,
            WebIcon = "fa-icon-Lock",
            WebRouter = "/dev/passwordRecord",
            WebComponent = "dev/passwordRecord/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/passwordMap.png",
            MobileRouter = "pages_dev/passwordRecord/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedTime = dateTime
        };
        passwordRecordModel = await db.Insertable(passwordRecordModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new ButtonModel
            {
                ButtonId = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.AppId,
                MenuId = passwordRecordModel.MenuId,
                ButtonCode = PermissionConst.PasswordRecordPaged,
                ButtonName = "列表",
                HasDesktop = true,
                HasWeb = true,
                HasMobile = true,
                Sort = buttonSort,
                Status = CommonStatusEnum.Enable,
                CreatedTime = dateTime
            })
            .ExecuteCommandAsync();

        #endregion
    }
}