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
/// 配置管理种子数据
/// </summary>
internal static partial class MenuSeedData
{
    private static async Task SeedConfigManagement(ISqlSugarClient db, ApplicationModel applicationModel, DateTime dateTime)
    {
        var configCLMenuModel = new MenuModel
        {
            MenuId = YitIdHelper.NextId(),
            Edition = EditionEnum.Basic,
            AppId = applicationModel.AppId,
            MenuCode = "Config:Catalog",
            MenuName = "配置管理",
            MenuTitle = "配置管理",
            ParentId = 0,
            ParentIds = [0],
            MenuType = MenuTypeEnum.Catalog,
            HasDesktop = true,
            DesktopIcon = "setting",
            HasWeb = true,
            WebIcon = "fa-icon-Setting",
            WebRouter = null,
            WebComponent = null,
            WebTab = false,
            WebKeepAlive = false,
            HasMobile = false,
            MobileIcon = "fa-icon-organization",
            MobileRouter = null,
            Visible = true,
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
            MenuCode = PermissionConst.Serial.Paged,
            MenuName = "单号配置",
            MenuTitle = "单号配置",
            ParentId = configCLMenuModel.MenuId,
            ParentIds = [0, configCLMenuModel.MenuId],
            MenuType = MenuTypeEnum.Menu,
            RoleType = RoleTypeEnum.Admin | RoleTypeEnum.IT,
            HasDesktop = true,
            DesktopIcon = null,
            HasWeb = true,
            WebIcon = null,
            WebRouter = "/system/serial",
            WebComponent = "system/serial/index",
            WebTab = true,
            WebKeepAlive = true,
            HasMobile = true,
            MobileIcon = "https://image.fastdotnet.com/menu/mobile/menu.png",
            MobileRouter = "pages_system/serial/page/index",
            Visible = true,
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
                    RoleType = RoleTypeEnum.Admin | RoleTypeEnum.IT,
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = 1,
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
                    RoleType = RoleTypeEnum.Admin | RoleTypeEnum.IT,
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = 2,
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
                    RoleType = RoleTypeEnum.Admin | RoleTypeEnum.IT,
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = 3,
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
                    RoleType = RoleTypeEnum.Admin | RoleTypeEnum.IT,
                    HasDesktop = true,
                    HasWeb = true,
                    HasMobile = true,
                    Sort = 4,
                    Status = CommonStatusEnum.Enable,
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();

        #endregion
    }
}