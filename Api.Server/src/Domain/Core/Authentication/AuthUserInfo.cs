

namespace Fast.Core;

/// <summary>
/// <see cref="AuthUserInfo"/> 授权用户信息
/// </summary>
[SuppressSniffer]
public class AuthUserInfo
{
    /// <summary>
    /// 设备类型
    /// </summary>
    public virtual AppEnvironmentEnum DeviceType { get; set; }

    /// <summary>
    /// 设备Id
    /// </summary>
    public virtual string DeviceId { get; set; }

    /// <summary>
    /// WebStock 连接Id
    /// </summary>
    public virtual string ConnectionId { get; set; }

    /// <summary>
    /// 应用编号
    /// </summary>
    public virtual string AppNo { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    public virtual string AppName { get; set; }

    /// <summary>
    /// 账号Id
    /// </summary>
    public virtual long AccountId { get; set; }

    /// <summary>
    /// 账号Key
    /// </summary>
    public virtual string AccountKey { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    public virtual string Mobile { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public virtual string NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    public virtual long TenantId { get; set; }

    /// <summary>
    /// 租户编号
    /// </summary>
    public virtual string TenantNo { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public virtual string TenantName { get; set; }

    /// <summary>
    /// 用户Id/职员Id
    /// </summary>
    public virtual long UserId { get; set; }

    /// <summary>
    /// 用户Key
    /// </summary>
    public virtual string UserKey { get; set; }

    /// <summary>
    /// 账户
    /// </summary>
    public virtual string Account { get; set; }

    /// <summary>
    /// 工号/客户端为OpenId
    /// </summary>
    public virtual string EmployeeNo { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public virtual string EmployeeName { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
    public virtual long? DepartmentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    public virtual string DepartmentName { get; set; }

    /// <summary>
    /// 是否超级管理员
    /// </summary>
    public virtual bool IsSuperAdmin { get; set; }

    /// <summary>
    /// 是否管理员
    /// </summary>
    public virtual bool IsAdmin { get; set; }

    /// <summary>
    /// 最后登录设备
    /// </summary>
    public virtual string LastLoginDevice { get; set; }

    /// <summary>
    /// 最后登录操作系统（版本）
    /// </summary>
    public virtual string LastLoginOS { get; set; }

    /// <summary>
    /// 最后登录浏览器（版本）
    /// </summary>
    public virtual string LastLoginBrowser { get; set; }

    /// <summary>
    /// 最后登录省份
    /// </summary>
    public virtual string LastLoginProvince { get; set; }

    /// <summary>
    /// 最后登录城市
    /// </summary>
    public virtual string LastLoginCity { get; set; }

    /// <summary>
    /// 最后登录Ip
    /// </summary>
    public virtual string LastLoginIp { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public virtual DateTime LastLoginTime { get; set; }

    /// <summary>
    /// 角色Id集合
    /// </summary>
    public virtual List<long> RoleIdList { get; set; } = new();

    /// <summary>
    /// 角色名称集合
    /// </summary>
    public List<string> RoleNameList { get; set; } = new();

    /// <summary>
    /// 菜单编码集合
    /// </summary>
    public virtual List<string> MenuCodeList { get; set; } = new();

    /// <summary>
    /// 按钮编码集合
    /// </summary>
    public virtual List<string> ButtonCodeList { get; set; } = new();
}