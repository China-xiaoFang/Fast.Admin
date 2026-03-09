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

namespace Fast.Core;

/// <summary>
/// <see cref="ExcelColumnAttribute"/> Excel列特性
/// </summary>
[SuppressSniffer]
[AttributeUsage(AttributeTargets.Property)]
public class ExcelColumnAttribute : Attribute
{
    /// <summary>
    /// <see cref="ExcelColumnAttribute"/> Excel列特性
    /// </summary>
    public ExcelColumnAttribute()
    {
    }

    /// <summary>
    /// <see cref="ExcelColumnAttribute"/> Excel列特性
    /// </summary>
    /// <param name="name"><see cref="string"/> 列名称</param>
    /// <param name="order"><see cref="int"/> 列排序</param>
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
    /// Bool 类型 true 显示文本
    /// </summary>
    /// <remarks>默认 "是"</remarks>
    public string TrueText { get; set; } = "是";

    /// <summary>
    /// Bool 类型 false 显示文本
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