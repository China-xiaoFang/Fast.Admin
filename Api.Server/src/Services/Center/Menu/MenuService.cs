using Fast.Center.Entity;
using Fast.Center.Service.Menu.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Center.Service.Menu;

/// <summary>
/// <see cref="MenuService"/> 菜单服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Center, Name = "menu")]
public class MenuService : IDynamicApplication
{
    private readonly ISqlSugarRepository<MenuModel> _repository;

    public MenuService(ISqlSugarRepository<MenuModel> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// 获取菜单列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取菜单列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.Menu.Paged)]
    public async Task<List<QueryMenuPagedOutput>> QueryMenuPaged(QueryMenuPagedInput input)
    {
        var data = await _repository.Entities.LeftJoin<ModuleModel>((t1, t2) => t1.ModuleId == t2.ModuleId)
            .LeftJoin<ApplicationModel>((t1, t2, t3) => t1.AppId == t3.AppId)
            .WhereIF(input.Edition != null, t1 => t1.Edition == input.Edition)
            .WhereIF(input.AppId != null, t1 => t1.AppId == input.AppId)
            .WhereIF(input.ModuleId != null, t1 => t1.ModuleId == input.ModuleId)
            .WhereIF(input.MenuType != null, t1 => t1.MenuType == input.MenuType)
            .WhereIF(input.HasDesktop != null, t1 => t1.HasDesktop == input.HasDesktop)
            .WhereIF(input.HasWeb != null, t1 => t1.HasWeb == input.HasWeb)
            .WhereIF(input.Visible != null, t1 => t1.Visible == input.Visible)
            .WhereIF(input.HasMobile != null, t1 => t1.HasMobile == input.HasMobile)
            .WhereIF(input.Status != null, t1 => t1.Status == input.Status)
            .OrderBy(t1 => t1.Sort)
            .Select((t1, t2, t3) => new QueryMenuPagedOutput
            {
                MenuId = t1.MenuId,
                Edition = t1.Edition,
                AppId = t1.AppId,
                AppName = t3.AppName,
                ModuleId = t1.ModuleId,
                ModuleName = t2.ModuleName,
                MenuCode = t1.MenuCode,
                MenuName = t1.MenuName,
                MenuTitle = t1.MenuTitle,
                ParentId = t1.ParentId,
                MenuType = t1.MenuType,
                HasDesktop = t1.HasDesktop,
                DesktopIcon = t1.DesktopIcon,
                DesktopRouter = t1.DesktopRouter,
                HasWeb = t1.HasWeb,
                WebIcon = t1.WebIcon,
                WebRouter = t1.WebRouter,
                WebComponent = t1.WebComponent,
                HasMobile = t1.HasMobile,
                MobileIcon = t1.MobileIcon,
                MobileRouter = t1.MobileRouter,
                Link = t1.Link,
                Visible = t1.Visible,
                Sort = t1.Sort,
                Status = t1.Status,
                CreatedUserName = t1.CreatedUserName,
                CreatedTime = t1.CreatedTime,
                UpdatedUserName = t1.UpdatedUserName,
                UpdatedTime = t1.UpdatedTime
            })
            .PagedWhere(input)
            .PagedSearch(input.SearchList)
            .PagedOrderBy(input.SortList)
            .ToListAsync();

        return new TreeBuildUtil<QueryMenuPagedOutput, long>().Build(data);
    }

    /// <summary>
    /// 获取菜单详情
    /// </summary>
    /// <param name="menuId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取菜单详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Menu.Detail)]
    public async Task<QueryMenuDetailOutput> QueryMenuDetail([Required(ErrorMessage = "菜单Id不能为为空")] long? menuId)
    {
        var result = await _repository.Entities.LeftJoin<ModuleModel>((t1, t2) => t1.ModuleId == t2.ModuleId)
            .LeftJoin<ApplicationModel>((t1, t2, t3) => t1.AppId == t3.AppId)
            .Where(t1 => t1.MenuId == menuId)
            .Select((t1, t2, t3) => new QueryMenuDetailOutput
            {
                MenuId = t1.MenuId,
                Edition = t1.Edition,
                AppId = t1.AppId,
                AppName = t3.AppName,
                ModuleId = t1.ModuleId,
                ModuleName = t2.ModuleName,
                MenuCode = t1.MenuCode,
                MenuName = t1.MenuName,
                MenuTitle = t1.MenuTitle,
                MenuType = t1.MenuType,
                HasDesktop = t1.HasDesktop,
                DesktopIcon = t1.DesktopIcon,
                DesktopRouter = t1.DesktopRouter,
                HasWeb = t1.HasWeb,
                WebIcon = t1.WebIcon,
                WebRouter = t1.WebRouter,
                WebComponent = t1.WebComponent,
                HasMobile = t1.HasMobile,
                MobileIcon = t1.MobileIcon,
                MobileRouter = t1.MobileRouter,
                Link = t1.Link,
                Visible = t1.Visible,
                Sort = t1.Sort,
                Status = t1.Status,
                CreatedUserName = t1.CreatedUserName,
                CreatedTime = t1.CreatedTime,
                UpdatedUserName = t1.UpdatedUserName,
                UpdatedTime = t1.UpdatedTime,
                RowVersion = t1.RowVersion
            })
            .SingleAsync();

        if (result == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        result.ButtonList = await _repository.Queryable<ButtonModel>()
            .Where(wh => wh.MenuId == menuId)
            .OrderBy(ob => ob.Sort)
            .Select(sl => new QueryMenuDetailOutput.QueryMenuButtonDetailDto
            {
                ButtonId = sl.ButtonId,
                Edition = sl.Edition,
                ButtonCode = sl.ButtonCode,
                ButtonName = sl.ButtonName,
                HasDesktop = sl.HasDesktop,
                HasWeb = sl.HasWeb,
                HasMobile = sl.HasMobile,
                Sort = sl.Sort,
                Status = sl.Status
            })
            .ToListAsync();

        return result;
    }

    /// <summary>
    /// 添加菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("添加菜单", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.Menu.Add)]
    public async Task AddMenu(AddMenuInput input)
    {
        var moduleModel = await _repository.Queryable<ModuleModel>()
            .SingleAsync(s => s.ModuleId == input.ModuleId);
        if (moduleModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        if (await _repository.AnyAsync(a => a.ModuleId == input.ModuleId && a.MenuName == input.MenuName))
        {
            throw new UserFriendlyException("菜单名称重复！");
        }

        var menuModel = new MenuModel
        {
            Edition = input.Edition,
            AppId = moduleModel.AppId,
            ModuleId = moduleModel.ModuleId,
            MenuCode = input.MenuCode,
            MenuName = input.MenuName,
            MenuTitle = input.MenuTitle,
            MenuType = input.MenuType,
            HasDesktop = input.HasDesktop,
            DesktopIcon = input.DesktopIcon,
            DesktopRouter = input.DesktopRouter,
            HasWeb = input.HasWeb,
            WebIcon = input.WebIcon,
            WebRouter = input.WebRouter,
            WebComponent = input.WebComponent,
            HasMobile = input.HasMobile,
            MobileIcon = input.MobileIcon,
            MobileRouter = input.MobileRouter,
            Link = input.Link,
            Visible = input.Visible,
            Sort = input.Sort,
            Status = CommonStatusEnum.Enable
        };

        if (input.ParentId > 0)
        {
            var parentMenu = await _repository.SingleOrDefaultAsync(s => s.MenuId == input.ParentId);
            if (parentMenu == null)
            {
                throw new UserFriendlyException("数据不存在！");
            }

            menuModel.ParentId = parentMenu.MenuId;
            menuModel.ParentIds = parentMenu.ParentIds;
            menuModel.ParentIds.Add(parentMenu.MenuId);
        }
        else
        {
            menuModel.ParentId = 0;
            menuModel.ParentIds = [0];
        }

        await _repository.InsertAsync(menuModel);
    }

    /// <summary>
    /// 编辑菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑菜单", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Menu.Edit)]
    public async Task EditMenu(EditMenuInput input)
    {
        var buttonIds = input.ButtonList.Select(sl => sl.ButtonId)
            .Distinct()
            .ToList();
        if (buttonIds.Count != input.ButtonList.Count)
        {
            throw new UserFriendlyException("按钮重复！");
        }

        var moduleModel = await _repository.Queryable<ModuleModel>()
            .SingleAsync(s => s.ModuleId == input.ModuleId);
        if (moduleModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        if (await _repository.AnyAsync(a =>
                a.ModuleId == input.ModuleId && a.MenuName == input.MenuName && a.MenuId != input.MenuId))
        {
            throw new UserFriendlyException("菜单名称重复！");
        }

        var menuModel = await _repository.SingleOrDefaultAsync(input.MenuId);
        if (menuModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var buttonList = await _repository.Queryable<ButtonModel>()
            .Where(wh => wh.MenuId == input.MenuId)
            .ToListAsync();

        menuModel.Edition = input.Edition;
        menuModel.AppId = moduleModel.AppId;
        menuModel.ModuleId = moduleModel.ModuleId;
        menuModel.MenuCode = input.MenuCode;
        menuModel.MenuName = input.MenuName;
        menuModel.MenuTitle = input.MenuTitle;
        menuModel.MenuType = input.MenuType;
        menuModel.HasDesktop = input.HasDesktop;
        menuModel.DesktopIcon = input.DesktopIcon;
        menuModel.DesktopRouter = input.DesktopRouter;
        menuModel.HasWeb = input.HasWeb;
        menuModel.WebIcon = input.WebIcon;
        menuModel.WebRouter = input.WebRouter;
        menuModel.WebComponent = input.WebComponent;
        menuModel.HasMobile = input.HasMobile;
        menuModel.MobileIcon = input.MobileIcon;
        menuModel.MobileRouter = input.MobileRouter;
        menuModel.Link = input.Link;
        menuModel.Visible = input.Visible;
        menuModel.Sort = input.Sort;
        menuModel.Status = input.Status;
        menuModel.RowVersion = input.RowVersion;

        var addButtonList = new List<ButtonModel>();
        var updateButtonList = new List<ButtonModel>();
        foreach (var item in input.ButtonList)
        {
            ButtonModel buttonModel;
            if (item.ButtonId == null)
            {
                // 新增的
                buttonModel = new ButtonModel
                {
                    Edition = item.Edition,
                    AppId = menuModel.AppId,
                    ModuleId = menuModel.ModuleId,
                    MenuId = menuModel.MenuId,
                    ButtonCode = item.ButtonCode,
                    ButtonName = item.ButtonName,
                    HasDesktop = item.HasDesktop,
                    HasWeb = item.HasWeb,
                    HasMobile = item.HasMobile,
                    Sort = item.Sort,
                    Status = item.Status
                };
                addButtonList.Add(buttonModel);
            }
            else
            {
                // 更新的
                buttonModel = buttonList.SingleOrDefault(s => s.ButtonId == item.ButtonId);
                if (buttonModel == null)
                {
                    throw new UserFriendlyException("数据不存在！");
                }

                buttonModel.Edition = item.Edition;
                buttonModel.AppId = menuModel.AppId;
                buttonModel.ModuleId = menuModel.ModuleId;
                buttonModel.MenuId = menuModel.MenuId;
                buttonModel.ButtonCode = item.ButtonCode;
                buttonModel.ButtonName = item.ButtonName;
                buttonModel.HasDesktop = item.HasDesktop;
                buttonModel.HasWeb = item.HasWeb;
                buttonModel.HasMobile = item.HasMobile;
                buttonModel.Sort = item.Sort;
                buttonModel.Status = item.Status;
                updateButtonList.Add(buttonModel);
            }
        }

        // 删除的
        var deleteButtonList = buttonList.Where(wh => input.ButtonList.All(a => a.ButtonId != wh.ButtonId))
            .ToList();

        await _repository.Ado.UseTranAsync(async () =>
        {
            await _repository.UpdateAsync(menuModel);
            await _repository.Deleteable(deleteButtonList)
                .ExecuteCommandAsync();
            await _repository.Updateable(updateButtonList)
                .ExecuteCommandAsync();
            await _repository.Insertable(addButtonList)
                .ExecuteCommandAsync();
        });
    }

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("删除菜单", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.Menu.Delete)]
    public async Task DeleteMenu(MenuIdInput input)
    {
        if (await _repository.Queryable<ButtonModel>()
                .AnyAsync(a => a.MenuId == input.MenuId))
        {
            throw new UserFriendlyException("菜单存在按钮信息，无法删除！");
        }

        var menuModel = await _repository.SingleOrDefaultAsync(input.MenuId);
        if (menuModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        await _repository.DeleteAsync(menuModel);
    }

    /// <summary>
    /// 菜单更改状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("菜单更改状态", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Menu.Status)]
    public async Task ChangeStatus(MenuIdInput input)
    {
        var menuModel = await _repository.SingleOrDefaultAsync(input.MenuId);
        if (menuModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        menuModel.Status = menuModel.Status switch
        {
            CommonStatusEnum.Enable => CommonStatusEnum.Disable,
            CommonStatusEnum.Disable => CommonStatusEnum.Enable,
            _ => menuModel.Status
        };
        menuModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(menuModel);
    }
}