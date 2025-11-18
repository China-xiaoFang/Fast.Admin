

namespace Fast.Core;

/// <summary>
/// <see cref="IUser"/> 授权用户信息
/// </summary>
/// <remarks>作用域注册，保证当前请求管道中是唯一的，并且只会加载一次</remarks>
public interface IUser
{
    /// <summary>
    /// 设备类型
    /// </summary>
    AppEnvironmentEnum DeviceType { get; set; }

    /// <summary>
    /// 设备Id
    /// </summary>
    string DeviceId { get; set; }

    /// <summary>
    /// WebStock 连接Id
    /// </summary>
    string ConnectionId { get; set; }

    /// <summary>
    /// 应用编号
    /// </summary>
    string AppNo { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    string AppName { get; set; }

    /// <summary>
    /// 账号Id
    /// </summary>
    long AccountId { get; set; }

    /// <summary>
    /// 账号Key
    /// </summary>
    string AccountKey { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    string Mobile { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    string NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    string Avatar { get; set; }

    /// <summary>
    /// 租户Id
    /// </summary>
    long TenantId { get; set; }

    /// <summary>
    /// 租户编号
    /// </summary>
    string TenantNo { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    string TenantName { get; set; }

    /// <summary>
    /// 用户Id/职员Id
    /// </summary>
    long UserId { get; set; }

    /// <summary>
    /// 用户Key
    /// </summary>
    string UserKey { get; set; }

    /// <summary>
    /// 账户
    /// </summary>
    string Account { get; set; }

    /// <summary>
    /// 工号/客户端为OpenId
    /// </summary>
    string EmployeeNo { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    string EmployeeName { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
    long? DepartmentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    string DepartmentName { get; set; }

    /// <summary>
    /// 是否超级管理员
    /// </summary>
    bool IsSuperAdmin { get; set; }

    /// <summary>
    /// 是否管理员
    /// </summary>
    bool IsAdmin { get; set; }

    /// <summary>
    /// 最后登录设备
    /// </summary>
    string LastLoginDevice { get; set; }

    /// <summary>
    /// 最后登录操作系统（版本）
    /// </summary>
    string LastLoginOS { get; set; }

    /// <summary>
    /// 最后登录浏览器（版本）
    /// </summary>
    string LastLoginBrowser { get; set; }

    /// <summary>
    /// 最后登录省份
    /// </summary>
    string LastLoginProvince { get; set; }

    /// <summary>
    /// 最后登录城市
    /// </summary>
    string LastLoginCity { get; set; }

    /// <summary>
    /// 最后登录Ip
    /// </summary>
    string LastLoginIp { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    DateTime LastLoginTime { get; set; }

    /// <summary>
    /// 角色Id集合
    /// </summary>
    List<long> RoleIdList { get; set; }

    /// <summary>
    /// 角色名称集合
    /// </summary>
    List<string> RoleNameList { get; set; }

    /// <summary>
    /// 菜单编码集合
    /// </summary>
    List<string> MenuCodeList { get; set; }

    /// <summary>
    /// 按钮编码集合
    /// </summary>
    List<string> ButtonCodeList { get; set; }

    /// <summary>
    /// 设置授权用户
    /// </summary>
    /// <param name="authUserInfo"><see cref="AuthUserInfo"/> 授权用户信息</param>
    /// <param name="forceUserInfo"><see cref="bool"/> 强制覆盖用户信息，默认 <c>false</c></param>
    /// <remarks>只会赋值一次</remarks>
    void SetAuthUser(AuthUserInfo authUserInfo, bool forceUserInfo = false);

    /// <summary>
    /// 从缓存中获取授权用户信息
    /// </summary>
    /// <param name="deviceType"><see cref="AppEnvironmentEnum"/> 设备类型</param>
    /// <param name="appNo"><see cref="string"/> 应用编号</param>
    /// <param name="tenantNo"><see cref="string"/> 租户编号</param>
    /// <param name="employeeNo"><see cref="string"/> 工号</param>
    /// <returns></returns>
    Task<AuthUserInfo> GetAuthUserInfo(AppEnvironmentEnum deviceType, string appNo, string tenantNo, string employeeNo);

    /// <summary>
    /// 统一登录
    /// </summary>
    /// <param name="authUserInfo"><see cref="AuthUserInfo"/> 授权用户信息</param>
    /// <returns></returns>
    Task Login(AuthUserInfo authUserInfo);

    /// <summary>
    /// 客户端统一登录
    /// </summary>
    /// <param name="authUserInfo"><see cref="AuthUserInfo"/> 授权用户信息</param>
    /// <returns></returns>
    Task ClientLogin(AuthUserInfo authUserInfo);

    /// <summary>
    /// 机器人登录
    /// </summary>
    /// <remarks>非调度作业请勿使用</remarks>
    /// <returns></returns>
    Task<string> RobotLogin();

    /// <summary>
    /// 刷新授权信息
    /// </summary>
    /// <param name="authUserInfo"><see cref="AuthUserInfo"/> 授权用户信息</param>
    /// <returns></returns>
    Task RefreshAuth(AuthUserInfo authUserInfo);

    /// <summary>
    /// 刷新账号信息
    /// </summary>
    /// <param name="authUserInfo"><see cref="AuthUserInfo"/> 授权用户信息</param>
    /// <returns></returns>
    Task RefreshAccount(AuthUserInfo authUserInfo);

    /// <summary>
    /// 刷新职员信息
    /// </summary>
    /// <param name="authUserInfo"><see cref="AuthUserInfo"/> 授权用户信息</param>
    /// <returns></returns>
    Task RefreshEmployee(AuthUserInfo authUserInfo);

    /// <summary>
    /// 统一退出登录
    /// </summary>
    /// <returns></returns>
    Task Logout();
}