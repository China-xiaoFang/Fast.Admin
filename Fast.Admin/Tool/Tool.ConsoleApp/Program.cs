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

namespace Fast.Tool.ConsoleApp;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("欢迎使用 Fast Swagger 文档生成 API TS 文件工具。");
        Console.ResetColor();
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("请选择工具......");
        Console.ResetColor();
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("[1].Swagger 文档生成 TypeScript 声明文件。");
        Console.ResetColor();
        Console.WriteLine();

        var inputIndex = Console.ReadLine();

        switch (inputIndex)
        {
            case "1":
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("请选择文档地址......");
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("[1].http://192.168.1.69:38081（开发环境）");
                Console.WriteLine("[2].http://127.0.0.1:38081（本地环境）");
                Console.ResetColor();
                Console.WriteLine();

                var inputIndex2 = Console.ReadLine();

                if (inputIndex2 != "1" && inputIndex2 != "2")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("输入的选项无效！");
                    Console.ResetColor();
                }
                else
                {
                    var baseUrl = inputIndex2 switch
                    {
                        "1" => "http://192.168.1.69:38081",
                        "2" => "http://127.0.0.1:38081",
                        _ => ""
                    };

                    await SwaggerUtil.GenerateTypeScript("fast-element-plus", baseUrl);
                    //await SwaggerUtil.GenerateTypeScript("fast-element-app", baseUrl);
                }

                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("输入的选项无效！");
                Console.ResetColor();
                break;
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("全部执行完成......");
        Console.ResetColor();

        Console.ReadLine();
    }
}