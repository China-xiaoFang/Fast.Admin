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

using System.Reflection;
using System.Text.RegularExpressions;

namespace Fast.Core;

/// <summary>
/// <see cref="ExcelPropertyInfo"/> Excel属性信息
/// </summary>
internal class ExcelPropertyInfo
{
    /// <summary>
    /// 属性信息
    /// </summary>
    public PropertyInfo Property { get; set; }

    /// <summary>
    /// 列特性
    /// </summary>
    public ExcelColumnAttribute ColumnAttribute { get; set; }

    /// <summary>
    /// Excel列名称
    /// </summary>
    /// <remarks>优先使用 <see cref="ExcelColumnAttribute.Name"/>，其次使用属性名</remarks>
    public string ColumnName { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// 必填验证特性
    /// </summary>
    public ExcelRequiredAttribute RequiredAttribute { get; set; }

    /// <summary>
    /// 正则验证特性列表
    /// </summary>
    public List<ExcelRegexAttribute> RegexAttributes { get; set; } = [];

    /// <summary>
    /// 预编译的正则表达式列表
    /// </summary>
    /// <remarks>在构建属性信息时一次编译，避免每行数据都重新编译正则引擎</remarks>
    public List<(Regex CompiledRegex, string ErrorMessage)> CompiledRegexPatterns { get; set; } = [];

    /// <summary>
    /// 属性的原始类型（如 int?、List&lt;string&gt;）
    /// </summary>
    public Type PropertyType { get; set; }

    /// <summary>
    /// 属性的底层类型
    /// <remarks>对于 Nullable&lt;T&gt; 返回 T，其他类型返回自身</remarks>
    /// </summary>
    public Type UnderlyingType { get; set; }

    /// <summary>
    /// 是否为可空类型（Nullable&lt;T&gt;）
    /// </summary>
    public bool IsNullable { get; set; }

    /// <summary>
    /// 是否为布尔类型
    /// </summary>
    public bool IsBool { get; set; }

    /// <summary>
    /// 是否为枚举类型
    /// </summary>
    public bool IsEnum { get; set; }

    /// <summary>
    /// 是否为 DateTime 类型
    /// </summary>
    public bool IsDateTime { get; set; }

    /// <summary>
    /// 是否为 DateTimeOffset 类型
    /// </summary>
    public bool IsDateTimeOffset { get; set; }

    /// <summary>
    /// 是否为 Guid 类型
    /// </summary>
    public bool IsGuid { get; set; }

    /// <summary>
    /// 是否为值类型集合（如 List&lt;int&gt;、List&lt;string&gt;）
    /// </summary>
    public bool IsValueTypeCollection { get; set; }

    /// <summary>
    /// 是否为复杂对象集合（非值类型的 IEnumerable）
    /// </summary>
    public bool IsComplexCollection { get; set; }

    /// <summary>
    /// 值类型集合的元素类型
    /// <remarks>仅当 IsValueTypeCollection 为 true 时有值，避免每次调用 GetGenericArguments</remarks>
    /// </summary>
    public Type CollectionElementType { get; set; }
}