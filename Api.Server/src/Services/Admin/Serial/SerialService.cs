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

using Fast.Admin.Entity;
using Fast.Admin.Service.Serial.Dto;
using Fast.Center.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Admin.Service.Serial;

/// <summary>
/// <see cref="SerialService"/> 序号规则服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Center, Name = "serial")]
public class SerialService : IDynamicApplication
{
    private readonly ISqlSugarRepository<SerialRuleModel> _repository;

    public SerialService(ISqlSugarRepository<SerialRuleModel> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// 获取序号规则分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取序号规则分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.Serial.Paged)]
    public async Task<PagedResult<QuerySerialRulePagedOutput>> QuerySerialRulePaged(PagedInput input)
    {
        return await _repository.Entities.LeftJoin<SerialSettingModel>((t1, t2) => t1.SerialRuleId == t2.SerialSettingId)
            .OrderByDescending(t1 => t1.RuleType)
            .Select((t1, t2) => new QuerySerialRulePagedOutput
            {
                SerialRuleId = t1.SerialRuleId,
                RuleType = t1.RuleType,
                Prefix = t1.Prefix,
                DateType = t1.DateType,
                Spacer = t1.Spacer,
                Length = t1.Length,
                LastSerial = t2.LastSerial,
                LastSerialNo = t2.LastSerialNo,
                LastTime = t2.LastTime,
                DepartmentName = t1.DepartmentName,
                CreatedUserName = t1.CreatedUserName,
                CreatedTime = t1.CreatedTime,
                UpdatedUserName = t1.UpdatedUserName,
                UpdatedTime = t1.UpdatedTime,
                RowVersion = t1.RowVersion
            })
            .ToPagedListAsync(input);
    }

    /// <summary>
    /// 获取序号规则详情
    /// </summary>
    /// <param name="serialRuleId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取序号规则详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Serial.Detail)]
    public async Task<QuerySerialRuleDetailOutput> QuerySerialRuleDetail(
        [Required(ErrorMessage = "序号规则Id不能为空")] long? serialRuleId)
    {
        var result = await _repository.Entities.Where(wh => wh.SerialRuleId == serialRuleId)
            .Select(sl => new QuerySerialRuleDetailOutput
            {
                SerialRuleId = sl.SerialRuleId,
                RuleType = sl.RuleType,
                Prefix = sl.Prefix,
                DateType = sl.DateType,
                Spacer = sl.Spacer,
                Length = sl.Length,
                DepartmentName = sl.DepartmentName,
                CreatedUserName = sl.CreatedUserName,
                CreatedTime = sl.CreatedTime,
                UpdatedUserName = sl.UpdatedUserName,
                UpdatedTime = sl.UpdatedTime,
                RowVersion = sl.RowVersion
            })
            .SingleAsync();

        if (result == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        return result;
    }

    /// <summary>
    /// 添加序号规则
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("添加序号规则", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.Serial.Add)]
    public async Task AddSerialRule(AddSerialRuleInput input)
    {
        if (await _repository.AnyAsync(a => a.RuleType == input.RuleType))
        {
            throw new UserFriendlyException("序号规则类型重复！");
        }

        var SerialRuleModel = new SerialRuleModel
        {
            RuleType = input.RuleType,
            Prefix = input.Prefix,
            DateType = input.DateType,
            Spacer = input.Spacer,
            Length = input.Length
        };

        await _repository.InsertAsync(SerialRuleModel);
    }

    /// <summary>
    /// 编辑序号规则
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑序号规则", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Serial.Edit)]
    public async Task EditSerialRule(EditSerialRuleInput input)
    {
        var merchantModel = await _repository.SingleOrDefaultAsync(s => s.SerialRuleId == input.SerialRuleId);
        if (merchantModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        merchantModel.Prefix = input.Prefix;
        merchantModel.DateType = input.DateType;
        merchantModel.Spacer = input.Spacer;
        merchantModel.Length = input.Length;
        merchantModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(merchantModel);
    }
}