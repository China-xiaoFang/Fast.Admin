// ------------------------------------------------------------------------
// Apache开源许可证
// 
// 版权所有 © 2018-Now 小方
// 
// 许可授权：
// 本协议授予任何获得本软件及其相关文档（以下简称"软件"）副本的个人或组织。
// 在遵守本协议条款的前提下，享有使用、复制、修改、合并、发布、分发、再许可、销售软件副本的权利：
// 1.所有软件副本或主要部分必须保留本版权声明及本许可协议。
// 2.软件的使用、复制、修改或分发不得违反适用法律或侵犯他人合法权益。
// 3.修改或衍生作品须明确标注原作者及原软件出处。
// 
// 特别声明：
// - 本软件按"原样"提供，不提供任何形式的明示或暗示的保证，包括但不限于对适销性、适用性和非侵权的保证。
// - 在任何情况下，作者或版权持有人均不对因使用或无法使用本软件导致的任何直接或间接损失的责任。
// - 包括但不限于数据丢失、业务中断等情况。
// 
// 免责条款：
// 禁止利用本软件从事危害国家安全、扰乱社会秩序或侵犯他人合法权益等违法活动。
// 对于基于本软件二次开发所引发的任何法律纠纷及责任，作者不承担任何责任。
// ------------------------------------------------------------------------

using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using MiniExcelLibs;
using MiniExcelLibs.Attributes;
using MiniExcelLibs.OpenXml;

namespace Fast.Core;

/// <summary>
/// <see cref="ExcelUtil"/> Excel工具类
/// <remarks>基于 MiniExcel 封装的 Excel 导入导出工具，支持 DTO 特性驱动</remarks>
/// </summary>
[SuppressSniffer]
public static class ExcelUtil
{
    #region 导出

    /// <summary>
    /// 导出到 Stream
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"><see cref="IEnumerable{T}"/> 数据集合</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称</param>
    /// <returns><see cref="MemoryStream"/></returns>
    public static MemoryStream ExportToStream<T>(IEnumerable<T> data, string sheetName = "Sheet1") where T : class
    {
        var memoryStream = new MemoryStream();
        ExportToStream(memoryStream, data, sheetName);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
    }

    /// <summary>
    /// 导出到指定 Stream
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream"><see cref="Stream"/> 输出流</param>
    /// <param name="data"><see cref="IEnumerable{T}"/> 数据集合</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称</param>
    public static void ExportToStream<T>(Stream stream, IEnumerable<T> data, string sheetName = "Sheet1")
        where T : class
    {
        var propertyInfos = GetExportPropertyInfos<T>();
        var exportData = ConvertExportData(data, propertyInfos);

        var config = new OpenXmlConfiguration();
        var dynamicColumns = BuildDynamicColumns(propertyInfos);
        if (dynamicColumns.Count > 0)
        {
            config.DynamicColumns = dynamicColumns.ToArray();
        }

        stream.SaveAs(exportData, sheetName: sheetName, excelType: ExcelType.XLSX, configuration: config);
    }

    /// <summary>
    /// 导出到字节数组
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"><see cref="IEnumerable{T}"/> 数据集合</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称</param>
    /// <returns><see cref="T:byte[]"/></returns>
    public static byte[] ExportToBytes<T>(IEnumerable<T> data, string sheetName = "Sheet1") where T : class
    {
        using var stream = ExportToStream(data, sheetName);
        return stream.ToArray();
    }

    /// <summary>
    /// 导出到文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"><see cref="string"/> 文件路径</param>
    /// <param name="data"><see cref="IEnumerable{T}"/> 数据集合</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称</param>
    public static void ExportToFile<T>(string filePath, IEnumerable<T> data, string sheetName = "Sheet1")
        where T : class
    {
        var propertyInfos = GetExportPropertyInfos<T>();
        var exportData = ConvertExportData(data, propertyInfos);

        var config = new OpenXmlConfiguration();
        var dynamicColumns = BuildDynamicColumns(propertyInfos);
        if (dynamicColumns.Count > 0)
        {
            config.DynamicColumns = dynamicColumns.ToArray();
        }

        MiniExcel.SaveAs(filePath, exportData, sheetName: sheetName, excelType: ExcelType.XLSX,
            configuration: config);
    }

    /// <summary>
    /// 导出模板到 Stream
    /// <remarks>导出一个只有表头的空模板</remarks>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sheetName"><see cref="string"/> Sheet名称</param>
    /// <returns><see cref="MemoryStream"/></returns>
    public static MemoryStream ExportTemplate<T>(string sheetName = "Sheet1") where T : class
    {
        return ExportToStream(Array.Empty<T>(), sheetName);
    }

    #endregion

    #region 导入

    /// <summary>
    /// 从 Stream 导入
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream"><see cref="Stream"/> Excel文件流</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称，为空时读取第一个Sheet</param>
    /// <returns><see cref="ExcelImportResult{T}"/></returns>
    public static ExcelImportResult<T> Import<T>(Stream stream, string sheetName = null) where T : class, new()
    {
        var rows = stream.Query(sheetName: sheetName, useHeaderRow: true, excelType: ExcelType.XLSX)
            .Cast<IDictionary<string, object>>().ToList();

        return ParseImportData<T>(rows);
    }

    /// <summary>
    /// 从文件路径导入
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"><see cref="string"/> 文件路径</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称，为空时读取第一个Sheet</param>
    /// <returns><see cref="ExcelImportResult{T}"/></returns>
    public static ExcelImportResult<T> Import<T>(string filePath, string sheetName = null) where T : class, new()
    {
        var rows = MiniExcel.Query(filePath, sheetName: sheetName, useHeaderRow: true, excelType: ExcelType.XLSX)
            .Cast<IDictionary<string, object>>().ToList();

        return ParseImportData<T>(rows);
    }

    /// <summary>
    /// 从 Stream 导入（自动检测格式）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream"><see cref="Stream"/> Excel文件流</param>
    /// <param name="excelType"><see cref="ExcelType"/> Excel文件类型</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称，为空时读取第一个Sheet</param>
    /// <returns><see cref="ExcelImportResult{T}"/></returns>
    public static ExcelImportResult<T> Import<T>(Stream stream, ExcelType excelType, string sheetName = null)
        where T : class, new()
    {
        var rows = stream.Query(sheetName: sheetName, useHeaderRow: true, excelType: excelType)
            .Cast<IDictionary<string, object>>().ToList();

        return ParseImportData<T>(rows);
    }

    #endregion

    #region 私有方法 - 导出

    /// <summary>
    /// 获取导出属性信息列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static List<ExcelPropertyInfo> GetExportPropertyInfos<T>()
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var result = new List<ExcelPropertyInfo>();

        foreach (var prop in properties)
        {
            var columnAttr = prop.GetCustomAttribute<ExcelColumnAttribute>();

            // 忽略标记了 Ignore 的属性
            if (columnAttr?.Ignore == true)
                continue;

            var info = new ExcelPropertyInfo
            {
                Property = prop,
                ColumnAttribute = columnAttr,
                ColumnName = columnAttr?.Name ?? prop.Name,
                Order = columnAttr?.Order ?? int.MaxValue,
                RequiredAttribute = prop.GetCustomAttribute<ExcelRequiredAttribute>(),
                RegexAttributes = prop.GetCustomAttributes<ExcelRegexAttribute>().ToList()
            };

            result.Add(info);
        }

        return result.OrderBy(p => p.Order).ThenBy(p => p.ColumnName).ToList();
    }

    /// <summary>
    /// 构建动态列配置
    /// </summary>
    /// <param name="propertyInfos"></param>
    /// <returns></returns>
    private static List<DynamicExcelColumn> BuildDynamicColumns(List<ExcelPropertyInfo> propertyInfos)
    {
        var columns = new List<DynamicExcelColumn>();

        for (var i = 0; i < propertyInfos.Count; i++)
        {
            var info = propertyInfos[i];
            var column = new DynamicExcelColumn(info.ColumnName) { Index = i, Width = info.ColumnAttribute?.Width ?? 0 };

            if (!string.IsNullOrEmpty(info.ColumnAttribute?.Format))
            {
                column.Format = info.ColumnAttribute.Format;
            }

            columns.Add(column);
        }

        return columns;
    }

    /// <summary>
    /// 转换导出数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="propertyInfos"></param>
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
                var value = info.Property.GetValue(item);
                dict[info.ColumnName] = ConvertExportValue(value, info);
            }

            result.Add(dict);
        }

        return result;
    }

    /// <summary>
    /// 转换导出值
    /// </summary>
    /// <param name="value"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    private static object ConvertExportValue(object value, ExcelPropertyInfo info)
    {
        if (value == null)
            return null;

        var columnAttr = info.ColumnAttribute;
        var propertyType = info.Property.PropertyType;
        var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

        // JSON 序列化
        if (columnAttr?.IsJson == true)
        {
            return value.ToJsonString();
        }

        // Bool 类型
        if (underlyingType == typeof(bool))
        {
            var boolValue = (bool)value;
            var trueText = columnAttr?.TrueText ?? "是";
            var falseText = columnAttr?.FalseText ?? "否";
            return boolValue ? trueText : falseText;
        }

        // 枚举类型 - 使用 Description 特性
        if (underlyingType.IsEnum)
        {
            return GetEnumDescription(underlyingType, value);
        }

        // DateTime 类型 - 使用 Format 格式化
        if (underlyingType == typeof(DateTime))
        {
            var dateValue = (DateTime)value;
            if (!string.IsNullOrEmpty(columnAttr?.Format))
            {
                return dateValue.ToString(columnAttr.Format);
            }

            return dateValue;
        }

        // DateTimeOffset 类型
        if (underlyingType == typeof(DateTimeOffset))
        {
            var dateValue = (DateTimeOffset)value;
            if (!string.IsNullOrEmpty(columnAttr?.Format))
            {
                return dateValue.ToString(columnAttr.Format);
            }

            return dateValue;
        }

        // 值类型集合 - 使用分隔符连接
        if (IsValueTypeCollection(propertyType))
        {
            var separator = columnAttr?.Separator ?? ",";
            return JoinCollection(value, separator);
        }

        // 复杂对象集合（非值类型集合） - 默认 JSON 序列化
        if (IsComplexCollection(propertyType))
        {
            return value.ToJsonString();
        }

        return value;
    }

    #endregion

    #region 私有方法 - 导入

    /// <summary>
    /// 解析导入数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rows"></param>
    /// <returns></returns>
    private static ExcelImportResult<T> ParseImportData<T>(List<IDictionary<string, object>> rows)
        where T : class, new()
    {
        var result = new ExcelImportResult<T>();
        var propertyInfos = GetExportPropertyInfos<T>();

        // 构建列名到属性的映射
        var columnMap = new Dictionary<string, ExcelPropertyInfo>(StringComparer.OrdinalIgnoreCase);
        foreach (var info in propertyInfos)
        {
            columnMap[info.ColumnName] = info;
            // 同时支持属性名匹配
            if (info.ColumnName != info.Property.Name)
            {
                columnMap[info.Property.Name] = info;
            }
        }

        for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
        {
            var row = rows[rowIndex];
            var item = new T();
            var rowErrors = new List<ExcelImportError>();
            var rowNumber = rowIndex + 1;

            foreach (var info in propertyInfos)
            {
                // 查找对应的列值
                object cellValue = null;
                var columnFound = false;

                if (row.ContainsKey(info.ColumnName))
                {
                    cellValue = row[info.ColumnName];
                    columnFound = true;
                }
                else if (info.ColumnName != info.Property.Name && row.ContainsKey(info.Property.Name))
                {
                    cellValue = row[info.Property.Name];
                    columnFound = true;
                }

                // 必填验证
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
                        continue;
                    }
                }

                if (!columnFound || cellValue == null)
                    continue;

                var cellString = cellValue.ToString()?.Trim();

                // 正则验证
                if (!string.IsNullOrEmpty(cellString))
                {
                    foreach (var regexAttr in info.RegexAttributes)
                    {
                        if (!Regex.IsMatch(cellString, regexAttr.Pattern))
                        {
                            rowErrors.Add(new ExcelImportError
                            {
                                RowIndex = rowNumber,
                                ColumnName = info.ColumnName,
                                PropertyName = info.Property.Name,
                                CellValue = cellValue,
                                ErrorMessage = string.IsNullOrEmpty(regexAttr.ErrorMessage)
                                    ? $"{info.ColumnName} 格式不正确"
                                    : regexAttr.ErrorMessage
                            });
                        }
                    }
                }

                // 如果有正则错误，跳过赋值
                if (rowErrors.Any(e => e.RowIndex == rowNumber && e.PropertyName == info.Property.Name))
                    continue;

                // 类型转换并赋值
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

            result.Data.Add(item);
            result.Errors.AddRange(rowErrors);
        }

        return result;
    }

    /// <summary>
    /// 转换导入值
    /// </summary>
    /// <param name="cellValue"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    private static object ConvertImportValue(object cellValue, ExcelPropertyInfo info)
    {
        if (cellValue == null)
            return null;

        var propertyType = info.Property.PropertyType;
        var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
        var isNullable = Nullable.GetUnderlyingType(propertyType) != null;
        var columnAttr = info.ColumnAttribute;
        var cellString = cellValue.ToString()?.Trim();

        if (string.IsNullOrEmpty(cellString))
        {
            return isNullable ? null : GetDefaultValue(underlyingType);
        }

        // JSON 反序列化
        if (columnAttr?.IsJson == true)
        {
            return cellString.ToObject(propertyType);
        }

        // Bool 类型
        if (underlyingType == typeof(bool))
        {
            var trueText = columnAttr?.TrueText ?? "是";
            var falseText = columnAttr?.FalseText ?? "否";

            if (string.Equals(cellString, trueText, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(cellString, "true", StringComparison.OrdinalIgnoreCase) ||
                cellString == "1")
            {
                return true;
            }

            if (string.Equals(cellString, falseText, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(cellString, "false", StringComparison.OrdinalIgnoreCase) ||
                cellString == "0")
            {
                return false;
            }

            throw new InvalidCastException($"无法将 \"{cellString}\" 转换为布尔值，期望值为 \"{trueText}\" 或 \"{falseText}\"");
        }

        // 枚举类型
        if (underlyingType.IsEnum)
        {
            return ParseEnum(underlyingType, cellString);
        }

        // DateTime 类型
        if (underlyingType == typeof(DateTime))
        {
            if (cellValue is DateTime dtValue)
                return dtValue;

            if (cellValue is double oleDate)
                return DateTime.FromOADate(oleDate);

            if (!string.IsNullOrEmpty(columnAttr?.Format))
            {
                return DateTime.ParseExact(cellString, columnAttr.Format,
                    System.Globalization.CultureInfo.InvariantCulture);
            }

            return DateTime.Parse(cellString);
        }

        // DateTimeOffset 类型
        if (underlyingType == typeof(DateTimeOffset))
        {
            if (cellValue is DateTimeOffset dtoValue)
                return dtoValue;

            if (cellValue is DateTime dtValue2)
                return new DateTimeOffset(dtValue2);

            if (cellValue is double oleDate2)
                return new DateTimeOffset(DateTime.FromOADate(oleDate2));

            return DateTimeOffset.Parse(cellString);
        }

        // 值类型集合
        if (IsValueTypeCollection(propertyType))
        {
            var separator = columnAttr?.Separator ?? ",";
            return ParseValueTypeCollection(cellString, propertyType, separator);
        }

        // 复杂对象集合 - JSON 反序列化
        if (IsComplexCollection(propertyType))
        {
            return cellString.ToObject(propertyType);
        }

        // Guid 类型
        if (underlyingType == typeof(Guid))
        {
            return Guid.Parse(cellString);
        }

        // 基础类型转换
        return Convert.ChangeType(cellValue, underlyingType);
    }

    #endregion

    #region 私有方法 - 辅助

    /// <summary>
    /// 获取枚举描述
    /// </summary>
    /// <param name="enumType"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private static string GetEnumDescription(Type enumType, object value)
    {
        var name = System.Enum.GetName(enumType, value);
        if (name == null)
            return value.ToString();

        var field = enumType.GetField(name);
        var descAttr = field?.GetCustomAttribute<DescriptionAttribute>();
        return descAttr?.Description ?? name;
    }

    /// <summary>
    /// 解析枚举值
    /// <remarks>支持 Description 特性匹配、枚举名称匹配和数值匹配</remarks>
    /// </summary>
    /// <param name="enumType"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    private static object ParseEnum(Type enumType, string text)
    {
        // 1. 先尝试按 Description 匹配
        foreach (var field in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var descAttr = field.GetCustomAttribute<DescriptionAttribute>();
            if (descAttr != null && string.Equals(descAttr.Description, text, StringComparison.OrdinalIgnoreCase))
            {
                return field.GetValue(null);
            }
        }

        // 2. 再尝试按枚举名称匹配
        if (System.Enum.TryParse(enumType, text, true, out var enumValue))
        {
            return enumValue;
        }

        // 3. 再尝试按数值匹配
        if (int.TryParse(text, out var intValue))
        {
            if (System.Enum.IsDefined(enumType, intValue))
            {
                return System.Enum.ToObject(enumType, intValue);
            }
        }

        if (long.TryParse(text, out var longValue))
        {
            if (System.Enum.IsDefined(enumType, longValue))
            {
                return System.Enum.ToObject(enumType, longValue);
            }
        }

        throw new InvalidCastException($"无法将 \"{text}\" 转换为枚举类型 {enumType.Name}");
    }

    /// <summary>
    /// 判断是否为值类型集合
    /// <remarks>如 List&lt;int&gt;、List&lt;string&gt;、List&lt;long&gt; 等</remarks>
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool IsValueTypeCollection(Type type)
    {
        if (!type.IsGenericType)
            return false;

        var genericDef = type.GetGenericTypeDefinition();
        if (genericDef != typeof(List<>) && genericDef != typeof(IList<>) &&
            genericDef != typeof(IEnumerable<>) && genericDef != typeof(ICollection<>))
            return false;

        var elementType = type.GetGenericArguments()[0];
        return elementType.IsPrimitive || elementType == typeof(string) || elementType == typeof(decimal) ||
               elementType == typeof(Guid) || elementType == typeof(DateTime);
    }

    /// <summary>
    /// 判断是否为复杂对象集合
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool IsComplexCollection(Type type)
    {
        if (type.IsArray)
            return true;

        if (!type.IsGenericType)
            return false;

        // 非值类型集合且实现了 IEnumerable
        if (typeof(IEnumerable).IsAssignableFrom(type) && !IsValueTypeCollection(type) &&
            type != typeof(string))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 集合转字符串
    /// </summary>
    /// <param name="value"></param>
    /// <param name="separator"></param>
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
    /// 解析值类型集合
    /// </summary>
    /// <param name="text"></param>
    /// <param name="collectionType"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    private static object ParseValueTypeCollection(string text, Type collectionType, string separator)
    {
        var elementType = collectionType.GetGenericArguments()[0];
        var parts = text.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);

        var listType = typeof(List<>).MakeGenericType(elementType);
        var list = (IList)Activator.CreateInstance(listType);

        foreach (var part in parts)
        {
            var trimmed = part.Trim();
            if (!string.IsNullOrEmpty(trimmed))
            {
                list.Add(Convert.ChangeType(trimmed, elementType));
            }
        }

        return list;
    }

    /// <summary>
    /// 获取类型默认值
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static object GetDefaultValue(Type type)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }

    #endregion

    #region 内部类

    /// <summary>
    /// Excel属性信息
    /// </summary>
    private class ExcelPropertyInfo
    {
        /// <summary>
        /// 属性信息
        /// </summary>
        public PropertyInfo Property { get; set; }

        /// <summary>
        /// 列特性
        /// </summary>
        public ExcelColumnAttribute ColumnAttribute { get; set; }

        /// <summary>
        /// 列名称
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 必填验证特性
        /// </summary>
        public ExcelRequiredAttribute RequiredAttribute { get; set; }

        /// <summary>
        /// 正则验证特性列表
        /// </summary>
        public List<ExcelRegexAttribute> RegexAttributes { get; set; } = new();
    }

    #endregion
}
