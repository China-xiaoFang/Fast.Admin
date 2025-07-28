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

internal class ApiUtil
{
    /// <summary>
    /// 获取 Api 请求类型
    /// </summary>
    /// <param name="funcName"></param>
    /// <returns></returns>
    static string GetApiRequestType(string funcName)
    {
        if (string.IsNullOrEmpty(funcName))
            return "other";

        foreach (var apiRequestTypeInfo in SwaggerCommon.ApiRequestTypeInfoList)
        {
            if (!string.IsNullOrWhiteSpace(apiRequestTypeInfo.StartStr))
            {
                if (funcName.StartsWith(apiRequestTypeInfo.StartStr))
                {
                    return apiRequestTypeInfo.RequestType;
                }
            }

            if (!string.IsNullOrWhiteSpace(apiRequestTypeInfo.EndStr))
            {
                if (funcName.EndsWith(apiRequestTypeInfo.EndStr))
                {
                    return apiRequestTypeInfo.RequestType;
                }
            }

            if (!string.IsNullOrWhiteSpace(apiRequestTypeInfo.EqualStr))
            {
                if (funcName.Equals(apiRequestTypeInfo.EqualStr))
                {
                    return apiRequestTypeInfo.RequestType;
                }
            }
        }

        return "other";
    }

    /// <summary>
    /// 生成 Api
    /// </summary>
    /// <param name="libName"></param>
    /// <param name="fileDir"></param>
    /// <param name="swaggerDocUrl"></param>
    /// <param name="jsonData"></param>
    /// <param name="generateDtoResult"></param>
    /// <param name="generateEnumResult"></param>
    public static async Task GenerateApiToTypeScript(string libName, string fileDir, SwaggerDocUrl swaggerDocUrl,
        SwaggerDocInfo jsonData, List<GenerateDtoResult> generateDtoResult, List<GenerateEnumResult> generateEnumResult)
    {
        foreach (var tags in jsonData.Tags)
        {
            if (string.IsNullOrEmpty(tags.Name))
                continue;

            // 创建模块文件夹
            var moduleFileDir = Path.Combine(fileDir, swaggerDocUrl.ModuleName, tags.Name);
            Directory.CreateDirectory(moduleFileDir);

            // 查找当前模块下面所有的接口
            var curPaths = jsonData.Paths.Where(wh =>
                    wh.Value.Post?.Tags?.Contains(tags.Name) == true || wh.Value.Get?.Tags?.Contains(tags.Name) == true)
                .ToList();

            // 内容
            var contentDetail = new StringBuilder();
            // 导入集合
            var importList = new List<string>();

            for (var i = 0; i < curPaths.Count; i++)
            {
                var apiInfo = curPaths[i].Value;
                var apiName = curPaths[i].Key;
                var apiFuncName = apiName.Split("/")
                    .LastOrDefault();

                // 请求类型
                var requestType = GetApiRequestType(apiFuncName);

                // 请求方式
                var apiMethod = "";
                // 请求方法信息
                SwaggerDocMethodInfo methodInfo;

                if (apiInfo.Post != null)
                {
                    apiMethod = "post";
                    methodInfo = apiInfo.Post;
                }
                else if (apiInfo.Get != null)
                {
                    apiMethod = "get";
                    methodInfo = apiInfo.Get;
                }
                else
                    continue;

                if (methodInfo == null)
                    continue;

                // 响应数据类型
                var resultDataType =
                    DtoUtil.DisposeDtoValueType(methodInfo.Responses?.Code200?.Content?.Json?.Schema?.Ref, importList);

                // 请求参数
                var postDataType = "";
                var paramStr = "";
                var paramSb = new StringBuilder();

                // 请求参数处理
                switch (apiMethod)
                {
                    case "post":
                        // 处理文件上传
                        if (methodInfo?.RequestBody?.Content.FormData != null)
                        {
                            postDataType = "FormData";
                        }
                        else if (!string.IsNullOrEmpty(methodInfo.RequestBody?.Content.Json?.Schema?.Ref))
                        {
                            postDataType = DtoUtil.DisposeDtoValueType(methodInfo.RequestBody?.Content.Json?.Schema?.Ref,
                                importList);
                        }
                        else if (!string.IsNullOrEmpty(methodInfo.RequestBody?.Content.Json?.Schema?.Items?.Ref))
                        {
                            postDataType = DtoUtil.DisposeDtoValueType(methodInfo.RequestBody?.Content.Json?.Schema?.Items?.Ref,
                                importList);
                        }

                        break;
                    case "get":
                        if (methodInfo?.Parameters != null)
                        {
                            for (var j = 0; j < methodInfo.Parameters.Count; j++)
                            {
                                if (j == 0 && methodInfo.Parameters[j].Name == "unionID")
                                {
                                    continue;
                                }

                                if (j == 1 && methodInfo.Parameters[j].Name == "clerkID")
                                {
                                    continue;
                                }

                                if (!string.IsNullOrEmpty(methodInfo.Parameters[j]?.Name))
                                {
                                    if (!string.IsNullOrEmpty(methodInfo.Parameters[j]?.Schema?.Ref))
                                    {
                                        var refName = DtoUtil.DisposeDtoValueType(methodInfo.Parameters[j]?.Schema?.Ref,
                                            importList);
                                        if (!string.IsNullOrEmpty(refName))
                                        {
                                            paramStr += $"{methodInfo.Parameters[j].Name}: {refName}, ";
                                        }
                                    }
                                    else
                                    {
                                        paramStr +=
                                            $"{methodInfo.Parameters[j].Name}: {DtoUtil.DisposeBaseType(methodInfo.Parameters[j]?.Schema?.Type)}, ";
                                    }

                                    paramSb.Append($"				{methodInfo.Parameters[j].Name},");

                                    if (j + 1 != methodInfo.Parameters.Count)
                                    {
                                        paramSb.Append(Environment.NewLine);
                                    }
                                }
                            }
                        }

                        paramStr = paramStr.TrimEnd(' ')
                            .TrimEnd(',');
                        break;
                }

                contentDetail.Append($@"	/**
	 *{(string.IsNullOrEmpty(methodInfo.Summary) ? "" : $" {methodInfo.Summary}")}
	 */
	{apiFuncName}(");

                if (!string.IsNullOrEmpty(paramStr))
                {
                    contentDetail.Append(paramStr);
                }

                if (!string.IsNullOrEmpty(postDataType))
                {
                    if (methodInfo?.RequestBody?.Content.Json?.Schema?.Type == "array")
                    {
                        contentDetail.Append($"data: {postDataType}[]");
                    }
                    else
                    {
                        contentDetail.Append($"data: {postDataType}");

                        if (postDataType != "FormData")
                        {
                            importList.Add(postDataType);
                        }
                    }
                }

                contentDetail.Append(@") {
		return axiosUtil.request");

                if (!string.IsNullOrEmpty(resultDataType))
                {
                    // 直接开启简洁响应
                    contentDetail.Append($"<{resultDataType}>");
                }
                //if (!string.IsNullOrEmpty(resultDataType) && !string.IsNullOrEmpty(postDataType))
                //{
                //    contentDetail.Append($"<ApiResponse<{resultDataType}, {postDataType}>>");
                //    importList.Add("ApiResponse");
                //}
                //else if (!string.IsNullOrEmpty(resultDataType))
                //{
                //    contentDetail.Append($"<ApiResponse<{resultDataType}>>");
                //    importList.Add("ApiResponse");
                //}
                //else if (!string.IsNullOrEmpty(postDataType))
                //{
                //    contentDetail.Append($"<ApiResponse<null, {postDataType}>>");
                //    importList.Add("ApiResponse");

                contentDetail.Append(@$"({{
			url: ""{swaggerDocUrl.BaseApi}{apiName}"",
			method: ""{apiMethod}"",");

                contentDetail.Append(Environment.NewLine);

                if (!string.IsNullOrEmpty(postDataType))
                {
                    contentDetail.Append("			data,");
                    contentDetail.Append(Environment.NewLine);
                }

                if (paramSb.Length > 0)
                {
                    contentDetail.Append($@"			params: {{
{paramSb.ToString()}
			}},");
                    contentDetail.Append(Environment.NewLine);
                }

                if (requestType == "download" || requestType == "export")
                {
                    contentDetail.Append(@"			responseType: ""blob"",");
                    contentDetail.Append(Environment.NewLine);
                    contentDetail.Append(@"			autoDownloadFile: true,");
                    contentDetail.Append(Environment.NewLine);
                }

                contentDetail.Append($@"			requestType: {(string.IsNullOrEmpty(requestType) ? "" : $@"""{requestType}""")},
		}});
	}},");


                if (i + 1 != curPaths.Count)
                {
                    contentDetail.Append(Environment.NewLine);
                }
            }

            var importSb = new StringBuilder();

            // 处理组件库的导入
            var elImportList = importList.Where(wh => SwaggerCommon.ElementPlus.Contains(wh))
                .Distinct()
                .ToList();

            if (elImportList.Any())
            {
                importSb.AppendLine(@$"import {{ {string.Join(", ", elImportList)} }} from ""{libName}"";");
            }

            if (importList.Any())
            {
                var dtoFileDir = Path.Combine(moduleFileDir, "models");
                Directory.CreateDirectory(dtoFileDir);

                var isModels = false;

                foreach (var item in importList.Where(wh => !SwaggerCommon.ElementPlus.Contains(wh))
                             .Distinct())
                {
                    // 判断是否为固定写入
                    var fixedWrite = SwaggerCommon.FixedWriteList.SingleOrDefault(s => s.Key == item);
                    if (fixedWrite.Value.importUrl != null)
                    {
                        if (!dtoFileDir.EndsWith(fixedWrite.Value.filePath))
                        {
                            importSb.AppendLine(@$"import {{ {fixedWrite.Key} }} from ""{fixedWrite.Value.importUrl}"";");
                            continue;
                        }
                    }

                    // 优先从枚举中查找
                    var enumResult = generateEnumResult.SingleOrDefault(s => s.EnumName == item);
                    if (enumResult != null)
                    {
                        importSb.AppendLine(enumResult.Import);
                        continue;
                    }

                    // 从集合中查找
                    var dtoResult = generateDtoResult.Single(s => s.DtoName == item);

                    importSb.AppendLine(@$"import {{ {dtoResult.DtoName} }} from ""./models/{dtoResult.DtoName}"";");
                    isModels = true;

                    DtoUtil.WriteDtoFile(dtoFileDir, dtoResult, generateDtoResult, generateEnumResult);
                }

                if (!isModels)
                {
                    Directory.Delete(dtoFileDir);
                }
            }

            if (importSb.Length != 0)
            {
                importSb.Append(Environment.NewLine);
            }

            if (contentDetail.Length != 0)
            {
                await File.WriteAllTextAsync(Path.Combine(moduleFileDir, "index.ts"),
                    @$"import {{ axiosUtil }} from ""@fast-china/axios"";
{importSb}/**
 *{(string.IsNullOrEmpty(tags.Description) ? "" : $" {tags.Description}Api")}
 */
export const {tags.Name}Api = {{
{contentDetail}
}};
".Replace("\r\n", "\n"));

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Api {tags.Name} 写入成功...");
                Console.ResetColor();
            }

            // 空写入
            if (contentDetail.Length == 0
                && !importList.Distinct()
                    .Any())
            {
                Directory.Delete(moduleFileDir);
            }
        }
    }

    /// <summary>
    /// 生成 Api
    /// </summary>
    /// <param name="fileDir"></param>
    /// <param name="swaggerDocUrl"></param>
    /// <param name="jsonData"></param>
    public static async Task GenerateApiToJavaScript(string fileDir, SwaggerDocUrl swaggerDocUrl, SwaggerDocInfo jsonData)
    {
        foreach (var tags in jsonData.Tags)
        {
            if (string.IsNullOrEmpty(tags.Name))
                continue;

            // 创建模块文件夹
            var moduleFileDir = Path.Combine(fileDir, swaggerDocUrl.ModuleName, tags.Name);
            Directory.CreateDirectory(moduleFileDir);

            // 查找当前模块下面所有的接口
            var curPaths = jsonData.Paths.Where(wh =>
                    wh.Value.Post?.Tags?.Contains(tags.Name) == true || wh.Value.Get?.Tags?.Contains(tags.Name) == true)
                .ToList();

            // 内容
            var contentDetail = new StringBuilder();

            for (var i = 0; i < curPaths.Count; i++)
            {
                var apiInfo = curPaths[i].Value;
                var apiName = curPaths[i].Key;
                var apiFuncName = apiName.Split("/")
                    .LastOrDefault();

                // 请求类型
                var requestType = GetApiRequestType(apiFuncName);

                // 请求方式
                var apiMethod = "";
                // 请求方法信息
                SwaggerDocMethodInfo methodInfo;

                if (apiInfo.Post != null)
                {
                    apiMethod = "post";
                    methodInfo = apiInfo.Post;
                }
                else if (apiInfo.Get != null)
                {
                    apiMethod = "get";
                    methodInfo = apiInfo.Get;
                }
                else
                    continue;

                if (methodInfo == null)
                    continue;

                // 请求参数
                var postDataType = "";
                var paramStr = "";
                var paramSb = new StringBuilder();

                // 请求参数处理
                switch (apiMethod)
                {
                    case "post":
                        // 处理文件上传
                        if (methodInfo?.RequestBody?.Content.FormData != null)
                        {
                            postDataType = "data";
                        }

                        if (methodInfo.RequestBody?.Content.Json?.Schema != null)
                        {
                            postDataType = "data";
                        }

                        break;
                    case "get":
                        if (methodInfo?.Parameters != null)
                        {
                            for (var j = 0; j < methodInfo.Parameters.Count; j++)
                            {
                                if (j == 0 && methodInfo.Parameters[j].Name == "unionID")
                                {
                                    continue;
                                }

                                if (j == 1 && methodInfo.Parameters[j].Name == "clerkID")
                                {
                                    continue;
                                }

                                if (string.IsNullOrEmpty(methodInfo.Parameters[j]?.Name))
                                    continue;

                                paramStr += $"{methodInfo.Parameters[j].Name}, ";

                                paramSb.Append($"				{methodInfo.Parameters[j].Name},");

                                if (j + 1 != methodInfo.Parameters.Count)
                                {
                                    paramSb.Append(Environment.NewLine);
                                }
                            }
                        }

                        paramStr = paramStr.TrimEnd(' ')
                            .TrimEnd(',');
                        break;
                }

                contentDetail.Append($@"	/**
	 *{(string.IsNullOrEmpty(methodInfo.Summary) ? "" : $" {methodInfo.Summary}")}
	 */
	{apiFuncName}(");

                if (!string.IsNullOrEmpty(paramStr))
                {
                    contentDetail.Append(paramStr);
                }

                if (!string.IsNullOrEmpty(postDataType))
                {
                    contentDetail.Append(postDataType);
                }

                contentDetail.Append(@") {
		return requestUtil.request");

                contentDetail.Append(@$"({{
			url: ""{swaggerDocUrl.BaseApi}{apiName}"",
			method: ""{apiMethod}"",");

                contentDetail.Append(Environment.NewLine);

                if (!string.IsNullOrEmpty(postDataType))
                {
                    contentDetail.Append("			data,");
                    contentDetail.Append(Environment.NewLine);
                }

                if (paramSb.Length > 0)
                {
                    contentDetail.Append($@"			data: {{
{paramSb.ToString()}
			}},");
                    contentDetail.Append(Environment.NewLine);
                }

                if (requestType == "download" || requestType == "export")
                {
                    contentDetail.Append(@"			responseType: ""blob"",");
                    contentDetail.Append(Environment.NewLine);
                    contentDetail.Append(@"			autoDownloadFile: true,");
                    contentDetail.Append(Environment.NewLine);
                }

                contentDetail.Append($@"			requestType: {(string.IsNullOrEmpty(requestType) ? "" : $@"""{requestType}""")},
		}});
	}},");


                if (i + 1 != curPaths.Count)
                {
                    contentDetail.Append(Environment.NewLine);
                }
            }

            if (contentDetail.Length != 0)
            {
                await File.WriteAllTextAsync(Path.Combine(moduleFileDir, "index.js"),
                    @$"import {{ requestUtil }} from ""@/utils"";

/**
 *{(string.IsNullOrEmpty(tags.Description) ? "" : $" {tags.Description}Api")}
 */
export const {tags.Name}Api = {{
{contentDetail}
}};
".Replace("\r\n", "\n"));

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Api {tags.Name} 写入成功...");
                Console.ResetColor();
            }

            // 空写入
            if (contentDetail.Length == 0)
            {
                Directory.Delete(moduleFileDir);
            }
        }
    }
}