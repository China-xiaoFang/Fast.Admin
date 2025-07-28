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

using System.Text;

// ReSharper disable once CheckNamespace
namespace Fast.Tool.ConsoleApp;

internal class DtoUtil
{
    /// <summary>
    /// 处理 Dto 类型
    /// </summary>
    /// <param name="key"></param>
    /// <param name="importList"></param>
    /// <returns></returns>
    public static string DisposeDtoValueType(string key, List<string> importList = null)
    {
        if (string.IsNullOrEmpty(key))
            return null;

        var localKey = key.Split("/")
            .LastOrDefault();

        if (string.IsNullOrEmpty(localKey))
            return null;

        // 处理导入映射
        var exportMapDto = SwaggerCommon.ExportMapDtoList.FirstOrDefault(f => f.Key == localKey);
        if (exportMapDto.Key != null)
        {
            importList?.Add(exportMapDto.Value.name);
            return exportMapDto.Value.name;
        }

        var addKey = "";

        var isAdd = false;

        foreach (var requestDataTypeInfo in SwaggerCommon.ApiRequestDataTypeList)
        {
            if (!localKey.StartsWith(requestDataTypeInfo.StartStr))
                continue;

            isAdd = true;

            if (!string.IsNullOrEmpty(requestDataTypeInfo.SubStr))
            {
                localKey = localKey[requestDataTypeInfo.SubStr.Length..];
            }

            if (requestDataTypeInfo.LibAddStrList?.Count > 0 && importList != null)
            {
                importList.AddRange(requestDataTypeInfo.LibAddStrList);
            }

            // 判断是否为基础类型
            if (SwaggerCommon.SwaggerBaseTypeInfoList.Any(a => a.SwaggerType == localKey))
            {
                var baseTypeKey = DisposeBaseType(localKey);

                if (!string.IsNullOrEmpty(baseTypeKey))
                {
                    addKey = null;
                    localKey = baseTypeKey;
                }
            }
            else
            {
                addKey = localKey;
            }

            if (!string.IsNullOrEmpty(requestDataTypeInfo.ResultDataType))
            {
                localKey = string.Format(requestDataTypeInfo.ResultDataType, localKey);
            }

            if (requestDataTypeInfo.DisabledImport)
            {
                addKey = null;
            }
        }

        // 如果没找到，直接添加
        if (!isAdd)
        {
            addKey = localKey;
        }

        if (!string.IsNullOrEmpty(addKey))
        {
            // 避免数组[]结尾
            importList?.Add(addKey.EndsWith("[]") ? addKey[..^2] : addKey);
        }

        return localKey;
    }

    /// <summary>
    /// 处理基础类型
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string DisposeBaseType(string key)
    {
        foreach (var swaggerBaseTypeInfo in SwaggerCommon.SwaggerBaseTypeInfoList)
        {
            if (key != swaggerBaseTypeInfo.SwaggerType)
                continue;

            return swaggerBaseTypeInfo.Type;
        }

        return "any";
    }

    /// <summary>
    /// 获取Dto导入
    /// </summary>
    /// <param name="libName"></param>
    /// <param name="key"></param>
    /// <param name="generateEnumResult"></param>
    /// <param name="rootPath"></param>
    /// <returns></returns>
    public static string GetDtoImportByKey(string libName, string key, List<GenerateEnumResult> generateEnumResult,
        string rootPath = "")
    {
        if (string.IsNullOrEmpty(key))
            return null;

        // 处理组件库导入
        if (SwaggerCommon.ElementPlus.Contains(key))
        {
            return @$"import {{ {key} }} from ""{libName}"";";
        }

        // 处理导入映射
        var exportMapDto = SwaggerCommon.ExportMapDtoList.FirstOrDefault(f => f.Key == key);
        if (exportMapDto.Key != null)
        {
            return $@"import {{ {exportMapDto.Value.name} }} from ""{exportMapDto.Value.importUrl}"";";
        }

        // 判断是否为枚举
        var fEnumResult = generateEnumResult.SingleOrDefault(s => s.EnumName == key);
        if (fEnumResult != null)
        {
            return fEnumResult.Import;
        }

        // 判断是否为固定写入
        var fixedWrite = SwaggerCommon.FixedWriteList.SingleOrDefault(s => s.Key == key);
        if (fixedWrite.Value.importUrl != null)
        {
            return $@"import {{ {key} }} from ""{fixedWrite.Value.importUrl}"";";
        }

        // 在当前根目录下
        return $@"import {{ {key} }} from ""./{rootPath}{key}"";";
    }

    /// <summary>
    /// 生成Dto到TS文件
    /// </summary>
    /// <param name="swaggerDocUrl"></param>
    /// <param name="jsonData"></param>
    /// <param name="generateDtoResult"></param>
    public static void GenerateDtoToTypeScript(SwaggerDocUrl swaggerDocUrl, SwaggerDocInfo jsonData,
        List<GenerateDtoResult> generateDtoResult)
    {
        generateDtoResult ??= [];

        var schemasList = jsonData.Components.Schemas
            // 不是枚举
            .Where(wh => wh.Value.Enum == null)
            // 不是 RestfulResult_ PagedResult_ 开头
            .Where(wh => !wh.Key.StartsWith("RestfulResult_") && !wh.Key.StartsWith("PagedResult_"))
            .ToList();

        foreach (var swaggerDocComponentsSchema in schemasList)
        {
            // 处理Key
            var refKey = DisposeDtoValueType(swaggerDocComponentsSchema.Key);

            // 判断是否为基类
            if (SwaggerCommon.SwaggerBaseTypeInfoList.Any(a => a.SwaggerType == refKey))
            {
                continue;
            }

            // 导出忽略集合
            if (SwaggerCommon.ExcludeDtoList.Contains(refKey))
                continue;

            var findGenerateDtoResult = generateDtoResult.SingleOrDefault(s => s.DtoName == refKey);
            if (findGenerateDtoResult != null)
            {
                continue;
            }

            var dtoResult = new GenerateDtoResult {DtoName = refKey, Content = new StringBuilder(), RefList = []};

            dtoResult.Content.Append(@$"/**
 *{(string.IsNullOrEmpty(swaggerDocComponentsSchema.Value.Description) ? "" : $" {swaggerDocComponentsSchema.Value.Description?.Replace("\r\n", "\r\n * ")}")}
 */
export interface {refKey} ");

            // 判断是否存在分页的类型
            if (swaggerDocComponentsSchema.Value.Properties?.Any(a => a.Key == "isPage") == true)
            {
                dtoResult.Content.Append("extends PageInput ");
                dtoResult.RefList.Add("PageInput");
            }

            dtoResult.Content.Append("{");

            if (swaggerDocComponentsSchema.Value.Properties != null)
            {
                foreach (var property in swaggerDocComponentsSchema.Value.Properties)
                {
                    if (string.IsNullOrEmpty(property.Key))
                        continue;

                    // 判断是否为分页字段
                    if (SwaggerCommon.PagedInputList.Contains(property.Key))
                        continue;

                    dtoResult.Content.Append(Environment.NewLine);
                    dtoResult.Content.Append(@$"	/**
	 *{(string.IsNullOrEmpty(property.Value.Description) ? "" : $" {property.Value.Description?.Replace("\r\n\r\n", "\r\n").Replace("\r\n", "\r\n	 * ")}")}
	 */
	{(property.Value.ReadOnly ? "readonly " : "")}{property.Key}?: ");

                    // 判断是否为引用的
                    if (property.Value.Ref != null)
                    {
                        var propertyRefKey = DisposeDtoValueType(property.Value.Ref, dtoResult.RefList);
                        dtoResult.Content.Append($"{propertyRefKey};");
                    }
                    else if (property.Value.Type == "array")
                    {
                        if (property.Value.Items.Ref != null)
                        {
                            var propertyRefKey = DisposeDtoValueType(property.Value.Items.Ref, dtoResult.RefList);
                            dtoResult.Content.Append($"Array<{propertyRefKey}>;");
                        }
                        else
                        {
                            var propertyRefKey = DisposeBaseType(property.Value.Items.Type);
                            dtoResult.Content.Append($"Array<{propertyRefKey}>;");
                        }
                    }
                    else
                    {
                        var propertyRefKey = DisposeBaseType(property.Value.Format ?? property.Value.Type);
                        dtoResult.Content.Append($"{propertyRefKey};");
                    }
                }
            }

            dtoResult.Content.Append(Environment.NewLine);
            dtoResult.Content.AppendLine("}");

            // 处理嵌套引用和重复引用的
            dtoResult.RefList = dtoResult.RefList.Where(wh => wh != dtoResult.DtoName)
                .Distinct()
                .ToList();

            generateDtoResult.Add(dtoResult);
        }
    }

    /// <summary>
    /// 处理Dto文件的导入
    /// </summary>
    /// <param name="libName"></param>
    /// <param name="swaggerDocUrl"></param>
    /// <param name="jsonData"></param>
    /// <param name="generateDtoResult"></param>
    /// <param name="generateEnumResult"></param>
    public static void DisposeDtoImport(string libName, SwaggerDocUrl swaggerDocUrl, SwaggerDocInfo jsonData,
        List<GenerateDtoResult> generateDtoResult, List<GenerateEnumResult> generateEnumResult)
    {
        foreach (var dtoResult in generateDtoResult)
        {
            var importSb = new StringBuilder();
            foreach (var refKey in dtoResult.RefList)
            {
                importSb.AppendLine(GetDtoImportByKey(libName, refKey, generateEnumResult));
            }

            if (dtoResult.RefList.Count > 0)
            {
                importSb.Append(Environment.NewLine);
            }

            // 插入引用
            dtoResult.Content.Insert(0, importSb.ToString());
        }
    }

    /// <summary>
    /// 写入Dto文件
    /// </summary>
    /// <param name="fileDir"></param>
    /// <param name="dtoResult"></param>
    /// <param name="generateDtoResult"></param>
    /// <param name="generateEnumResult"></param>
    /// <returns></returns>
    public static bool WriteDtoFile(string fileDir, GenerateDtoResult dtoResult, List<GenerateDtoResult> generateDtoResult,
        List<GenerateEnumResult> generateEnumResult)
    {
        // 判断是否已经写入
        if (dtoResult.IsWrite)
            return false;

        if (SwaggerCommon.ElementPlus.Contains(dtoResult.DtoName))
            return false;

        // 判断是否为固定写入
        var fixedWrite = SwaggerCommon.FixedWriteList.SingleOrDefault(s => s.Key == dtoResult.DtoName);
        if (fixedWrite.Value.importUrl != null)
        {
            if (!fileDir.EndsWith(fixedWrite.Value.filePath))
            {
                return false;
            }
        }

        // 标记写入
        dtoResult.IsWrite = true;

        File.WriteAllText(Path.Combine(fileDir, $"{dtoResult.DtoName}.ts"), dtoResult.Content.ToString());

        // 还存在引用的问题
        foreach (var item in dtoResult.RefList.Where(wh => !SwaggerCommon.ElementPlus.Contains(wh))
                     .Distinct())
        {
            // 优先从枚举中查找
            if (generateEnumResult.Any(a => a.EnumName == item))
                continue;

            // 从集合中查找
            var dtoResult1 = generateDtoResult.Single(s => s.DtoName == item);
            WriteDtoFile(fileDir, dtoResult1, generateDtoResult, generateEnumResult);
        }

        return true;
    }
}