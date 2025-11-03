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
/// <see cref="MenuSeedData"/> 开发模块种子数据
/// </summary>
internal static partial class MenuSeedData
{
    /// <summary>
    /// 开发模块种子数据
    /// </summary>
    /// <param name="db"></param>
    /// <param name="applicationModel"><see cref="ApplicationModel"/> 应用</param>
    /// <param name="userId"><see cref="long"/> 用户Id</param>
    /// <param name="userName"><see cref="string"/> 用户名称</param>
    /// <param name="dateTime"><see cref="DateTime"/> 时间</param>
    /// <returns></returns>
    private static async Task DevModuleSeedData(ISqlSugarClient db, ApplicationModel applicationModel, long userId,
        string userName, DateTime dateTime)
    {
        var devModuleModel = new ModuleModel
        {
            Id = YitIdHelper.NextId(),
            AppId = applicationModel.Id,
            ModuleName = "开发应用",
            Icon = "dev",
            Color = "#409EFF",
            ViewType = ModuleViewTypeEnum.All,
            Sort = moduleSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
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
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Catalog,
            HasDesktop = true,
            DesktopIcon = "api",
            HasWeb = true,
            WebIcon = "api",
            WebRouter = null,
            WebComponent = null,
            HasMobile = true,
            MobileIcon = "api",
            MobileRouter = null,
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
        };
        apiCLMenuModel = await db.Insertable(apiCLMenuModel)
            .ExecuteReturnEntityAsync();

        var apiMenuModel = new MenuModel
        {
            Id = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.Id,
            ModuleId = devModuleModel.Id,
            MenuCode = "Api:Paged",
            MenuName = "接口管理",
            MenuTitle = "接口管理",
            ParentId = apiCLMenuModel.Id,
            ParentIds = [apiCLMenuModel.Id],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopRouter = null,
            DesktopIcon = "api",
            HasWeb = true,
            WebIcon = "api",
            WebRouter = "/dev/api",
            WebComponent = "dev/api/index",
            HasMobile = true,
            MobileIcon = null,
            MobileRouter = null,
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
        };
        apiMenuModel = await db.Insertable(apiMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new ButtonModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                MenuId = apiMenuModel.Id,
                ButtonCode = "Api:Paged",
                ButtonName = "列表",
                HasDesktop = true,
                HasWeb = true,
                HasMobile = true,
                Sort = buttonSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = userId,
                CreatedUserName = userName,
                CreatedTime = dateTime
            })
            .ExecuteCommandAsync();

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
                    ParentIds = [apiCLMenuModel.Id],
                    MenuType = MenuTypeEnum.Internal,
                    HasDesktop = true,
                    DesktopIcon = "api",
                    DesktopRouter = null,
                    HasWeb = true,
                    WebIcon = "api",
                    WebRouter = null,
                    WebComponent = null,
                    HasMobile = true,
                    MobileIcon = null,
                    MobileRouter = null,
                    Link = "http://127.0.0.1:38081",
                    Visible = YesOrNotEnum.Y,
                    Sort = menuSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
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
                    ParentIds = [apiCLMenuModel.Id],
                    MenuType = MenuTypeEnum.Internal,
                    HasDesktop = true,
                    DesktopIcon = "api",
                    DesktopRouter = null,
                    HasWeb = true,
                    WebIcon = "api",
                    WebRouter = null,
                    WebComponent = null,
                    HasMobile = true,
                    MobileIcon = null,
                    MobileRouter = null,
                    Link = "http://127.0.0.1:38081/knife4j",
                    Visible = YesOrNotEnum.Y,
                    Sort = menuSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion

        #region 菜单管理

        var menuModel = new MenuModel
        {
            Id = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.Id,
            ModuleId = devModuleModel.Id,
            MenuCode = "Menu:Paged",
            MenuName = "菜单管理",
            MenuTitle = "菜单管理",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "api",
            HasWeb = true,
            WebIcon = "api",
            WebRouter = "/dev/menu",
            WebComponent = "dev/menu/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/menu.png",
            MobileRouter = "pages_dev/menu/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
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
                    ButtonCode = "Menu:Paged",
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
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
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
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
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
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
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
                    CreatedUserId = userId,
                    CreatedUserName = userName,
                    CreatedTime = dateTime
                },
                new()
                {
                    Id = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.Id,
                    MenuId = menuModel.Id,
                    ButtonCode = "Menu:Status",
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

        #region 表格配置

        var tableConfigMenuModel = new MenuModel
        {
            Id = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.Id,
            ModuleId = devModuleModel.Id,
            MenuCode = "Table:Paged",
            MenuName = "表格配置",
            MenuTitle = "表格配置",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "api",
            HasWeb = true,
            WebIcon = "api",
            WebRouter = "/dev/tableConfig",
            WebComponent = "dev/tableConfig/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/menu.png",
            MobileRouter = "pages_dev/tableConfig/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
        };
        tableConfigMenuModel = await db.Insertable(tableConfigMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    Id = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.Id,
                    MenuId = tableConfigMenuModel.Id,
                    ButtonCode = "Table:Paged",
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
                    Id = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.Id,
                    MenuId = tableConfigMenuModel.Id,
                    ButtonCode = "Table:Detail",
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
                    Id = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.Id,
                    MenuId = tableConfigMenuModel.Id,
                    ButtonCode = "Table:Add",
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
                    Id = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.Id,
                    MenuId = tableConfigMenuModel.Id,
                    ButtonCode = "Table:Edit",
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
                    Id = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.Id,
                    MenuId = tableConfigMenuModel.Id,
                    ButtonCode = "Table:Delete",
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

        #region 数据字典

        var dictionaryMenuModel = new MenuModel
        {
            Id = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.Id,
            ModuleId = devModuleModel.Id,
            MenuCode = "Dictionary:Paged",
            MenuName = "数据字典",
            MenuTitle = "数据字典",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "api",
            HasWeb = true,
            WebIcon = "api",
            WebRouter = "/dev/dictionary",
            WebComponent = "dev/dictionary/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/menu.png",
            MobileRouter = "pages_dev/dictionary/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
        };
        dictionaryMenuModel = await db.Insertable(dictionaryMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    Id = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.Id,
                    MenuId = dictionaryMenuModel.Id,
                    ButtonCode = "Dictionary:Paged",
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
                    Id = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.Id,
                    MenuId = dictionaryMenuModel.Id,
                    ButtonCode = "Dictionary:Detail",
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
                    Id = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.Id,
                    MenuId = dictionaryMenuModel.Id,
                    ButtonCode = "Dictionary:Add",
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
                    Id = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.Id,
                    MenuId = dictionaryMenuModel.Id,
                    ButtonCode = "Dictionary:Edit",
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
                    Id = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.Id,
                    MenuId = dictionaryMenuModel.Id,
                    ButtonCode = "Dictionary:Delete",
                    ButtonName = "删除",
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
                    Id = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.Id,
                    MenuId = dictionaryMenuModel.Id,
                    ButtonCode = "Dictionary:Status",
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

        #region 密码映射

        var passwordMapModel = new MenuModel
        {
            Id = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.Id,
            ModuleId = devModuleModel.Id,
            MenuCode = "PasswordMap:Paged",
            MenuName = "密码映射",
            MenuTitle = "密码映射",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = true,
            DesktopIcon = "api",
            HasWeb = true,
            WebIcon = "api",
            WebRouter = "/dev/passwordMap",
            WebComponent = "dev/passwordMap/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/passwordMap.png",
            MobileRouter = "pages_dev/passwordMap/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
        };
        passwordMapModel = await db.Insertable(passwordMapModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new ButtonModel
            {
                Id = YitIdHelper.NextId(),
                Edition = EditionEnum.Internal,
                AppId = applicationModel.Id,
                MenuId = passwordMapModel.Id,
                ButtonCode = "PasswordMap:Paged",
                ButtonName = "列表",
                HasDesktop = true,
                HasWeb = true,
                HasMobile = true,
                Sort = buttonSort,
                Status = CommonStatusEnum.Enable,
                CreatedUserId = userId,
                CreatedUserName = userName,
                CreatedTime = dateTime
            })
            .ExecuteCommandAsync();

        #endregion
    }
}