#region

using System.Windows.Media;

#endregion

// ReSharper disable once CheckNamespace
namespace System.Windows
{
    /// <summary>
    ///     DependencyObject扩展方法
    /// </summary>
    public static class DependencyObjectExtensions
    {
        #region Properties

        /// <summary>
        ///     按类型获取父级对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d"></param>
        /// <param name="level">回溯层级</param>
        /// <returns></returns>
        public static T? GetParentUtil<T>(this DependencyObject d, int level) where T : DependencyObject
        {
            var item = VisualTreeHelper.GetParent(d);
            if (item == null)
            {
                return default;
            }

            var parent = item as T ?? item.GetParentUtil<T>(level);
            if (parent != null)
            {
                return level > 0 ? parent.GetParentUtil<T>(level - 1) : parent;
            }

            return null;
        }

        /// <summary>
        ///     按类型获取父级对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d"></param>
        /// <returns></returns>
        public static T? GetParentUtil<T>(this DependencyObject d) where T : DependencyObject
        {
            return d.GetParentUtil<T>(0);
        }

        /// <summary>
        ///     按类型获取子对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d"></param>
        /// <returns></returns>
        public static T? GetVisualChild<T>(this DependencyObject d) where T : DependencyObject
        {
            var num = VisualTreeHelper.GetChildrenCount(d);
            for (var i = 0; i < num; i++)
            {
                var item = VisualTreeHelper.GetChild(d, i);
                var child = item as T ?? item.GetVisualChild<T>();
                if (child != null)
                {
                    break;
                }
            }

            return default;
        }

        #endregion
    }
}