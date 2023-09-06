#region

using System.Windows;
using System.Windows.Media;

#endregion

namespace Packages.Windows
{
    public class DpiUtils
    {
        public static double DpiY { get; set; }

        public static double DpiX { get; set; }

        public static Point DevideByDpi(Point p)
        {
            return new Point(p.X / DpiX, p.Y / DpiY);
        }

        public static Rect DevideByApi(Rect r)
        {
            return new Rect(r.Left / DpiX, r.Top / DpiY, r.Width, r.Height);
        }

        public static void Init(Visual visual)
        {
            var transformToDevice = PresentationSource.FromVisual(visual).CompositionTarget.TransformToDevice;
            DpiX = transformToDevice.M11;
            DpiY = transformToDevice.M22;
        }
    }
}