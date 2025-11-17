namespace Fast.Admin.Service.Auth.Dto;

/// <summary>
/// <see cref="GetLoginUserInfoOutput"/> 获取登录用户信息输出
/// </summary>
public class GetLoginUserInfoOutput
{
    /// <summary>
    /// 账号Key
    /// </summary>
    public string AccountKey { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 租户编号
    /// </summary>
    public string TenantNo { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public string TenantName { get; set; }

    /// <summary>
    /// 用户Key
    /// </summary>
    public string UserKey { get; set; }

    /// <summary>
    /// 账户
    /// </summary>
    public string Account { get; set; }

    /// <summary>
    /// 工号
    /// </summary>
    public string EmployeeNo { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string EmployeeName { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
    public long? DepartmentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    public string DepartmentName { get; set; }

    /// <summary>
    /// 是否超级管理员
    /// </summary>
    public bool IsSuperAdmin { get; set; }

    /// <summary>
    /// 是否管理员
    /// </summary>
    public bool IsAdmin { get; set; }

    /// <summary>
    /// 角色名称集合
    /// </summary>
    public List<string> RoleNameList { get; set; } = [];

    /// <summary>
    /// 按钮编码集合
    /// </summary>
    public List<string> ButtonCodeList { get; set; } = [];

    /// <summary>
    /// 菜单集合
    /// </summary>
    /// <remarks>第一层是模块</remarks>
    public List<AuthModuleInfoDto> MenuList { get; set; } = [];
}