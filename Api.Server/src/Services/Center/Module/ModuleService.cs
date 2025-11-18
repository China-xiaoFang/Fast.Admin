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
using Fast.Center.Service.Menu;
using Fast.Center.Service.Module.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Center.Service.Module;

/// <summary>
/// <see cref="MenuService"/> 菜单服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Center, Name = "module")]
public class Module : IDynamicApplication
{
    private readonly ISqlSugarRepository<ModuleModel> _repository;

    public Module(ISqlSugarRepository<ModuleModel> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// 模块选择器
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("模块选择器", HttpRequestActionEnum.Query)]
    public async Task<List<ElSelectorOutput<long>>> ModuleSelector(long? appId)
    {
        var data = await _repository.Entities.Where(wh => wh.Status == CommonStatusEnum.Enable)
            .WhereIF(appId != null, wh => wh.AppId == appId)
            .OrderBy(ob => ob.Sort)
            .Select(sl => new {sl.ModuleId, sl.ModuleName, sl.ViewType})
            .ToListAsync();

        return data
            .Select(sl => new ElSelectorOutput<long> {Value = sl.ModuleId, Label = sl.ModuleName, Data = new {sl.ViewType}})
            .ToList();
    }

    /// <summary>
    /// 获取模块分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取模块分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.Menu.Paged)]
    public async Task<PagedResult<QueryModulePagedOutput>> QueryModulePaged(QueryModulePagedInput input)
    {
        return await _repository.Entities.LeftJoin<ApplicationModel>((t1, t2) => t1.AppId == t2.AppId)
            .WhereIF(input.AppId != null, t1 => t1.AppId == input.AppId)
            .WhereIF(input.ViewType != null, t1 => t1.ViewType == input.ViewType)
            .WhereIF(input.Status != null, t1 => t1.Status == input.Status)
            .OrderBy(t1 => t1.Sort)
            .Select((t1, t2) => new QueryModulePagedOutput
            {
                ModuleId = t1.ModuleId,
                AppName = t2.AppName,
                ModuleName = t1.ModuleName,
                Icon = t1.Icon,
                Color = t1.Color,
                ViewType = t1.ViewType,
                Sort = t1.Sort,
                Status = t1.Status,
                DepartmentId = t1.DepartmentId,
                DepartmentName = t1.DepartmentName,
                CreatedUserName = t1.CreatedUserName,
                CreatedTime = t1.CreatedTime,
                UpdatedUserName = t1.UpdatedUserName,
                UpdatedTime = t1.UpdatedTime,
                RowVersion = t1.RowVersion
            })
            .ToPagedListAsync(input);
    }

    /// <summary>
    /// 获取模块详情
    /// </summary>
    /// <param name="moduleId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取模块详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Menu.Detail)]
    public async Task<QueryModuleDetailOutput> QueryModuleDetail([Required(ErrorMessage = "模块Id不能为空")] long? moduleId)
    {
        var result = await _repository.Entities.LeftJoin<ApplicationModel>((t1, t2) => t1.AppId == t2.AppId)
            .Where(t1 => t1.ModuleId == moduleId)
            .Select((t1, t2) => new QueryModuleDetailOutput
            {
                ModuleId = t1.ModuleId,
                AppId = t1.AppId,
                AppName = t2.AppName,
                ModuleName = t1.ModuleName,
                Icon = t1.Icon,
                Color = t1.Color,
                ViewType = t1.ViewType,
                Sort = t1.Sort,
                Status = t1.Status,
                DepartmentId = t1.DepartmentId,
                DepartmentName = t1.DepartmentName,
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

        return result;
    }

    /// <summary>
    /// 添加模块
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("添加模块", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.Menu.Add)]
    public async Task AddModule(AddModuleInput input)
    {
        if (!await _repository.Queryable<ApplicationModel>()
                .AnyAsync(a => a.AppId == input.AppId))
        {
            throw new UserFriendlyException("数据不存在！");
        }

        if (await _repository.AnyAsync(a => a.AppId == input.AppId && a.ModuleName == input.ModuleName))
        {
            throw new UserFriendlyException("模块名称重复！");
        }

        var moduleModel = new ModuleModel
        {
            AppId = input.AppId,
            ModuleName = input.ModuleName,
            Icon = input.Icon,
            Color = input.Color,
            ViewType = input.ViewType,
            Sort = input.Sort,
            Status = CommonStatusEnum.Enable
        };

        await _repository.InsertAsync(moduleModel);
    }

    /// <summary>
    /// 编辑模块
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑模块", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Menu.Edit)]
    public async Task EditModule(EditModuleInput input)
    {
        if (!await _repository.Queryable<ApplicationModel>()
                .AnyAsync(a => a.AppId == input.AppId))
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var moduleModel = await _repository.SingleOrDefaultAsync(input.ModuleId);
        if (moduleModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        if (await _repository.AnyAsync(a => a.ModuleName == input.ModuleName && a.ModuleId != input.ModuleId))
        {
            throw new UserFriendlyException("模块名称重复！");
        }

        moduleModel.ModuleName = input.ModuleName;
        moduleModel.Icon = input.Icon;
        moduleModel.Color = input.Color;
        moduleModel.ViewType = input.ViewType;
        moduleModel.Sort = input.Sort;
        moduleModel.Status = input.Status;
        moduleModel.RowVersion = input.RowVersion;

        await _repository.Ado.UseTranAsync(async () =>
        {
            if (moduleModel.AppId != input.AppId)
            {
                moduleModel.AppId = input.AppId;
                // 更新所有菜单和按钮
                await _repository.Updateable<MenuModel>()
                    .SetColumns(_ => new MenuModel {AppId = moduleModel.AppId})
                    .Where(wh => wh.ModuleId == moduleModel.ModuleId)
                    .ExecuteCommandAsync();
                await _repository.Updateable<ButtonModel>()
                    .SetColumns(_ => new ButtonModel {AppId = moduleModel.AppId})
                    .Where(wh => wh.ModuleId == moduleModel.ModuleId)
                    .ExecuteCommandAsync();
            }

            await _repository.UpdateAsync(moduleModel);
        });
    }

    /// <summary>
    /// 删除模块
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("删除模块", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.Menu.Delete)]
    public async Task DeleteModule(ModuleIdInput input)
    {
        if (await _repository.Queryable<MenuModel>()
                .AnyAsync(a => a.ModuleId == input.ModuleId))
        {
            throw new UserFriendlyException("模块存在菜单信息，无法删除！");
        }

        var moduleModel = await _repository.SingleOrDefaultAsync(input.ModuleId);
        if (moduleModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        await _repository.DeleteAsync(moduleModel);
    }

    /// <summary>
    /// 模块更改状态
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("模块更改状态", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Menu.Status)]
    public async Task ChangeStatus(ModuleIdInput input)
    {
        var moduleModel = await _repository.SingleOrDefaultAsync(input.ModuleId);
        if (moduleModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        moduleModel.Status = moduleModel.Status switch
        {
            CommonStatusEnum.Enable => CommonStatusEnum.Disable,
            CommonStatusEnum.Disable => CommonStatusEnum.Enable,
            _ => moduleModel.Status
        };
        moduleModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(moduleModel);
    }
}