#region

using System.Collections.Generic;
using System.Runtime.CompilerServices;

#endregion

// ReSharper disable once CheckNamespace
namespace System.ComponentModel
{
    /// <summary>
    ///     INotifyPropertyChanged接口实现
    /// </summary>
    public abstract class PropertyChangedBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        ///     通知属性更改
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void NotifyOfPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        ///     当属性发生变更时
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
        }

        /// <summary>
        ///     设置属性值
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            NotifyOfPropertyChanging(propertyName);
            field = value;
            NotifyOfPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        ///     通知属性即将更改
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void NotifyOfPropertyChanging(string propertyName)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
            OnPropertyChanging(propertyName);
        }

        /// <summary>
        ///     当属性值将发生变化时
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanging(string propertyName)
        {
        }
    }
}