#region

using System;
using System.Linq;
using Avalonia.Media;
using Microsoft.Extensions.Configuration;
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
    public static IConfiguration Configuration { get; set; }

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
    ///     加载配置文件
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static AppBuilder LoadConfig(this AppBuilder builder)
    {
        return LoadConfig(builder, "appSettings.json");
    }

    private static AppBuilder LoadConfig(AppBuilder builder, string settingsFile, params string[] optionalSettingsFiles)
    {
        var configurationBuilder = new ConfigurationBuilder().AddJsonFile(settingsFile, false, true);
        foreach (var optionalSettingsFile in optionalSettingsFiles)
        {
            configurationBuilder.AddJsonFile(optionalSettingsFile, true, true);
        }

        Configuration = configurationBuilder.Build();
        var fonts = Configuration["Fonts"]?.Split(',', StringSplitOptions.RemoveEmptyEntries);
        if (fonts == null || !fonts.Any())
        {
            return builder;
        }

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

        return builder;
    }
}