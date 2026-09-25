// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Core;

/// <summary>
/// Excel列特性
/// </summary>
[SuppressSniffer]
[AttributeUsage(AttributeTargets.Property)]
public sealed class ExcelColumnAttribute : Attribute
{
    /// <summary>
    /// Excel列特性
    /// </summary>
    public ExcelColumnAttribute()
    {
    }

    /// <summary>
    /// Excel列特性
    /// </summary>
    public ExcelColumnAttribute(string name, int order)
    {
        Name = name;
        Order = order;
    }

    /// <summary>
    /// 列名称
    /// </summary>
    /// <remarks>对应Excel的列头名称，为空时使用属性名称</remarks>
    public string Name { get; set; }

    /// <summary>
    /// 列排序
    /// </summary>
    /// <remarks>数值越小越靠前</remarks>
    public int Order { get; set; } = int.MaxValue;

    /// <summary>
    /// 列宽度
    /// </summary>
    /// <remarks>默认宽度10</remarks>
    public int Width { get; set; } = 10;

    /// <summary>
    /// 格式化字符串
    /// </summary>
    /// <remarks>用于 DateTime / 数字类型的格式化，如 "yyyy-MM-dd"、"0.00"</remarks>
    public string Format { get; set; }

    /// <summary>
    /// 是否忽略该列
    /// </summary>
    public bool Ignore { get; set; }

    /// <summary>
    /// <see langword="bool"/> 类型为 <see langword="true"/> 时的显示文本
    /// </summary>
    /// <remarks>默认 "是"</remarks>
    public string TrueText { get; set; } = "是";

    /// <summary>
    /// <see langword="bool"/> 类型为 <see langword="false"/> 时的显示文本
    /// </summary>
    /// <remarks>默认 "否"</remarks>
    public string FalseText { get; set; } = "否";

    /// <summary>
    /// 集合分隔符
    /// </summary>
    /// <remarks>用于 List&lt;值类型&gt; 导出时分隔，默认 ","</remarks>
    public string Separator { get; set; } = ",";

    /// <summary>
    /// 是否转为 JSON 字符串
    /// </summary>
    /// <remarks>用于复杂对象或集合序列化为 JSON 字符串存储到 Excel，导入时反序列化</remarks>
    public bool IsJson { get; set; }
}
