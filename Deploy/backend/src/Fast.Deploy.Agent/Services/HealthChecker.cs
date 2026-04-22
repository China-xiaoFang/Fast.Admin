namespace Fast.Deploy.Agent.Services;

/// <summary>健康检查器 — 对给定 URL 发 HTTP 请求，判断应用是否存活</summary>
public class HealthChecker
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HealthChecker(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> CheckAsync(string url, int timeoutSeconds = 10)
    {
        if (string.IsNullOrWhiteSpace(url)) return false;
        var client = _httpClientFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        try
        {
            var resp = await client.GetAsync(url);
            return resp.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
