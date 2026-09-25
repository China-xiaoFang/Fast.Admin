// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Shared;

/// <summary>
/// 版本枚举
/// </summary>
[Flags]
[FastEnum("版本枚举")]
public enum EditionEnum : byte
{
    /// <summary>
    /// 无
    /// </summary>
    [TagType(TagTypeEnum.Info)]
    [Description("无")]
    None = 0,

    /// <summary>
    /// 试用版
    /// </summary>
    [TagType(TagTypeEnum.Warning)]
    [Description("试用版")]
    Trial = 1,

    /// <summary>
    /// 基础版
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("基础版")]
    Basic = 2,

    /// <summary>
    /// 标准版
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("标准版")]
    Standard = 4,

    /// <summary>
    /// 专业版
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("专业版")]
    Professional = 8,

    /// <summary>
    /// 企业版
    /// </summary>
    [TagType(TagTypeEnum.Primary)]
    [Description("企业版")]
    Enterprise = 16,

    /// <summary>
    /// 旗舰版
    /// </summary>
    [TagType(TagTypeEnum.Success)]
    [Description("旗舰版")]
    Flagship = 32,

    /// <summary>
    /// 定制版
    /// </summary>
    [TagType(TagTypeEnum.Success)]
    [Description("定制版")]
    Custom = 64,

    /// <summary>
    /// 内部版  
    /// </summary>
    /// <remarks>不对外出售</remarks>
    [TagType(TagTypeEnum.Warning)]
    [Description("内部版")]
    Internal = 128
}
