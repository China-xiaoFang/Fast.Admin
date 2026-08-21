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
using Fast.Center.Service.Table.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Center.Service.Table;

public partial class TableService
{
    /// <summary>
    /// 同步用户表格配置
    /// </summary>
    [HttpPost]
    [ApiInfo("同步用户表格配置", HttpRequestActionEnum.Edit)]
    public async Task SyncUserTableConfig(SyncUserTableConfigInput input)
    {
        var tableConfigModel = await QueryTableConfigCache(input.TableKey);
        if (tableConfigModel == null)
        {
            throw new UserFriendlyException("表格列配置不存在！");
        }

        // 获取缓存
        var tableColumnCacheList = await QueryUserTableColumnConfigCache(tableConfigModel.TableId, tableConfigModel.TableKey);

        var columnIds = tableColumnCacheList.Select(sl => sl.ColumnId)
            .ToList();
        var sourceColumnIds = tableConfigModel.TableColumnConfigList.Select(sl => sl.ColumnId)
            .ToList();

        var dateTime = DateTime.Now;

        // 添加的
        var addTableColumnCacheList = tableConfigModel.TableColumnConfigList.Where(wh => !columnIds.Contains(wh.ColumnId))
            .Select(sl => new TableColumnConfigCacheModel
            {
                UserId = _user.EmployeeId,
                TableId = sl.TableId,
                ColumnId = sl.ColumnId,
                Label = sl.Label,
                Fixed = sl.Fixed,
                AutoWidth = sl.AutoWidth,
                Width = sl.Width,
                SmallWidth = sl.SmallWidth,
                Order = sl.Order,
                Show = sl.Show,
                Copy = sl.Copy,
                Sortable = sl.Sortable,
                SearchLabel = sl.SearchLabel,
                SearchOrder = sl.SearchOrder,
                CreatedTime = dateTime,
                TenantId = _user.TenantId
            })
            .ToList();

        // 删除的
        var deleteTableColumnCacheList = tableColumnCacheList.Where(wh => !sourceColumnIds.Contains(wh.ColumnId))
            .ToList();

        // 更新的
        var sourceDict = tableConfigModel.TableColumnConfigList.ToDictionary(k => k.ColumnId);
        var updateTableColumnCacheList = tableColumnCacheList.Where(wh => sourceColumnIds.Contains(wh.ColumnId))
            .ToList();

        foreach (var item in updateTableColumnCacheList)
        {
            if (!sourceDict.TryGetValue(item.ColumnId, out var sourceItem))
                continue;

            item.Label = sourceItem.Label;
            item.Fixed = sourceItem.Fixed;
            item.AutoWidth = sourceItem.AutoWidth;
            item.Width = sourceItem.Width;
            item.SmallWidth = sourceItem.SmallWidth;
            item.Order = sourceItem.Order;
            item.Show = sourceItem.Show;
            item.Copy = sourceItem.Copy;
            item.Sortable = sourceItem.Sortable;
            item.SearchLabel = sourceItem.SearchLabel;
            item.SearchOrder = sourceItem.SearchOrder;
            item.CreatedTime = dateTime;
        }

        await _columnCacheRepository.Ado.UseTranAsync(async () =>
        {
            await _columnCacheRepository.DeleteAsync(deleteTableColumnCacheList);
            await _columnCacheRepository.InsertAsync(addTableColumnCacheList);
            await _columnCacheRepository.UpdateAsync(updateTableColumnCacheList);
        }, ex => throw ex);

        // 删除缓存
        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.UserTableConfigCache, tableConfigModel.TableKey, _user.TenantNo,
            _user.EmployeeNo);
        await _centerCache.DelAsync(cacheKey);
    }

    /// <summary>
    /// 保存用户表格配置
    /// </summary>
    [HttpPost]
    [ApiInfo("保存用户表格配置", HttpRequestActionEnum.Edit)]
    public async Task SaveUserTableConfig(SaveUserTableConfigInput input)
    {
        var tableConfigModel = await QueryTableConfigCache(input.TableKey);
        if (tableConfigModel == null)
        {
            throw new UserFriendlyException("表格列配置不存在！");
        }

        // 获取缓存
        var tableColumnCacheList = await QueryUserTableColumnConfigCache(tableConfigModel.TableId, tableConfigModel.TableKey);
        var addTableColumnCacheList = new List<TableColumnConfigCacheModel>();
        var dateTime = DateTime.Now;

        // 保存的时候没有删除的
        foreach (var item in input.Columns)
        {
            var tableColumnModel = tableConfigModel.TableColumnConfigList.SingleOrDefault(s => s.ColumnId == item.ColumnId);
            if (tableColumnModel == null)
            {
                throw new UserFriendlyException("数据不存在！");
            }

            var tableColumnCacheModel = tableColumnCacheList.SingleOrDefault(s => s.ColumnId == item.ColumnId);
            if (tableColumnCacheModel == null)
            {
                tableColumnCacheModel = new TableColumnConfigCacheModel
                {
                    UserId = _user.EmployeeId,
                    TableId = tableColumnModel.TableId,
                    ColumnId = tableColumnModel.ColumnId,
                    Label = string.IsNullOrWhiteSpace(item.Label) ? tableColumnModel.Label : item.Label,
                    Fixed = string.IsNullOrWhiteSpace(item.Fixed) ? tableColumnModel.Fixed : item.Fixed,
                    AutoWidth = item.AutoWidth,
                    Width = item.Width ?? tableColumnModel.Width,
                    SmallWidth = item.SmallWidth ?? tableColumnModel.SmallWidth,
                    Order = item.Order ?? tableColumnModel.Order,
                    Show = item.Show,
                    Copy = item.Copy,
                    Sortable = item.Sortable,
                    SearchLabel =
                        string.IsNullOrWhiteSpace(item.SearchLabel) ? tableColumnModel.SearchLabel : item.SearchLabel,
                    SearchOrder = item.SearchOrder ?? tableColumnModel.SearchOrder,
                    CreatedTime = dateTime,
                    TenantId = _user.TenantId
                };
                addTableColumnCacheList.Add(tableColumnCacheModel);
            }
            else
            {
                tableColumnCacheModel.Label = string.IsNullOrWhiteSpace(item.Label) ? tableColumnModel.Label : item.Label;
                tableColumnCacheModel.Fixed = string.IsNullOrWhiteSpace(item.Fixed) ? tableColumnModel.Fixed : item.Fixed;
                tableColumnCacheModel.AutoWidth = item.AutoWidth;
                tableColumnCacheModel.Width = item.Width ?? tableColumnModel.Width;
                tableColumnCacheModel.SmallWidth = item.SmallWidth ?? tableColumnModel.SmallWidth;
                tableColumnCacheModel.Order = item.Order ?? tableColumnModel.Order;
                tableColumnCacheModel.Show = item.Show;
                tableColumnCacheModel.Copy = item.Copy;
                tableColumnCacheModel.Sortable = item.Sortable;
                tableColumnCacheModel.SearchLabel = string.IsNullOrWhiteSpace(item.SearchLabel)
                    ? tableColumnModel.SearchLabel
                    : item.SearchLabel;
                tableColumnCacheModel.SearchOrder = item.SearchOrder ?? tableColumnModel.SearchOrder;
            }
        }

        await _columnCacheRepository.Ado.UseTranAsync(async () =>
        {
            await _columnCacheRepository.UpdateAsync(tableColumnCacheList);
            await _columnCacheRepository.InsertAsync(addTableColumnCacheList);
        }, ex => throw ex);

        // 删除缓存
        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.UserTableConfigCache, tableConfigModel.TableKey, _user.TenantNo,
            _user.EmployeeNo);
        await _centerCache.DelAsync(cacheKey);
    }

    /// <summary>
    /// 清除用户表格配置
    /// </summary>
    [HttpPost]
    [ApiInfo("清除用户表格配置", HttpRequestActionEnum.Delete)]
    public async Task ClearUserTableConfig(SyncUserTableConfigInput input)
    {
        var tableConfigModel = await QueryTableConfigCache(input.TableKey);
        if (tableConfigModel == null)
        {
            throw new UserFriendlyException("表格列配置不存在！");
        }

        await _columnCacheRepository.DeleteAsync(wh => wh.TableId == tableConfigModel.TableId);

        // 删除缓存
        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.UserTableConfigCache, tableConfigModel.TableKey, _user.TenantNo,
            _user.EmployeeNo);
        await _centerCache.DelAsync(cacheKey);
    }
}