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
/// <see cref="MainDatabaseService"/> 主库模板服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Center, Name = "mainDatabase")]
public class MainDatabaseService : IDynamicApplication
{
    private readonly ISqlSugarRepository<MainDatabaseModel> _repository;

    public MainDatabaseService(ISqlSugarRepository<MainDatabaseModel> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// 主库模板选择器
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("主库模板选择器", HttpRequestActionEnum.Query)]
    public async Task<List<ElSelectorOutput<long>>> MainDatabaseSelector()
    {
        var data = await _repository.Entities
            .Where(wh => wh.Status == CommonStatusEnum.Enable)
            .OrderBy(ob => ob.DbName)
            .Select(sl => new { sl.MainId, sl.DbName, sl.DbType })
            .ToListAsync();

        return data.Select(sl => new ElSelectorOutput<long>
        {
            Value = sl.MainId,
            Label = sl.DbName,
            Data = new { sl.DbType }
        }).ToList();
    }

    /// <summary>
    /// 获取主库模板分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取主库模板分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.Database.Paged)]
    public async Task<PagedResult<QueryMainDatabasePagedOutput>> QueryMainDatabasePaged(QueryMainDatabasePagedInput input)
    {
        return await _repository.Entities
            .WhereIF(input.DbType != null, wh => wh.DbType == input.DbType)
            .WhereIF(input.Status != null, wh => wh.Status == input.Status)
            .WhereIF(!string.IsNullOrEmpty(input.DatabaseName), wh => wh.DbName.Contains(input.DatabaseName))
            .OrderByDescending(ob => ob.CreatedTime)
            .ToPagedListAsync(input, sl => new QueryMainDatabasePagedOutput
            {
                MainDatabaseId = sl.MainId,
                DatabaseName = sl.DbName,
                DbType = sl.DbType,
                Host = sl.PublicIp,
                Port = sl.Port,
                UserName = sl.DbUser,
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
    /// 获取主库模板详情
    /// </summary>
    /// <param name="mainDatabaseId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取主库模板详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Database.Detail)]
    public async Task<QueryMainDatabaseDetailOutput> QueryMainDatabaseDetail([Required(ErrorMessage = "主库模板Id不能为空")] long? mainDatabaseId)
    {
        var result = await _repository.Entities
            .Where(wh => wh.MainId == mainDatabaseId)
            .Select(sl => new QueryMainDatabaseDetailOutput
            {
                MainDatabaseId = sl.MainId,
                DatabaseName = sl.DbName,
                DbType = sl.DbType,
                Host = sl.PublicIp,
                Port = sl.Port,
                UserName = sl.DbUser,
                Password = sl.DbPwd,
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
    /// 添加主库模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("添加主库模板", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.Database.Add)]
    public async Task AddMainDatabase(AddMainDatabaseInput input)
    {
        if (await _repository.AnyAsync(a => a.DbName == input.DatabaseName))
        {
            throw new UserFriendlyException("主库模板名称重复！");
        }

        var mainDatabaseModel = new MainDatabaseModel
        {
            DbName = input.DatabaseName,
            DbType = input.DbType,
            PublicIp = input.Host,
            IntranetIp = input.Host,
            Port = input.Port,
            DbUser = input.UserName,
            DbPwd = input.Password,
            Status = CommonStatusEnum.Enable,
            Remark = input.Remark
        };

        await _repository.InsertAsync(mainDatabaseModel);
    }

    /// <summary>
    /// 编辑主库模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑主库模板", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Database.Edit)]
    public async Task EditMainDatabase(EditMainDatabaseInput input)
    {
        if (await _repository.AnyAsync(a => a.DbName == input.DatabaseName && a.MainId != input.MainDatabaseId))
        {
            throw new UserFriendlyException("主库模板名称重复！");
        }

        var mainDatabaseModel = await _repository.SingleOrDefaultAsync(input.MainDatabaseId);
        if (mainDatabaseModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        mainDatabaseModel.DbName = input.DatabaseName;
        mainDatabaseModel.DbType = input.DbType;
        mainDatabaseModel.PublicIp = input.Host;
        mainDatabaseModel.IntranetIp = input.Host;
        mainDatabaseModel.Port = input.Port;
        mainDatabaseModel.DbUser = input.UserName;
        mainDatabaseModel.DbPwd = input.Password;
        mainDatabaseModel.Status = input.Status;
        mainDatabaseModel.Remark = input.Remark;
        mainDatabaseModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(mainDatabaseModel);
    }

    /// <summary>
    /// 删除主库模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("删除主库模板", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.Database.Delete)]
    public async Task DeleteMainDatabase(MainDatabaseIdInput input)
    {
        // 检查是否有从库引用
        if (await _repository.Queryable<SlaveDatabaseModel>()
            .AnyAsync(a => a.MainId == input.MainDatabaseId))
        {
            throw new UserFriendlyException("主库模板存在从库引用，无法删除！");
        }

        var mainDatabaseModel = await _repository.SingleOrDefaultAsync(input.MainDatabaseId);
        if (mainDatabaseModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        await _repository.DeleteAsync(mainDatabaseModel);
    }
}
