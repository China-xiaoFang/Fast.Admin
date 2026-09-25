// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using System.Reflection;
using System.Text.RegularExpressions;

namespace Fast.Core;

/// <summary>
/// Excel属性信息
/// </summary>
internal sealed class ExcelPropertyInfo
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
    /// 属性的原始类型（如 <c>int?</c>、<c>List&lt;string&gt;</c>）
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
    /// 是否为 <see cref="DateTimeOffset"/> 类型
    /// </summary>
    public bool IsDateTimeOffset { get; set; }

    /// <summary>
    /// 是否为 <see cref="Guid"/> 类型
    /// </summary>
    public bool IsGuid { get; set; }

    /// <summary>
    /// 是否为值类型集合（如 <c>List&lt;int&gt;</c>、<c>List&lt;string&gt;</c>）
    /// </summary>
    public bool IsValueTypeCollection { get; set; }

    /// <summary>
    /// 是否为复杂对象集合（实现 <see cref="System.Collections.IEnumerable"/> 的非值类型集合）
    /// </summary>
    public bool IsComplexCollection { get; set; }

    /// <summary>
    /// 值类型集合的元素类型
    /// <remarks>仅当 <see cref="IsValueTypeCollection"/> 为 <see langword="true"/> 时有值，避免重复调用 <see cref="Type.GetGenericArguments()"/></remarks>
    /// </summary>
    public Type CollectionElementType { get; set; }
}
