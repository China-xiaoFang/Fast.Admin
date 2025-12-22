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
using Fast.Admin.Service.JobLevel.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Admin.Service.JobLevel;

/// <summary>
/// <see cref="JobLevelService"/> 职级服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Admin, Name = "jobLevel")]
public class JobLevelService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<JobLevelModel> _repository;

    public JobLevelService(IUser user, ISqlSugarRepository<JobLevelModel> repository)
    {
        _user = user;
        _repository = repository;
    }

    /// <summary>
    /// 职级选择器
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("职级选择器", HttpRequestActionEnum.Query)]
    public async Task<List<ElSelectorOutput<long>>> JobLevelSelector()
    {
        var data = await _repository.Entities
            .Where(wh => wh.Status == CommonStatusEnum.Enable)
            .OrderBy(ob => ob.Level)
            .Select(sl => new { sl.JobLevelId, sl.LevelName, sl.Level })
            .ToListAsync();

        return data.Select(sl => new ElSelectorOutput<long>
        {
            Value = sl.JobLevelId,
            Label = sl.LevelName,
            Data = new { sl.Level }
        }).ToList();
    }

    /// <summary>
    /// 获取职级分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取职级分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.JobLevel.Paged)]
    public async Task<PagedResult<QueryJobLevelPagedOutput>> QueryJobLevelPaged(PagedInput input)
    {
        return await _repository.Entities
            .OrderBy(ob => ob.Level)
            .ToPagedListAsync(input, sl => new QueryJobLevelPagedOutput
            {
                JobLevelId = sl.JobLevelId,
                LevelName = sl.LevelName,
                Level = sl.Level,
                Status = sl.Status,
                Remark = sl.Remark,
                DepartmentName = sl.DepartmentName,
                CreatedUserName = sl.CreatedUserName,
                CreatedTime = sl.CreatedTime,
                UpdatedUserName = sl.UpdatedUserName,
                UpdatedTime = sl.UpdatedTime,
                RowVersion = sl.RowVersion
            });
    }

    /// <summary>
    /// 获取职级详情
    /// </summary>
    /// <param name="jobLevelId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取职级详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.JobLevel.Detail)]
    public async Task<QueryJobLevelDetailOutput> QueryJobLevelDetail([Required(ErrorMessage = "职级Id不能为空")] long? jobLevelId)
    {
        var result = await _repository.Entities
            .Where(wh => wh.JobLevelId == jobLevelId)
            .Select(sl => new QueryJobLevelDetailOutput
            {
                JobLevelId = sl.JobLevelId,
                LevelName = sl.LevelName,
                Level = sl.Level,
                Status = sl.Status,
                Remark = sl.Remark,
                DepartmentName = sl.DepartmentName,
                CreatedUserName = sl.CreatedUserName,
                CreatedTime = sl.CreatedTime,
                UpdatedUserName = sl.UpdatedUserName,
                UpdatedTime = sl.UpdatedTime,
                RowVersion = sl.RowVersion
            })
            .SingleAsync();

        if (result == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        return result;
    }

    /// <summary>
    /// 添加职级
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("添加职级", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.JobLevel.Add)]
    public async Task AddJobLevel(AddJobLevelInput input)
    {
        if (await _repository.AnyAsync(a => a.LevelName == input.LevelName))
        {
            throw new UserFriendlyException("职级名称重复！");
        }

        if (await _repository.AnyAsync(a => a.Level == input.Level))
        {
            throw new UserFriendlyException("职级等级重复！");
        }

        var jobLevelModel = new JobLevelModel
        {
            LevelName = input.LevelName,
            Level = input.Level,
            Status = CommonStatusEnum.Enable,
            Remark = input.Remark
        };

        await _repository.InsertAsync(jobLevelModel);
    }

    /// <summary>
    /// 编辑职级
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑职级", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.JobLevel.Edit)]
    public async Task EditJobLevel(EditJobLevelInput input)
    {
        if (await _repository.AnyAsync(a => a.LevelName == input.LevelName && a.JobLevelId != input.JobLevelId))
        {
            throw new UserFriendlyException("职级名称重复！");
        }

        if (await _repository.AnyAsync(a => a.Level == input.Level && a.JobLevelId != input.JobLevelId))
        {
            throw new UserFriendlyException("职级等级重复！");
        }

        var jobLevelModel = await _repository.SingleOrDefaultAsync(input.JobLevelId);
        if (jobLevelModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        jobLevelModel.LevelName = input.LevelName;
        jobLevelModel.Level = input.Level;
        jobLevelModel.Status = input.Status;
        jobLevelModel.Remark = input.Remark;
        jobLevelModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(jobLevelModel);
    }

    /// <summary>
    /// 删除职级
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("删除职级", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.JobLevel.Delete)]
    public async Task DeleteJobLevel(JobLevelIdInput input)
    {
        // 检查是否有员工关联
        if (await _repository.Queryable<EmployeeOrgModel>()
            .AnyAsync(a => a.JobLevelId == input.JobLevelId))
        {
            throw new UserFriendlyException("职级存在员工关联，无法删除！");
        }

        var jobLevelModel = await _repository.SingleOrDefaultAsync(input.JobLevelId);
        if (jobLevelModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        await _repository.DeleteAsync(jobLevelModel);
    }
}
