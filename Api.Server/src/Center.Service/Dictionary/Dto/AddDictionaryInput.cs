// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Fast.Center.Domain;

namespace Fast.Center.Service.Dictionary.Dto;

/// <summary>
/// 添加字典输入
/// </summary>
public class AddDictionaryInput
{
    /// <summary>
    /// 字典Key
    /// </summary>
    [StringRequired(ErrorMessage = "字典Key不能为空")]
    public string DictionaryKey { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    [StringRequired(ErrorMessage = "字典名称不能为空")]
    public string DictionaryName { get; set; }

    /// <summary>
    /// 字典值类型
    /// </summary>
    [EnumRequired(ErrorMessage = "字典值类型不能为空")]
    public DictionaryValueTypeEnum ValueType { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }

    /// <summary>
    /// 字典项集合
    /// </summary>
    public List<AddDictionaryItemInput> DictionaryItemList { get; set; } = [];

    /// <summary>
    /// 添加字典项输入
    /// </summary>
    public class AddDictionaryItemInput
    {
        /// <summary>
        /// 字典项名称
        /// </summary>
        [StringRequired(ErrorMessage = "字典项名称不能为空")]
        public string Label { get; set; }

        /// <summary>
        /// 字典项值
        /// </summary>
        [StringRequired(ErrorMessage = "字典项值不能为空")]
        public string Value { get; set; }

        /// <summary>
        /// 标签类型
        /// </summary>
        [EnumRequired(ErrorMessage = "标签类型不能为空")]
        public TagTypeEnum Type { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [IntRequired(ErrorMessage = "排序不能为空")]
        public int Order { get; set; }

        /// <summary>
        /// 提示
        /// </summary>
        public string Tips { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        [Required(ErrorMessage = "是否显示不能为空")]
        public bool Visible { get; set; }
    }
}
