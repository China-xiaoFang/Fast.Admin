// ------------------------------------------------------------------------
// Apache开源许可证
// 
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本协议授予任何获得本软件及其相关文档（以下简称"软件"）副本的个人或组织。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
// 3.修改或衍生作品须明确标注原作者及原软件出处。
// 
// 特别声明：
// - 本软件按"原样"提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
// - 包括但不限于数据丢失、业务中断等情况。
// 
// 免责条款：
// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
// ------------------------------------------------------------------------

using Fast.Admin.Entity;
using Fast.Admin.Service.Position.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Admin.Service.Position;

/// <summary>
/// <see cref="PositionService"/> 职位服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Admin, Name = "position")]
public class PositionService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<PositionModel> _repository;

    public PositionService(IUser user, ISqlSugarRepository<PositionModel> repository)
    {
        _user = user;
        _repository = repository;
    }

    /// <summary>
    /// 职位选择器
    /// </summary>
    /// <param name="orgId">组织Id</param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("职位选择器", HttpRequestActionEnum.Query)]
    public async Task<List<ElSelectorOutput<long>>> PositionSelector(long? orgId)
    {
        var data = await _repository.Entities
            .Where(wh => wh.Status == CommonStatusEnum.Enable)
            .WhereIF(orgId != null, wh => wh.OrgId == orgId)
            .OrderBy(ob => ob.Sort)
            .Select(sl => new { sl.PositionId, sl.PositionName, sl.PositionCode })
            .ToListAsync();

        return data.Select(sl => new ElSelectorOutput<long>
        {
            Value = sl.PositionId,
            Label = sl.PositionName,
            Data = new { sl.PositionCode }
        }).ToList();
    }

    /// <summary>
    /// 获取职位分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取职位分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.Position.Paged)]
    public async Task<PagedResult<QueryPositionPagedOutput>> QueryPositionPaged(QueryPositionPagedInput input)
    {
        return await _repository.Entities
            .LeftJoin<OrganizationModel>((t1, t2) => t1.OrgId == t2.OrgId)
            .WhereIF(input.OrgId != null, t1 => t1.OrgId == input.OrgId)
            .WhereIF(!string.IsNullOrEmpty(input.PositionName), t1 => t1.PositionName.Contains(input.PositionName))
            .WhereIF(!string.IsNullOrEmpty(input.PositionCode), t1 => t1.PositionCode.Contains(input.PositionCode))
            .WhereIF(input.Status != null, t1 => t1.Status == input.Status)
            .OrderByDescending(t1 => t1.CreatedTime)
            .Select((t1, t2) => new QueryPositionPagedOutput
            {
                PositionId = t1.PositionId,
                OrgId = t1.OrgId,
                OrgName = t2.OrgName,
                PositionName = t1.PositionName,
                PositionCode = t1.PositionCode,
                Sort = t1.Sort,
                Status = t1.Status,
                Remark = t1.Remark,
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
    /// 获取职位详情
    /// </summary>
    /// <param name="positionId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取职位详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Position.Detail)]
    public async Task<QueryPositionDetailOutput> QueryPositionDetail([Required(ErrorMessage = "职位Id不能为空")] long? positionId)
    {
        var result = await _repository.Entities
            .LeftJoin<OrganizationModel>((t1, t2) => t1.OrgId == t2.OrgId)
            .Where(t1 => t1.PositionId == positionId)
            .Select((t1, t2) => new QueryPositionDetailOutput
            {
                PositionId = t1.PositionId,
                OrgId = t1.OrgId,
                OrgName = t2.OrgName,
                PositionName = t1.PositionName,
                PositionCode = t1.PositionCode,
                Sort = t1.Sort,
                Status = t1.Status,
                Remark = t1.Remark,
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
    /// 添加职位
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("添加职位", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.Position.Add)]
    public async Task AddPosition(AddPositionInput input)
    {
        // 检查组织是否存在
        if (!await _repository.Queryable<OrganizationModel>()
            .AnyAsync(a => a.OrgId == input.OrgId))
        {
            throw new UserFriendlyException("组织不存在！");
        }

        if (await _repository.AnyAsync(a => a.PositionName == input.PositionName && a.OrgId == input.OrgId))
        {
            throw new UserFriendlyException("职位名称重复！");
        }

        if (await _repository.AnyAsync(a => a.PositionCode == input.PositionCode))
        {
            throw new UserFriendlyException("职位编码重复！");
        }

        var positionModel = new PositionModel
        {
            OrgId = input.OrgId,
            PositionName = input.PositionName,
            PositionCode = input.PositionCode,
            Sort = input.Sort,
            Status = CommonStatusEnum.Enable,
            Remark = input.Remark
        };

        await _repository.InsertAsync(positionModel);
    }

    /// <summary>
    /// 编辑职位
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑职位", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Position.Edit)]
    public async Task EditPosition(EditPositionInput input)
    {
        // 检查组织是否存在
        if (!await _repository.Queryable<OrganizationModel>()
            .AnyAsync(a => a.OrgId == input.OrgId))
        {
            throw new UserFriendlyException("组织不存在！");
        }

        if (await _repository.AnyAsync(a => a.PositionName == input.PositionName && a.OrgId == input.OrgId && a.PositionId != input.PositionId))
        {
            throw new UserFriendlyException("职位名称重复！");
        }

        if (await _repository.AnyAsync(a => a.PositionCode == input.PositionCode && a.PositionId != input.PositionId))
        {
            throw new UserFriendlyException("职位编码重复！");
        }

        var positionModel = await _repository.SingleOrDefaultAsync(input.PositionId);
        if (positionModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        positionModel.OrgId = input.OrgId;
        positionModel.PositionName = input.PositionName;
        positionModel.PositionCode = input.PositionCode;
        positionModel.Sort = input.Sort;
        positionModel.Status = input.Status;
        positionModel.Remark = input.Remark;
        positionModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(positionModel);
    }

    /// <summary>
    /// 删除职位
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("删除职位", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.Position.Delete)]
    public async Task DeletePosition(PositionIdInput input)
    {
        // 检查是否有员工关联
        if (await _repository.Queryable<EmployeeOrgModel>()
            .AnyAsync(a => a.PositionId == input.PositionId))
        {
            throw new UserFriendlyException("职位存在员工关联，无法删除！");
        }

        var positionModel = await _repository.SingleOrDefaultAsync(input.PositionId);
        if (positionModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        await _repository.DeleteAsync(positionModel);
    }
}
