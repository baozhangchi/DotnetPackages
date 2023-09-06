#region

using System.Windows;
using System.Windows.Media;

#endregion

namespace Packages.Windows
{
    /// <summary>
    ///     Dpi转换相关
    /// </summary>
    public class DpiUtils
    {
        /// <summary>
        ///     Y轴Dpi
        /// </summary>
        public static double DpiY { get; set; }

        /// <summary>
        ///     X轴Dpi
        /// </summary>
        public static double DpiX { get; set; }

        /// <summary>
        ///     转换Point
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Point DevideByDpi(Point p)
        {
            return new Point(p.X / DpiX, p.Y / DpiY);
        }

        /// <summary>
        ///     转换Rect
        /// </summary>
        /// <param name="r"></param>
        /// <returns></returns>
        public static Rect DevideByApi(Rect r)
        {
            return new Rect(r.Left / DpiX, r.Top / DpiY, r.Width, r.Height);
        }

        /// <summary>
        ///     初始化Dpi
        /// </summary>
        /// <param name="visual"></param>
        public static void Init(Visual visual)
        {
            var transformToDevice = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice;
            DpiX = transformToDevice.M11;
            DpiY = transformToDevice.M22;
        }
    }
}