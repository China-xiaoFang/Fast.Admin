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

using Fast.Center.Domain;
using Fast.Center.Service.ClientUser.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fast.Center.Service.ClientUser;

/// <summary>
/// 客户端用户服务
/// </summary>
[ApiDescriptionSettings(ApiGroupConst.Center, Name = "clientUser")]
public class ClientUserService : IDynamicApplication
{
    private readonly IUser _user;
    private readonly ISqlSugarRepository<ClientUserModel> _repository;

    public ClientUserService(IUser user, ISqlSugarRepository<ClientUserModel> repository)
    {
        _user = user;
        _repository = repository;
    }

    /// <summary>
    /// 获取客户端用户分页列表
    /// </summary>
    [HttpPost]
    [ApiInfo("获取客户端用户分页列表", HttpRequestActionEnum.Paged)]
    [Permission(PermissionConst.ClientUser.Paged)]
    public async Task<PagedResult<QueryClientUserPagedOutput>> QueryClientUserPaged(QueryClientUserPagedInput input)
    {
        return await _repository.Entities.WhereIF(input.AppId != null, wh => wh.AppId == input.AppId)
            .WhereIF(input.UserType != null, wh => wh.UserType == input.UserType)
            .WhereIF(input.Sex != null, wh => wh.Sex == input.Sex)
            .OrderByIF(input.IsOrderBy, ob => ob.CreatedTime, OrderByType.Desc)
            .Select(sl => new QueryClientUserPagedOutput
            {
                UserId = sl.UserId,
                AppId = sl.AppId,
                UserType = sl.UserType,
                Mobile = sl.Mobile,
                OpenId = sl.OpenId,
                UnionId = sl.UnionId,
                NickName = sl.NickName,
                Avatar = sl.Avatar,
                Sex = sl.Sex,
                LastLoginDevice = sl.LastLoginDevice,
                LastLoginOS = sl.LastLoginOS,
                LastLoginBrowser = sl.LastLoginBrowser,
                LastLoginProvince = sl.LastLoginProvince,
                LastLoginCity = sl.LastLoginCity,
                LastLoginIp = sl.LastLoginIp,
                LastLoginTime = sl.LastLoginTime,
                MobileUpdateTime = sl.MobileUpdateTime,
                CreatedTime = sl.CreatedTime,
                UpdatedTime = sl.UpdatedTime,
                RowVersion = sl.RowVersion
            })
            .ToPagedListAsync(input);
    }

    /// <summary>
    /// 获取客户端用户详情
    /// </summary>
    [HttpGet]
    [ApiInfo("获取客户端用户详情", HttpRequestActionEnum.Query)]
    public async Task<QueryClientUserDetailOutput> QueryClientUserDetail()
    {
        // 查询应用信息
        var applicationModel = await ApplicationContext.GetApplication(GlobalContext.Origin);

        if (applicationModel.AppType != GlobalContext.DeviceType)
        {
            throw new UserFriendlyException("应用类型不匹配！");
        }

        // 获取当前客户端用户信息
        var result = await _repository.Entities.Where(wh => wh.AppId == applicationModel.AppId)
            .Where(wh => wh.UserId == _user.ClientUserId)
            .Select(sl => new QueryClientUserDetailOutput
            {
                UserId = sl.UserId,
                UserType = sl.UserType,
                Mobile = sl.Mobile,
                OpenId = sl.OpenId,
                UnionId = sl.UnionId,
                NickName = sl.NickName,
                Avatar = sl.Avatar,
                Sex = sl.Sex,
                LastLoginDevice = sl.LastLoginDevice,
                LastLoginOS = sl.LastLoginOS,
                LastLoginBrowser = sl.LastLoginBrowser,
                LastLoginProvince = sl.LastLoginProvince,
                LastLoginCity = sl.LastLoginCity,
                LastLoginIp = sl.LastLoginIp,
                LastLoginTime = sl.LastLoginTime,
                CreatedTime = sl.CreatedTime,
                UpdatedTime = sl.UpdatedTime,
                MobileUpdateTime = sl.MobileUpdateTime,
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
    /// 编辑客户端用户
    /// </summary>
    [HttpPost]
    [ApiInfo("编辑客户端用户", HttpRequestActionEnum.Edit)]
    [Permission(PermissionConst.ClientService)]
    public async Task EditClientUser(EditClientUserInput input)
    {
        // 查询应用信息
        var applicationModel = await ApplicationContext.GetApplication(GlobalContext.Origin);

        if (applicationModel.AppType != GlobalContext.DeviceType)
        {
            throw new UserFriendlyException("应用类型不匹配！");
        }

        var userModel = await _repository.Entities.Where(wh => wh.AppId == applicationModel.AppId)
            .Where(wh => wh.UserId == _user.ClientUserId)
            .SingleAsync();

        if (userModel == null)
        {
            throw new UserFriendlyException("数据不存在！");
        }

        if (!string.IsNullOrWhiteSpace(input.Mobile) && userModel.Mobile != input.Mobile)
        {
            // 检查手机号是否已存在账号
            if (await _repository.AnyAsync(a => a.Mobile == input.Mobile && a.UserId != userModel.UserId))
            {
                throw new UserFriendlyException("该手机号已被其他用户绑定，请更换手机号！");
            }

            userModel.Mobile = input.Mobile;
            userModel.MobileUpdateTime = DateTime.Now;
        }

        userModel.NickName = input.NickName;
        userModel.Avatar = input.Avatar;
        userModel.Sex = input.Sex;
        userModel.RowVersion = input.RowVersion;

        await _repository.UpdateAsync(userModel);

        // 刷新缓存
        await _user.RefreshClientUser(new RefreshClientUserDto
        {
            DeviceType = _user.DeviceType,
            AppNo = _user.AppNo,
            Mobile = userModel.Mobile,
            NickName = userModel.NickName,
            Avatar = userModel.Avatar,
            TenantNo = _user.TenantNo,
            ClientUserOpenId = userModel.OpenId
        });
    }
}