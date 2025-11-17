using System.Reflection;
using System.Text;
using Dm.util;
using Fast.Center.Entity;
using Fast.Center.Enum;
using Fast.SqlSugar;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;
using Yitter.IdGenerator;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="SyncDictionaryHostedService"/> 同步字典托管服务
/// </summary>
[Order(104)]
public class SyncDictionaryHostedService : IHostedService
{
    /// <summary>
    /// 缓存
    /// </summary>
    private readonly ICache<CenterCCL> _centerCache;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// <see cref="SyncDictionaryHostedService"/> 同步字典托管服务
    /// </summary>
    /// <param name="centerCache"><see cref="ICache"/> 缓存</param>
    /// <param name="logger"><see cref="ILogger"/> 日志</param>
    public SyncDictionaryHostedService(ICache<CenterCCL> centerCache, ILogger<SyncDictionaryHostedService> logger)
    {
        _centerCache = centerCache;
        _logger = logger;
    }

    /// <summary>
    /// Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous Start operation.</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var dateTime = DateTime.Now;

        var addDictionaryTypeList = new List<DictionaryTypeModel>();
        var addDictionaryItemList = new List<DictionaryItemModel>();
        var updateDictionaryTypeList = new List<DictionaryTypeModel>();
        var updateDictionaryItemList = new List<DictionaryItemModel>();
        var deleteDictionaryItemList = new List<DictionaryItemModel>();

        {
            var logSb = new StringBuilder();
            logSb.Append("\u001b[40m\u001b[1m\u001b[32m");
            logSb.Append("info");
            logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
            logSb.Append(": ");
            logSb.Append($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffffff zzz dddd}");
            logSb.Append(Environment.NewLine);
            logSb.Append("\u001b[40m\u001b[90m");
            logSb.Append("      ");
            logSb.Append("开始同步字典信息...");
            logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
            Console.WriteLine(logSb.ToString());
        }

        _ = Task.Run(async () =>
        {
            try
            {
                var db = new SqlSugarClient(SqlSugarContext.GetConnectionConfig(SqlSugarContext.ConnectionSettings));

                var dictionaryTypeList = await db.Queryable<DictionaryTypeModel>()
                    .ToListAsync(cancellationToken);
                var dictionaryItemList = await db.Queryable<DictionaryItemModel>()
                    .ToListAsync(cancellationToken);

                // 获取所有带 FastEnumAttribute 特性的枚举
                var enumTypes = MAppContext.EffectiveTypes.Where(wh => wh.IsEnum)
                    .Select(sl => new
                    {
                        Type = sl,
                        FastEnumAttribute = sl.GetCustomAttribute<FastEnumAttribute>(),
                        FlagsAttribute = sl.GetCustomAttribute<FlagsAttribute>()
                    })
                    .Where(wh => wh.FastEnumAttribute != null)
                    .ToList();

                // 循环所有枚举类型
                foreach (var enumType in enumTypes)
                {
                    var enumItemList = enumType.Type.EnumToList<long>();

                    var dictionaryTypeInfo =
                        dictionaryTypeList.SingleOrDefault(s => s.DictionaryKey == enumType.Type.Name);

                    var dictionaryTypeModel = new DictionaryTypeModel
                    {
                        DictionaryKey = enumType.Type.Name,
                        DictionaryName =
                            enumType.FastEnumAttribute.ChName
                            ?? enumType.FastEnumAttribute.EnName ?? enumType.Type.Name,
                        ValueType = Enum.GetUnderlyingType(enumType.Type) == typeof(long)
                            ? DictionaryValueTypeEnum.Long
                            : DictionaryValueTypeEnum.Int,
                        HasFlags = enumType.FlagsAttribute != null ? YesOrNotEnum.Y : YesOrNotEnum.N,
                        Status = CommonStatusEnum.Enable,
                        Remark = enumType.FastEnumAttribute.Remark,
                        UpdatedTime = dateTime
                    };

                    if (dictionaryTypeInfo != null)
                    {
                        dictionaryTypeModel.DictionaryId = dictionaryTypeInfo.DictionaryId;
                        // 不相同才修改
                        if (!dictionaryTypeInfo.Equals(dictionaryTypeModel))
                        {
                            dictionaryTypeInfo.DictionaryName = dictionaryTypeModel.DictionaryName;
                            // 这里只会存在 long 或者 int
                            dictionaryTypeInfo.ValueType = dictionaryTypeModel.ValueType;
                            dictionaryTypeInfo.HasFlags = dictionaryTypeModel.HasFlags;
                            if (string.IsNullOrWhiteSpace(dictionaryTypeInfo.Remark))
                            {
                                dictionaryTypeInfo.Remark = dictionaryTypeModel.Remark;
                            }

                            dictionaryTypeInfo.UpdatedTime = dateTime;
                            updateDictionaryTypeList.Add(dictionaryTypeInfo);
                        }

                        deleteDictionaryItemList.AddRange(dictionaryItemList
                            .Where(wh => wh.DictionaryId == dictionaryTypeInfo.DictionaryId)
                            .Where(wh => enumItemList.All(a => a.Value.toString() != wh.Value))
                            .ToList());

                        var orderIndex = 1;

                        foreach (var enumItem in enumItemList)
                        {
                            var fieldInfo = enumType.Type.GetField(enumItem.Name);
                            var tagType = fieldInfo.GetCustomAttribute<TagTypeAttribute>();

                            var dictionaryItemModel = new DictionaryItemModel
                            {
                                DictionaryId = dictionaryTypeInfo.DictionaryId,
                                Label = enumItem.Describe ?? enumItem.Name,
                                Value = enumItem.Value.toString(),
                                Type = tagType?.TagType ?? TagTypeEnum.Primary,
                                Order = orderIndex,
                                Visible = YesOrNotEnum.Y,
                                Status = CommonStatusEnum.Enable,
                                UpdatedTime = dateTime
                            };

                            var dictionaryItemInfo = dictionaryItemList
                                .Where(wh => wh.DictionaryId == dictionaryTypeInfo.DictionaryId)
                                .SingleOrDefault(s => s.Value == enumItem.Value.toString());
                            if (dictionaryItemInfo != null)
                            {
                                dictionaryItemModel.DictionaryItemId = dictionaryItemInfo.DictionaryItemId;
                                // 不相同才修改
                                if (!dictionaryItemInfo.Equals(dictionaryItemModel))
                                {
                                    dictionaryItemInfo.Label = dictionaryItemModel.Label;
                                    dictionaryItemInfo.Value = dictionaryItemModel.Value;
                                    dictionaryItemInfo.Type = dictionaryItemModel.Type;
                                    dictionaryItemInfo.Order = dictionaryItemModel.Order;
                                    dictionaryItemInfo.UpdatedTime = dateTime;
                                    updateDictionaryItemList.Add(dictionaryItemInfo);
                                }
                            }
                            else
                            {
                                dictionaryItemModel.DictionaryItemId = YitIdHelper.NextId();
                                dictionaryItemModel.CreatedTime = dateTime;
                                addDictionaryItemList.Add(dictionaryItemModel);
                            }

                            orderIndex++;
                        }
                    }
                    else
                    {
                        dictionaryTypeModel.DictionaryId = YitIdHelper.NextId();
                        dictionaryTypeModel.CreatedTime = dateTime;
                        addDictionaryTypeList.Add(dictionaryTypeModel);

                        var orderIndex = 1;

                        foreach (var enumItem in enumItemList)
                        {
                            var fieldInfo = enumType.Type.GetField(enumItem.Name);
                            var tagType = fieldInfo.GetCustomAttribute<TagTypeAttribute>();

                            var dictionaryItemModel = new DictionaryItemModel
                            {
                                DictionaryItemId = YitIdHelper.NextId(),
                                DictionaryId = dictionaryTypeModel.DictionaryId,
                                Label = enumItem.Describe ?? enumItem.Name,
                                Value = enumItem.Value.toString(),
                                Type = tagType?.TagType ?? TagTypeEnum.Primary,
                                Order = orderIndex,
                                Visible = YesOrNotEnum.Y,
                                Status = CommonStatusEnum.Enable,
                                CreatedTime = dateTime,
                                UpdatedTime = dateTime
                            };
                            addDictionaryItemList.Add(dictionaryItemModel);

                            orderIndex++;
                        }
                    }
                }

                // 加载Aop
                SugarEntityFilter.LoadSugarAop(FastContext.HostEnvironment.IsDevelopment(), db);

                if (deleteDictionaryItemList.Count > 0)
                {
                    await db.Deleteable(deleteDictionaryItemList)
                        .ExecuteCommandAsync(cancellationToken);
                }

                var deleteDictionaryTypeList = dictionaryTypeList.Where(x => !enumTypes.Select(sl => sl.Type.Name)
                        .ToHashSet()
                        .Contains(x.DictionaryKey))
                    .ToList();
                if (deleteDictionaryTypeList.Count > 0)
                {
                    await db.Deleteable(deleteDictionaryTypeList)
                        .ExecuteCommandAsync(cancellationToken);
                }

                await db.Updateable(updateDictionaryTypeList)
                    .ExecuteCommandAsync(cancellationToken);
                await db.Updateable(updateDictionaryItemList)
                    .ExecuteCommandAsync(cancellationToken);
                await db.Insertable(addDictionaryTypeList)
                    .ExecuteCommandAsync(cancellationToken);
                await db.Insertable(addDictionaryItemList)
                    .ExecuteCommandAsync(cancellationToken);

                // 删除缓存
                await _centerCache.DelAsync(CacheConst.Center.Dictionary);

                {
                    var logSb = new StringBuilder();
                    logSb.Append("\u001b[40m\u001b[1m\u001b[32m");
                    logSb.Append("info");
                    logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
                    logSb.Append(": ");
                    logSb.Append($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fffffff zzz dddd}");
                    logSb.Append(Environment.NewLine);
                    logSb.Append("\u001b[40m\u001b[90m");
                    logSb.Append("      ");
                    logSb.Append(
                        $"同步字典信息成功。新增 {addDictionaryTypeList.Count}/{addDictionaryItemList.Count} 个，更新 {updateDictionaryTypeList.Count}/{updateDictionaryItemList.Count} 个，删除 {deleteDictionaryTypeList.Count}/{deleteDictionaryItemList.Count} 个。");
                    logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
                    Console.WriteLine(logSb.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sync dictionary error...");
            }
        }, cancellationToken);

        await Task.CompletedTask;
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the asynchronous Stop operation.</returns>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}