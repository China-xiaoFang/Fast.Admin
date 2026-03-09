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

using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using MiniExcelLibs.Attributes;
using MiniExcelLibs.OpenXml;

namespace Fast.Core;

/// <summary>
/// <see cref="MiniExcelUtil"/> Excel工具类
/// </summary>
/// <remarks>基于 MiniExcel 封装的 Excel 导入导出工具，支持 DTO 特性驱动</remarks>
[SuppressSniffer]
public static class MiniExcelUtil
{
    /// <summary>
    /// 属性信息缓存
    /// </summary>
    /// <remarks>按 DTO 类型缓存属性元信息（特性、类型标记等），避免每次导入导出都重复反射解析</remarks>
    private static readonly ConcurrentDictionary<Type, List<ExcelPropertyInfo>> _propertyInfoCache = new();

    /// <summary>
    /// 枚举映射缓存
    /// </summary>
    /// <remarks>按枚举类型缓存 值↔描述 的双向映射，避免每次循环都重复遍历枚举字段和 Description 特性</remarks>
    private static readonly ConcurrentDictionary<Type, ExcelEnumMapping> _enumMappingCache = new();

    #region 导出

    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"><see cref="IEnumerable{T}"/> 数据集合</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称</param>
    /// <param name="excelType"><see cref="ExcelType"/> Excel类型</param>
    /// <returns><see cref="MemoryStream"/></returns>
    public static MemoryStream ExportExcel<T>(IEnumerable<T> data, string sheetName = "Sheet1",
        ExcelType excelType = ExcelType.XLSX) where T : class, new()
    {
        // 从缓存获取属性元信息（含列名、排序、类型标记等）
        var propertyInfos = GetPropertyInfos<T>();

        // 根据属性元信息将 DTO 数据转换为 Dictionary 列表（适配 MiniExcel 的动态列导出）
        var exportData = ConvertExportData(data, propertyInfos);

        // 构建 MiniExcel 的动态列配置（列名、宽度、格式等）
        var config = new OpenXmlConfiguration();
        var dynamicColumns = BuildDynamicColumns(propertyInfos);
        if (dynamicColumns.Count > 0)
        {
            config.DynamicColumns = dynamicColumns.ToArray();
        }

        // 使用 MiniExcel 写入 Excel 到流
        var memoryStream = new MemoryStream();
        memoryStream.SaveAs(exportData, sheetName: sheetName, excelType: excelType, configuration: config);

        // 重置流位置到开头，以便调用方可以直接读取
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"><see cref="IEnumerable{T}"/> 数据集合</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称</param>
    /// <param name="excelType"><see cref="ExcelType"/> Excel类型</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 取消令牌</param>
    /// <returns><see cref="Task{MemoryStream}"/></returns>
    public static async Task<MemoryStream> ExportExcelAsync<T>(IEnumerable<T> data, string sheetName = "Sheet1",
        ExcelType excelType = ExcelType.XLSX, CancellationToken cancellationToken = default) where T : class, new()
    {
        // 从缓存获取属性元信息（含列名、排序、类型标记等）
        var propertyInfos = GetPropertyInfos<T>();

        // 根据属性元信息将 DTO 数据转换为 Dictionary 列表（适配 MiniExcel 的动态列导出）
        var exportData = ConvertExportData(data, propertyInfos);

        // 构建 MiniExcel 的动态列配置（列名、宽度、格式等）
        var config = new OpenXmlConfiguration();
        var dynamicColumns = BuildDynamicColumns(propertyInfos);
        if (dynamicColumns.Count > 0)
        {
            config.DynamicColumns = dynamicColumns.ToArray();
        }

        // 使用 MiniExcel 写入 Excel 到流
        var memoryStream = new MemoryStream();
        await memoryStream.SaveAsAsync(exportData, sheetName: sheetName, excelType: excelType, configuration: config,
            cancellationToken: cancellationToken);

        // 重置流位置到开头，以便调用方可以直接读取
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"><see cref="IEnumerable{T}"/> 数据集合</param>
    /// <param name="fileName"><see cref="string"/> 文件名称</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称</param>
    /// <param name="excelType"><see cref="ExcelType"/> Excel类型</param>
    /// <returns><see cref="FileStreamResult"/></returns>
    public static FileStreamResult ExportExcel<T>(IEnumerable<T> data, string fileName, string sheetName = "Sheet1",
        ExcelType excelType = ExcelType.XLSX) where T : class, new()
    {
        var memoryStream = ExportExcel(data, sheetName, excelType);
        return new FileStreamResult(memoryStream, "application/octet-stream") {FileDownloadName = fileName.UrlEncode()};
    }

    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"><see cref="IEnumerable{T}"/> 数据集合</param>
    /// <param name="fileName"><see cref="string"/> 文件名称</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称</param>
    /// <param name="excelType"><see cref="ExcelType"/> Excel类型</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 取消令牌</param>
    /// <returns><see cref="Task{FileStreamResult}"/></returns>
    public static async Task<FileStreamResult> ExportExcelAsync<T>(IEnumerable<T> data, string fileName,
        string sheetName = "Sheet1", ExcelType excelType = ExcelType.XLSX, CancellationToken cancellationToken = default)
        where T : class, new()
    {
        var memoryStream = await ExportExcelAsync(data, sheetName, excelType, cancellationToken);
        return new FileStreamResult(memoryStream, "application/octet-stream") {FileDownloadName = fileName.UrlEncode()};
    }

    /// <summary>
    /// 导出到文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"><see cref="string"/> 文件路径</param>
    /// <param name="data"><see cref="IEnumerable{T}"/> 数据集合</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称</param>
    /// <param name="excelType"><see cref="ExcelType"/> Excel类型</param>
    public static void ExportToFile<T>(string filePath, IEnumerable<T> data, string sheetName = "Sheet1",
        ExcelType excelType = ExcelType.XLSX) where T : class, new()
    {
        // 从缓存获取属性元信息
        var propertyInfos = GetPropertyInfos<T>();

        // 转换数据为 MiniExcel 可写入的字典格式
        var exportData = ConvertExportData(data, propertyInfos);

        // 构建 MiniExcel 的动态列配置（列名、宽度、格式等）
        var config = new OpenXmlConfiguration();
        var dynamicColumns = BuildDynamicColumns(propertyInfos);
        if (dynamicColumns.Count > 0)
        {
            config.DynamicColumns = dynamicColumns.ToArray();
        }

        // 使用 MiniExcel 写入文件
        MiniExcel.SaveAs(filePath, exportData, sheetName: sheetName, excelType: excelType, configuration: config);
    }

    /// <summary>
    /// 导出到文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"><see cref="string"/> 文件路径</param>
    /// <param name="data"><see cref="IEnumerable{T}"/> 数据集合</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称</param>
    /// <param name="excelType"><see cref="ExcelType"/> Excel类型</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 取消令牌</param>
    public static async Task ExportToFileAsync<T>(string filePath, IEnumerable<T> data, string sheetName = "Sheet1",
        ExcelType excelType = ExcelType.XLSX, CancellationToken cancellationToken = default) where T : class, new()
    {
        // 从缓存获取属性元信息
        var propertyInfos = GetPropertyInfos<T>();

        // 转换数据为 MiniExcel 可写入的字典格式
        var exportData = ConvertExportData(data, propertyInfos);

        // 构建 MiniExcel 的动态列配置（列名、宽度、格式等）
        var config = new OpenXmlConfiguration();
        var dynamicColumns = BuildDynamicColumns(propertyInfos);
        if (dynamicColumns.Count > 0)
        {
            config.DynamicColumns = dynamicColumns.ToArray();
        }

        // 使用 MiniExcel 写入文件
        await MiniExcel.SaveAsAsync(filePath, exportData, sheetName: sheetName, excelType: excelType, configuration: config,
            cancellationToken: cancellationToken);
    }

    #endregion

    #region 导入

    /// <summary>
    /// 导入Excel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream"><see cref="Stream"/> Excel文件流</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称，为空时读取第一个Sheet</param>
    /// <param name="excelType"><see cref="ExcelType"/> Excel类型</param>
    /// <returns><see cref="ExcelImportResult{T}"/></returns>
    public static ExcelImportResult<T> ImportExcel<T>(Stream stream, string sheetName = null,
        ExcelType excelType = ExcelType.XLSX) where T : class, new()
    {
        // 使用 MiniExcel 读取 Excel 数据（启用 useHeaderRow 以列头名称作为 Key）
        var rows = stream.Query(sheetName: sheetName, useHeaderRow: true, excelType: excelType)
            .Cast<IDictionary<string, object>>()
            .ToList();

        // 将原始行数据解析为强类型 DTO 列表，并进行验证
        return ParseImportData<T>(rows);
    }

    /// <summary>
    /// 导入Excel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream"><see cref="Stream"/> Excel文件流</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称，为空时读取第一个Sheet</param>
    /// <param name="excelType"><see cref="ExcelType"/> Excel类型</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 取消令牌</param>
    /// <returns><see cref="ExcelImportResult{T}"/></returns>
    public static async Task<ExcelImportResult<T>> ImportExcelAsync<T>(Stream stream, string sheetName = null,
        ExcelType excelType = ExcelType.XLSX, CancellationToken cancellationToken = default) where T : class, new()
    {
        // 使用 MiniExcel 读取 Excel 数据（启用 useHeaderRow 以列头名称作为 Key）
        var rows = await stream.QueryAsync(true, sheetName, excelType, cancellationToken: cancellationToken);

        // 将动态行数据转换为字典列表
        var rowList = rows.Cast<IDictionary<string, object>>()
            .ToList();

        // 将原始行数据解析为强类型 DTO 列表，并进行验证
        return ParseImportData<T>(rowList);
    }

    /// <summary>
    /// 从文件导入Excel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"><see cref="string"/> 文件路径</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称，为空时读取第一个Sheet</param>
    /// <param name="excelType"><see cref="ExcelType"/> Excel类型</param>
    /// <returns><see cref="ExcelImportResult{T}"/></returns>
    public static ExcelImportResult<T> ImportExcel<T>(string filePath, string sheetName = null,
        ExcelType excelType = ExcelType.XLSX) where T : class, new()
    {
        // 使用 MiniExcel 从文件读取数据
        var rows = MiniExcel.Query(filePath, sheetName: sheetName, useHeaderRow: true, excelType: excelType)
            .Cast<IDictionary<string, object>>()
            .ToList();

        // 将原始行数据解析为强类型 DTO 列表，并进行验证
        return ParseImportData<T>(rows);
    }

    /// <summary>
    /// 从文件导入Excel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"><see cref="string"/> 文件路径</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称，为空时读取第一个Sheet</param>
    /// <param name="excelType"><see cref="ExcelType"/> Excel类型</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 取消令牌</param>
    /// <returns><see cref="ExcelImportResult{T}"/></returns>
    public static async Task<ExcelImportResult<T>> ImportExcelAsync<T>(string filePath, string sheetName = null,
        ExcelType excelType = ExcelType.XLSX, CancellationToken cancellationToken = default) where T : class, new()
    {
        // 使用 MiniExcel 异步从文件读取数据
        var rows = await MiniExcel.QueryAsync(filePath, true, sheetName, excelType, cancellationToken: cancellationToken);

        // 将动态行数据转换为字典列表
        var rowList = rows.Cast<IDictionary<string, object>>()
            .ToList();

        // 将原始行数据解析为强类型 DTO 列表，并进行验证
        return ParseImportData<T>(rowList);
    }

    #endregion

    #region 属性元信息解析与缓存

    /// <summary>
    /// 获取 DTO 类型的属性元信息列表（带缓存）
    /// </summary>
    /// <remarks>
    /// 首次调用时通过反射解析属性特性和类型信息，后续调用直接从缓存读取。
    /// 缓存内容包括：列名、排序、类型标记（IsBool/IsEnum/IsDateTime 等）、验证规则、预编译正则等。
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static List<ExcelPropertyInfo> GetPropertyInfos<T>() where T : class, new()
    {
        return _propertyInfoCache.GetOrAdd(typeof(T), type =>
        {
            // 获取所有属性
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var result = new List<ExcelPropertyInfo>();

            foreach (var prop in properties)
            {
                var columnAttr = prop.GetCustomAttribute<ExcelColumnAttribute>();

                // 跳过没有标记 [ExcelColumn] 特性的属性（未标记的属性默认忽略，不参与导入导出）
                if (columnAttr == null)
                    continue;

                // 跳过标记了 Ignore = true 的属性
                if (columnAttr.Ignore)
                    continue;

                // 解析属性类型信息
                var propertyType = prop.PropertyType;
                var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

                // 构建属性元信息，一次性计算所有类型标记
                var info = new ExcelPropertyInfo
                {
                    // 基础信息
                    Property = prop,
                    ColumnAttribute = columnAttr,
                    ColumnName = columnAttr?.Name ?? prop.Name,
                    Order = columnAttr?.Order ?? int.MaxValue,

                    // 验证特性
                    RequiredAttribute = prop.GetCustomAttribute<ExcelRequiredAttribute>(),
                    RegexAttributes = prop.GetCustomAttributes<ExcelRegexAttribute>()
                        .ToList(),

                    // 缓存的类型信息（避免每行数据都重复判断类型）
                    PropertyType = propertyType,
                    UnderlyingType = underlyingType,
                    IsNullable = Nullable.GetUnderlyingType(propertyType) != null,
                    IsBool = underlyingType == typeof(bool),
                    IsEnum = underlyingType.IsEnum,
                    IsDateTime = underlyingType == typeof(DateTime),
                    IsDateTimeOffset = underlyingType == typeof(DateTimeOffset),
                    IsGuid = underlyingType == typeof(Guid),
                    IsValueTypeCollection = CheckIsValueTypeCollection(propertyType),
                    IsComplexCollection = CheckIsComplexCollection(propertyType)
                };

                // 如果是值类型集合，缓存元素类型（避免每次 GetGenericArguments / GetElementType）
                if (info.IsValueTypeCollection)
                {
                    if (propertyType.IsArray)
                    {
                        info.CollectionElementType = propertyType.GetElementType();
                    }
                    else if (propertyType.IsGenericType)
                    {
                        info.CollectionElementType = propertyType.GetGenericArguments()[0];
                    }
                }

                // 如果是枚举类型，预先构建枚举映射缓存
                if (info.IsEnum)
                {
                    GetOrBuildEnumMapping(underlyingType);
                }

                // 预编译正则表达式（避免每行数据都重新编译正则）
                foreach (var regexAttr in info.RegexAttributes)
                {
                    info.CompiledRegexPatterns.Add((new Regex(regexAttr.Pattern, RegexOptions.Compiled), regexAttr.ErrorMessage));
                }

                result.Add(info);
            }

            // 按 Order 排序，Order 相同则按列名排序
            return result.OrderBy(p => p.Order)
                .ThenBy(p => p.ColumnName)
                .ToList();
        });
    }

    /// <summary>
    /// 获取或构建枚举映射（带缓存）
    /// </summary>
    /// <remarks>
    /// 构建枚举类型的双向映射：
    /// <para>- 导出方向：枚举值 → Description 文本</para>
    /// <para>- 导入方向：Description/名称/数值字符串 → 枚举值</para>
    /// </remarks>
    /// <param name="enumType"><see cref="Type"/> 枚举类型</param>
    /// <returns><see cref="ExcelEnumMapping"/></returns>
    private static ExcelEnumMapping GetOrBuildEnumMapping(Type enumType)
    {
        return _enumMappingCache.GetOrAdd(enumType, type =>
        {
            var mapping = new ExcelEnumMapping();

            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var value = field.GetValue(null);
                var name = field.Name;

                // 获取 [Description] 特性的描述文本，如果没有则使用枚举名称
                var descAttr = field.GetCustomAttribute<DescriptionAttribute>();
                var description = descAttr?.Description ?? name;

                // 导出映射：枚举值 → 描述文本
                mapping.ValueToDescription[value] = description;

                /*
                 * 导入映射：描述文本 → 枚举值（忽略大小写）
                 * 注意：当多个枚举值共享相同 Description 时，TryAdd 只保留第一个映射，
                 * 这是预期行为（导入时同名描述只能映射到一个值）
                 */
                mapping.TextToValue.TryAdd(description, value);

                // 导入映射：枚举名称 → 枚举值
                mapping.TextToValue.TryAdd(name, value);

                // 导入映射：数值字符串 → 枚举值（如 "1" → SomeEnum.Value1）
                var numericValue = Convert.ChangeType(value, Enum.GetUnderlyingType(type));
                mapping.TextToValue.TryAdd(numericValue.ToString(), value);
            }

            return mapping;
        });
    }

    #endregion

    #region 导出数据转换

    /// <summary>
    /// 构建 MiniExcel 的动态列配置
    /// </summary>
    /// <remarks>根据属性元信息生成列名、列序、列宽、格式等配置</remarks>
    /// <param name="propertyInfos"><see cref="List{T}"/> 属性元信息列表</param>
    /// <returns></returns>
    private static List<DynamicExcelColumn> BuildDynamicColumns(List<ExcelPropertyInfo> propertyInfos)
    {
        var columns = new List<DynamicExcelColumn>();

        for (var i = 0; i < propertyInfos.Count; i++)
        {
            var info = propertyInfos[i];

            // 创建动态列：设置列名和索引
            var column = new DynamicExcelColumn(info.ColumnName) {Index = i, Width = info.ColumnAttribute?.Width ?? 0};

            // 仅当标记了 ExcelColumn 特性时才处理格式：优先使用显式指定的格式，否则按属性类型设置默认格式
            if (info.ColumnAttribute != null)
            {
                column.Format = !string.IsNullOrEmpty(info.ColumnAttribute.Format)
                    ? info.ColumnAttribute.Format
                    : GetDefaultFormat(info);
            }

            columns.Add(column);
        }

        return columns;
    }

    /// <summary>
    /// 根据属性类型获取默认的 Excel 格式化字符串
    /// </summary>
    /// <remarks>
    /// 当 <see cref="ExcelColumnAttribute.Format"/> 未指定时，根据属性类型返回默认格式：
    /// <para>- DateTime / DateTimeOffset → yyyy-MM-dd HH:mm:ss</para>
    /// <para>- decimal / double / float → 0.00</para>
    /// </remarks>
    /// <param name="info"><see cref="ExcelPropertyInfo"/> 属性元信息</param>
    /// <returns>默认格式化字符串，无需默认格式时返回 null</returns>
    private static string GetDefaultFormat(ExcelPropertyInfo info)
    {
        if (info.IsDateTime || info.IsDateTimeOffset)
            return "yyyy-MM-dd HH:mm:ss";

        var type = info.UnderlyingType;
        if (type == typeof(decimal) || type == typeof(double) || type == typeof(float))
            return "0.00";

        return null;
    }

    /// <summary>
    /// 将 DTO 数据集合转换为 MiniExcel 可写入的字典列表
    /// </summary>
    /// <remarks>遍历每条数据的每个属性，根据类型标记进行值转换（枚举→描述、Bool→是/否、集合→分隔字符串等）</remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"><see cref="IEnumerable{T}"/> 数据集合</param>
    /// <param name="propertyInfos"><see cref="List{T}"/> 属性元信息列表</param>
    /// <returns></returns>
    private static List<Dictionary<string, object>> ConvertExportData<T>(IEnumerable<T> data,
        List<ExcelPropertyInfo> propertyInfos)
    {
        var result = new List<Dictionary<string, object>>();

        foreach (var item in data)
        {
            var dict = new Dictionary<string, object>();

            foreach (var info in propertyInfos)
            {
                // 读取属性值并转换为导出格式
                var value = info.Property.GetValue(item);
                dict[info.ColumnName] = ConvertExportValue(value, info);
            }

            result.Add(dict);
        }

        return result;
    }

    /// <summary>
    /// 将单个属性值转换为导出格式
    /// </summary>
    /// <remarks>
    /// 根据预缓存的类型标记进行快速分支判断（无需每次重新检测类型）：
    /// <para>- IsJson → JSON 序列化</para>
    /// <para>- IsBool → 是/否 文本</para>
    /// <para>- IsEnum → Description 描述文本</para>
    /// <para>- IsDateTime/IsDateTimeOffset → 格式化字符串</para>
    /// <para>- IsValueTypeCollection → 分隔符连接</para>
    /// <para>- IsComplexCollection → JSON 序列化</para>
    /// </remarks>
    /// <param name="value">属性原始值</param>
    /// <param name="info"><see cref="ExcelPropertyInfo"/> 属性元信息</param>
    /// <returns></returns>
    private static object ConvertExportValue(object value, ExcelPropertyInfo info)
    {
        if (value == null)
            return null;

        var columnAttr = info.ColumnAttribute;

        // JSON 序列化：标记了 IsJson 的属性直接序列化为 JSON 字符串
        if (columnAttr?.IsJson == true)
        {
            return value.ToJsonString();
        }

        // Bool 类型：转换为配置的 TrueText/FalseText（默认 "是"/"否"）
        if (info.IsBool)
        {
            var boolValue = (bool) value;
            var trueText = columnAttr?.TrueText ?? "是";
            var falseText = columnAttr?.FalseText ?? "否";
            return boolValue ? trueText : falseText;
        }

        // 枚举类型：从缓存的映射表中查找 Description 描述文本
        if (info.IsEnum)
        {
            var mapping = GetOrBuildEnumMapping(info.UnderlyingType);
            return mapping.ValueToDescription.TryGetValue(value, out var desc) ? desc : value.ToString();
        }

        // DateTime 类型：按 Format 格式化输出
        if (info.IsDateTime)
        {
            var dateValue = (DateTime) value;
            if (!string.IsNullOrEmpty(columnAttr?.Format))
            {
                return dateValue.ToString(columnAttr.Format);
            }

            return dateValue;
        }

        // DateTimeOffset 类型：按 Format 格式化输出
        if (info.IsDateTimeOffset)
        {
            var dateValue = (DateTimeOffset) value;
            if (!string.IsNullOrEmpty(columnAttr?.Format))
            {
                return dateValue.ToString(columnAttr.Format);
            }

            return dateValue;
        }

        // 值类型集合（如 List<int>、List<string>）：使用分隔符连接为字符串
        if (info.IsValueTypeCollection)
        {
            var separator = columnAttr?.Separator ?? ",";
            return JoinCollection(value, separator);
        }

        // 复杂对象集合（非值类型集合）：默认 JSON 序列化
        if (info.IsComplexCollection)
        {
            return value.ToJsonString();
        }

        // 其他类型直接返回原值
        return value;
    }

    #endregion

    #region 导入数据解析

    /// <summary>
    /// 将 Excel 原始行数据解析为强类型 DTO 列表
    /// </summary>
    /// <remarks>
    /// 处理流程：
    /// <para>1. 构建列名→属性的映射关系</para>
    /// <para>2. 逐行遍历，对每个属性执行：查找列值 → 必填验证 → 正则验证 → 类型转换 → 赋值</para>
    /// <para>3. 收集所有验证错误和转换错误</para>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    /// <param name="rows">从 MiniExcel 读取的原始行数据</param>
    /// <returns><see cref="ExcelImportResult{T}"/></returns>
    private static ExcelImportResult<T> ParseImportData<T>(List<IDictionary<string, object>> rows) where T : class, new()
    {
        var result = new ExcelImportResult<T>();

        // 从缓存获取属性元信息
        var propertyInfos = GetPropertyInfos<T>();

        // 逐行解析数据
        for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
        {
            var row = rows[rowIndex];
            var item = new T();
            var rowErrors = new List<ExcelImportError>();
            // 行号从 1 开始（不含表头行）
            var rowNumber = rowIndex + 1;

            // 遍历每个属性，尝试从行数据中提取并转换值
            foreach (var info in propertyInfos)
            {
                // --- 第一步：从行数据中查找对应列的值 ---
                var columnFound = false;

                // 优先按 ExcelColumn.Name 匹配列
                if (row.TryGetValue(info.ColumnName, out var cellValue))
                {
                    columnFound = true;
                }
                // 其次按属性名匹配列
                else if (info.ColumnName != info.Property.Name && row.TryGetValue(info.Property.Name, out cellValue))
                {
                    columnFound = true;
                }

                // --- 第二步：必填验证 ---
                if (info.RequiredAttribute != null)
                {
                    if (!columnFound || cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                    {
                        rowErrors.Add(new ExcelImportError
                        {
                            RowIndex = rowNumber,
                            ColumnName = info.ColumnName,
                            PropertyName = info.Property.Name,
                            CellValue = cellValue,
                            ErrorMessage = string.IsNullOrEmpty(info.RequiredAttribute.ErrorMessage)
                                ? $"{info.ColumnName} 不能为空"
                                : info.RequiredAttribute.ErrorMessage
                        });
                        // 必填字段为空时跳过后续验证和赋值
                        continue;
                    }
                }

                // 列不存在或值为空时跳过（非必填字段）
                if (!columnFound || cellValue == null)
                    continue;

                var cellString = cellValue.ToString()
                    ?.Trim();

                /*
                 * --- 第三步：正则验证（使用预编译的 Regex 对象） ---
                 * 注意：仅对非空值执行正则验证。空值的存在性应由 ExcelRequired 特性控制，
                 * 正则验证仅校验"有值时格式是否正确"，不负责判断"是否必须有值"
                 */
                if (!string.IsNullOrEmpty(cellString))
                {
                    var hasRegexError = false;
                    foreach (var (compiledRegex, errorMessage) in info.CompiledRegexPatterns)
                    {
                        if (!compiledRegex.IsMatch(cellString))
                        {
                            rowErrors.Add(new ExcelImportError
                            {
                                RowIndex = rowNumber,
                                ColumnName = info.ColumnName,
                                PropertyName = info.Property.Name,
                                CellValue = cellValue,
                                ErrorMessage = string.IsNullOrEmpty(errorMessage) ? $"{info.ColumnName} 格式不正确" : errorMessage
                            });
                            hasRegexError = true;
                        }
                    }

                    // 正则验证失败时跳过类型转换和赋值
                    if (hasRegexError)
                        continue;
                }

                // --- 第四步：类型转换并赋值 ---
                try
                {
                    var convertedValue = ConvertImportValue(cellValue, info);
                    if (convertedValue != null)
                    {
                        info.Property.SetValue(item, convertedValue);
                    }
                }
                catch (Exception ex)
                {
                    rowErrors.Add(new ExcelImportError
                    {
                        RowIndex = rowNumber,
                        ColumnName = info.ColumnName,
                        PropertyName = info.Property.Name,
                        CellValue = cellValue,
                        ErrorMessage = $"{info.ColumnName} 数据转换失败：{ex.Message}"
                    });
                }
            }

            // 无论是否有错误，都将解析结果加入（由调用方决定如何处理错误行）
            result.Data.Add(item);
            result.Errors.AddRange(rowErrors);
        }

        return result;
    }

    /// <summary>
    /// 将单元格值转换为属性对应的目标类型
    /// </summary>
    /// <remarks>
    /// 根据预缓存的类型标记进行快速分支判断（无需每次重新检测类型）：
    /// <para>- IsJson → JSON 反序列化</para>
    /// <para>- IsBool → 是/否/true/false/1/0 文本解析</para>
    /// <para>- IsEnum → 从缓存的映射表中查找（支持描述、名称、数值）</para>
    /// <para>- IsDateTime/IsDateTimeOffset → 日期解析（支持 OLE 日期格式）</para>
    /// <para>- IsValueTypeCollection → 按分隔符拆分为集合</para>
    /// <para>- IsComplexCollection → JSON 反序列化</para>
    /// <para>- IsGuid → Guid 解析</para>
    /// <para>- 其他 → Convert.ChangeType 基础类型转换</para>
    /// </remarks>
    /// <param name="cellValue">单元格原始值</param>
    /// <param name="info"><see cref="ExcelPropertyInfo"/> 属性元信息</param>
    /// <returns></returns>
    private static object ConvertImportValue(object cellValue, ExcelPropertyInfo info)
    {
        if (cellValue == null)
            return null;

        var columnAttr = info.ColumnAttribute;
        var cellString = cellValue.ToString()
            ?.Trim();

        // 空字符串处理：可空类型返回 null，值类型返回默认值
        if (string.IsNullOrEmpty(cellString))
        {
            return info.IsNullable ? null : GetDefaultValue(info.UnderlyingType);
        }

        // JSON 反序列化：标记了 IsJson 的属性从 JSON 字符串反序列化
        if (columnAttr?.IsJson == true)
        {
            return cellString.ToObject(info.PropertyType);
        }

        // Bool 类型：支持 是/否、true/false、1/0 多种格式
        if (info.IsBool)
        {
            var trueText = columnAttr?.TrueText ?? "是";
            var falseText = columnAttr?.FalseText ?? "否";

            if (string.Equals(cellString, trueText, StringComparison.OrdinalIgnoreCase)
                || string.Equals(cellString, "true", StringComparison.OrdinalIgnoreCase)
                || cellString == "1")
            {
                return true;
            }

            if (string.Equals(cellString, falseText, StringComparison.OrdinalIgnoreCase)
                || string.Equals(cellString, "false", StringComparison.OrdinalIgnoreCase)
                || cellString == "0")
            {
                return false;
            }

            throw new InvalidCastException($"无法将 \"{cellString}\" 转换为布尔值，期望值为 \"{trueText}\" 或 \"{falseText}\"");
        }

        // 枚举类型：从缓存的映射表中快速查找（支持描述文本、枚举名称、数值字符串）
        if (info.IsEnum)
        {
            var mapping = GetOrBuildEnumMapping(info.UnderlyingType);

            // 从缓存映射中查找（已包含 Description、Name、数值字符串的映射）
            if (mapping.TextToValue.TryGetValue(cellString, out var enumValue))
            {
                return enumValue;
            }

            // 回退：尝试 Enum.TryParse（处理映射中未覆盖的情况）
            if (Enum.TryParse(info.UnderlyingType, cellString, true, out var parsedEnum))
            {
                return parsedEnum;
            }

            throw new InvalidCastException($"无法将 \"{cellString}\" 转换为枚举类型 {info.UnderlyingType.Name}");
        }

        // DateTime 类型：支持原生 DateTime、OLE 日期（double）、格式化字符串
        if (info.IsDateTime)
        {
            if (cellValue is DateTime dtValue)
                return dtValue;

            // Excel 内部存储日期为 OLE Automation 日期（double 类型）
            if (cellValue is double oleDate)
                return DateTime.FromOADate(oleDate);

            // 如果指定了格式化字符串，使用精确解析
            if (!string.IsNullOrEmpty(columnAttr?.Format))
            {
                return DateTime.ParseExact(cellString, columnAttr.Format, System.Globalization.CultureInfo.InvariantCulture);
            }

            return DateTime.Parse(cellString);
        }

        // DateTimeOffset 类型：支持原生 DateTimeOffset、DateTime、OLE 日期
        if (info.IsDateTimeOffset)
        {
            if (cellValue is DateTimeOffset dtoValue)
                return dtoValue;

            if (cellValue is DateTime dtValue2)
                return new DateTimeOffset(dtValue2);

            if (cellValue is double oleDate2)
                return new DateTimeOffset(DateTime.FromOADate(oleDate2));

            return DateTimeOffset.Parse(cellString);
        }

        // 值类型集合（如 List<int>、List<string>）：按分隔符拆分并转换每个元素
        if (info.IsValueTypeCollection)
        {
            var separator = columnAttr?.Separator ?? ",";
            return ParseValueTypeCollection(cellString, info.PropertyType, info.CollectionElementType, separator);
        }

        // 复杂对象集合（非值类型）：JSON 反序列化
        if (info.IsComplexCollection)
        {
            return cellString.ToObject(info.PropertyType);
        }

        // Guid 类型：解析 Guid 字符串
        if (info.IsGuid)
        {
            return Guid.Parse(cellString);
        }

        // 其他基础类型：使用 Convert.ChangeType 进行通用转换
        return Convert.ChangeType(cellValue, info.UnderlyingType);
    }

    #endregion

    #region 类型判断辅助

    /// <summary>
    /// 判断类型是否为值类型集合
    /// </summary>
    /// <remarks>如 List&lt;int&gt;、List&lt;string&gt;、int[]、string[] 等</remarks>
    /// <param name="type"><see cref="Type"/> 待检查的类型</param>
    /// <returns></returns>
    private static bool CheckIsValueTypeCollection(Type type)
    {
        // 数组类型：检查元素类型是否为值类型或常用简单类型
        if (type.IsArray)
        {
            var arrayElementType = type.GetElementType();
            return arrayElementType != null
                   && (arrayElementType.IsPrimitive
                       || arrayElementType == typeof(string)
                       || arrayElementType == typeof(decimal)
                       || arrayElementType == typeof(Guid)
                       || arrayElementType == typeof(DateTime));
        }

        if (!type.IsGenericType)
            return false;

        // 检查是否为常见的集合泛型定义
        var genericDef = type.GetGenericTypeDefinition();
        if (genericDef != typeof(List<>)
            && genericDef != typeof(IList<>)
            && genericDef != typeof(IEnumerable<>)
            && genericDef != typeof(ICollection<>))
            return false;

        // 检查元素类型是否为值类型或常用简单类型
        var elementType = type.GetGenericArguments()[0];
        return elementType.IsPrimitive
               || elementType == typeof(string)
               || elementType == typeof(decimal)
               || elementType == typeof(Guid)
               || elementType == typeof(DateTime);
    }

    /// <summary>
    /// 判断类型是否为复杂对象集合
    /// </summary>
    /// <remarks>非值类型集合但实现了 IEnumerable 的类型（数组、List&lt;对象&gt; 等）</remarks>
    /// <param name="type"><see cref="Type"/> 待检查的类型</param>
    /// <returns></returns>
    private static bool CheckIsComplexCollection(Type type)
    {
        // 值类型集合已在 CheckIsValueTypeCollection 中处理，此处排除
        if (CheckIsValueTypeCollection(type))
            return false;

        // 数组类型且非值类型数组视为复杂集合
        if (type.IsArray)
            return true;

        if (!type.IsGenericType)
            return false;

        // 实现了 IEnumerable 且不是字符串
        if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string))
        {
            return true;
        }

        return false;
    }

    #endregion

    #region 值转换辅助

    /// <summary>
    /// 将集合转换为分隔符连接的字符串
    /// </summary>
    /// <remarks>用于导出时将 List&lt;int&gt; 等值类型集合转为 "1,2,3" 格式的字符串</remarks>
    /// <param name="value">集合对象</param>
    /// <param name="separator">分隔符</param>
    /// <returns></returns>
    private static string JoinCollection(object value, string separator)
    {
        if (value is not IEnumerable enumerable)
            return value?.ToString();

        var items = new List<string>();
        foreach (var item in enumerable)
        {
            items.Add(item?.ToString() ?? string.Empty);
        }

        return string.Join(separator, items);
    }

    /// <summary>
    /// 将分隔符字符串解析为值类型集合
    /// </summary>
    /// <remarks>用于导入时将 "1,2,3" 格式的字符串转为 List&lt;int&gt;、int[] 等值类型集合</remarks>
    /// <param name="text">分隔符连接的字符串</param>
    /// <param name="collectionType">目标集合类型</param>
    /// <param name="elementType">集合元素类型（从缓存获取，避免重复 GetGenericArguments）</param>
    /// <param name="separator">分隔符</param>
    /// <returns></returns>
    private static object ParseValueTypeCollection(string text, Type collectionType, Type elementType, string separator)
    {
        /*
         * 防御性编程：正常情况下 elementType 在 ExcelPropertyInfo 初始化时已缓存，
         * 此处兜底处理仅用于方法被独立调用时的安全保障
         */
        elementType ??= collectionType.IsArray ? collectionType.GetElementType() : collectionType.GetGenericArguments()[0];

        // 按分隔符拆分并去除空项
        var parts = text.Split([separator], StringSplitOptions.RemoveEmptyEntries);

        // 数组类型：直接创建对应类型的数组
        if (collectionType.IsArray)
        {
            var convertedParts = new List<object>(parts.Length);
            foreach (var part in parts)
            {
                var trimmed = part.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    convertedParts.Add(Convert.ChangeType(trimmed, elementType));
                }
            }

            var array = Array.CreateInstance(elementType, convertedParts.Count);
            for (var i = 0; i < convertedParts.Count; i++)
            {
                array.SetValue(convertedParts[i], i);
            }

            return array;
        }

        // 泛型集合类型：创建对应类型的 List<T> 实例
        var listType = typeof(List<>).MakeGenericType(elementType);
        var list = (IList) Activator.CreateInstance(listType);

        // 逐项转换并添加到列表
        foreach (var part in parts)
        {
            var trimmed = part.Trim();
            if (!string.IsNullOrEmpty(trimmed))
            {
                list?.Add(Convert.ChangeType(trimmed, elementType));
            }
        }

        return list;
    }

    /// <summary>
    /// 获取类型的默认值
    /// <remarks>值类型返回 default(T)，引用类型返回 null</remarks>
    /// </summary>
    /// <param name="type"><see cref="Type"/> 目标类型</param>
    /// <returns></returns>
    private static object GetDefaultValue(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }

    #endregion
}