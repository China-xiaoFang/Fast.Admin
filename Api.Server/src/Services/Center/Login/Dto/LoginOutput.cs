namespace Fast.Center.Service.Login.Dto;

/// <summary>
/// <see cref="LoginOutput"/> 登录输出
/// </summary>
public class LoginOutput
{
    /// <summary>
    /// 登录状态
    /// </summary>
    public LoginStatusEnum Status { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 账号Key
    /// </summary>
    public string AccountKey { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }

    /// <summary>
    /// 租户集合
    /// </summary>
    public List<LoginTenantOutput> TenantList { get; set; }

    /// <summary>
    /// <see cref="LoginTenantOutput"/> 登录租户输出
    /// </summary>
    public class LoginTenantOutput
    {
        /// <summary>
        /// 用户Key
        /// </summary>
        public string UserKey { get; set; }

        /// <summary>
        /// 租户名称
        /// </summary>
        public string TenantName { get; set; }

        /// <summary>
        /// 租户简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 租户英文名称
        /// </summary>
        /// <remarks>根据 <see cref="TenantName"/> 生成的拼音</remarks>
        public string SpellName { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public EditionEnum Edition { get; set; }

        /// <summary>
        /// LogoUrl
        /// </summary>
        public string LogoUrl { get; set; }

        /// <summary>
        /// 工号
        /// </summary>
        /// <remarks>2024010101 ~ 20240101999</remarks>
        public string EmployeeNo { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 证件照
        /// </summary>
        public string IdPhoto { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public long? DepartmentId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public UserTypeEnum UserType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public CommonStatusEnum Status { get; set; }
    }
}