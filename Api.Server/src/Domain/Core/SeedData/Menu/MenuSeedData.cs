using Fast.Center.Entity;
using SqlSugar;

// ReSharper disable once CheckNamespace
namespace Fast.Core;

/// <summary>
/// <see cref="MenuSeedData"/> 菜单种子数据
/// </summary>
internal static partial class MenuSeedData
{
    private static int _moduleSort = 1;
    private static int _menuSort = 1;
    private static int _buttonSort = 1;

    /// <summary>
    /// 模块顺序
    /// </summary>
    private static int moduleSort
    {
        get
        {
            _moduleSort++;
            return _moduleSort;
        }
    }

    /// <summary>
    /// 菜单顺序
    /// </summary>
    private static int menuSort
    {
        get
        {
            _menuSort++;
            return _menuSort;
        }
    }

    /// <summary>
    /// 按钮顺序
    /// </summary>
    private static int buttonSort
    {
        get
        {
            _buttonSort++;
            return _buttonSort;
        }
    }

    /// <summary>
    /// 默认菜单种子数据
    /// </summary>
    /// <param name="db"></param>
    /// <param name="applicationModel"><see cref="ApplicationModel"/> 应用</param>
    /// <param name="dateTime"><see cref="DateTime"/> 时间</param>
    /// <returns></returns>
    public static async Task DefaultMenuSeedData(ISqlSugarClient db, ApplicationModel applicationModel,
        DateTime dateTime)
    {
        // 系统模块
        await SystemModuleSeedData(db, applicationModel, dateTime);

        // 开发模块
        await DevModuleSeedData(db, applicationModel, dateTime);
    }
}