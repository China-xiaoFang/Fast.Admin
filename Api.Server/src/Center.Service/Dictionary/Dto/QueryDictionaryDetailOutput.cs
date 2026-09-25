// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Fast.Center.Domain;

namespace Fast.Center.Service.Dictionary.Dto;

/// <summary>
/// 获取字典详情输出
/// </summary>
public class QueryDictionaryDetailOutput : PagedOutput
{
    /// <summary>
    /// 字典Id
    /// </summary>
    public long DictionaryId { get; set; }

    /// <summary>
    /// 字典Key
    /// </summary>
    public string DictionaryKey { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    public string DictionaryName { get; set; }

    /// <summary>
    /// 字典值类型
    /// </summary>
    public DictionaryValueTypeEnum ValueType { get; set; }

    /// <summary>
    /// Flags枚举
    /// </summary>
    public bool HasFlags { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public CommonStatusEnum Status { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 字典项集合
    /// </summary>
    public List<EditDictionaryItemInput> DictionaryItemList { get; set; }
}
