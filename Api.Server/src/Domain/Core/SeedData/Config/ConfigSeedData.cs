using Fast.Center.Entity;
using SqlSugar;
using Yitter.IdGenerator;


namespace Fast.Core;

/// <summary>
/// <see cref="ConfigSeedData"/> 配置种子数据
/// </summary>
internal static class ConfigSeedData
{
    /// <summary>
    /// 配置种子数据
    /// </summary>
    /// <param name="db"></param>
    /// <param name="dateTime"><see cref="DateTime"/> 时间</param>
    /// <returns></returns>
    public static async Task SystemConfigSeedData(ISqlSugarClient db, DateTime dateTime)
    {
        await db.Insertable(new List<ConfigModel>
            {
                new()
                {
                    ConfigId = YitIdHelper.NextId(),
                    ConfigCode = ConfigConst.SingleTenantWhenAutoLogin,
                    ConfigName = "单租户自动登录",
                    ConfigValue = "True",
                    Remark = "True：打开（如果只有一个租户，则默认当前租户自动登录）；False：关闭；",
                    CreatedTime = dateTime
                },
                new()
                {
                    ConfigId = YitIdHelper.NextId(),
                    ConfigCode = ConfigConst.SingleLogin,
                    ConfigName = "单点登录",
                    ConfigValue = "True",
                    Remark = "True：打开（多次登录只会保留最后一次登录有效）；False：关闭；",
                    CreatedTime = dateTime
                },
                new()
                {
                    ConfigId = YitIdHelper.NextId(),
                    ConfigCode = ConfigConst.LoginCaptchaOpen,
                    ConfigName = "登录验证码开关",
                    ConfigValue = "True",
                    Remark = "True：打开；False：关闭；",
                    CreatedTime = dateTime
                }
            })
            .ExecuteCommandAsync();
    }
}