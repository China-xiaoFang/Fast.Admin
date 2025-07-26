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

using Fast.FastCloud.Enum;
using Fast.FastCloud.Service.Auth.Dto;

namespace Fast.FastCloud.Service.Auth;

/// <summary>
/// <see cref="AuthService "/> 授权服务
/// </summary>
public class AuthService : IAuthService, ITransientDependency
{
    private readonly IUser _user;
    private readonly ISqlSugarClient _repository;

    public AuthService(IUser user, ISqlSugarClient repository)
    {
        _user = user;
        _repository = repository;
    }

    /// <summary>
    /// 获取登录用户信息
    /// </summary>
    /// <returns></returns>
    public async Task<GetLoginUserInfoOutput> GetLoginUserInfo()
    {
        var hasDesktop = (GlobalContext.DeviceType
                          & (AppEnvironmentEnum.Windows | AppEnvironmentEnum.Mac | AppEnvironmentEnum.Linux))
                         != 0;
        var hasWeb = (GlobalContext.DeviceType & AppEnvironmentEnum.Web) != 0;
        var hasMobile = GlobalContext.IsMobile;

        var result = new GetLoginUserInfoOutput
        {
            DeviceType = _user.DeviceType,
            DeviceId = _user.DeviceId,
            UserId = _user.UserId,
            Mobile = _user.Mobile,
            NickName = _user.NickName,
            IsAdmin = _user.IsAdmin,
            LastLoginDevice = _user.LastLoginDevice,
            LastLoginOS = _user.LastLoginOS,
            LastLoginBrowser = _user.LastLoginBrowser,
            LastLoginProvince = _user.LastLoginProvince,
            LastLoginCity = _user.LastLoginCity,
            LastLoginIp = _user.LastLoginIp,
            LastLoginTime = _user.LastLoginTime,
            PlatformNoList = _user.PlatformNoList,
            ButtonCodeList = _user.ButtonCodeList
        };

        var moduleQueryable = _repository.Queryable<ModuleModel>()
            .Where(wh => wh.AppId == CommonConst.DefaultAppId)
            .Where(wh => wh.Status == CommonStatusEnum.Enable);
        var menuQueryable = _repository.Queryable<MenuModel>()
            .Where(wh => wh.AppId == CommonConst.DefaultAppId)
            .Where(wh => wh.Status == CommonStatusEnum.Enable)
            .WhereIF(hasDesktop, wh => wh.HasDesktop)
            .WhereIF(hasWeb, wh => wh.HasWeb)
            .WhereIF(hasMobile, wh => wh.HasMobile);

        if (_user.IsAdmin)
        {
            moduleQueryable =
                moduleQueryable.Where(wh => (wh.ViewType & (ModuleViewTypeEnum.Admin | ModuleViewTypeEnum.All)) != 0);
        }
        else
        {
            // 查询所有平台编号
            result.PlatformNoList = await _repository.Queryable<UserPlatformModel>()
                .LeftJoin<PlatformModel>((t1, t2) => t1.PlatformId == t2.Id)
                .Where((t1, t2) => t2.Status == CommonStatusEnum.Enable)
                .Select((t1, t2) => t2.PlatformNo)
                .ToListAsync();

            var userModuleIds = await _repository.Queryable<UserModuleModel>()
                .Where(wh => wh.UserId == _user.UserId)
                .Select(sl => sl.ModuleId)
                .ToListAsync();
            moduleQueryable = moduleQueryable.Where(wh => (wh.ViewType & ModuleViewTypeEnum.All) != 0)
                .WhereIF(userModuleIds.Any(), wh => userModuleIds.Contains(wh.Id));

            var userMenuIds = await _repository.Queryable<UserMenuModel>()
                .Where(wh => wh.UserId == _user.UserId)
                .Select(sl => sl.MenuId)
                .ToListAsync();
            menuQueryable = menuQueryable.WhereIF(userMenuIds.Any(), wh => userMenuIds.Contains(wh.Id));
        }

        // 查询所有模块
        var moduleList = await moduleQueryable.Clone()
            .OrderByDescending(ob => ob.Sort)
            .Select(sl => new AuthModuleInfoDto {Id = sl.Id, ModuleName = sl.ModuleName, Icon = sl.Icon, Color = sl.Color})
            .ToListAsync();
        result.ModuleList = moduleList;

        // 查询所有菜单
        var menuList = await menuQueryable.Clone()
            .InnerJoin(moduleQueryable.Clone(), (t1, t2) => t1.ModuleId == t2.Id)
            .OrderByDescending((t1, t2) => t2.Sort)
            .Select((t1, t2) => new AuthMenuInfoDto
            {
                Id = t1.Id,
                ModuleId = t1.ModuleId,
                MenuCode = t1.MenuCode,
                MenuName = t1.MenuName,
                MenuTitle = t1.MenuTitle,
                ParentId = t1.ParentId,
                MenuType = t1.MenuType,
                Icon = hasDesktop ? t1.DesktopIcon :
                    hasWeb ? t1.WebIcon :
                    hasMobile ? t1.MobileIcon : null,
                Router = hasDesktop ? t1.DesktopRouter :
                    hasWeb ? t1.WebRouter :
                    hasMobile ? t1.MobileRouter : null,
                Component = hasWeb ? t1.WebComponent : null,
                Link = t1.Link,
                Visible = t1.Visible,
                Sort = t1.Sort
            })
            .ToListAsync();

        if (!_user.IsAdmin)
        {
            // 查询所有按钮编码
            var buttonQueryable = _repository.Queryable<UserButtonModel>()
                .LeftJoin<ButtonModel>((t1, t2) => t1.ButtonId == t2.Id)
                .Where((t1, t2) => t2.AppId == CommonConst.DefaultAppId)
                .Where((t1, t2) => t2.Status == CommonStatusEnum.Enable)
                .WhereIF(hasDesktop, (t1, t2) => t2.HasDesktop)
                .WhereIF(hasWeb, (t1, t2) => t2.HasWeb)
                .WhereIF(hasMobile, (t1, t2) => t2.HasMobile)
                .Select((t1, t2) => new {t2.MenuId, t2.ButtonCode, t2.Sort});
            result.ButtonCodeList = await buttonQueryable.InnerJoin(menuQueryable.Clone(), (t1, t2) => t1.MenuId == t2.Id)
                .OrderByDescending((t1, t2) => t1.Sort)
                .Select((t1, t2) => t1.ButtonCode)
                .ToListAsync();
        }

        // 更新缓存
        await _user.Refresh(result);

        // 组装菜单树形
        result.MenuList = new TreeBuildUtil<AuthMenuInfoDto, long>().Build(menuList);

        return result;
    }
}