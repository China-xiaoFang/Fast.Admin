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

// ReSharper disable once CheckNamespace

namespace Fast.Core;

/// <summary>
/// <see cref="PermissionConst"/> 权限常量
/// </summary>
[SuppressSniffer]
public class PermissionConst
{
    /// <summary>客户端服务</summary>
    public const string ClientService = "ClientService";

    /// <summary>接口列表</summary>
    public const string ApiPaged = "Api:Paged";

    /// <summary>Swagger</summary>
    public const string ApiSwagger = "Api:Swagger";

    /// <summary>Knife4j</summary>
    public const string ApiKnife4j = "Api:Knife4j";

    /// <summary><see cref="Menu"/> 菜单</summary>
    public class Menu
    {
        /// <summary>菜单列表</summary>
        public const string Paged = "Menu:Paged";

        /// <summary>菜单详情</summary>
        public const string Detail = "Menu:Detail";

        /// <summary>菜单新增</summary>
        public const string Add = "Menu:Add";

        /// <summary>菜单编辑</summary>
        public const string Edit = "Menu:Edit";

        /// <summary>菜单删除</summary>
        public const string Delete = "Menu:Delete";

        /// <summary>菜单状态更改</summary>
        public const string Status = "Menu:Status";
    }

    /// <summary><see cref="Table"/> 表格</summary>
    public class Table
    {
        /// <summary>表格列表</summary>
        public const string Paged = "Table:Paged";

        /// <summary>表格详情</summary>
        public const string Detail = "Table:Detail";

        /// <summary>表格新增</summary>
        public const string Add = "Table:Add";

        /// <summary>表格编辑</summary>
        public const string Edit = "Table:Edit";

        /// <summary>表格删除</summary>
        public const string Delete = "Table:Delete";
    }

    /// <summary><see cref="Dictionary"/> 字典</summary>
    public class Dictionary
    {
        /// <summary>字典列表</summary>
        public const string Paged = "Dictionary:Paged";

        /// <summary>字典详情</summary>
        public const string Detail = "Dictionary:Detail";

        /// <summary>字典新增</summary>
        public const string Add = "Dictionary:Add";

        /// <summary>字典编辑</summary>
        public const string Edit = "Dictionary:Edit";

        /// <summary>字典删除</summary>
        public const string Delete = "Dictionary:Delete";

        /// <summary>字典状态更改</summary>
        public const string Status = "Dictionary:Status";
    }

    /// <summary>密码映射列表</summary>
    public const string PasswordMapPaged = "PasswordMap:Paged";

    /// <summary><see cref="Tenant"/> 租户</summary>
    public class Tenant
    {
        /// <summary>租户列表</summary>
        public const string Paged = "Tenant:Paged";

        /// <summary>租户详情</summary>
        public const string Detail = "Tenant:Detail";

        /// <summary>租户新增</summary>
        public const string Add = "Tenant:Add";

        /// <summary>租户编辑</summary>
        public const string Edit = "Tenant:Edit";

        /// <summary>租户状态更改</summary>
        public const string Status = "Tenant:Status";
    }

    /// <summary><see cref="Account"/> 账号</summary>
    public class Account
    {
        /// <summary>账号列表</summary>
        public const string Paged = "Account:Paged";

        /// <summary>账号详情</summary>
        public const string Detail = "Account:Detail";

        /// <summary>账号解除锁定</summary>
        public const string Unlock = "Account:Unlock";

        /// <summary>账号重置密码</summary>
        public const string ResetPassword = "Account:ResetPassword";

        /// <summary>账号状态更改</summary>
        public const string Status = "Account:Status";
    }

    /// <summary><see cref="App"/> 应用</summary>
    public class App
    {
        /// <summary>应用列表</summary>
        public const string Paged = "App:Paged";

        /// <summary>应用详情</summary>
        public const string Detail = "App:Detail";

        /// <summary>应用新增</summary>
        public const string Add = "App:Add";

        /// <summary>应用编辑</summary>
        public const string Edit = "App:Edit";

        /// <summary>应用删除</summary>
        public const string Delete = "App:Delete";
    }

    /// <summary><see cref="Database"/> 数据库</summary>
    public class Database
    {
        /// <summary>数据库列表</summary>
        public const string Paged = "Database:Paged";

        /// <summary>数据库详情</summary>
        public const string Detail = "Database:Detail";

        /// <summary>数据库新增</summary>
        public const string Add = "Database:Add";

        /// <summary>数据库编辑</summary>
        public const string Edit = "Database:Edit";

        /// <summary>数据库删除</summary>
        public const string Delete = "Database:Delete";
    }

    /// <summary><see cref="Config"/> 配置</summary>
    public class Config
    {
        /// <summary>配置列表</summary>
        public const string Paged = "Config:Paged";

        /// <summary>配置详情</summary>
        public const string Detail = "Config:Detail";

        /// <summary>配置新增</summary>
        public const string Add = "Config:Add";

        /// <summary>配置编辑</summary>
        public const string Edit = "Config:Edit";
    }
}