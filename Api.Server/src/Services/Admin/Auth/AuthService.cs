using Fast.Admin.Entity;
using Fast.Admin.Service.Auth.Dto;
using Fast.Center.Entity;
using Fast.Center.Enum;
using Fast.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Admin.Service.Auth;

/// <summary>
/// <see cref="AuthService"/> 鉴权服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Auth, Name = "auth", Order = 998)]
public class AuthService : IAuthService, ITransientDependency, IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarClient _repository;
    private readonly ISqlSugarRepository<EmployeeModel> _empRepository;

    public AuthService(IUser user, ISqlSugarClient repository, ISqlSugarRepository<EmployeeModel> empRepository)
    {
        _user = user;
        _repository = repository;
        _empRepository = empRepository;
    }

    /// <summary>
    /// 获取登录用户信息
    /// </summary>
    /// <returns></returns>
    [HttpGet("/getLoginUserInfo")]
    [ApiInfo("获取登录用户信息", HttpRequestActionEnum.Auth)]
    [AllowForbidden]
    public async Task<GetLoginUserInfoOutput> GetLoginUserInfo()
    {
        // 查询应用信息
        var applicationModel = await _repository.Queryable<ApplicationOpenIdModel>()
            .Includes(e => e.Application)
            .Where(wh => wh.OpenId == GlobalContext.Origin)
            .SingleAsync();

        if (applicationModel == null)
        {
            throw new UserFriendlyException("未知的应用！");
        }

        if (applicationModel.AppType != GlobalContext.DeviceType)
        {
            throw new UserFriendlyException("应用类型不匹配！");
        }

        var hasDesktop = (GlobalContext.DeviceType
                          & (AppEnvironmentEnum.Windows | AppEnvironmentEnum.Mac | AppEnvironmentEnum.Linux))
                         != 0;
        var hasWeb = (GlobalContext.DeviceType & AppEnvironmentEnum.Web) != 0;
        var hasMobile = GlobalContext.IsMobile;

        var result = new GetLoginUserInfoOutput
        {
            AccountKey = _user.AccountKey,
            Mobile = _user.Mobile,
            NickName = _user.NickName,
            Avatar = _user.Avatar,
            TenantNo = _user.TenantNo,
            TenantName = _user.TenantName,
            UserKey = _user.UserKey,
            Account = _user.Account,
            EmployeeNo = _user.EmployeeNo,
            EmployeeName = _user.EmployeeName,
            DepartmentId = _user.DepartmentId,
            DepartmentName = _user.DepartmentName,
            IsSuperAdmin = _user.IsSuperAdmin,
            IsAdmin = _user.IsAdmin
        };

        // 查询租户信息
        var tenantModel = await _repository.Queryable<TenantModel>()
            .Where(wh => wh.TenantId == _user.TenantId)
            .SingleAsync();

        if (tenantModel == null)
        {
            throw new UserFriendlyException("租户不存在！");
        }

        if (tenantModel.Status == CommonStatusEnum.Disable)
        {
            throw new UserFriendlyException("租户已被禁用！");
        }

        if (!tenantModel.DatabaseInitialized)
        {
            throw new UserFriendlyException("数据库未初始化！");
        }

        // 查询角色
        var roleList = await _empRepository.Queryable<EmployeeRoleModel>()
            .Includes(e => e.Role)
            .Where(wh => wh.EmployeeId == _user.UserId)
            .Select(sl => new { sl.RoleId, sl.Role.RoleName })
            .ToListAsync();
        var roleIds = roleList.Select(sl => sl.RoleId)
            .ToList();
        result.RoleNameList = roleList.Select(sl => sl.RoleName)
            .ToList();

        var moduleQueryable = _repository.Queryable<ModuleModel>()
            .Where(wh => wh.AppId == applicationModel.AppId)
            .Where(wh => wh.Status == CommonStatusEnum.Enable);
        var menuQueryable = _repository.Queryable<MenuModel>()
            .Where(wh => wh.AppId == applicationModel.AppId)
            .Where(wh => wh.Status == CommonStatusEnum.Enable)
            .Where(wh => tenantModel.Edition >= wh.Edition)
            .WhereIF(hasDesktop, wh => wh.HasDesktop)
            .WhereIF(hasWeb, wh => wh.HasWeb)
            .WhereIF(hasMobile, wh => wh.HasMobile);

        if (_user.IsSuperAdmin)
        {
            moduleQueryable = moduleQueryable.Where(wh =>
                (wh.ViewType & (ModuleViewTypeEnum.SuperAdmin | ModuleViewTypeEnum.Admin | ModuleViewTypeEnum.All))
                != 0);
        }
        else if (_user.IsAdmin)
        {
            moduleQueryable =
                moduleQueryable.Where(wh => (wh.ViewType & (ModuleViewTypeEnum.Admin | ModuleViewTypeEnum.All)) != 0);
        }
        else
        {
            moduleQueryable = moduleQueryable.Where(wh => (wh.ViewType & ModuleViewTypeEnum.All) != 0);
            // 查询当前用户角色对应的菜单Id
            var roleMenuIds = await _empRepository.Queryable<RoleMenuModel>()
                .Where(wh => roleIds.Contains(wh.RoleId))
                .Select(sl => sl.MenuId)
                .ToListAsync();
            // 查询当前用户对应的菜单Id
            var userMenuIds = await _empRepository.Queryable<EmployeeMenuModel>()
                .Where(wh => wh.EmployeeId == _user.UserId)
                .Select(sl => sl.MenuId)
                .ToListAsync();
            var menuIds = new List<long>();
            menuIds.AddRange(roleMenuIds);
            menuIds.AddRange(userMenuIds);
            menuIds = menuIds.Distinct()
                .ToList();
            menuQueryable = menuQueryable.Where(wh => menuIds.Contains(wh.MenuId));
        }

        // 查询所有模块
        var moduleList = await moduleQueryable.Clone()
            .OrderBy(ob => ob.Sort)
            .Select(sl =>
                new AuthModuleInfoDto
                    { ModuleId = sl.ModuleId, ModuleName = sl.ModuleName, Icon = sl.Icon, Color = sl.Color })
            .ToListAsync();

        // 查询所有菜单
        var menuList = await menuQueryable.Clone()
            .InnerJoin(moduleQueryable.Clone(), (t1, t2) => t1.ModuleId == t2.ModuleId)
            .OrderByDescending(t1 => t1.Sort)
            .Select((t1, t2) => new AuthMenuInfoDto
            {
                MenuId = t1.MenuId,
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

        // 组建菜单树形
        var menuTreeList = new TreeBuildUtil<AuthMenuInfoDto, long>().Build(menuList);

        // 组装菜单
        foreach (var item in moduleList)
        {
            var curMenuTreeList = menuTreeList.Where(wh => wh.ModuleId == item.ModuleId)
                .ToList();
            if (curMenuTreeList.Count > 0)
            {
                item.Children.AddRange(curMenuTreeList);
                result.MenuList.Add(item);
            }
        }

        if (!_user.IsSuperAdmin)
        {
            var buttonQueryable = _repository.Queryable<ButtonModel>()
                .Where(wh => wh.AppId == applicationModel.AppId)
                .Where(wh => wh.Status == CommonStatusEnum.Enable)
                .Where(wh => tenantModel.Edition >= wh.Edition)
                .WhereIF(hasDesktop, wh => wh.HasDesktop)
                .WhereIF(hasWeb, wh => wh.HasWeb)
                .WhereIF(hasMobile, wh => wh.HasMobile);
            if (!_user.IsAdmin)
            {
                // 查询当前用户角色对应的按钮Id
                var roleButtonIds = await _empRepository.Queryable<RoleButtonModel>()
                    .Where(wh => roleIds.Contains(wh.RoleId))
                    .Select(sl => sl.ButtonId)
                    .ToListAsync();
                // 查询当前用户对应的按钮Id
                var userButtonIds = await _empRepository.Queryable<EmployeeButtonModel>()
                    .Where(wh => wh.EmployeeId == _user.UserId)
                    .Select(sl => sl.ButtonId)
                    .ToListAsync();
                var buttonIds = new List<long>();
                buttonIds.AddRange(roleButtonIds);
                buttonIds.AddRange(userButtonIds);
                buttonIds = buttonIds.Distinct()
                    .ToList();
                buttonQueryable = buttonQueryable.Where(wh => buttonIds.Contains(wh.ButtonId));
            }

            result.ButtonCodeList = await buttonQueryable
                .InnerJoin(menuQueryable.Clone(), (t1, t2) => t1.MenuId == t2.MenuId)
                .OrderBy(t1 => t1.Sort)
                .Select(t1 => t1.ButtonCode)
                .ToListAsync();
        }

        // 刷新缓存
        await _user.RefreshAuth(new AuthUserInfo
        {
            DeviceType = _user.DeviceType,
            DeviceId = _user.DeviceId,
            AppNo = _user.AppNo,
            Mobile = _user.Mobile,
            TenantNo = _user.TenantNo,
            EmployeeNo = _user.EmployeeNo,
            IsSuperAdmin = _user.IsSuperAdmin,
            IsAdmin = _user.IsAdmin,
            RoleIdList = roleIds,
            RoleNameList = result.RoleNameList,
            MenuCodeList = _user.IsSuperAdmin
                ? []
                : menuList.Select(sl => sl.MenuCode)
                    .ToList(),
            ButtonCodeList = result.ButtonCodeList
        });

        return result;
    }
}