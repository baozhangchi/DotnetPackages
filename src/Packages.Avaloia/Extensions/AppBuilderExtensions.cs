#region

using System.Linq;
using Avalonia.Media;

#endregion

// ReSharper disable once IdentifierTypo
// ReSharper disable once CheckNamespace
namespace Avalonia
{
    /// <summary>
    ///     AppBuilder的扩展方法
    /// </summary>
    public static class AppBuilderExtensions
    {
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
    }
}