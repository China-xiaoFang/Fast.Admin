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

using Fast.Center.Domain;
using SqlSugar;
using Yitter.IdGenerator;

namespace Fast.Core;

/// <summary>
/// 开发接口种子数据
/// </summary>
internal static partial class MenuSeedData
{
    private static async Task SeedDevApi(ISqlSugarClient db, ApplicationModel applicationModel, DateTime dateTime)
    {
        var apiCLMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Internal,
            AppId = applicationModel.AppId,
            MenuCode = "DevApi:Catalog",
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
            WebTab = false,
            WebKeepAlive = false,
            HasMobile = false,
            MobileIcon = "fa-icon-api",
            MobileRouter = null,
            Visible = true,
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
            MenuCode = PermissionConst.ApiPaged,
            MenuName = "接口管理",
            MenuTitle = "接口管理",
            ParentId = apiCLMenuModel.MenuId,
            ParentIds = [0, apiCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            RoleType = RoleTypeEnum.IT,
            HasDesktop = true,
            DesktopRouter = null,
            DesktopIcon = "api",
            HasWeb = true,
            WebIcon = "fa-icon-Gateway",
            WebRouter = "/dev/api",
            WebComponent = "dev/api/index",
            WebTab = true,
            WebKeepAlive = true,
            HasMobile = false,
            MobileIcon = null,
            MobileRouter = null,
            Visible = true,
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
                RoleType = RoleTypeEnum.IT,
                HasDesktop = true,
                HasWeb = true,
                HasMobile = false,
                Sort = 1,
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
                    MenuCode = PermissionConst.ApiSwagger,
                    MenuName = "Swagger",
                    MenuTitle = "Swagger",
                    ParentId = apiCLMenuModel.MenuId,
                    ParentIds = [0, apiCLMenuModel.MenuId],
                    MenuType = MenuTypeEnum.Internal,
                    RoleType = RoleTypeEnum.IT,
                    HasDesktop = true,
                    DesktopIcon = "api",
                    DesktopRouter = null,
                    HasWeb = true,
                    WebIcon = "fa-icon-Swagger",
                    WebRouter = null,
                    WebComponent = null,
                    WebTab = false,
                    WebKeepAlive = false,
                    HasMobile = false,
                    MobileIcon = null,
                    MobileRouter = null,
                    Link = "http://127.0.0.1:38081",
                    Visible = true,
                    Sort = menuSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                },
                new()
                {
                    MenuId = YitIdHelper.NextId(),
                    Edition = EditionEnum.Internal,
                    AppId = applicationModel.AppId,
                    MenuCode = PermissionConst.ApiKnife4j,
                    MenuName = "Knife4j",
                    MenuTitle = "Knife4j",
                    ParentId = apiCLMenuModel.MenuId,
                    ParentIds = [0, apiCLMenuModel.MenuId],
                    MenuType = MenuTypeEnum.Internal,
                    RoleType = RoleTypeEnum.IT,
                    HasDesktop = true,
                    DesktopIcon = "api",
                    DesktopRouter = null,
                    HasWeb = true,
                    WebIcon = "fa-icon-Swagger",
                    WebRouter = null,
                    WebComponent = null,
                    WebTab = false,
                    WebKeepAlive = false,
                    HasMobile = false,
                    MobileIcon = null,
                    MobileRouter = null,
                    Link = "http://127.0.0.1:38081/knife4j",
                    Visible = true,
                    Sort = menuSort,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion
    }
}