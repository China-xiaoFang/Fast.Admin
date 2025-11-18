

namespace Fast.Core;

/// <summary>
/// <see cref="GlobalContext"/> 系统通用上下文
/// </summary>
[SuppressSniffer]
public class GlobalContext
{
    /// <summary>
    /// 来源
    /// </summary>
    public static string Origin
    {
        get
        {
            string result;
            var httpContext = FastContext.HttpContext;
            if (httpContext.IsWebSocketRequest())
            {
                result = httpContext.Request.Query[HttpHeaderConst.Origin]
                    .ToString()
                    .UrlDecode();
            }
            else
            {
                result = httpContext.Request.Headers[HttpHeaderConst.Origin]
                    .ToString()
                    .UrlDecode();
            }

            if (!string.IsNullOrWhiteSpace(result))
                return result;

            throw new UserFriendlyException("未知的设备信息！");
        }
    }

    /// <summary>
    /// 设备类型
    /// </summary>
    public static AppEnvironmentEnum DeviceType
    {
        get
        {
            string result;
            var httpContext = FastContext.HttpContext;
            if (httpContext.IsWebSocketRequest())
            {
                result = FastContext.HttpContext.Request.Query[HttpHeaderConst.DeviceType]
                    .ToString()
                    .UrlDecode();
            }
            else
            {
                result = FastContext.HttpContext.Request.Headers[HttpHeaderConst.DeviceType]
                    .ToString()
                    .UrlDecode();
            }

            if (!string.IsNullOrWhiteSpace(result)
                && Enum.TryParse<AppEnvironmentEnum>(result, true, out var environment))
                return environment;

            throw new UserFriendlyException("未知的设备信息！");
        }
    }

    /// <summary>
    /// 设备Id
    /// </summary>
    public static string DeviceId
    {
        get
        {
            string result;
            var httpContext = FastContext.HttpContext;
            if (httpContext.IsWebSocketRequest())
            {
                result = httpContext.Request.Query[HttpHeaderConst.DeviceId]
                    .ToString()
                    .UrlDecode();
            }
            else
            {
                result = httpContext.Request.Headers[HttpHeaderConst.DeviceId]
                    .ToString()
                    .UrlDecode();
            }

            if (!string.IsNullOrWhiteSpace(result))
                return result;

            throw new UserFriendlyException("未知的设备信息！");
        }
    }

    /// <summary>
    /// 是否为Web端
    /// </summary>
    public static bool IsWeb = (DeviceType & AppEnvironmentEnum.Web) != 0;

    /// <summary>
    /// 是否为桌面端
    /// </summary>
    public static bool IsDesktop = (DeviceType & AppEnvironmentEnum.Desktop) != 0;

    /// <summary>
    /// 是否为移动端
    /// </summary>
    public static bool IsMobile = (DeviceType & AppEnvironmentEnum.MobileThree) != 0;
}