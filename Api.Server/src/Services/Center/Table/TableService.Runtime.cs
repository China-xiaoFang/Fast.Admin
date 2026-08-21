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
using Newtonsoft.Json.Linq;

namespace Fast.Center.Service.Table;

public partial class TableService
{
    /// <summary>
    /// 获取表格配置缓存
    /// </summary>
    /// <returns>表格配置缓存</returns>
    internal async Task<TableConfigModel> QueryTableConfigCache(string tableKey)
    {
        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.TableConfig, tableKey);

        return await _centerCache.GetAndSetAsync(cacheKey, async () =>
        {
            return await _tableRepository.Entities.Includes(e => e.TableColumnConfigList.OrderBy(ob => ob.Order)
                    .ToList())
                .Where(wh => wh.TableKey == tableKey)
                .SingleAsync();
        });
    }

    /// <summary>
    /// 获取用户表格列配置缓存
    /// </summary>
    /// <returns>当前用户的表格列配置缓存</returns>
    internal async Task<List<TableColumnConfigCacheModel>> QueryUserTableColumnConfigCache(long tableId, string tableKey)
    {
        var cacheKey = CacheConst.GetCacheKey(CacheConst.Center.UserTableConfigCache, tableKey, _user.TenantNo, _user.EmployeeNo);
        return await _centerCache.GetAndSetAsync(cacheKey, async () =>
               {
                   return await _columnCacheRepository.Entities
                       .Where(wh => wh.UserId == _user.EmployeeId && wh.TableId == tableId)
                       .OrderBy(ob => ob.Order)
                       .ToListAsync();
               })
               ?? [];
    }

    /// <summary>
    /// 获取表格列配置
    /// </summary>
    [HttpGet]
    [ApiInfo("获取表格列配置", HttpRequestActionEnum.Query)]
    [DisabledRequestLog]
    public async Task<QueryTableColumnConfigOutput> QueryTableColumnConfig([Required(ErrorMessage = "表格Key不能为空")] string tableKey)
    {
        var tableConfigModel = await QueryTableConfigCache(tableKey);
        if (tableConfigModel == null)
        {
            throw new UserFriendlyException("表格列配置不存在！");
        }

        var result = new QueryTableColumnConfigOutput
        {
            TableKey = tableConfigModel.TableKey,
            Columns = new List<IDictionary<string, object>>(),
            UpdatedTime = tableConfigModel.UpdatedTime,
            Change = false,
            Cache = false
        };

        // 权限判断
        if (!_user.IsSuperAdmin)
        {
            tableConfigModel.TableColumnConfigList = tableConfigModel.TableColumnConfigList.Where(wh =>
                    !wh.AuthTag.Any() || wh.AuthTag.Any(a => _user.ButtonCodeList.Contains(a)))
                .ToList();
        }

        // 循环源列数据
        foreach (var item in tableConfigModel.TableColumnConfigList)
        {
            object columnFixed = string.IsNullOrWhiteSpace(item.Fixed) ? false : item.Fixed;

            var column = new Dictionary<string, object>
            {
                {"columnId", item.ColumnId},
                {"prop", item.Prop},
                {"label", string.IsNullOrWhiteSpace(item.Label) ? null : item.Label},
                {"fixed", columnFixed},
                {"autoWidth", item.AutoWidth},
                {"width", item.Width},
                {"smallWidth", item.SmallWidth},
                {"order", item.Order},
                {"show", item.Show},
                {"copy", item.Copy},
                {"sortable", item.Sortable},
                // 如果配置原本不支持排序，则直接禁用
                {"disabledSortable", !item.Sortable},
                {"sortableField", string.IsNullOrWhiteSpace(item.SortableField) ? null : item.SortableField},
                {"type", string.IsNullOrWhiteSpace(item.Type) ? "default" : item.Type},
                {"link", item.Link},
                {"clickEmit", string.IsNullOrWhiteSpace(item.ClickEmit) ? null : item.ClickEmit},
                {"tag", item.Tag},
                {"enum", string.IsNullOrWhiteSpace(item.Enum) ? null : item.Enum},
                {"dateFix", item.DateFix},
                {"dateFormat", string.IsNullOrWhiteSpace(item.DateFormat) ? null : item.DateFormat},
                {"dataDeleteField", string.IsNullOrWhiteSpace(item.DataDeleteField) ? null : item.DataDeleteField},
                {"slot", string.IsNullOrWhiteSpace(item.Slot) ? null : item.Slot},
                {"pureSearch", item.PureSearch}
            };

            // 其他不常用配置选项
            if (item.OtherConfig?.Any() == true)
            {
                foreach (var oItem in item.OtherConfig)
                {
                    switch (oItem.Type)
                    {
                        default:
                        case ColumnAdvancedTypeEnum.String:
                            try
                            {
                                column.TryAdd(oItem.Prop, JToken.Parse(oItem.Value));
                            }
                            catch
                            {
                                column.TryAdd(oItem.Prop, oItem.Value);
                            }

                            break;
                        case ColumnAdvancedTypeEnum.Number:
                            column.TryAdd(oItem.Prop, oItem.Value.ParseToInt());
                            break;
                        case ColumnAdvancedTypeEnum.Boolean:
                            column.TryAdd(oItem.Prop, oItem.Value.ParseToBool());
                            break;
                        case ColumnAdvancedTypeEnum.Function:
                            column.TryAdd(oItem.Prop, oItem.Value);
                            break;
                    }
                }

                column.TryAdd("otherAdvancedConfig", item.OtherConfig.Select(sl => new {sl.Prop, sl.Type}));
            }

            // 搜素项
            if (!string.IsNullOrWhiteSpace(item.SearchEl))
            {
                var searchConfig = new Dictionary<string, object>
                {
                    {"el", item.SearchEl},
                    {"key", string.IsNullOrWhiteSpace(item.SearchKey) ? null : item.SearchKey},
                    {"label", string.IsNullOrWhiteSpace(item.SearchLabel) ? null : item.SearchLabel},
                    {"order", item.SearchOrder},
                    {"slot", string.IsNullOrWhiteSpace(item.SearchSlot) ? null : item.SearchSlot}
                };

                var searchPropsConfig = new Dictionary<string, object>();

                if (item.SearchConfig?.Any() == true)
                {
                    foreach (var oItem in item.SearchConfig)
                    {
                        switch (oItem.Type)
                        {
                            default:
                            case ColumnAdvancedTypeEnum.String:
                                try
                                {
                                    searchPropsConfig.TryAdd(oItem.Prop, JToken.Parse(oItem.Value));
                                }
                                catch
                                {
                                    searchPropsConfig.TryAdd(oItem.Prop, oItem.Value);
                                }

                                break;
                            case ColumnAdvancedTypeEnum.Number:
                                searchPropsConfig.TryAdd(oItem.Prop, oItem.Value.ParseToInt());
                                break;
                            case ColumnAdvancedTypeEnum.Boolean:
                                searchPropsConfig.TryAdd(oItem.Prop, oItem.Value.ParseToBool());
                                break;
                            case ColumnAdvancedTypeEnum.Function:
                                searchPropsConfig.TryAdd(oItem.Prop, oItem.Value);
                                break;
                        }
                    }

                    column.TryAdd("searchAdvancedConfig", item.SearchConfig.Select(sl => new {sl.Prop, sl.Type}));
                }

                if (searchPropsConfig.Count > 0)
                {
                    searchConfig.TryAdd("props", searchPropsConfig);
                }

                column.Add("search", searchConfig);
            }

            result.Columns.Add(column);
        }

        // 尝试获取缓存
        var tableColumnCacheList = await QueryUserTableColumnConfigCache(tableConfigModel.TableId, tableConfigModel.TableKey);

        // 判断是否存在缓存
        if (tableColumnCacheList?.Any() == true)
        {
            result.Cache = true;
            result.UpdatedTime = tableColumnCacheList.Max(m => m.CreatedTime);
            result.Change = tableConfigModel.UpdatedTime > result.UpdatedTime;

            // 深拷贝一份
            result.CacheColumns = result.Columns.Select(IDictionary<string, object> (sl) => new Dictionary<string, object>(sl))
                .ToList();

            // 循环缓存数据
            foreach (var item in tableColumnCacheList)
            {
                var columnIdx = result.CacheColumns.FindIndex(f => $"{f["columnId"]}" == item.ColumnId.ToString());

                if (columnIdx == -1)
                    continue;

                result.CacheColumns[columnIdx]["label"] = string.IsNullOrWhiteSpace(item.Label) ? null : item.Label;
                result.CacheColumns[columnIdx]["fixed"] = string.IsNullOrWhiteSpace(item.Fixed) ? false : item.Fixed;
                result.CacheColumns[columnIdx]["autoWidth"] = item.AutoWidth;
                result.CacheColumns[columnIdx]["width"] = item.Width;
                result.CacheColumns[columnIdx]["smallWidth"] = item.SmallWidth;
                result.CacheColumns[columnIdx]["order"] = item.Order;
                result.CacheColumns[columnIdx]["show"] = item.Show;
                result.CacheColumns[columnIdx]["copy"] = item.Copy;
                result.CacheColumns[columnIdx]["sortable"] = item.Sortable;

                if (result.CacheColumns[columnIdx]
                    .ContainsKey("search"))
                {
                    if (result.CacheColumns[columnIdx]["search"] is JObject searchJObject)
                    {
                        var newSearchDic = new Dictionary<string, object>();
                        foreach (var property in searchJObject.Properties())
                        {
                            newSearchDic.Add(property.Name, property.Value);
                        }

                        newSearchDic["label"] = string.IsNullOrWhiteSpace(item.SearchLabel) ? null : item.SearchLabel;
                        newSearchDic["order"] = item.SearchOrder;

                        result.CacheColumns[columnIdx]["search"] = JObject.FromObject(newSearchDic);
                    }
                }
            }

            result.CacheColumns = result.CacheColumns.OrderBy(ob => ob["order"])
                .ToList();
        }

        return result;
    }
}