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

using System.ComponentModel.DataAnnotations;
using Fast.Common;
using Fast.FastCloud.Service.Platform;
using Fast.FastCloud.Service.Platform.Dto;
using Fast.SqlSugar;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable once CheckNamespace
namespace Fast.FastCloud.Api;

/// <summary>
/// <see cref="PlatformApplication"/> 平台
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.FastCloud, Name = "platform", Order = 100)]
public class PlatformApplication : IDynamicApplication
{
    private readonly IPlatformService _platformService;

    public PlatformApplication(IPlatformService platformService)
    {
        _platformService = platformService;
    }

    /// <summary>
    /// 平台选择器
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("平台选择器", HttpRequestActionEnum.Paged)]
    [Permission("Platform:Page")]
    public async Task<PagedResult<ElSelectorOutput<long>>> PlatformSelector(PagedInput input)
    {
        return await _platformService.PlatformSelector(input);
    }

    /// <summary>
    /// 获取平台分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取平台分页列表", HttpRequestActionEnum.Paged)]
    [Permission("Platform:Page")]
    public async Task<PagedResult<QueryPlatformPagedOutput>> QueryPlatformPaged(PagedInput input)
    {
        return await _platformService.QueryPlatformPaged(input);
    }

    /// <summary>
    /// 获取平台详情
    /// </summary>
    /// <param name="platformId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取平台详情", HttpRequestActionEnum.Query)]
    [Permission("Platform:Detail")]
    public async Task<QueryPlatformDetailOutput> QueryPlatformDetail([LongRequired(ErrorMessage = "平台Id不能为空")] long? platformId)
    {
        return await _platformService.QueryPlatformDetail(platformId!.Value);
    }

    /// <summary>
    /// 获取平台续费记录分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取平台续费记录分页列表", HttpRequestActionEnum.Paged)]
    [Permission("Platform:Detail")]
    public async Task<PagedResult<QueryPlatformRenewalRecordPagedOutput>> QueryPlatformRenewalRecord(
        QueryPlatformRenewalRecordInput input)
    {
        return await _platformService.QueryPlatformRenewalRecord(input);
    }

    /// <summary>
    /// 初次开通平台
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("初次开通平台", HttpRequestActionEnum.Add)]
    [Permission("Platform:Activation")]
    public async Task FirstActivationPlatform(FirstActivationPlatformInput input)
    {
        await _platformService.FirstActivationPlatform(input);
    }

    /// <summary>
    /// 编辑平台
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑平台", HttpRequestActionEnum.Edit)]
    [Permission("Platform:Edit")]
    public async Task EditPlatform(EditPlatformInput input)
    {
        await _platformService.EditPlatform(input);
    }

    /// <summary>
    /// 启用/禁用平台
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("启用/禁用平台", HttpRequestActionEnum.Edit)]
    [Permission("Platform:ChangeStatus")]
    public async Task ChangePlatformStatus(ChangePlatformStatusInput input)
    {
        await _platformService.ChangePlatformStatus(input);
    }
}