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

using Fast.Center.Entity;
using Fast.Center.Service.Database.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Center.Service.Database;

/// <summary>
/// <see cref="SlaveDatabaseService"/> 从库模板服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Center, Name = "slaveDatabase")]
public class SlaveDatabaseService : IDynamicApplication
{
    private readonly ISqlSugarRepository<SlaveDatabaseModel> _repository;

    public SlaveDatabaseService(ISqlSugarRepository<SlaveDatabaseModel> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// 从库模板选择器
    /// </summary>
    /// <param name="mainDatabaseId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("从库模板选择器", HttpRequestActionEnum.Query)]
    public async Task<List<ElSelectorOutput<long>>> SlaveDatabaseSelector(long? mainDatabaseId)
    {
        var data = await _repository.Entities
            .Where(wh => wh.Status == CommonStatusEnum.Enable)
            .WhereIF(mainDatabaseId != null, wh => wh.MainId == mainDatabaseId)
            .OrderBy(ob => ob.DbName)
            .Select(sl => new { sl.SlaveId, sl.DbName, sl.MainId })
            .ToListAsync();

        return data.Select(sl => new ElSelectorOutput<long>
        {
            Value = sl.SlaveId,
            Label = sl.DbName,
            Data = new { MainDatabaseId = sl.MainId }
        }).ToList();
    }

    /// <summary>
    /// 获取从库模板分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取从库模板分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.Database.Paged)]
    public async Task<PagedResult<QuerySlaveDatabasePagedOutput>> QuerySlaveDatabasePaged(QuerySlaveDatabasePagedInput input)
    {
        return await _repository.Entities
            .LeftJoin<MainDatabaseModel>((t1, t2) => t1.MainId == t2.MainId)
            .WhereIF(input.MainDatabaseId != null, t1 => t1.MainId == input.MainDatabaseId)
            .WhereIF(input.Status != null, t1 => t1.Status == input.Status)
            .WhereIF(!string.IsNullOrEmpty(input.DatabaseName), t1 => t1.DbName.Contains(input.DatabaseName))
            .OrderByDescending(t1 => t1.CreatedTime)
            .Select((t1, t2) => new QuerySlaveDatabasePagedOutput
            {
                SlaveDatabaseId = t1.SlaveId,
                MainDatabaseId = t1.MainId,
                MainDatabaseName = t2.DbName,
                DatabaseName = t1.DbName,
                Host = t1.PublicIp,
                Port = t1.Port ?? 0,
                UserName = t1.DbUser,
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
    /// 获取从库模板详情
    /// </summary>
    /// <param name="slaveDatabaseId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取从库模板详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Database.Detail)]
    public async Task<QuerySlaveDatabaseDetailOutput> QuerySlaveDatabaseDetail([Required(ErrorMessage = "从库模板Id不能为空")] long? slaveDatabaseId)
    {
        var result = await _repository.Entities
            .LeftJoin<MainDatabaseModel>((t1, t2) => t1.MainId == t2.MainId)
            .Where(t1 => t1.SlaveId == slaveDatabaseId)
            .Select((t1, t2) => new QuerySlaveDatabaseDetailOutput
            {
                SlaveDatabaseId = t1.SlaveId,
                MainDatabaseId = t1.MainId,
                MainDatabaseName = t2.DbName,
                DatabaseName = t1.DbName,
                Host = t1.PublicIp,
                Port = t1.Port ?? 0,
                UserName = t1.DbUser,
                Password = t1.DbPwd,
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
    /// 添加从库模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("添加从库模板", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.Database.Add)]
    public async Task AddSlaveDatabase(AddSlaveDatabaseInput input)
    {
        // 检查主库是否存在
        if (!await _repository.Queryable<MainDatabaseModel>()
            .AnyAsync(a => a.MainId == input.MainDatabaseId))
        {
            throw new UserFriendlyException("主库模板不存在！");
        }

        if (await _repository.AnyAsync(a => a.DbName == input.DatabaseName))
        {
            throw new UserFriendlyException("从库模板名称重复！");
        }

        var slaveDatabaseModel = new SlaveDatabaseModel
        {
            MainId = input.MainDatabaseId,
            DbName = input.DatabaseName,
            PublicIp = input.Host,
            IntranetIp = input.Host,
            Port = input.Port,
            DbUser = input.UserName,
            DbPwd = input.Password,
            Status = CommonStatusEnum.Enable,
            Remark = input.Remark
        };

        await _repository.InsertAsync(slaveDatabaseModel);
    }

    /// <summary>
    /// 编辑从库模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑从库模板", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Database.Edit)]
    public async Task EditSlaveDatabase(EditSlaveDatabaseInput input)
    {
        // 检查主库是否存在
        if (!await _repository.Queryable<MainDatabaseModel>()
            .AnyAsync(a => a.MainId == input.MainDatabaseId))
        {
            throw new UserFriendlyException("主库模板不存在！");
        }

        if (await _repository.AnyAsync(a => a.DbName == input.DatabaseName && a.SlaveId != input.SlaveDatabaseId))
        {
            throw new UserFriendlyException("从库模板名称重复！");
        }

        var slaveDatabaseModel = await _repository.SingleOrDefaultAsync(input.SlaveDatabaseId);
        if (slaveDatabaseModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        slaveDatabaseModel.MainId = input.MainDatabaseId;
        slaveDatabaseModel.DbName = input.DatabaseName;
        slaveDatabaseModel.PublicIp = input.Host;
        slaveDatabaseModel.IntranetIp = input.Host;
        slaveDatabaseModel.Port = input.Port;
        slaveDatabaseModel.DbUser = input.UserName;
        slaveDatabaseModel.DbPwd = input.Password;
        slaveDatabaseModel.Status = input.Status;
        slaveDatabaseModel.Remark = input.Remark;
        slaveDatabaseModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(slaveDatabaseModel);
    }

    /// <summary>
    /// 删除从库模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("删除从库模板", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.Database.Delete)]
    public async Task DeleteSlaveDatabase(SlaveDatabaseIdInput input)
    {
        var slaveDatabaseModel = await _repository.SingleOrDefaultAsync(input.SlaveDatabaseId);
        if (slaveDatabaseModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        await _repository.DeleteAsync(slaveDatabaseModel);
    }
}
