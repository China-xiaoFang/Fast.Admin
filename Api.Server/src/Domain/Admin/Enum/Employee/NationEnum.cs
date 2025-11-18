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

namespace Fast.Admin.Enum;

/// <summary>
/// <see cref="NationEnum"/> 民族枚举
/// </summary>
[FastEnum("民族枚举")]
public enum NationEnum : byte
{
    /// <summary>
    /// 汉族
    /// </summary>
    [Description("汉族")]
    Han = 1,

    /// <summary>
    /// 蒙古族
    /// </summary>
    [Description("蒙古族")]
    Mongol = 2,

    /// <summary>
    /// 回族
    /// </summary>
    [Description("回族")]
    Hui = 3,

    /// <summary>
    /// 藏族
    /// </summary>
    [Description("藏族")]
    Tibetan = 4,

    /// <summary>
    /// 维吾尔族
    /// </summary>
    [Description("维吾尔族")]
    Uygur = 5,

    /// <summary>
    /// 苗族
    /// </summary>
    [Description("苗族")]
    Miao = 6,

    /// <summary>
    /// 彝族
    /// </summary>
    [Description("彝族")]
    Yi = 7,

    /// <summary>
    /// 壮族
    /// </summary>
    [Description("壮族")]
    Zhuang = 8,

    /// <summary>
    /// 布依族
    /// </summary>
    [Description("布依族")]
    Buyi = 9,

    /// <summary>
    /// 朝鲜族
    /// </summary>
    [Description("朝鲜族")]
    Korean = 10,

    /// <summary>
    /// 满族
    /// </summary>
    [Description("满族")]
    Manchu = 11,

    /// <summary>
    /// 侗族
    /// </summary>
    [Description("侗族")]
    Dong = 12,

    /// <summary>
    /// 瑶族
    /// </summary>
    [Description("瑶族")]
    Yao = 13,

    /// <summary>
    /// 白族
    /// </summary>
    [Description("白族")]
    Bai = 14,

    /// <summary>
    /// 土家族
    /// </summary>
    [Description("土家族")]
    Tujia = 15,

    /// <summary>
    /// 哈尼族
    /// </summary>
    [Description("哈尼族")]
    Hani = 16,

    /// <summary>
    /// 哈萨克族
    /// </summary>
    [Description("哈萨克族")]
    Kazak = 17,

    /// <summary>
    /// 傣族
    /// </summary>
    [Description("傣族")]
    Dai = 18,

    /// <summary>
    /// 黎族
    /// </summary>
    [Description("黎族")]
    Li = 19,

    /// <summary>
    /// 傈僳族
    /// </summary>
    [Description("傈僳族")]
    Lisu = 20,

    /// <summary>
    /// 佤族
    /// </summary>
    [Description("佤族")]
    Wa = 21,

    /// <summary>
    /// 畲族
    /// </summary>
    [Description("畲族")]
    She = 22,

    /// <summary>
    /// 高山族
    /// </summary>
    [Description("高山族")]
    Gaoshan = 23,

    /// <summary>
    /// 拉祜族
    /// </summary>
    [Description("拉祜族")]
    Lahu = 24,

    /// <summary>
    /// 水族
    /// </summary>
    [Description("水族")]
    Shui = 25,

    /// <summary>
    /// 东乡族
    /// </summary>
    [Description("东乡族")]
    Dongxiang = 26,

    /// <summary>
    /// 纳西族
    /// </summary>
    [Description("纳西族")]
    Naxi = 27,

    /// <summary>
    /// 景颇族
    /// </summary>
    [Description("景颇族")]
    Jingpo = 28,

    /// <summary>
    /// 柯尔克孜族
    /// </summary>
    [Description("柯尔克孜族")]
    Kirgiz = 29,

    /// <summary>
    /// 土族
    /// </summary>
    [Description("土族")]
    Tu = 30,

    /// <summary>
    /// 达斡尔族
    /// </summary>
    [Description("达斡尔族")]
    Daur = 31,

    /// <summary>
    /// 仫佬族
    /// </summary>
    [Description("仫佬族")]
    Mulam = 32,

    /// <summary>
    /// 羌族
    /// </summary>
    [Description("羌族")]
    Qiang = 33,

    /// <summary>
    /// 布朗族
    /// </summary>
    [Description("布朗族")]
    Blang = 34,

    /// <summary>
    /// 撒拉族
    /// </summary>
    [Description("撒拉族")]
    Salar = 35,

    /// <summary>
    /// 毛南族
    /// </summary>
    [Description("毛南族")]
    Maonan = 36,

    /// <summary>
    /// 仡佬族
    /// </summary>
    [Description("仡佬族")]
    Gelao = 37,

    /// <summary>
    /// 锡伯族
    /// </summary>
    [Description("锡伯族")]
    Xibe = 38,

    /// <summary>
    /// 阿昌族
    /// </summary>
    [Description("阿昌族")]
    Achang = 39,

    /// <summary>
    /// 普米族
    /// </summary>
    [Description("普米族")]
    Pumi = 40,

    /// <summary>
    /// 塔吉克族
    /// </summary>
    [Description("塔吉克族")]
    Tajik = 41,

    /// <summary>
    /// 怒族
    /// </summary>
    [Description("怒族")]
    Nu = 42,

    /// <summary>
    /// 乌孜别克族
    /// </summary>
    [Description("乌孜别克族")]
    Uzbek = 43,

    /// <summary>
    /// 俄罗斯族
    /// </summary>
    [Description("俄罗斯族")]
    Russian = 44,

    /// <summary>
    /// 鄂温克族
    /// </summary>
    [Description("鄂温克族")]
    Ewenki = 45,

    /// <summary>
    /// 德昂族
    /// </summary>
    [Description("德昂族")]
    Deang = 46,

    /// <summary>
    /// 保安族
    /// </summary>
    [Description("保安族")]
    Bonan = 47,

    /// <summary>
    /// 裕固族
    /// </summary>
    [Description("裕固族")]
    Yugur = 48,

    /// <summary>
    /// 京族
    /// </summary>
    [Description("京族")]
    Gin = 49,

    /// <summary>
    /// 塔塔尔族
    /// </summary>
    [Description("塔塔尔族")]
    Tatar = 50,

    /// <summary>
    /// 独龙族
    /// </summary>
    [Description("独龙族")]
    Derung = 51,

    /// <summary>
    /// 鄂伦春族
    /// </summary>
    [Description("鄂伦春族")]
    Oroqen = 52,

    /// <summary>
    /// 赫哲族
    /// </summary>
    [Description("赫哲族")]
    Hezhen = 53,

    /// <summary>
    /// 门巴族
    /// </summary>
    [Description("门巴族")]
    Monba = 54,

    /// <summary>
    /// 珞巴族
    /// </summary>
    [Description("珞巴族")]
    Lhoba = 55,

    /// <summary>
    /// 基诺族
    /// </summary>
    [Description("基诺族")]
    Jino = 56,

    /// <summary>
    /// 其他
    /// </summary>
    [Description("其他")]
    Other = 99
}