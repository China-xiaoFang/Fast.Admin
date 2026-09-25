// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Fast.SqlSugar;

namespace Fast.File.Applications.Dto;

/// <summary>
/// 获取文件分页列表输出
/// </summary>
public class QueryFilePagedOutput
{
    /// <summary>
    /// 文件Id
    /// </summary>
    public long FileId { get; set; }

    /// <summary>
    /// 文件唯一标识
    /// </summary>
    [SugarSearchValue]
    public string FileObjectName { get; set; }

    /// <summary>
    /// 原始文件名
    /// </summary>
    [SugarSearchValue]
    public string FileOriginName { get; set; }

    /// <summary>
    /// 文件后缀
    /// </summary>
    public string FileSuffix { get; set; }

    /// <summary>
    /// 文件Mime类型
    /// </summary>
    public string FileMimeType { get; set; }

    /// <summary>
    /// 文件大小kb
    /// </summary>
    public long FileSizeKb { get; set; }

    /// <summary>
    /// 存储路径
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// 访问地址
    /// </summary>
    public string FileLocation { get; set; }

    /// <summary>
    /// 文件哈希
    /// </summary>
    [SugarSearchValue]
    public string FileHash { get; set; }

    /// <summary>
    /// 上传设备
    /// </summary>
    public string UploadDevice { get; set; }

    /// <summary>
    /// 上传操作系统（版本）
    /// </summary>
    public string UploadOS { get; set; }

    /// <summary>
    /// 上传浏览器（版本）
    /// </summary>
    public string UploadBrowser { get; set; }

    /// <summary>
    /// 上传省份
    /// </summary>
    public string UploadProvince { get; set; }

    /// <summary>
    /// 上传城市
    /// </summary>
    public string UploadCity { get; set; }

    /// <summary>
    /// 上传Ip
    /// </summary>
    public string UploadIp { get; set; }

    /// <summary>
    /// 创建者用户名称
    /// </summary>
    public string CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [SugarSearchTime]
    public DateTime? CreatedTime { get; set; }

    /// <summary>
    /// 租户名称
    /// </summary>
    public string TenantName { get; set; }
}
