﻿// ------------------------------------------------------------------------
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

using Dm.util;
using Fast.Common;
using Fast.FastCloud.Entity;
using Fast.FastCloud.Enum;
using Fast.IaaS;
using Fast.NET.Core;
using Fast.SqlSugar;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Reflection;
using System.Text;
using Yitter.IdGenerator;

// ReSharper disable once CheckNamespace
namespace Fast.FastCloud.Core;

/// <summary>
/// <see cref="SyncDictionaryHostedService"/> 同步字典托管服务
/// </summary>
[Order(104)]
public class SyncDictionaryHostedService : IHostedService
{
    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    public SyncDictionaryHostedService(ILogger<SyncDictionaryHostedService> logger)
    {
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

                var dictionaryTypeInfo = dictionaryTypeList.SingleOrDefault(s => s.DictionaryKey == enumType.Type.Name);

                var dictionaryTypeModel = new DictionaryTypeModel
                {
                    DictionaryKey = enumType.Type.Name,
                    DictionaryName =
                        enumType.FastEnumAttribute.ChName ?? enumType.FastEnumAttribute.EnName ?? enumType.Type.Name,
                    ValueType = System.Enum.GetUnderlyingType(enumType.Type) == typeof(long)
                        ? DictionaryValueTypeEnum.Long
                        : DictionaryValueTypeEnum.Int,
                    HasFlags = enumType.FlagsAttribute != null ? YesOrNotEnum.Y : YesOrNotEnum.N,
                    Status = CommonStatusEnum.Enable,
                    Remark = enumType.FastEnumAttribute.Remark,
                    UpdatedTime = dateTime
                };

                if (dictionaryTypeInfo != null)
                {
                    dictionaryTypeModel.Id = dictionaryTypeInfo.Id;
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

                    deleteDictionaryItemList.AddRange(dictionaryItemList.Where(wh => wh.DictionaryId == dictionaryTypeInfo.Id)
                        .Where(wh => enumItemList.All(a => a.Value.toString() != wh.Value))
                        .ToList());

                    var orderIndex = 1;

                    foreach (var enumItem in enumItemList)
                    {
                        var dictionaryItemModel = new DictionaryItemModel
                        {
                            DictionaryId = dictionaryTypeInfo.Id,
                            Label = enumItem.Describe ?? enumItem.Name,
                            Value = enumItem.Value.toString(),
                            Type = DictionaryItemTypeEnum.Primary,
                            Order = orderIndex,
                            Visible = YesOrNotEnum.Y,
                            Status = CommonStatusEnum.Enable,
                            UpdatedTime = dateTime
                        };

                        var dictionaryItemInfo = dictionaryItemList.Where(wh => wh.DictionaryId == dictionaryTypeInfo.Id)
                            .SingleOrDefault(s => s.Value == enumItem.Value.toString());
                        if (dictionaryItemInfo != null)
                        {
                            dictionaryItemModel.Id = dictionaryItemInfo.Id;
                            // 不相同才修改
                            if (!dictionaryItemInfo.Equals(dictionaryItemModel))
                            {
                                dictionaryItemInfo.Label = dictionaryItemModel.Label;
                                dictionaryItemInfo.Order = dictionaryItemModel.Order;
                                dictionaryItemInfo.UpdatedTime = dateTime;
                                updateDictionaryItemList.Add(dictionaryItemInfo);
                            }
                        }
                        else
                        {
                            dictionaryItemModel.Id = YitIdHelper.NextId();
                            dictionaryItemModel.CreatedTime = dateTime;
                            addDictionaryItemList.Add(dictionaryItemModel);
                        }

                        orderIndex++;
                    }
                }
                else
                {
                    dictionaryTypeModel.Id = YitIdHelper.NextId();
                    dictionaryTypeModel.CreatedTime = dateTime;
                    addDictionaryTypeList.Add(dictionaryTypeModel);

                    var orderIndex = 1;

                    foreach (var enumItem in enumItemList)
                    {
                        var dictionaryItemModel = new DictionaryItemModel
                        {
                            Id = YitIdHelper.NextId(),
                            DictionaryId = dictionaryTypeModel.Id,
                            Label = enumItem.Describe ?? enumItem.Name,
                            Value = enumItem.Value.toString(),
                            Type = DictionaryItemTypeEnum.Primary,
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

            await db.Updateable(updateDictionaryTypeList)
                .ExecuteCommandAsync(cancellationToken);
            await db.Updateable(updateDictionaryItemList)
                .ExecuteCommandAsync(cancellationToken);
            await db.Insertable(addDictionaryTypeList)
                .ExecuteCommandAsync(cancellationToken);
            await db.Insertable(addDictionaryItemList)
                .ExecuteCommandAsync(cancellationToken);

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
                logSb.Append($"同步字典信息成功。新增 {addDictionaryTypeList.Count} 个，更新 {updateDictionaryTypeList.Count} 个，删除 {deleteDictionaryItemList.Count} 个。");
                logSb.Append("\u001b[39m\u001b[22m\u001b[49m");
                Console.WriteLine(logSb.ToString());
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sync dictionary error...");
        }
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