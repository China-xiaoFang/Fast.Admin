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

using System.Collections.Concurrent;
using Fast.Admin.Entity;
using Fast.Admin.Enum;
using Yitter.IdGenerator;

namespace Fast.Admin.Service.Contexts;

/// <summary>
/// <see cref="SerialContext"/> 序号规则上下文
/// </summary>
[SuppressSniffer]
public class SerialContext
{
    /// <summary>
    /// 线程锁
    /// </summary>
    private static readonly object _lock = new();

    /// <summary>
    /// 请求作用域系统序号规则缓存
    /// </summary>
    internal static readonly AsyncLocal<ConcurrentDictionary<SerialRuleTypeEnum, SerialRuleModel>> SerialRuleRequestAsyncLocal =
        new();

    /// <summary>
    /// 系统序号规则缓存
    /// </summary>
    internal static ConcurrentDictionary<SerialRuleTypeEnum, SerialRuleModel> SerialRuleList
    {
        get
        {
            SerialRuleRequestAsyncLocal.Value ??= new ConcurrentDictionary<SerialRuleTypeEnum, SerialRuleModel>();
            return SerialRuleRequestAsyncLocal.Value;
        }
    }

    /// <summary>
    /// 请求作用域系统序号配置缓存
    /// </summary>
    internal static readonly AsyncLocal<ConcurrentDictionary<SerialRuleTypeEnum, SerialSettingModel>>
        SerialSettingRequestAsyncLocal = new();

    /// <summary>
    /// 系统序号配置缓存
    /// </summary>
    internal static ConcurrentDictionary<SerialRuleTypeEnum, SerialSettingModel> SerialSettingList
    {
        get
        {
            SerialSettingRequestAsyncLocal.Value ??= new ConcurrentDictionary<SerialRuleTypeEnum, SerialSettingModel>();
            return SerialSettingRequestAsyncLocal.Value;
        }
    }

    /// <summary>
    /// 生成工号
    /// </summary>
    /// <returns></returns>
    public static string GenEmployeeNo<TEntity>(ISqlSugarRepository<TEntity> repository) where TEntity : class, new()
    {
        return GenerateSerialNo(repository, SerialRuleTypeEnum.EmployeeNo, null);
    }

    /// <summary>
    /// 生成序号
    /// </summary>
    /// <param name="repository"><see cref="ISqlSugarRepository{TEntity}"/> 仓储上下文</param>
    /// <param name="ruleType"><see cref="SerialRuleTypeEnum"/> 序号规则类型</param>
    /// <param name="defaultPrefix"><see cref="string"/> 默认序号</param>
    /// <returns></returns>
    internal static string GenerateSerialNo<TEntity>(ISqlSugarRepository<TEntity> repository, SerialRuleTypeEnum ruleType,
        string defaultPrefix) where TEntity : class, new()
    {
        if (!repository.Ado.IsAnyTran())
        {
            throw new SqlSugarException("请保证当前上下文在事务中，才能正确的调用此方法！");
        }

        var user = FastContext.GetService<IUser>();

        var dateTime = DateTime.Now;

        // 获取租户信息
        var tenantModel = TenantContext.GetTenantSync(user.TenantNo);

        // 获取序号规则配置
        var serialRuleModel = SerialRuleList.GetOrAdd(ruleType, key =>
        {
            var result = repository.Queryable<SerialRuleModel>()
                .Where(wh => wh.RuleType == key)
                .Single();

            if (result == null)
            {
                throw new UserFriendlyException($"未能找到【{key.GetDescription()}】规则配置！");
            }

            return result;
        });

        // 获取序号配置
        var serialSettingModel = SerialSettingList.GetOrAdd(ruleType, key =>
        {
            return repository.Queryable<SerialSettingModel>()
                .Where(wh => wh.RuleType == key)
                .Single();
        });

        lock (_lock)
        {
            if (serialSettingModel == null)
            {
                serialSettingModel = new SerialSettingModel
                {
                    Id = YitIdHelper.NextId(),
                    RuleType = ruleType,
                    LastSerial = null,
                    LastSerialNo = null,
                    LastTime = null
                };
                serialSettingModel = repository.Insertable(serialSettingModel)
                    .ExecuteReturnEntity();
            }

            var curSerialNo = $"{tenantModel.TenantCode}{serialRuleModel.Prefix}";

            // 拼接分隔符
            switch (serialRuleModel.Spacer)
            {
                default:
                case SerialSpacerEnum.None:
                    break;
                case SerialSpacerEnum.Underscore:
                    curSerialNo += "_";
                    break;
                case SerialSpacerEnum.Hyphen:
                    curSerialNo += "-";
                    break;
                case SerialSpacerEnum.Dot:
                    curSerialNo += ".";
                    break;
            }

            var lastSerial = serialSettingModel.LastSerial.GetValueOrDefault();

            switch (serialRuleModel.DateType)
            {
                default:
                case SerialDateTypeEnum.Year:
                    if (serialSettingModel.LastTime == null || serialSettingModel.LastTime.Value.Year != dateTime.Year)
                    {
                        lastSerial = 0;
                    }

                    break;
                case SerialDateTypeEnum.Month:
                    if (serialSettingModel.LastTime == null
                        || serialSettingModel.LastTime.Value.Year != dateTime.Year
                        || serialSettingModel.LastTime.Value.Month != dateTime.Month)
                    {
                        lastSerial = 0;
                    }

                    break;
                case SerialDateTypeEnum.Day:
                    if (serialSettingModel.LastTime == null || serialSettingModel.LastTime.Value.Date != dateTime.Date)
                    {
                        lastSerial = 0;
                    }

                    break;
                case SerialDateTypeEnum.Hour:
                    if (serialSettingModel.LastTime == null
                        || serialSettingModel.LastTime.Value.Year != dateTime.Year
                        || serialSettingModel.LastTime.Value.Month != dateTime.Month
                        || serialSettingModel.LastTime.Value.Day != dateTime.Day
                        || serialSettingModel.LastTime.Value.Hour != dateTime.Hour)
                    {
                        lastSerial = 0;
                    }

                    break;
            }

            var curSerial = lastSerial + 1;

            // 组装左侧的0
            var serialLength = $"D{serialRuleModel.Length}";
            curSerialNo += curSerial.ToString(serialLength);

            serialSettingModel.LastSerial = curSerial;
            serialSettingModel.LastSerialNo = curSerialNo;
            serialSettingModel.LastTime = dateTime;


            serialSettingModel = repository.Updateable(serialSettingModel)
                .ExecuteReturnEntity();
            SerialSettingList.AddOrUpdate(ruleType, serialSettingModel, (_, _) => serialSettingModel);

            return curSerialNo;
        }
    }
}