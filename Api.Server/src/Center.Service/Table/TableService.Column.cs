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

namespace Fast.Center.Service.Table;

public partial class TableService
{
    /// <summary>
    /// 获取表格列配置详情
    /// </summary>
    [HttpGet]
    [ApiInfo("获取表格列配置详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Table.Detail)]
    [PlatformOnly]
    public async Task<List<FaTableColumnCtx>> QueryTableColumnConfigDetail([Required(ErrorMessage = "表格Id不能为空")] long? tableId)
    {
        return await _columnRepository.Entities.Where(wh => wh.TableId == tableId)
            .OrderBy(ob => ob.Order)
            .Select(sl => new FaTableColumnCtx
            {
                ColumnId = sl.ColumnId,
                Prop = sl.Prop,
                Label = sl.Label,
                Fixed = sl.Fixed,
                AutoWidth = sl.AutoWidth,
                Width = sl.Width,
                SmallWidth = sl.SmallWidth,
                Order = sl.Order,
                Show = sl.Show,
                Copy = sl.Copy,
                Sortable = sl.Sortable,
                SortableField = sl.SortableField,
                Type = sl.Type,
                Link = sl.Link,
                ClickEmit = sl.ClickEmit,
                Tag = sl.Tag,
                Enum = sl.Enum,
                DateFix = sl.DateFix,
                DateFormat = sl.DateFormat,
                AuthTag = sl.AuthTag,
                DataDeleteField = sl.DataDeleteField,
                Slot = sl.Slot,
                OtherConfig = sl.OtherConfig,
                PureSearch = sl.PureSearch,
                SearchEl = sl.SearchEl,
                SearchKey = sl.SearchKey,
                SearchLabel = sl.SearchLabel,
                SearchOrder = sl.SearchOrder,
                SearchSlot = sl.SearchSlot,
                SearchConfig = sl.SearchConfig
            })
            .ToListAsync();
    }

    /// <summary>
    /// 编辑表格列配置
    /// </summary>
    [HttpPost]
    [ApiInfo("编辑表格列配置", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Table.Edit)]
    [PlatformOnly]
    public async Task EditTableColumnConfig(EditTableColumnConfigInput input)
    {
        var columnIds = input.Columns.Where(wh => wh.ColumnId != null)
            .Select(sl => sl.ColumnId)
            .Distinct()
            .ToList();

        if (columnIds.Count != input.Columns.Count(c => c.ColumnId != null))
        {
            throw new UserFriendlyException("传入的列重复！");
        }

        var tableConfigModel = await _tableRepository.SingleOrDefaultAsync(input.TableId);
        if (tableConfigModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        // 查询表格配置的所有列
        var tableColumnList = await _columnRepository.Entities.Where(wh => wh.TableId == input.TableId)
            .ToListAsync();

        // 更新的
        var updateTableColumnList = input.Columns.Where(wh => wh.ColumnId != null)
            .Select(item =>
            {
                var tableColumnModel = tableColumnList.SingleOrDefault(s => s.ColumnId == item.ColumnId);
                if (tableColumnModel == null)
                {
                    throw new UserFriendlyException("数据不存在！");
                }

                tableColumnModel.Prop = item.Prop;
                tableColumnModel.Label = item.Label;
                tableColumnModel.Fixed = item.Fixed;
                tableColumnModel.AutoWidth = item.AutoWidth;
                tableColumnModel.Width = item.Width;
                tableColumnModel.SmallWidth = item.SmallWidth;
                tableColumnModel.Order = item.Order;
                tableColumnModel.Show = item.Show;
                tableColumnModel.Copy = item.Copy;
                tableColumnModel.Sortable = item.Sortable;
                tableColumnModel.SortableField = item.SortableField;
                tableColumnModel.Type = item.Type;
                tableColumnModel.Link = item.Link;
                tableColumnModel.ClickEmit = item.ClickEmit;
                tableColumnModel.Tag = item.Tag;
                tableColumnModel.Enum = item.Enum;
                tableColumnModel.DateFix = item.DateFix;
                tableColumnModel.DateFormat = item.DateFormat;
                tableColumnModel.AuthTag = item.AuthTag;
                tableColumnModel.DataDeleteField = item.DataDeleteField;
                tableColumnModel.Slot = item.Slot;
                tableColumnModel.OtherConfig = item.OtherConfig;
                tableColumnModel.PureSearch = item.PureSearch;
                tableColumnModel.SearchEl = item.SearchEl;
                tableColumnModel.SearchKey = item.SearchKey;
                tableColumnModel.SearchLabel = item.SearchLabel;
                tableColumnModel.SearchOrder = item.SearchOrder;
                tableColumnModel.SearchSlot = item.SearchSlot;
                tableColumnModel.SearchConfig = item.SearchConfig;

                return tableColumnModel;
            })
            .ToList();

        // 添加的
        var addTableColumnList = input.Columns.Where(wh => wh.ColumnId == null)
            .Select(sl => new TableColumnConfigModel
            {
                TableId = tableConfigModel.TableId,
                Prop = sl.Prop,
                Label = sl.Label,
                Fixed = sl.Fixed,
                AutoWidth = sl.AutoWidth,
                Width = sl.Width,
                SmallWidth = sl.SmallWidth,
                Order = sl.Order,
                Show = sl.Show,
                Copy = sl.Copy,
                Sortable = sl.Sortable,
                SortableField = sl.SortableField,
                Type = sl.Type,
                Link = sl.Link,
                ClickEmit = sl.ClickEmit,
                Tag = sl.Tag,
                Enum = sl.Enum,
                DateFix = sl.DateFix,
                DateFormat = sl.DateFormat,
                AuthTag = sl.AuthTag,
                DataDeleteField = sl.DataDeleteField,
                Slot = sl.Slot,
                OtherConfig = sl.OtherConfig,
                PureSearch = sl.PureSearch,
                SearchEl = sl.SearchEl,
                SearchKey = sl.SearchKey,
                SearchLabel = sl.SearchLabel,
                SearchOrder = sl.SearchOrder,
                SearchSlot = sl.SearchSlot,
                SearchConfig = sl.SearchConfig
            })
            .ToList();

        var deleteTableColumnList = tableColumnList.Where(wh => !columnIds.Contains(wh.ColumnId))
            .ToList();

        tableConfigModel.RowVersion = input.RowVersion;

        await _tableRepository.Ado.UseTranAsync(async () =>
        {
            await _tableRepository.Updateable(tableConfigModel)
                .ExecuteCommandAsync();
            await _columnRepository.DeleteAsync(deleteTableColumnList);
            await _columnRepository.UpdateAsync(updateTableColumnList);
            await _columnRepository.InsertAsync(addTableColumnList);
        }, ex => throw ex);

        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.TableConfig, tableConfigModel.TableKey);
        await _centerCache.DelAsync(cacheKey);
    }
}