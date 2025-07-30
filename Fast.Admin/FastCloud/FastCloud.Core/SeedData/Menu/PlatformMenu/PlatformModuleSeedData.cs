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
/// <see cref="MenuSeedData"/> 平台模块种子数据
/// </summary>
internal static partial class MenuSeedData
{
    /// <summary>
    /// 平台模块种子数据
    /// </summary>
    /// <param name="db"></param>
    /// <param name="applicationModel"><see cref="ApplicationModel"/> 应用</param>
    /// <param name="userId"><see cref="long"/> 用户Id</param>
    /// <param name="userName"><see cref="string"/> 用户名称</param>
    /// <param name="dateTime"><see cref="DateTime"/> 时间</param>
    /// <returns></returns>
    private static async Task PlatformModuleSeedData(ISqlSugarClient db, ApplicationModel applicationModel, long userId,
        string userName, DateTime dateTime)
    {
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
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
        };
        platformModuleModel = await db.Insertable(platformModuleModel)
            .ExecuteReturnEntityAsync();

        #region 平台管理

        var platformMenuModel = new MenuModel
        {
            Id = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.Id,
            ModuleId = platformModuleModel.Id,
            MenuCode = "Platform:Page",
            MenuName = "平台管理",
            MenuTitle = "平台管理",
            MenuType = MenuTypeEnum.Menu,
            HasDesktop = false,
            HasWeb = true,
            WebIcon = "platform",
            WebRouter = "/platform/platform/page",
            WebComponent = "platform/platform/index",
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/platform.png",
            MobileRouter = "pages_platform/platform/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
        };
        platformMenuModel = await db.Insertable(platformMenuModel)
            .ExecuteReturnEntityAsync();
        await db.Insertable(new List<ButtonModel>
            {
                new()
                {
                    Id = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.Id,
                    MenuId = platformMenuModel.Id,
                    ButtonCode = "Platform:Page",
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
                    MenuId = platformMenuModel.Id,
                    ButtonCode = "Platform:Detail",
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
                    MenuId = platformMenuModel.Id,
                    ButtonCode = "Platform:Activation",
                    ButtonName = "开通",
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
                    MenuId = platformMenuModel.Id,
                    ButtonCode = "Platform:Edit",
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
                    MenuId = platformMenuModel.Id,
                    ButtonCode = "Platform:ChangeStatus",
                    ButtonName = "启用/禁用",
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
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
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
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/exception.png",
            MobileRouter = "pages_platform/log/exception/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
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
                    MenuId = exLogMenuModel.Id,
                    ButtonCode = "ExceptionLog:Delete",
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
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/sqlException.png",
            MobileRouter = "pages_platform/log/sql/exception/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
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
                    MenuId = sqlExLogMenuModel.Id,
                    ButtonCode = "SqlExceptionLog:Delete",
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
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/sqlTimeout.png",
            MobileRouter = "pages_platform/log/sql/timeout/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
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
                    MenuId = sqlTimeoutLogMenuModel.Id,
                    ButtonCode = "SqlTimeoutLog:Delete",
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
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
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
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/visit.png",
            MobileRouter = "pages_platform/log/visit/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
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
                    MenuId = visitLogMenuModel.Id,
                    ButtonCode = "VisitLog:Delete",
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
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/operate.png",
            MobileRouter = "pages_platform/log/operate/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
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
                    MenuId = operateLogMenuModel.Id,
                    ButtonCode = "OperateLog:Delete",
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
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/sqlExecution.png",
            MobileRouter = "pages_platform/log/sql/execution/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
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
                    MenuId = sqlExecLogMenuModel.Id,
                    ButtonCode = "SqlExecutionLog:Delete",
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
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/sqlDiff.png",
            MobileRouter = "pages_platform/log/sql/diff/page/index",
            Visible = YesOrNotEnum.Y,
            Sort = menuSort,
            Status = CommonStatusEnum.Enable,
            CreatedUserId = userId,
            CreatedUserName = userName,
            CreatedTime = dateTime
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
                    MenuId = sqlDiffLogMenuModel.Id,
                    ButtonCode = "SqlDiffLog:Delete",
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