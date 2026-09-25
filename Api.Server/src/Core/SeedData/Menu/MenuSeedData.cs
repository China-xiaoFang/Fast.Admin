// Copyright © 2018-Present 小方
// SPDX-License-Identifier: Apache-2.0
// 
// 本文件依据 Apache License 2.0 授权，完整条款见仓库根目录 LICENSE。
// 本软件按“原样”提供，相关免责声明及责任限制以许可证及适用法律为准。
// 版权来源、合法使用与二次开发责任说明见仓库根目录 README.md。

using Fast.Center.Domain;
using SqlSugar;

namespace Fast.Core;

/// <summary>
/// 菜单种子数据
/// </summary>
internal static partial class MenuSeedData
{
    /// <summary>
    /// 菜单顺序
    /// </summary>
    private static int menuSort
    {
        get
        {
            field++;
            return field;
        }
        set;
    } = 1;

    /// <summary>
    /// 默认菜单种子数据
    /// </summary>
    public static async Task DefaultMenuSeedData(ISqlSugarClient db, ApplicationModel applicationModel, DateTime dateTime)
    {
        // 重置菜单排序
        menuSort = 1000;

        await SeedSystemMonitor(db, applicationModel, dateTime);


        await SeedDevApi(db, applicationModel, dateTime);
        await SeedDevTools(db, applicationModel, dateTime);
        await SeedDevLogs(db, applicationModel, dateTime);


        await SeedFileStorage(db, applicationModel, dateTime);
        await SeedAccountManagement(db, applicationModel, dateTime);
        await SeedSystemManagement(db, applicationModel, dateTime);
        await SeedConfigManagement(db, applicationModel, dateTime);
        await SeedFinanceManagement(db, applicationModel, dateTime);
        await SeedPlatformManagement(db, applicationModel, dateTime);


        await SeedOrganizationManagement(db, applicationModel, dateTime);
        await SeedLogManagement(db, applicationModel, dateTime);
    }
}
