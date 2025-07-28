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

using Fast.Serialization;

// ReSharper disable once CheckNamespace
namespace Fast.Tool.ConsoleApp;

internal class SwaggerUtil
{
    /// <summary>
    /// 生成 TS
    /// </summary>
    /// <param name="libName">组件库名称</param>
    /// <param name="baseUrl"></param>
    /// <returns></returns>
    public static async Task GenerateTypeScript(string libName, string baseUrl)
    {
        var enumFileDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{libName}-tsEnumFile");
        if (Directory.Exists(enumFileDir))
        {
            Directory.Delete(enumFileDir, true);
        }

        Directory.CreateDirectory(enumFileDir);

        var apiFileDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{libName}-tsApiFile");
        if (Directory.Exists(apiFileDir))
        {
            Directory.Delete(apiFileDir, true);
        }

        Directory.CreateDirectory(apiFileDir);

        // 获取文档信息
        var swaggerJsonDataList = new List<(SwaggerDocUrl swaggerDocUrl, SwaggerDocInfo swaggerJsonData)>();

        foreach (var swaggerDocUrl in SwaggerCommon.SwaggerDocUrlList.DeepCopy())
        {
            swaggerDocUrl.Url = string.Format(swaggerDocUrl.Url, baseUrl);

            // 获取文档信息
            var swaggerJsonData = await HttpRequestUtil.GetSwaggerDoc(swaggerDocUrl);

            if (swaggerJsonData == null)
                continue;

            swaggerJsonDataList.Add((swaggerDocUrl, swaggerJsonData));
        }

        var generateEnumResult = new List<GenerateEnumResult>();

        // 循环文档信息，生成枚举
        foreach (var (swaggerDocUrl, swaggerJsonData) in swaggerJsonDataList)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"开始写入 {swaggerDocUrl.ModuleName} 枚举文件......");
            Console.WriteLine();
            Console.ResetColor();

            // 优先生成枚举文件
            EnumUtil.GenerateEnumToTypeScript(enumFileDir, swaggerDocUrl, swaggerJsonData, generateEnumResult);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{swaggerDocUrl.ModuleName} 枚举文件写入成功...");
            Console.WriteLine();
            Console.ResetColor();
        }

        // 循环文档信息，生成 Dto 和 Api 文件
        foreach (var (swaggerDocUrl, swaggerJsonData) in swaggerJsonDataList)
        {
            var generateDtoResult = new List<GenerateDtoResult>();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"开始处理 {swaggerDocUrl.ModuleName} Dto文件......");
            Console.WriteLine();
            Console.ResetColor();

            // 生成 Dto 文件
            DtoUtil.GenerateDtoToTypeScript(swaggerDocUrl, swaggerJsonData, generateDtoResult);

            // 处理 Dto Import
            DtoUtil.DisposeDtoImport(libName, swaggerDocUrl, swaggerJsonData, generateDtoResult, generateEnumResult);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"处理 {swaggerDocUrl.ModuleName} Dto文件成功...");
            Console.WriteLine();
            Console.ResetColor();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"开始写入 {swaggerDocUrl.ModuleName} Api文件......");
            Console.WriteLine();
            Console.ResetColor();

            // 处理 Api
            await ApiUtil.GenerateApiToTypeScript(libName, apiFileDir, swaggerDocUrl, swaggerJsonData, generateDtoResult,
                generateEnumResult);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"写入 {swaggerDocUrl.ModuleName} Api文件成功...");
            Console.WriteLine();
            Console.ResetColor();
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("获取重复文件中......");
        Console.ResetColor();
        Console.WriteLine();

        var fileNameList = Directory.GetFiles(apiFileDir, "*", SearchOption.AllDirectories)
            .ToList();
        fileNameList.AddRange(Directory.GetFiles(enumFileDir, "*", SearchOption.AllDirectories)
            .ToList());

        var fileCount = 0;

        foreach (var fNames in fileNameList.Select(sl => new {PathUrl = sl, FileName = Path.GetFileName(sl)})
                     .Where(wh => wh.FileName != "index.ts")
                     .GroupBy(gb => gb.FileName)
                     .Where(wh => wh.Count() > 1))
        {
            fileCount++;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"重复文件名称：{fNames.Key}");
            Console.ResetColor();
            foreach (var fName in fNames)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"重复文件路径：{fName.PathUrl}");
            }

            Console.ResetColor();
            Console.WriteLine();
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"共 {fileCount} 个重复文件......");
        Console.ResetColor();
        Console.WriteLine();
    }

    /// <summary>
    /// 生成 JS
    /// </summary>
    /// <param name="baseUrl"></param>
    /// <returns></returns>
    public static async Task GenerateJavaScript(string baseUrl)
    {
        var enumFileDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "jsEnumFile");
        if (Directory.Exists(enumFileDir))
        {
            Directory.Delete(enumFileDir, true);
        }

        Directory.CreateDirectory(enumFileDir);

        var apiFileDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "jsApiFile");
        if (Directory.Exists(apiFileDir))
        {
            Directory.Delete(apiFileDir, true);
        }

        Directory.CreateDirectory(apiFileDir);

        // 获取文档信息
        var swaggerJsonDataList = new List<(SwaggerDocUrl swaggerDocUrl, SwaggerDocInfo swaggerJsonData)>();

        foreach (var swaggerDocUrl in SwaggerCommon.SwaggerDocUrlList.DeepCopy())
        {
            swaggerDocUrl.Url = string.Format(swaggerDocUrl.Url, baseUrl);

            // 获取文档信息
            var swaggerJsonData = await HttpRequestUtil.GetSwaggerDoc(swaggerDocUrl);

            if (swaggerJsonData == null)
                continue;

            swaggerJsonDataList.Add((swaggerDocUrl, swaggerJsonData));
        }

        var generateEnumResult = new List<GenerateEnumResult>();

        // 循环文档信息，生成枚举
        foreach (var (swaggerDocUrl, swaggerJsonData) in swaggerJsonDataList)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"开始写入 {swaggerDocUrl.ModuleName} 枚举文件......");
            Console.WriteLine();
            Console.ResetColor();

            // 优先生成枚举文件
            EnumUtil.GenerateEnumToJavaScript(enumFileDir, swaggerDocUrl, swaggerJsonData, generateEnumResult);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{swaggerDocUrl.ModuleName} 枚举文件写入成功...");
            Console.WriteLine();
            Console.ResetColor();
        }

        // 循环文档信息，生成 Dto 和 Api 文件
        foreach (var (swaggerDocUrl, swaggerJsonData) in swaggerJsonDataList)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"开始写入 {swaggerDocUrl.ModuleName} Api文件......");
            Console.WriteLine();
            Console.ResetColor();

            // 处理 Api
            await ApiUtil.GenerateApiToJavaScript(apiFileDir, swaggerDocUrl, swaggerJsonData);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"写入 {swaggerDocUrl.ModuleName} Api文件成功...");
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}