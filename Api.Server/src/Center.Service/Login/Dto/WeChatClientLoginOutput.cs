// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

namespace Fast.Center.Service.Login.Dto;

/// <summary>
/// 微信客户端登录输出
/// </summary>
public class WeChatClientLoginOutput
{
    /// <summary>
    /// 登录状态
    /// </summary>
    public LoginStatusEnum Status { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 唯一用户标识
    /// </summary>
    public string OpenId { get; set; }

    /// <summary>
    /// 统一用户标识
    /// </summary>
    public string UnionId { get; set; }

    /// <summary>
    /// 手机
    /// </summary>
    public string Mobile { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar { get; set; }
}
