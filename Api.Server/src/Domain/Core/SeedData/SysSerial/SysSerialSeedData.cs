using Fast.Center.Entity;
using Fast.Center.Enum;
using SqlSugar;
using Yitter.IdGenerator;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="SysSerialSeedData"/> 系统序号种子数据
/// </summary>
internal static class SysSerialSeedData
{
    /// <summary>
    /// 系统序号种子数据
    /// </summary>
    /// <param name="db"></param>
    /// <returns></returns>
    public static async Task SeedData(ISqlSugarClient db)
    {
        await db.Insertable(new List<SysSerialRuleModel>
            {
                new()
                {
                    SerialRuleId = YitIdHelper.NextId(),
                    RuleType = SysSerialRuleTypeEnum.AppNo,
                    Prefix = "App",
                    DateType = SysSerialDateTypeEnum.Year,
                    Spacer = SysSerialSpacerEnum.None,
                    Length = 2
                },
                new()
                {
                    SerialRuleId = YitIdHelper.NextId(),
                    RuleType = SysSerialRuleTypeEnum.TenantNo,
                    Prefix = "Tnt",
                    DateType = SysSerialDateTypeEnum.Month,
                    Spacer = SysSerialSpacerEnum.None,
                    Length = 2
                }
            })
            .ExecuteCommandAsync();
    }
}