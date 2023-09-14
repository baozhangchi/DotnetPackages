#region

using System;
using System.IO;
using System.Linq;
using Avalonia.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FontManager = System.Drawing.Text.FontManager;

#endregion

// ReSharper disable once IdentifierTypo
// ReSharper disable once CheckNamespace
namespace Avalonia;

/// <summary>
///     AppBuilder的扩展方法
/// </summary>
public static class AppBuilderExtensions
{
    static AppBuilderExtensions()
    {
        ConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"appSettings.json");
    }

    /// <summary>
    ///     设置默认字体
    /// </summary>
    /// <param name="appBuilder"></param>
    /// <param name="defaultFamilyName">默认字体名称</param>
    /// <param name="fallbackFonts">备用字体</param>
    /// <returns></returns>
    public static AppBuilder UseCustomDefaultFont(this AppBuilder appBuilder, string defaultFamilyName,
        params string[] fallbackFonts)
    {
        var fallbacks = fallbackFonts.ToList();
        fallbacks.Insert(0, defaultFamilyName);
        return appBuilder.With(new FontManagerOptions
        {
            DefaultFamilyName = defaultFamilyName,
            FontFallbacks = fallbacks.Select(x => new FontFallback
            {
                FontFamily = new FontFamily(x)
            }).ToList()
        });
    }

    /// <summary>
    /// 加载配置文件
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static AppBuilder LoadConfig(this AppBuilder builder)
    {
        if (File.Exists(ConfigFile))
        {
            var obj = JsonConvert.DeserializeObject<JObject>(File.ReadAllText(ConfigFile));
            if (obj != null)
            {
                if (obj.TryGetValue("Fonts", StringComparison.OrdinalIgnoreCase, out var value))
                {
                    var fonts = value.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                    var systemFonts = FontManager.GetFontFamilyNames();
                    fonts = fonts.Where(x => systemFonts.Contains(x, StringComparer.OrdinalIgnoreCase)).ToArray();
                    if (fonts.Length > 0)
                    {
                        var options = new FontManagerOptions
                        {
                            DefaultFamilyName = fonts.FirstOrDefault(),
                            FontFallbacks = fonts.Select(x => new FontFallback { FontFamily = new FontFamily(x) })
                                .ToList()
                        };

                        builder.With(options);
                    }
                }
            }
        }

        return builder;
    }

    /// <summary>
    /// 获取或这是配置文件路径
    /// 默认为应用目录下appSettings.json文件
    /// </summary>
    public static string ConfigFile { get; set; }
}