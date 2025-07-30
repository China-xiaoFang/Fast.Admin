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

// ReSharper disable once CheckNamespace

namespace Fast.Tool.ConsoleApp;

/// <summary>
/// <see cref="SwaggerCommon"/> Swagger 常量
/// </summary>
internal class SwaggerCommon
{
    /// <summary>
    /// FastElementPlus 导入
    /// </summary>
    public static readonly List<string> ElementPlus = ["ElSelectorOutput", "ElTreeOutput", "PageInput", "PagedResult"];

    /// <summary>
    /// 导出忽略集合
    /// </summary>
    public static readonly List<string> ExcludeDtoList = ["ElSelectorOutput", "ElTreeOutput", "PageInput", "PagedResult"];

    /// <summary>
    /// 导出枚举忽略
    /// </summary>
    public static readonly List<string> ExcludeEnumList = [];

    /// <summary>
    /// 固定写入
    /// </summary>
    public static readonly Dictionary<string, (string importUrl, string filePath)> FixedWriteList = new();

    /// <summary>
    /// 导入映射
    /// </summary>
    public static readonly Dictionary<string, (string importUrl, string name)> ExportMapDtoList = new();

    /// <summary>
    /// 分页集合输入
    /// </summary>
    public static readonly List<string> PagedInputList = new()
    {
        "pageIndex",
        "pageSize",
        "searchValue",
        "searchTimeList",
        "searchList",
        "sortList",
        "enablePaged"
    };

    /// <summary>
    /// Swagger 文档地址
    /// </summary>
    public static readonly List<SwaggerDocUrl> SwaggerDocUrlList =
    [
        new()
        {
            Url = "{0}/swagger/Auth/swagger.json",
            ModuleName = "Auth",
            BaseApi = "",
            Assemblies = new List<string> {"Fast.Runtime", "Fast.Common", "SqlSugarCore", "Fast.FastCloud.Enum"}
        },
        //new()
        //{
        //    Url = "{0}/swagger/FastCloud/swagger.json",
        //    ModuleName = "FastCloud",
        //    BaseApi = "",
        //    Assemblies = new List<string> {"Fast.Runtime", "Fast.Common", "SqlSugarCore", "Fast.FastCloud.Enum"}
        //}
    ];

    /// <summary>
    /// Api请求类型集合
    /// </summary>
    public static readonly List<ApiRequestTypeInfo> ApiRequestTypeInfoList =
    [
        new() {StartStr = "query", RequestType = "query"},
        new() {StartStr = "paged", RequestType = "query"},
        new() {StartStr = "add", RequestType = "add"},
        new() {StartStr = "update", RequestType = "edit"},
        new() {StartStr = "delete", RequestType = "delete"},
        new() {StartStr = "import", RequestType = "import"},
        new() {StartStr = "export", RequestType = "export"},
        new() {StartStr = "download", RequestType = "download"},
        new() {StartStr = "upload", RequestType = "upload"},
        new() {StartStr = "get", RequestType = "query"},
        new() {EndStr = "Selector", EqualStr = "selector", RequestType = "query"},
        new() {EndStr = "Detail", RequestType = "query"}
    ];

    /// <summary>
    /// Swagger 基础类型集合
    /// </summary>
    public static readonly List<SwaggerBaseTypeInfo> SwaggerBaseTypeInfoList =
    [
        new() {SwaggerType = "List_Nullable_Int64", Type = "number[] | null"},
        new() {SwaggerType = "List_Nullable_Int32", Type = "number[] | null"},
        new() {SwaggerType = "List_Nullable_Boolean", Type = "boolean[] | null"},
        new() {SwaggerType = "List_Nullable_String", Type = "string[] | null"},
        new() {SwaggerType = "Nullable_Int64", Type = "number | null"},
        new() {SwaggerType = "Nullable_Int32", Type = "number | null"},
        new() {SwaggerType = "Nullable_Boolean", Type = "boolean | null"},
        new() {SwaggerType = "Nullable_String", Type = "string | null"},
        new() {SwaggerType = "List_Int64", Type = "number[]"},
        new() {SwaggerType = "List_Int32", Type = "number[]"},
        new() {SwaggerType = "List_Boolean", Type = "boolean[]"},
        new() {SwaggerType = "List_String", Type = "string[]"},
        new() {SwaggerType = "Int64", Type = "number"},
        new() {SwaggerType = "Int32", Type = "number"},
        new() {SwaggerType = "Boolean", Type = "boolean"},
        new() {SwaggerType = "String", Type = "string"},
        new() {SwaggerType = "string", Type = "string"},
        new() {SwaggerType = "date-time", Type = "Date"},
        new() {SwaggerType = "int64", Type = "number"},
        new() {SwaggerType = "int32", Type = "number"},
        new() {SwaggerType = "double", Type = "number"},
        new() {SwaggerType = "integer", Type = "number"},
        new() {SwaggerType = "number", Type = "number"},
        new() {SwaggerType = "boolean", Type = "boolean"},
        new() {SwaggerType = "object", Type = "any"}
    ];

    /// <summary>
    /// Api请求返回数据类型集合
    /// </summary>
    public static readonly List<ApiRequestDataTypeInfo> ApiRequestDataTypeList =
    [
        new() {StartStr = "RestfulResult_", SubStr = "RestfulResult_", ResultDataType = "{0}"},
        new() {StartStr = "IActionResult", SubStr = "IActionResult", DisabledImport = true},
        new() {StartStr = "BasePageInput", LibAddStrList = ["PageInput"], ResultDataType = "PageInput"},
        new()
        {
            StartStr = "PagedResult_List_ElSelectorOutput_",
            SubStr = "PagedResult_List_ElSelectorOutput_",
            LibAddStrList = ["PagedResult", "ElSelectorOutput"],
            ResultDataType = "PagedResult<ElSelectorOutput<{0}>[]>"
        },
        new()
        {
            StartStr = "PagedResult_List_ElTreeOutput_",
            SubStr = "PagedResult_List_ElTreeOutput_",
            LibAddStrList = ["PagedResult", "ElTreeOutput"],
            ResultDataType = "PagedResult<ElTreeOutput<{0}>[]>"
        },
        new()
        {
            StartStr = "PagedResult_ElSelectorOutput_",
            SubStr = "PagedResult_ElSelectorOutput_",
            LibAddStrList = ["PagedResult", "ElSelectorOutput"],
            ResultDataType = "PagedResult<ElSelectorOutput<{0}>>"
        },
        new()
        {
            StartStr = "PagedResult_ElTreeOutput_",
            SubStr = "PagedResult_ElTreeOutput_",
            LibAddStrList = ["PagedResult", "ElTreeOutput"],
            ResultDataType = "PagedResult<ElTreeOutput<{0}>>"
        },
        new()
        {
            StartStr = "PagedResult_List_",
            SubStr = "PagedResult_List_",
            LibAddStrList = ["PagedResult"],
            ResultDataType = "PagedResult<{0}[]>"
        },
        new()
        {
            StartStr = "PagedResult_",
            SubStr = "PagedResult_",
            LibAddStrList = ["PagedResult"],
            ResultDataType = "PagedResult<{0}>"
        },
        new()
        {
            StartStr = "List_ElSelectorOutput_",
            SubStr = "List_ElSelectorOutput_",
            LibAddStrList = ["ElSelectorOutput"],
            ResultDataType = "ElSelectorOutput<{0}>[]"
        },
        new()
        {
            StartStr = "List_ElTreeOutput_",
            SubStr = "List_ElTreeOutput_",
            LibAddStrList = ["ElTreeOutput"],
            ResultDataType = "ElTreeOutput<{0}>[]"
        },
        new()
        {
            StartStr = "ElSelectorOutput_",
            SubStr = "ElSelectorOutput_",
            LibAddStrList = ["ElSelectorOutput"],
            ResultDataType = "ElSelectorOutput<{0}>"
        },
        new()
        {
            StartStr = "ElTreeOutput_",
            SubStr = "ElTreeOutput_",
            LibAddStrList = ["ElTreeOutput"],
            ResultDataType = "ElTreeOutput<{0}>"
        },
        new() {StartStr = "List_IDictionary_StringInt64", ResultDataType = "Record<string, number>[]", DisabledImport = true},
        new() {StartStr = "List_Dictionary_StringInt64", ResultDataType = "Record<string, number>[]", DisabledImport = true},
        new() {StartStr = "List_IDictionary_StringInt32", ResultDataType = "Record<string, number>[]", DisabledImport = true},
        new() {StartStr = "List_Dictionary_StringInt32", ResultDataType = "Record<string, number>[]", DisabledImport = true},
        new() {StartStr = "List_IDictionary_StringObject", ResultDataType = "Record<string, any>[]", DisabledImport = true},
        new() {StartStr = "List_Dictionary_StringObject", ResultDataType = "Record<string, any>[]", DisabledImport = true},
        new() {StartStr = "List_IDictionary_StringString", ResultDataType = "Record<string, string>[]", DisabledImport = true},
        new() {StartStr = "List_Dictionary_StringString", ResultDataType = "Record<string, string>[]", DisabledImport = true},
        new() {StartStr = "List_", SubStr = "List_", ResultDataType = "{0}[]"},
        new() {StartStr = "IDictionary_StringInt64", ResultDataType = "Record<string, number>", DisabledImport = true},
        new() {StartStr = "Dictionary_StringInt64", ResultDataType = "Record<string, number>", DisabledImport = true},
        new() {StartStr = "IDictionary_StringInt32", ResultDataType = "Record<string, number>", DisabledImport = true},
        new() {StartStr = "Dictionary_StringInt32", ResultDataType = "Record<string, number>", DisabledImport = true},
        new() {StartStr = "IDictionary_StringObject", ResultDataType = "Record<string, any>", DisabledImport = true},
        new() {StartStr = "Dictionary_StringObject", ResultDataType = "Record<string, any>", DisabledImport = true},
        new() {StartStr = "IDictionary_StringString", ResultDataType = "Record<string, string>", DisabledImport = true},
        new() {StartStr = "Dictionary_StringString", ResultDataType = "Record<string, string>", DisabledImport = true}
    ];
}