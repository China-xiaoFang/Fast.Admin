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

using Fast.Center.Entity;
using Fast.Center.Enum;
using Fast.Center.Service.File.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Center.Service.File;

/// <summary>
/// <see cref="FileService"/> 文件服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.File, Name = "file", Order = 997)]
public class FileService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<FileModel> _repository;

    public FileService(IUser user, ISqlSugarRepository<FileModel> repository)
    {
        _user = user;
        _repository = repository;
    }

    /// <summary>
    /// 获取文件分页列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiInfo("获取文件分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.FilePaged)]
    public async Task<PagedResult<QueryFilePagedOutput>> QueryFilePaged(QueryFilePagedInput input)
    {
        var tenantModel = await TenantContext.GetTenant(_user.TenantNo);

        var queryable = _repository.Entities.LeftJoin<TenantModel>((t1, t2) => t1.TenantId == t2.TenantId);

        if (tenantModel.TenantType == TenantTypeEnum.System)
        {
            queryable = queryable.ClearFilter<IBaseTEntity>()
                .WhereIF(input.TenantId != null, t1 => t1.TenantId == input.TenantId);
        }
        else if (!_user.IsAdmin)
        {
            queryable = queryable.Where(t1 => t1.CreatedUserId == _user.UserId);
        }

        return await queryable.SelectMergeTable((t1, t2) => new QueryFilePagedOutput
            {
                FileId = t1.FileId,
                FileObjectName = t1.FileObjectName,
                FileOriginName = t1.FileOriginName,
                FileSuffix = t1.FileSuffix,
                FileMimeType = t1.FileMimeType,
                FileSizeKb = t1.FileSizeKb,
                FilePath = t1.FilePath,
                FileLocation = t1.FileLocation,
                FileHash = t1.FileHash,
                UploadDevice = t1.UploadDevice,
                UploadOS = t1.UploadOS,
                UploadBrowser = t1.UploadBrowser,
                UploadProvince = t1.UploadProvince,
                UploadCity = t1.UploadCity,
                UploadIp = t1.UploadIp,
                CreatedUserName = t1.CreatedUserName,
                CreatedTime = t1.CreatedTime,
                TenantName = t2.TenantName
            })
            .OrderByIF(input.IsOrderBy, ob => ob.CreatedTime, OrderByType.Desc)
            .ToPagedListAsync(input);
    }
}