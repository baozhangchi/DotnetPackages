#region

using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

#endregion

namespace Packages.Windows.Behaviors

{
    /// <inheritdoc />
    public class GridBehavior : Behavior<Grid>
    {
        // Using a DependencyProperty as the backing store for RowCount.  This enables animation, styling, binding, etc...
        /// <summary>
        ///     RowCountProperty
        /// </summary>
        public static readonly DependencyProperty RowCountProperty =
            DependencyProperty.RegisterAttached("RowCount", typeof(int), typeof(GridBehavior)
                , new PropertyMetadata(0, OnRowCountPropertyChanged));

        // Using a DependencyProperty as the backing store for ColumnCount.  This enables animation, styling, binding, etc...
        /// <summary>
        ///     ColumnCountProperty
        /// </summary>
        public static readonly DependencyProperty ColumnCountProperty =
            DependencyProperty.RegisterAttached("ColumnCount", typeof(int), typeof(GridBehavior),
                new PropertyMetadata(0, OnColumnCountPropertyChanged));

        // Using a DependencyProperty as the backing store for RowName.  This enables animation, styling, binding, etc...
        /// <summary>
        ///     RowNameProperty
        /// </summary>
        public static readonly DependencyProperty RowNameProperty =
            DependencyProperty.RegisterAttached("RowName", typeof(string), typeof(GridBehavior),
                new PropertyMetadata(default(string), OnRowNamePropertyChanged));

        // Using a DependencyProperty as the backing store for ColumnName.  This enables animation, styling, binding, etc...
        /// <summary>
        ///     ColumnNameProperty
        /// </summary>
        public static readonly DependencyProperty ColumnNameProperty =
            DependencyProperty.RegisterAttached("ColumnName", typeof(string), typeof(GridBehavior),
                new PropertyMetadata(default(string), OnColumnNamePropertyChanged));

        private static void OnColumnCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid grid)
            {
                grid.ColumnDefinitions.Clear();
                for (var i = 0; i < GetColumnCount(grid); i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                }
            }
        }

        private static void OnRowCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid grid)
            {
                grid.RowDefinitions.Clear();
                for (var i = 0; i < GetRowCount(grid); i++)
                {
                    grid.RowDefinitions.Add(new RowDefinition());
                }
            }
        }

        /// <summary>
        ///     获取RowCount
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetRowCount(DependencyObject obj)
        {
            return (int)obj.GetValue(RowCountProperty);
        }

        /// <summary>
        ///     设置RowCount
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetRowCount(DependencyObject obj, int value)
        {
            obj.SetValue(RowCountProperty, value);
        }

        /// <summary>
        ///     获取ColumnCount
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetColumnCount(DependencyObject obj)
        {
            return (int)obj.GetValue(ColumnCountProperty);
        }

        /// <summary>
        ///     设置ColumnCount
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetColumnCount(DependencyObject obj, int value)
        {
            obj.SetValue(ColumnCountProperty, value);
        }

        /// <summary>
        ///     获取行名称
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetRowName(DependencyObject obj)
        {
            return (string)obj.GetValue(RowNameProperty);
        }

        /// <summary>
        ///     设置行名称
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetRowName(DependencyObject obj, string value)
        {
            obj.SetValue(RowNameProperty, value);
        }

        private static void OnRowNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                if (element.Parent is Grid grid)
                {
                    for (var index = 0; index < grid.RowDefinitions.Count; index++)
                    {
                        if (!string.IsNullOrWhiteSpace(grid.RowDefinitions[index].Name) &&
                            grid.RowDefinitions[index].Name == GetRowName(element))
                        {
                            Grid.SetRow(element, index);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     获取列名称
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetColumnName(DependencyObject obj)
        {
            return (string)obj.GetValue(ColumnNameProperty);
        }

        /// <summary>
        ///     设置列名称
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetColumnName(DependencyObject obj, string value)
        {
            obj.SetValue(ColumnNameProperty, value);
        }

        private static void OnColumnNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                if (element.Parent is Grid grid)
                {
                    for (var index = 0; index < grid.ColumnDefinitions.Count; index++)
                    {
                        if (!string.IsNullOrWhiteSpace(grid.ColumnDefinitions[index].Name) &&
                            grid.ColumnDefinitions[index].Name == GetColumnName(element))
                        {
                            Grid.SetColumn(element, index);
                            break;
                        }
                    }
                }
            }
        }
    }
}