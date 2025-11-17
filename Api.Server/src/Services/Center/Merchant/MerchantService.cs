using Fast.Center.Entity;
using Fast.Center.Service.Merchant.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Center.Service.Merchant;

/// <summary>
/// <see cref="MerchantService"/> 商户号服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Center, Name = "merchant")]
public class MerchantService : IDynamicApplication
{
    private readonly ISqlSugarRepository<MerchantModel> _repository;

    public MerchantService(ISqlSugarRepository<MerchantModel> repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// 获取商户号分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取商户号分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.Merchant.Paged)]
    public async Task<PagedResult<QueryMerchantPagedOutput>> QueryMerchantPaged(QueryMerchantPagedInput input)
    {
        return await _repository.Entities
            .WhereIF(input.MerchantType != null, wh => wh.MerchantType == input.MerchantType)
            .OrderByDescending(ob => ob.CreatedTime)
            .Select(sl => new QueryMerchantPagedOutput
            {
                MerchantId = sl.MerchantId,
                MerchantType = sl.MerchantType,
                MerchantNo = sl.MerchantNo,
                Remark = sl.Remark,
                CreatedUserName = sl.CreatedUserName,
                CreatedTime = sl.CreatedTime,
                UpdatedUserName = sl.UpdatedUserName,
                UpdatedTime = sl.UpdatedTime
            })
            .ToPagedListAsync(input);
    }

    /// <summary>
    /// 获取商户号详情
    /// </summary>
    /// <param name="merchantId"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiInfo("获取商户号详情", HttpRequestActionEnum.Query)]
    [Permission(PermissionConst.Merchant.Detail)]
    public async Task<QueryMerchantDetailOutput> QueryMerchantDetail(
        [Required(ErrorMessage = "商户号Id不能为空")] long? merchantId)
    {
        var result = await _repository.Entities.Where(wh => wh.MerchantId == merchantId)
            .Select(sl => new QueryMerchantDetailOutput
            {
                MerchantId = sl.MerchantId,
                MerchantType = sl.MerchantType,
                MerchantNo = sl.MerchantNo,
                MerchantSecret = sl.MerchantSecret,
                PublicSerialNum = sl.PublicSerialNum,
                PublicKey = sl.PublicKey,
                CertSerialNum = sl.CertSerialNum,
                Cert = sl.Cert,
                CertPrivateKey = sl.CertPrivateKey,
                Remark = sl.Remark,
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
    /// 添加商户号
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("添加商户号", HttpRequestActionEnum.Add)]
    [Permission(PermissionConst.Merchant.Add)]
    public async Task AddMerchant(AddMerchantInput input)
    {
        if (await _repository.AnyAsync(a => a.MerchantNo == input.MerchantNo))
        {
            throw new UserFriendlyException("商户号重复！");
        }

        var merchantModel = new MerchantModel
        {
            MerchantType = input.MerchantType,
            MerchantNo = input.MerchantNo,
            MerchantSecret = input.MerchantSecret,
            PublicSerialNum = input.PublicSerialNum,
            PublicKey = input.PublicKey,
            CertSerialNum = input.CertSerialNum,
            Cert = input.Cert,
            CertPrivateKey = input.CertPrivateKey,
            Remark = input.Remark
        };

        await _repository.InsertAsync(merchantModel);
    }

    /// <summary>
    /// 编辑商户号
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("编辑商户号", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.Merchant.Edit)]
    public async Task EditMerchant(EditMerchantInput input)
    {
        if (await _repository.AnyAsync(a => a.MerchantNo == input.MerchantNo && a.MerchantId != input.MerchantId))
        {
            throw new UserFriendlyException("商户号重复！");
        }

        var merchantModel = await _repository.SingleOrDefaultAsync(s => s.MerchantId == input.MerchantId);
        if (merchantModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        merchantModel.MerchantType = input.MerchantType;
        merchantModel.MerchantNo = input.MerchantNo;
        merchantModel.MerchantSecret = input.MerchantSecret;
        merchantModel.PublicSerialNum = input.PublicSerialNum;
        merchantModel.PublicKey = input.PublicKey;
        merchantModel.CertSerialNum = input.CertSerialNum;
        merchantModel.Cert = input.Cert;
        merchantModel.CertPrivateKey = input.CertPrivateKey;
        merchantModel.Remark = input.Remark;
        merchantModel.RowVersion = input.RowVersion;

        await _repository.Ado.UseTranAsync(async () =>
        {
            await _repository.UpdateAsync(merchantModel);
            await _repository.Updateable<ApplicationOpenIdModel>()
                .SetColumns(_ => new ApplicationOpenIdModel { WeChatMerchantNo = merchantModel.MerchantNo })
                .Where(wh => wh.WeChatMerchantId == merchantModel.MerchantId)
                .ExecuteCommandAsync();
            await _repository.Updateable<ApplicationOpenIdModel>()
                .SetColumns(_ => new ApplicationOpenIdModel { AlipayMerchantNo = merchantModel.MerchantNo })
                .Where(wh => wh.AlipayMerchantId == merchantModel.MerchantId)
                .ExecuteCommandAsync();
        });

        // 删除缓存
        await MerchantContext.DeleteMerchant(merchantModel.MerchantNo);
    }

    /// <summary>
    /// 删除商户号
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("删除商户号", HttpRequestActionEnum.Delete)]
    [Permission(PermissionConst.Merchant.Delete)]
    public async Task DeleteMerchant(MerchantIdInput input)
    {
        if (await _repository.Queryable<ApplicationOpenIdModel>()
                .AnyAsync(a => a.WeChatMerchantId == input.MerchantId || a.AlipayMerchantId == input.MerchantId))
        {
            throw new UserFriendlyException("商户号存在绑定应用，无法删除！");
        }

        var merchantModel = await _repository.SingleOrDefaultAsync(s => s.MerchantId == input.MerchantId);
        if (merchantModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        await _repository.DeleteAsync(merchantModel);
        // 删除缓存
        await MerchantContext.DeleteMerchant(merchantModel.MerchantNo);
    }
}