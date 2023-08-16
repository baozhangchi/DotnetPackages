#region

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

#endregion

// ReSharper disable once CheckNamespace
namespace System.Collections
{
    /// <summary>
    ///     支持通知字典
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged where TKey : notnull
    {
        #region Fields

        private readonly IDictionary<TKey, TValue> _dictionary;

        #endregion

        #region Methods

        /// <inheritdoc />
        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Constructors

        /// <summary>
        ///     实例化一个ObservableDictionary
        /// </summary>
        public ObservableDictionary() : this(new Dictionary<TKey, TValue>())
        {
        }

        /// <summary>
        ///     实例化一个ObservableDictionary
        /// </summary>
        /// <param name="dictionary"></param>
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary;
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return RemoveWithNotification(item.Key);
        }

        /// <inheritdoc />

        #endregion

        #region Properties

        public int Count => _dictionary.Count;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => _dictionary.IsReadOnly;

        /// <inheritdoc />

        public ICollection<TKey> Keys => _dictionary.Keys;

        /// <inheritdoc />

        public ICollection<TValue> Values => _dictionary.Values;

        /// <inheritdoc />
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            AddWithNotification(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _dictionary.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Keys)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Values)));
        }

        private void AddWithNotification(KeyValuePair<TKey, TValue> item)
        {
            AddWithNotification(item.Key, item.Value);
        }

        /// <inheritdoc />

        #endregion

        #region Methods

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public void Add(TKey key, TValue value)
        {
            AddWithNotification(key, value);
        }

        /// <inheritdoc />
        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <inheritdoc />
        public bool Remove(TKey key)
        {
            return RemoveWithNotification(key);
        }

        /// <inheritdoc />
#pragma warning disable CS8767
        public bool TryGetValue(TKey key, out TValue? value)
#pragma warning restore CS8767
        {
            value = default;
            var result = _dictionary.TryGetValue(key, out var item);
            if (item != null)
            {
                value = item;
            }

            return result;
        }

        /// <inheritdoc />
        public TValue this[TKey key] { get => _dictionary[key]; set => UpdateWithNotification(key, value); }

        private void UpdateWithNotification(TKey key, TValue value)
        {
            if (_dictionary.TryGetValue(key, out var oldValue))
            {
                if (!EqualityComparer<TValue>.Default.Equals(oldValue, value))
                {
                    var index = Keys.ToList().IndexOf(key);
                    _dictionary[key] = value;
                    CollectionChanged?.Invoke
                    (
                        this,
                        new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value), new KeyValuePair<TKey, TValue>(key, oldValue), index)
                    );
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Values)));
                }
                else
                {
                    AddWithNotification(key, value);
                }
            }
        }

        private void AddWithNotification(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Keys)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Values)));
        }

        private bool RemoveWithNotification(TKey key)
        {
            if (_dictionary.TryGetValue(key, out var value))
            {
                var index = Keys.ToList().IndexOf(key);
                if (_dictionary.Remove(key))
                {
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value), index));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Count)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Keys)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Values)));

                    return true;
                }
            }

            return false;
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dictionary).GetEnumerator();
        }

        /// <inheritdoc />

        #endregion

        public event NotifyCollectionChangedEventHandler? CollectionChanged;
    }
}