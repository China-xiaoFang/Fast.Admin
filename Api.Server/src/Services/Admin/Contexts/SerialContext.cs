using System.Collections.Concurrent;
using Fast.Admin.Entity;
using Fast.Admin.Enum;
using Yitter.IdGenerator;

// ReSharper disable once CheckNamespace
namespace Fast.Admin.Service;

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
    private static readonly AsyncLocal<ConcurrentDictionary<SerialRuleTypeEnum, SerialRuleModel>>
        SerialRuleRequestAsyncLocal =
            new();

    /// <summary>
    /// 系统序号规则缓存
    /// </summary>
    private static ConcurrentDictionary<SerialRuleTypeEnum, SerialRuleModel> SerialRuleList
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
    private static readonly AsyncLocal<ConcurrentDictionary<SerialRuleTypeEnum, SerialSettingModel>>
        SerialSettingRequestAsyncLocal = new();

    /// <summary>
    /// 系统序号配置缓存
    /// </summary>
    private static ConcurrentDictionary<SerialRuleTypeEnum, SerialSettingModel> SerialSettingList
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
    /// <param name="db"><see cref="ISqlSugarClient"/> SqlSugar上下文</param>
    /// <returns></returns>
    public static string GenEmployeeNo(ISqlSugarClient db)
    {
        return GenerateSerialNo(db, SerialRuleTypeEnum.EmployeeNo);
    }

    /// <summary>
    /// 生成序号
    /// </summary>
    /// <param name="db"><see cref="ISqlSugarClient"/> SqlSugar上下文</param>
    /// <param name="ruleType"><see cref="SerialRuleTypeEnum"/> 序号规则类型</param>
    /// <param name="tenantCode"><see cref="string"/>租户编码</param>
    /// <returns></returns>
    public static string GenerateSerialNo(ISqlSugarClient db, SerialRuleTypeEnum ruleType, string tenantCode = null)
    {
        if (!db.Ado.IsAnyTran())
        {
            throw new SqlSugarException("请保证当前上下文在事务中，才能正确的调用此方法！");
        }


        var dateTime = DateTime.Now;

        if (string.IsNullOrWhiteSpace(tenantCode))
        {
            var user = FastContext.GetService<IUser>();
            // 获取租户信息
            var tenantModel = TenantContext.GetTenantSync(user.TenantNo);
            tenantCode = tenantModel.TenantCode;
        }

        // 获取序号规则配置
        var serialRuleModel = SerialRuleList.GetOrAdd(ruleType, key =>
        {
            var result = db.Queryable<SerialRuleModel>()
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
            return db.Queryable<SerialSettingModel>()
                .Where(wh => wh.RuleType == key)
                .Single();
        });

        lock (_lock)
        {
            if (serialSettingModel == null)
            {
                serialSettingModel = new SerialSettingModel
                {
                    SerialSettingId = YitIdHelper.NextId(),
                    RuleType = ruleType,
                    LastSerial = null,
                    LastSerialNo = null,
                    LastTime = null
                };
                serialSettingModel = db.Insertable(serialSettingModel)
                    .ExecuteReturnEntity();
            }

            var curSerialNo = $"{tenantCode}{serialRuleModel.Prefix}";

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

            // 组装年月日
            switch (serialRuleModel.DateType)
            {
                default:
                case SerialDateTypeEnum.Year:
                    curSerialNo += dateTime.ToString("yyyy");
                    if (serialSettingModel.LastTime == null || serialSettingModel.LastTime.Value.Year != dateTime.Year)
                    {
                        lastSerial = 0;
                    }

                    break;
                case SerialDateTypeEnum.Month:
                    curSerialNo += dateTime.ToString("yyyyMM");
                    if (serialSettingModel.LastTime == null
                        || serialSettingModel.LastTime.Value.Year != dateTime.Year
                        || serialSettingModel.LastTime.Value.Month != dateTime.Month)
                    {
                        lastSerial = 0;
                    }

                    break;
                case SerialDateTypeEnum.Day:
                    curSerialNo += dateTime.ToString("yyyyMMdd");
                    if (serialSettingModel.LastTime == null || serialSettingModel.LastTime.Value.Date != dateTime.Date)
                    {
                        lastSerial = 0;
                    }

                    break;
                case SerialDateTypeEnum.Hour:
                    curSerialNo += dateTime.ToString("yyyyMMddHH");
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


            serialSettingModel = db.Updateable(serialSettingModel)
                .UpdateColumns(e => new { e.LastSerial, e.LastSerialNo, e.LastTime })
                .ExecuteReturnEntity();
            SerialSettingList.AddOrUpdate(ruleType, serialSettingModel, (_, _) => serialSettingModel);

            return curSerialNo;
        }
    }
}