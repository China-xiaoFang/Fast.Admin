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
using SqlSugar;
using Yitter.IdGenerator;

// ReSharper disable once CheckNamespace
namespace Fast.FastCloud.Core;

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
    public static async Task SystemModuleSeedData(ISqlSugarClient db, ApplicationModel applicationModel, long userId,
        string userName, DateTime dateTime)
    {
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
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
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
            WebIcon = "user",
            WebRouter = "/system/user/page",
            WebComponent = "system/user/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/user.png",
            MobileRouter = "pages_system/user/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
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
                    HasDesktop = false,
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
                    MenuId = userMenuModel.Id,
                    ButtonCode = "User:Detail",
                    ButtonName = "详情",
                    HasDesktop = false,
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
                    MenuId = userMenuModel.Id,
                    ButtonCode = "User:Add",
                    ButtonName = "新增",
                    HasDesktop = false,
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
                    MenuId = userMenuModel.Id,
                    ButtonCode = "User:Edit",
                    ButtonName = "编辑",
                    HasDesktop = false,
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
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/edition.png",
            MobileRouter = "pages_system/edition/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
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
                    HasDesktop = false,
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
                    MenuId = editionMenuModel.Id,
                    ButtonCode = "Edition:Detail",
                    ButtonName = "详情",
                    HasDesktop = false,
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
                    MenuId = editionMenuModel.Id,
                    ButtonCode = "Edition:Add",
                    ButtonName = "新增",
                    HasDesktop = false,
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
                    MenuId = editionMenuModel.Id,
                    ButtonCode = "Edition:Edit",
                    ButtonName = "编辑",
                    HasDesktop = false,
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
                    MenuId = editionMenuModel.Id,
                    ButtonCode = "Edition:Delete",
                    ButtonName = "删除",
                    HasDesktop = false,
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
                    Id = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.Id,
                    MenuId = dbMenuModel.Id,
                    ButtonCode = "Database:Page",
                    ButtonName = "列表",
                    HasDesktop = false,
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
                    MenuId = dbMenuModel.Id,
                    ButtonCode = "Database:Detail",
                    ButtonName = "详情",
                    HasDesktop = false,
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
                    MenuId = dbMenuModel.Id,
                    ButtonCode = "Database:Add",
                    ButtonName = "新增",
                    HasDesktop = false,
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
                    MenuId = dbMenuModel.Id,
                    ButtonCode = "Database:Edit",
                    ButtonName = "编辑",
                    HasDesktop = false,
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
                    MenuId = dbMenuModel.Id,
                    ButtonCode = "Database:Delete",
                    ButtonName = "删除",
                    HasDesktop = false,
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
                    Id = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.Id,
                    MenuId = appMenuModel.Id,
                    ButtonCode = "App:Page",
                    ButtonName = "列表",
                    HasDesktop = false,
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
                    MenuId = appMenuModel.Id,
                    ButtonCode = "App:Detail",
                    ButtonName = "详情",
                    HasDesktop = false,
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
                    MenuId = appMenuModel.Id,
                    ButtonCode = "App:Add",
                    ButtonName = "新增",
                    HasDesktop = false,
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
                    MenuId = appMenuModel.Id,
                    ButtonCode = "App:Edit",
                    ButtonName = "编辑",
                    HasDesktop = false,
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
                    MenuId = appMenuModel.Id,
                    ButtonCode = "App:Delete",
                    ButtonName = "删除",
                    HasDesktop = false,
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