#region

using System.Linq;
using Avalonia;
using Avalonia.VisualTree;

#endregion

namespace Packages.Avalonia.Extensions
{
    /// <summary>
    ///     Visual的扩展方法
    /// </summary>
    public static class VisualExtensions
    {
        /// <summary>
        ///     按类型搜索子对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="visual"></param>
        /// <returns></returns>
        public static T FindChildByType<T>(this Visual visual)
            where T : Visual
        {
            foreach (var child in visual.GetVisualChildren().ToList())
            {
                if (child is T target)
                {
                    return target;
                }

                var result = child.FindChildByType<T>();
                if (result != null)
                {
                    return result;
                }
            }

            return default;
        }
    }
}