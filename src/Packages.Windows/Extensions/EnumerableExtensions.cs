#region

using System.Collections.Generic;

#endregion

// ReSharper disable once CheckNamespace
namespace System.Linq
{
    /// <summary>
    ///     集合扩展方法
    /// </summary>
    public static class EnumerableExtensions
    {
        #region Fields

        private class DelegateEqualityComparer<T, TKey> : IEqualityComparer<T>
        {
            #region Fields

            private readonly Func<T, TKey> _selector;

            #endregion

            #region Constructors

            public DelegateEqualityComparer(Func<T, TKey> selector)
            {
                _selector = selector;
            }

            #endregion

            #region Methods

            public bool Equals(T x, T y)
            {
                if (x == null && y == null)
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                return EqualityComparer<TKey>.Default.Equals(_selector(x), _selector(y));
            }

            public int GetHashCode(T obj)
            {
                var value = _selector(obj);
                return value == null ? default : EqualityComparer<TKey>.Default.GetHashCode(value);
            }

            #endregion
        }

        #endregion

        #region Methods

        /// <summary>
        ///     添加或更新项
        /// </summary>
        /// <param name="dictionary">子电源</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            dictionary[key] = value;
        }

        /// <summary>
        ///     获取或添加值
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        public static void GetOrAddValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, out TValue value, TValue defaultValue)
        {
            if (!dictionary.TryGetValue(key, out value))
            {
                dictionary.Add(key, defaultValue);
                value = defaultValue;
            }
        }

        /// <summary>
        ///     通过指定的对象选择器返回序列中的非重复元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="selector">对象选择器</param>
        /// <returns>返回去重后的结果</returns>
        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
        {
            return source.Distinct(new DelegateEqualityComparer<T, TKey>(selector));
        }

        /// <summary>
        ///     通过指定的对象选择器生成两个需略的差集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="selector">对象选择器</param>
        /// <returns></returns>
        public static IEnumerable<T> Except<T, TKey>(this IEnumerable<T> left, IEnumerable<T> right, Func<T, TKey> selector)
        {
            return left.Except(right, new DelegateEqualityComparer<T, TKey>(selector));
        }

        /// <summary>
        ///     通过指定的对象选择器生成两个需略的交集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="selector">对象选择器</param>
        /// <returns></returns>
        public static IEnumerable<T> Intersect<T, TKey>(this IEnumerable<T> left, IEnumerable<T> right, Func<T, TKey> selector)
        {
            return left.Intersect(right, new DelegateEqualityComparer<T, TKey>(selector));
        }

        /// <summary>
        ///     通过指定的对象选择器生成两个需略的并集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="selector">对象选择器</param>
        /// <returns></returns>
        public static IEnumerable<T> Union<T, TKey>(this IEnumerable<T> left, IEnumerable<T> right, Func<T, TKey> selector)
        {
            return left.Union(right, new DelegateEqualityComparer<T, TKey>(selector));
        }

        #endregion
    }
}