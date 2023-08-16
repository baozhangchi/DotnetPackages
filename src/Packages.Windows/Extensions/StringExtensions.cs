#region

using System.Globalization;
using System.Windows;
using System.Windows.Media;

#endregion

// ReSharper disable once CheckNamespace
namespace System
{
    /// <summary>
    ///     字符串扩展方法
    /// </summary>
    public static class StringExtensions
    {
        #region Methods

#if NET5_0_OR_GREATER
        /// <summary>
        ///     测量字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="container"></param>
        /// <param name="fontFamily"></param>
        /// <param name="fontStyle"></param>
        /// <param name="fontWeight"></param>
        /// <param name="fontStretch"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
        public static Size MeasureString(this string input, Visual container, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
        {
            var formattedText = new FormattedText
            (
                input, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface(fontFamily, fontStyle, fontWeight, fontStretch), fontSize, Brushes.Black,
                VisualTreeHelper.GetDpi(container).PixelsPerDip
            );
            return new Size(formattedText.Width, formattedText.Height);
        }
#endif
        /// <summary>
        ///     测量字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="fontFamily"></param>
        /// <param name="fontStyle"></param>
        /// <param name="fontWeight"></param>
        /// <param name="fontStretch"></param>
        /// <param name="fontSize"></param>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [Obsolete]
#endif
        public static Size MeasureString(this string input, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
        {
            var formattedText = new FormattedText
            (
                input, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface(fontFamily, fontStyle, fontWeight, fontStretch), fontSize, Brushes.Black, new NumberSubstitution(),
                TextFormattingMode.Display
            );
            return new Size(formattedText.Width, formattedText.Height);
        }

        #endregion
    }
}