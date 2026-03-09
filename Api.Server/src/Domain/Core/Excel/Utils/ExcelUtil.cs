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

using Microsoft.AspNetCore.Mvc;
using MiniExcelLibs;
using MiniExcelLibs.OpenXml;
using System.Reflection;

namespace Fast.Core;

/// <summary>
/// <see cref="ExcelUtil"/> Excel工具类
/// </summary>
/// <remarks>基于 MiniExcel 封装的 Excel 导入导出工具，支持 DTO 特性驱动</remarks>
[SuppressSniffer]
public static class ExcelUtil
{
    #region 导出

    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"><see cref="IEnumerable{T}"/> 数据集合</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称</param>
    /// <param name="excelType"><see cref="ExcelType"/> Excel类型</param>
    /// <returns></returns>
    public static MemoryStream ExportExcel<T>(IEnumerable<T> data, string sheetName = "Sheet1",
        ExcelType excelType = ExcelType.XLSX)
    {
        var memoryStream = new MemoryStream();
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
    /// <returns></returns>
    public static async Task<MemoryStream> ExportExcelAsync<T>(IEnumerable<T> data, string sheetName = "Sheet1",
        ExcelType excelType = ExcelType.XLSX)
    {
        var memoryStream = new MemoryStream();
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
    /// <returns></returns>
    public static FileStreamResult ExportExcel<T>(IEnumerable<T> data, string fileName, string sheetName = "Sheet1",
        ExcelType excelType = ExcelType.XLSX)
    {
        var memoryStream = ExportExcel(data, sheetName, excelType);

        return new FileStreamResult(memoryStream, "application/octet-stream") { FileDownloadName = fileName.UrlEncode() };
    }

    /// <summary>
    /// 导出Excel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"><see cref="IEnumerable{T}"/> 数据集合</param>
    /// <param name="fileName"><see cref="string"/> 文件名称</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称</param>
    /// <param name="excelType"><see cref="ExcelType"/> Excel类型</param>
    /// <returns></returns>
    public static async Task<FileStreamResult> ExportExcelAsync<T>(IEnumerable<T> data,string fileName, string sheetName = "Sheet1",
        ExcelType excelType = ExcelType.XLSX)
    {
        var memoryStream = await ExportExcelAsync(data, sheetName, excelType);

        return new FileStreamResult(memoryStream, "application/octet-stream") { FileDownloadName = fileName.UrlEncode() };
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
        ExcelType excelType = ExcelType.XLSX)
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

        MiniExcel.SaveAs(filePath, exportData, sheetName: sheetName, excelType: excelType,
            configuration: config);
    }

    /// <summary>
    /// 导出到文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"><see cref="string"/> 文件路径</param>
    /// <param name="data"><see cref="IEnumerable{T}"/> 数据集合</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称</param>
    /// <param name="excelType"><see cref="ExcelType"/> Excel类型</param>
    public static async Task ExportToFileAsync<T>(string filePath, IEnumerable<T> data, string sheetName = "Sheet1",
        ExcelType excelType = ExcelType.XLSX)
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

        await MiniExcel.SaveAsAsync(filePath, exportData, sheetName: sheetName, excelType: excelType,
            configuration: config);
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

    }

    /// <summary>
    /// 导入Excel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream"><see cref="Stream"/> Excel文件流</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称，为空时读取第一个Sheet</param>
    /// <param name="excelType"><see cref="ExcelType"/> Excel类型</param>
    /// <returns><see cref="ExcelImportResult{T}"/></returns>
    public static async Task<ExcelImportResult<T>> ImportExcelAsync<T>(Stream stream, string sheetName = null,
        ExcelType excelType = ExcelType.XLSX) where T : class, new()
    {

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

    }

    /// <summary>
    /// 从文件导入Excel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"><see cref="string"/> 文件路径</param>
    /// <param name="sheetName"><see cref="string"/> Sheet名称，为空时读取第一个Sheet</param>
    /// <param name="excelType"><see cref="ExcelType"/> Excel类型</param>
    /// <returns><see cref="ExcelImportResult{T}"/></returns>
    public static async Task<ExcelImportResult<T>> ImportExcelAsync<T>(string filePath, string sheetName = null,
        ExcelType excelType = ExcelType.XLSX) where T : class, new()
    {

    }

    #endregion

    /// <summary>
    /// 获取导出属性信息列表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    internal static List<ExcelPropertyInfo> GetExportPropertyInfos<T>() where T : class, new()
    {
        // 获取所有属性
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var result = new List<ExcelPropertyInfo>();

        // 遍历所有属性
        foreach (var prop in properties)
        {

        }
    }
}
