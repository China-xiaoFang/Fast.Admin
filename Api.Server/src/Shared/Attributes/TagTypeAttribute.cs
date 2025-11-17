// ReSharper disable once CheckNamespace

namespace Fast.Shared;

/// <summary>
/// <see cref="TagTypeAttribute"/> 标签类型特性
/// </summary>
[SuppressSniffer]
[AttributeUsage(AttributeTargets.Field)]
public class TagTypeAttribute : Attribute
{
    /// <summary>
    /// 标签类型
    /// </summary>
    public TagTypeEnum TagType { get; set; }

    /// <summary>
    /// <see cref="TagTypeAttribute"/> 标签类型特性
    /// </summary>
    /// <param name="tagType"><see cref="TagTypeEnum"/> 标签类型</param>
    public TagTypeAttribute(TagTypeEnum tagType)
    {
        TagType = tagType;
    }
}