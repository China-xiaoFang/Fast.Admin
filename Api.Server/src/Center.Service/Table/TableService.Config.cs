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
using Fast.Center.Service.Table.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Yitter.IdGenerator;

namespace Fast.Center.Service.Table;

public partial class TableService
{
    /// <summary>
    /// 获取表格配置分页列表
    /// </summary>
    [HttpPost]
    [ApiInfo("获取表格配置分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.Table.Paged)]
    [PlatformOnly]
    public async Task<PagedResult<QueryTableConfigPagedOutput>> QueryTableConfigPaged(PagedInput input)
    {
        return await _tableRepository.Entities.OrderByIF(input.IsOrderBy, ob => ob.CreatedTime, OrderByType.Desc)
            .Select(sl => new QueryTableConfigPagedOutput
            {
                TableId = sl.TableId,
                TableKey = sl.TableKey,
                TableName = sl.TableName,
                Remark = sl.Remark,
                DepartmentName = sl.DepartmentName,
                CreatedUserName = sl.CreatedUserName,
                CreatedTime = sl.CreatedTime,
                UpdatedUserName = sl.UpdatedUserName,
                UpdatedTime = sl.UpdatedTime,
                RowVersion = sl.RowVersion
            })
            .ToPagedListAsync(input);
    }

    /// <summary>
    /// 获取表格配置详情
    /// </summary>
    [HttpGet]
    [ApiInfo("获取表格配置详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Table.Detail)]
    [PlatformOnly]
    public async Task<QueryTableConfigDetailOutput> QueryTableConfigDetail([Required(ErrorMessage = "表格Id不能为空")] long? tableId)
    {
        var result = await _tableRepository.Entities.Where(wh => wh.TableId == tableId)
            .Select(sl => new QueryTableConfigDetailOutput
            {
                TableId = sl.TableId,
                TableKey = sl.TableKey,
                TableName = sl.TableName,
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
    /// 添加表格配置
    /// </summary>
    [HttpPost]
    [ApiInfo("添加表格配置", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.Table.Add)]
    [PlatformOnly]
    public async Task AddTableConfig(AddTableConfigInput input)
    {
        // 判断表格名称是否重复
        if (await _tableRepository.AnyAsync(a => a.TableName == input.TableName))
        {
            throw new UserFriendlyException("表格名称不能重复！");
        }

        var tableId = YitIdHelper.NextId();
        var tableConfigModel = new TableConfigModel
        {
            TableId = tableId,
            TableKey = NumberUtil.IdToCodeByLong(tableId),
            TableName = input.TableName,
            Remark = input.Remark
        };

        await _tableRepository.InsertAsync(tableConfigModel);
    }

    /// <summary>
    /// 编辑表格配置
    /// </summary>
    [HttpPost]
    [ApiInfo("编辑表格配置", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Table.Edit)]
    [PlatformOnly]
    public async Task EditTableConfig(EditTableConfigInput input)
    {
        // 判断表格名称是否重复
        if (await _tableRepository.AnyAsync(a => a.TableName == input.TableName && a.TableId != input.TableId))
        {
            throw new UserFriendlyException("表格名称不能重复！");
        }

        var tableConfigModel = await _tableRepository.SingleOrDefaultAsync(input.TableId);
        if (tableConfigModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        tableConfigModel.TableName = input.TableName;
        tableConfigModel.Remark = input.Remark;
        tableConfigModel.RowVersion = input.RowVersion;

        await _tableRepository.Updateable(tableConfigModel)
            // 避免表格同步循环问题，这里不更新时间
            .IgnoreColumns(it => new {it.UpdatedTime})
            .ExecuteCommandWithOptLockAsync(true);
    }

    /// <summary>
    /// 删除表格配置
    /// </summary>
    [HttpPost]
    [ApiInfo("删除表格配置", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.Table.Delete)]
    [PlatformOnly]
    public async Task DeleteTableConfig(TableIdInput input)
    {
        var tableConfigModel = await _tableRepository.SingleOrDefaultAsync(input.TableId);
        if (tableConfigModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        await _tableRepository.Ado.UseTranAsync(async () =>
        {
            await _columnCacheRepository.DeleteAsync(wh => wh.TableId == tableConfigModel.TableId);
            await _columnRepository.DeleteAsync(wh => wh.TableId == tableConfigModel.TableId);
            await _tableRepository.DeleteAsync(tableConfigModel);
        }, ex => throw ex);

        // 清除缓存
        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.UserTableConfigCache, tableConfigModel.TableKey, "*", "*");
        await _centerCache.DelByPatternAsync(cacheKey);
    }

    /// <summary>
    /// 复制表格配置
    /// </summary>
    [HttpPost]
    [ApiInfo("复制表格配置", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Table.Edit)]
    [PlatformOnly]
    public async Task CopyTableConfig(CopyTableConfigInput input)
    {
        // 判断表格名称是否重复
        if (await _tableRepository.AnyAsync(a => a.TableName == input.TableName))
        {
            throw new UserFriendlyException("表格名称不能重复！");
        }

        if (!await _tableRepository.AnyAsync(a => a.TableId == input.TableId))
        {
            throw new UserFriendlyException("数据不存在！");
        }

        var tableId = YitIdHelper.NextId();
        var tableConfigModel = new TableConfigModel
        {
            TableId = tableId,
            TableKey = NumberUtil.IdToCodeByLong(tableId),
            TableName = input.TableName,
            Remark = input.Remark
        };

        // 查询表格所有列
        var columnConfigList = await _columnRepository.Entities.Where(wh => wh.TableId == input.TableId)
            .OrderBy(ob => ob.Order)
            .ToListAsync();

        // 重置列Id和表格Id
        columnConfigList.ForEach(item =>
        {
            item.ColumnId = YitIdHelper.NextId();
            item.TableId = tableConfigModel.TableId;
        });

        await _tableRepository.Ado.UseTranAsync(async () =>
        {
            await _tableRepository.InsertAsync(tableConfigModel);
            await _columnRepository.InsertAsync(columnConfigList);
        }, ex => throw ex);
    }
}