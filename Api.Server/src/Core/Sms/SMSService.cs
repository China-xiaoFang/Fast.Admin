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

using System.Text.RegularExpressions;
using AlibabaCloud.SDK.Dysmsapi20180501;
using AlibabaCloud.SDK.Dysmsapi20180501.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fast.Core;

/// <summary>
/// <see cref="ISmsService"/> 默认实现
/// </summary>
public class SMSService : ISmsService
{
    /// <summary>
    /// 缓存
    /// </summary>
    private readonly ICache<CenterCCL> _centerCache;

    /// <summary>
    /// 短信配置
    /// </summary>
    private readonly SmsSettingsOptions _smsSettingsOptions;

    /// <summary>
    /// 日志
    /// </summary>
    private readonly ILogger _logger;

    /// <summary>
    /// 客户端
    /// </summary>
    private readonly Client _client;

    /// <summary>
    /// 初始化短信服务
    /// </summary>
    public SMSService(IOptions<SmsSettingsOptions> options, ICache<CenterCCL> centerCache, ILogger<ISmsService> logger)
    {
        _smsSettingsOptions = options.Value;
        _centerCache = centerCache;
        _logger = logger;
        _client = new Client(new AlibabaCloud.OpenApiClient.Models.Config
        {
            AccessKeyId = _smsSettingsOptions.AccessKeyId,
            AccessKeySecret = _smsSettingsOptions.AccessKeySecret,
            Endpoint = "dysmsapi.aliyuncs.com"
        });
    }

    /// <summary>
    /// 验证码缓存Dto
    /// </summary>
    private class VerificationCodeCacheDto
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerificationCode { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 错误次数
        /// </summary>
        /// <remarks>超过5次自动失效</remarks>
        public int ErrorCount { get; set; }
    }

    /// <inheritdoc/>
    public async Task SendVerificationCode(SmsTypeEnum smsType, string mobile)
    {
        if (string.IsNullOrWhiteSpace(mobile) || !new Regex(RegexConst.Mobile).IsMatch(mobile))
        {
            throw new UserFriendlyException("手机号码不正确！");
        }

        mobile = mobile.Trim();

        // 获取缓存Key
        var cacheKey = CacheConst.GetCacheKey(CacheConst.Sms, smsType.ToString(), mobile);
        var dto = await _centerCache.GetAsync<VerificationCodeCacheDto>(cacheKey);
        if (dto != null && dto.SendTime.AddSeconds(60) > DateTime.Now)
        {
            throw new UserFriendlyException("验证码发送过于频繁，请稍后重试！");
        }

        // 生成验证码
        dto ??= new VerificationCodeCacheDto();
        dto.VerificationCode = VerificationUtil.GenNumVerCode();
        dto.SendTime = DateTime.Now;
        dto.ErrorCount = 0;

        var response = await _client.SendMessageWithTemplateAsync(new SendMessageWithTemplateRequest
        {
            To = $"86{mobile}",
            From = _smsSettingsOptions.SignName,
            TemplateCode = "",
            TemplateParam = $$"""
                              { "code": "{{dto.VerificationCode}}" }
                              """
        });

        if (!string.Equals(response.Body.ResponseCode, "OK", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogError(
                $"短信验证码发送失败。ResponseCode: {response.Body.ResponseCode}, ResponseDescription: {response.Body.ResponseDescription}, RequestId: {response.Body.RequestId}");
            throw new UserFriendlyException("短信验证码发送失败，请稍后重试！");
        }

        await _centerCache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));
    }

    /// <inheritdoc/>
    public async Task SendVerificationCode(SmsTypeEnum smsType, string mobile, string verificationCode)
    {
        if (string.IsNullOrWhiteSpace(mobile) || !new Regex(RegexConst.Mobile).IsMatch(mobile))
        {
            throw new UserFriendlyException("手机号码不正确！");
        }

        mobile = mobile.Trim();

        // 获取缓存Key
        var cacheKey = CacheConst.GetCacheKey(CacheConst.Sms, smsType.ToString(), mobile);
        var dto = await _centerCache.GetAsync<VerificationCodeCacheDto>(cacheKey);
        if (dto is not {ErrorCount: < 5})
        {
            throw new UserFriendlyException("验证码无效或已过期！");
        }

        if (!string.Equals(dto.VerificationCode, verificationCode, StringComparison.Ordinal))
        {
            dto.ErrorCount++;
            await _centerCache.SetAsync(cacheKey, dto);
            throw new UserFriendlyException("验证码无效或已过期！");
        }

        await _centerCache.DelAsync(cacheKey);
    }
}