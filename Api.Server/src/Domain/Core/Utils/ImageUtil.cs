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

using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;

namespace Fast.Core;

/// <summary>
/// <see cref="ImageUtil"/> 图片工具类
/// </summary>
[SuppressSniffer]
public static class ImageUtil
{
    /// <summary>
    /// 默认字体
    /// </summary>
    private static readonly FontFamily DefaultFont;

    /// <summary>
    /// 加粗字体
    /// </summary>
    private static readonly FontFamily BoldFont;

    /// <summary>
    /// 默认行高
    /// </summary>
    private static int DefaultLineHeight => 20;

    static ImageUtil()
    {
        // 加载根目录下面的字体
        var basePath = Path.Combine(AppContext.BaseDirectory, "Assets/Fonts");
        var fontCollection = new FontCollection();
        DefaultFont = fontCollection.Add(Path.Combine(basePath, "NotoSansSC-Regular.ttf"));
        BoldFont = fontCollection.Add(Path.Combine(basePath, "NotoSansSC-Medium.ttf"));
    }

    /// <summary>
    /// 生成图片
    /// </summary>
    /// <param name="drawAction"><see cref="Action{T}"/> 操作 drawAction(image, font, color, lineHeight, width, height)</param>
    /// <param name="width"><see cref="int"/> 宽度</param>
    /// <param name="height"><see cref="int"/> 高度</param>
    /// <param name="fontSize"><see cref="float"/> 文字大小</param>
    /// <param name="lineHeight"><see cref="int"/> 行高</param>
    /// <returns></returns>
    public static Image<Rgba32> GenImage(Action<Image<Rgba32>, Font, Color, int, int, int> drawAction, int width = 450,
        int height = 750, float fontSize = 16f, int? lineHeight = null)
    {
        lineHeight ??= DefaultLineHeight;

        // 创建图片
        using var image = new Image<Rgba32>(width, height);

        // 设置图片背景颜色
        var backgroundColor = new Rgba32(255, 255, 255); // 白色
        image.Mutate(x => x.BackgroundColor(backgroundColor));

        // 固定字体
        var font = new Font(DefaultFont, fontSize);

        // 固定颜色
        var color = Color.Black;

        // 绘制调试边框
        //var borderWidth = 1F;
        //var borderColor = Color.Red;
        //image.Mutate(ctx =>
        //{
        //    // 绘制上边框
        //    ctx.DrawLine(borderColor, borderWidth, new PointF(0, 0), new PointF(width, 0));
        //    // 绘制下边框
        //    ctx.DrawLine(borderColor, borderWidth, new PointF(0, height - 1), new PointF(width, height - 1));
        //    // 绘制左边框
        //    ctx.DrawLine(borderColor, borderWidth, new PointF(0, 0), new PointF(0, height));
        //    // 绘制右边框
        //    ctx.DrawLine(borderColor, borderWidth, new PointF(width - 1, 0), new PointF(width - 1, width));
        //});

        drawAction?.Invoke(image, font, color, lineHeight.Value, width, height);

        // 这里克隆一份，避免被提前释放
        return image.Clone();
    }

    /// <summary>
    /// 图片转为Base64图片
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    public static async Task<string> ConvertToBase64Image(Image image)
    {
        // 将图片转换为 Base64 字符串
        using var memoryStream = new MemoryStream();
        await image.SaveAsync(memoryStream, new PngEncoder());
        var imageBytes = memoryStream.ToArray();
        return Convert.ToBase64String(imageBytes);
    }

    /// <summary>
    /// 图片转为Base64图片
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    public static string ConvertToBase64ImageSync(Image image)
    {
        // 将图片转换为 Base64 字符串
        using var memoryStream = new MemoryStream();
        image.Save(memoryStream, new PngEncoder());
        var imageBytes = memoryStream.ToArray();
        return Convert.ToBase64String(imageBytes);
    }

    /// <summary>
    /// 获取对应文本字体的文本宽度
    /// </summary>
    /// <param name="content"><see cref="string"/> 文本</param>
    /// <param name="font"><see cref="Font"/> 字体</param>
    /// <returns></returns>
    public static float GetFontWidthByContent(string content, Font font)
    {
        // 获取要渲染的文字宽度
        return TextMeasurer.MeasureSize(content, new TextOptions(font))
            .Width;
    }

    /// <summary>
    /// 生成 Code_128 条形码
    /// </summary>
    /// <param name="content"><see cref="string"/> 条码内容</param>
    /// <param name="width"><see cref="int"/> 条码宽度</param>
    /// <param name="height"><see cref="int"/> 条码高度</param>
    /// <param name="dWidth"><see cref="int"/> 底图宽度</param>
    /// <param name="showContent"><see cref="bool"/> 显示文字</param>
    /// <param name="fontSize"><see cref="float"/> 文字大小</param>
    /// <param name="drawAction"><see cref="Action{T}"/> 操作 drawAction(image, font, color, lineHeight, width, height)</param>
    /// <returns></returns>
    public static Image GenBarCode_128(string content, int width = 200, int height = 200, int dWidth = 300,
        bool showContent = true, float fontSize = 16f, Action<Image<Rgba32>, Font, Color, int, int, int> drawAction = null)
    {
        return GenQrOrBarCode(BarcodeFormat.CODE_128, content, width, height, dWidth, showContent, fontSize, drawAction);
    }

    /// <summary>
    /// 生成 QrCode 二维码
    /// </summary>
    /// <param name="content"><see cref="string"/> 二维码内容</param>
    /// <param name="width"><see cref="int"/> 二维码宽度</param>
    /// <param name="height"><see cref="int"/> 二维码高度</param>
    /// <param name="drawAction"><see cref="Action{T}"/> 操作 drawAction(image, font, color, lineHeight, width, height)</param>
    /// <returns></returns>
    public static Image GenQrCode(string content, int width = 200, int height = 200,
        Action<Image<Rgba32>, Font, Color, int, int, int> drawAction = null)
    {
        return GenQrOrBarCode(BarcodeFormat.QR_CODE, content, width, height, width, false, 16f, drawAction);
    }

    /// <summary>
    /// 生成条码或二维码
    /// </summary>
    /// <returns></returns>
    private static Image GenQrOrBarCode(BarcodeFormat barcodeFormat, string content, int width = 100, int height = 100,
        int dWidth = 100, bool showContent = true, float fontSize = 16f,
        Action<Image<Rgba32>, Font, Color, int, int, int> drawAction = null)
    {
        var barcodeWriterPixelData = new BarcodeWriter<PixelData>
        {
            // 使用 ITF 格式，不能被现在常用的支付宝，微信扫出来
            // 如果想生成可识别的，可以使用 CODE_128 格式
            Format = barcodeFormat,
            Options = new EncodingOptions
            {
                // 条形码宽度
                Width = width,
                // 条形码高度
                Height = height,
                // 条形码前后间距
                Margin = 0
            },
            Renderer = new PixelDataRenderer
            {
                Foreground = new PixelDataRenderer.Color(unchecked((int) 0xFF000000)),
                Background = new PixelDataRenderer.Color(unchecked((int) 0xFFFFFFFF))
            }
        };

        // 写入内容
        var pixelData = barcodeWriterPixelData.Write(content);

        // 文字字体
        var textFont = new Font(BoldFont, fontSize);

        // 固定颜色
        var color = Color.Black;

        // 判断是否显示详情文本
        if (showContent)
        {
            // 条形码的高度 + 字体大小 + 5 像素
            var dHeight = height + (int) fontSize + 5;

            // 文字Y轴位置
            var textY = pixelData.Height;

            // 判断如果是条形码，则 -4 二维码则 -10
            switch (barcodeFormat)
            {
                case BarcodeFormat.ITF:
                case BarcodeFormat.CODE_39:
                case BarcodeFormat.CODE_128:
                    //textY -= 5;
                    break;
                case BarcodeFormat.QR_CODE:
                    textY -= 5;
                    break;
            }

            // 写入详情文本，居中
            using var loadPixelData = GenImage((image, _, _, _, _, _) =>
            {
                // 获取条形码图片
                var barcodeImage = Image.LoadPixelData<Rgba32>(pixelData.Pixels, pixelData.Width, pixelData.Height);

                // 绘制条形码
                image.Mutate(ctx =>
                {
                    // 绘制条形码
                    ctx.DrawImage(barcodeImage, new Point((dWidth - pixelData.Width) / 2, 0), 1f);

                    // 将文字写入底图中，文字居中显示
                    ctx.DrawText(content, textFont, color,
                        new PointF((dWidth - GetFontWidthByContent(content, textFont)) / 2, textY));
                });

                // 其余操作
                drawAction?.Invoke(image, textFont, color, DefaultLineHeight, dWidth, dHeight);
            }, dWidth, dHeight, fontSize);

            // 这里克隆一份，避免被提前释放
            return loadPixelData.Clone();
        }
        else
        {
            using var loadPixelData = GenImage((image, _, _, _, _, _) =>
            {
                // 获取条形码图片
                var barcodeImage = Image.LoadPixelData<Rgba32>(pixelData.Pixels, pixelData.Width, pixelData.Height);

                // 绘制条形码
                image.Mutate(ctx =>
                {
                    // 绘制条形码
                    ctx.DrawImage(barcodeImage, new Point((dWidth - pixelData.Width) / 2, 0), 1f);
                });

                // 其余操作
                drawAction?.Invoke(image, textFont, color, DefaultLineHeight, dWidth, height);
            }, dWidth, height, fontSize);

            // 这里克隆一份，避免被提前释放
            return loadPixelData.Clone();
        }
    }
}