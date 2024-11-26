using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoCommerceDbApp
{
    public class DictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly ICollection<TValue> _values;
        private readonly Func<TValue, TKey> _keySelector;
        private readonly Action<TValue, TKey> _keySetter;

        public DictionaryWrapper(ICollection<TValue> values, Func<TValue, TKey> keySelector, Action<TValue, TKey> keySetter)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            _values = values;
            _keySelector = keySelector;
            _keySetter = keySetter;
        }

        public TValue this[TKey key]
        {
            get => _values.FirstOrDefault(v => _keySelector(v).Equals(key));
            set
            {
                var existing = _values.FirstOrDefault(v => _keySelector(v).Equals(key));
                if (existing != null)
                {
                    _values.Remove(existing);
                }
                _keySetter(value, key);
                _values.Add(value);
            }
        }

        public ICollection<TKey> Keys => _values.Select(_keySelector).ToList();

        public ICollection<TValue> Values => _values;

        public int Count => _values.Count;

        public bool IsReadOnly => _values.IsReadOnly;

        public void Add(TKey key, TValue value)
        {
            if (ContainsKey(key))
                throw new ArgumentException("An item with the same key has already been added.");

            _keySetter(value, key);
            _values.Add(value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _values.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _values.Any(v => _keySelector(v).Equals(item.Key) && v.Equals(item.Value));
        }

        public bool ContainsKey(TKey key)
        {
            return _values.Any(v => _keySelector(v).Equals(key));
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            foreach (var value in _values)
            {
                array[arrayIndex++] = new KeyValuePair<TKey, TValue>(_keySelector(value), value);
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _values.Select(v => new KeyValuePair<TKey, TValue>(_keySelector(v), v)).GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            var existing = _values.FirstOrDefault(v => _keySelector(v).Equals(key));
            if (existing != null)
            {
                _values.Remove(existing);
                return true;
            }
            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var existing = _values.FirstOrDefault(v => _keySelector(v).Equals(item.Key) && v.Equals(item.Value));
            if (existing != null)
            {
                _values.Remove(existing);
                return true;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var existing = _values.FirstOrDefault(v => _keySelector(v).Equals(key));
            if (existing != null)
            {
                value = existing;
                return true;
            }
            value = default;
            return false;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
