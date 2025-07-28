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

using System.ComponentModel;
using System.Reflection;
using System.Text;
using Fast.Runtime;

// ReSharper disable once CheckNamespace
namespace Fast.Tool.ConsoleApp;

internal class EnumUtil
{
    private static IEnumerable<Assembly> Assemblies { get; }

    static EnumUtil()
    {
        // 加载程序集
        Assemblies = Assembly.GetEntryAssembly()
            .GetEntryReferencedAssembly();
    }

    /// <summary>
    /// 生成枚举到TS文件
    /// </summary>
    /// <param name="fileDir"></param>
    /// <param name="swaggerDocUrl"></param>
    /// <param name="jsonData"></param>
    /// <param name="generateEnumResult"></param>
    public static void GenerateEnumToTypeScript(string fileDir, SwaggerDocUrl swaggerDocUrl, SwaggerDocInfo jsonData,
        List<GenerateEnumResult> generateEnumResult)
    {
        generateEnumResult ??= [];

        var enumTypeList = Assemblies.Where(wh => swaggerDocUrl.Assemblies.Any(a => wh.GetName()
                                                                                        .Name?.Contains(a)
                                                                                    == true))
            .SelectMany(s => s.GetAssemblyTypes(wh => wh.FullName != null && wh.IsEnum))
            .ToList();

        foreach (var swaggerDocComponentsSchema in jsonData.Components.Schemas.Where(wh => wh.Value.Enum != null))
        {
            var enumType = enumTypeList.FirstOrDefault(wh => wh.Name == swaggerDocComponentsSchema.Key);

            if (enumType == null)
            {
                continue;
            }

            if (SwaggerCommon.ExcludeEnumList.Any(a => a == enumType.Name))
            {
                continue;
            }

            if (generateEnumResult.Any(a => a.EnumName == enumType.Name))
            {
                continue;
            }

            var underlyingType = Nullable.GetUnderlyingType(enumType) ?? enumType;

            // 通过 Type.GetTypeCode() 获取底层类型的 TypeCode，判断是是什么类型的值
            var typeCode = Type.GetTypeCode(underlyingType);

            var enumDetail = new StringBuilder();

            var enumNames = Enum.GetNames(enumType);
            var enumValues = Enum.GetValues(enumType);

            for (var i = 0; i < enumValues.Length; i++)
            {
                var enumValue = Convert.ToInt64(enumValues.GetValue(i))
                    .ToString();

                // 判断是否为 long 类型
                if (!string.IsNullOrEmpty(enumValue) && typeCode == TypeCode.Int64)
                {
                    enumValue = @$"""{enumValue}""";
                }

                enumDetail.Append($@"	/**
	 * {enumType.GetField(enumNames[i])?.GetCustomAttribute<DescriptionAttribute>(false)?.Description ?? enumNames[i]}
	 */
	{enumNames[i]} = {enumValue ?? enumNames[i]},");
                if (i + 1 != enumValues.Length)
                {
                    enumDetail.Append(Environment.NewLine);
                }
            }

            var filePath = Path.Combine(fileDir, $"{enumType.Name}.ts");

            File.WriteAllText(filePath, @$"/**
 *{(string.IsNullOrEmpty(swaggerDocComponentsSchema.Value.Description) ? "" : $" {swaggerDocComponentsSchema.Value.Description?.Replace("\r\n", "\r\n * ")}")}
 */
export enum {enumType.Name} {{
{enumDetail}
}}
".Replace("\r\n", "\n"));

            generateEnumResult.Add(new GenerateEnumResult
            {
                EnumName = enumType.Name, Import = @$"import {{ {enumType.Name} }} from ""@/api/enums/{enumType.Name}"";"
            });

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"枚举 {enumType.Name} 写入成功...");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// 生成枚举到JS文件
    /// </summary>
    /// <param name="fileDir"></param>
    /// <param name="swaggerDocUrl"></param>
    /// <param name="jsonData"></param>
    /// <param name="generateEnumResult"></param>
    public static void GenerateEnumToJavaScript(string fileDir, SwaggerDocUrl swaggerDocUrl, SwaggerDocInfo jsonData,
        List<GenerateEnumResult> generateEnumResult)
    {
        generateEnumResult ??= [];

        var enumTypeList = Assemblies.SelectMany(s => s.GetAssemblyTypes(wh =>
                wh.FullName != null && wh.IsEnum && swaggerDocUrl.Assemblies.Any(a => wh.FullName.Contains(a))))
            .ToList();

        foreach (var swaggerDocComponentsSchema in jsonData.Components.Schemas.Where(wh => wh.Value.Enum != null))
        {
            var enumType = enumTypeList.FirstOrDefault(wh => wh.Name == swaggerDocComponentsSchema.Key);

            if (enumType == null)
            {
                continue;
            }

            if (SwaggerCommon.ExcludeEnumList.Any(a => a == enumType.Name))
            {
                continue;
            }

            if (generateEnumResult.Any(a => a.EnumName == enumType.Name))
            {
                continue;
            }

            var underlyingType = Nullable.GetUnderlyingType(enumType) ?? enumType;

            // 通过 Type.GetTypeCode() 获取底层类型的 TypeCode，判断是是什么类型的值
            var typeCode = Type.GetTypeCode(underlyingType);

            var enumDetail = new StringBuilder();

            var enumNames = Enum.GetNames(enumType);
            var enumValues = Enum.GetValues(enumType);

            for (var i = 0; i < enumValues.Length; i++)
            {
                var enumValue = Convert.ToInt64(enumValues.GetValue(i))
                    .ToString();

                // 判断是否为 long 类型
                if (!string.IsNullOrEmpty(enumValue) && typeCode == TypeCode.Int64)
                {
                    enumValue = @$"""{enumValue}""";
                }

                enumDetail.Append($@"	/**
	 * {enumType.GetField(enumNames[i])?.GetCustomAttribute<DescriptionAttribute>(false)?.Description ?? enumNames[i]}
	 */
	{enumNames[i]}: {enumValue ?? enumNames[i]},");
                if (i + 1 != enumValues.Length)
                {
                    enumDetail.Append(Environment.NewLine);
                }
            }

            var filePath = Path.Combine(fileDir, $"{enumType.Name}.js");

            File.WriteAllText(filePath, @$"/**
 *{(string.IsNullOrEmpty(swaggerDocComponentsSchema.Value.Description) ? "" : $" {swaggerDocComponentsSchema.Value.Description?.Replace("\r\n", "\r\n * ")}")}
 */
export const {enumType.Name} = {{
{enumDetail}
}};
".Replace("\r\n", "\n"));

            generateEnumResult.Add(new GenerateEnumResult
            {
                EnumName = enumType.Name, Import = @$"import {{ {enumType.Name} }} from ""@/enums/{enumType.Name}.js"";"
            });

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"枚举 {enumType.Name} 写入成功...");
            Console.ResetColor();
        }
    }
}