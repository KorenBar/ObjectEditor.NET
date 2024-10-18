using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ObjectEditor.Extensions;

namespace ObjectEditor.Data
{
    /// <summary>
    /// Reference any IDictionary<K, V> as IDictionary<object, object> to access its elements as objects dynamically.
    /// </summary>
    public class DynamicDictionaryWrapper : DynamicCollectionWrapper, IDictionary<object, object>
    {
        /// <summary>
        /// The generic type definition of the source dictionary.
        /// </summary>
        Type _genericTypeDefinition;

        /// <summary>
        /// The generic argument type of the source dictionary keys.
        /// </summary>
        public Type KeyType { get; }
        /// <summary>
        /// The generic argument type of the source dictionary values.
        /// </summary>
        public Type ValueType { get; }

        #region Dynamic Members Info
        MethodInfo GetKeys => _genericTypeDefinition.GetProperty("Keys").GetGetMethod();
        MethodInfo GetValues => _genericTypeDefinition.GetProperty("Values").GetGetMethod();
        MethodInfo IndexerGet => _genericTypeDefinition.GetProperty("Item").GetGetMethod();
        MethodInfo IndexerSet => _genericTypeDefinition.GetProperty("Item").GetSetMethod();
        MethodInfo AddMethod => _genericTypeDefinition.GetMethod("Add", new Type[] { KeyType, ValueType });
        MethodInfo AddEntryMethod => _genericTypeDefinition.GetMethod("Add", new Type[] { typeof(KeyValuePair<,>) });
        MethodInfo ContainsKeyMethod => _genericTypeDefinition.GetMethod("ContainsKey");
        MethodInfo RemoveMethod => _genericTypeDefinition.GetMethod("Remove", new Type[] { KeyType });
        MethodInfo TryGetValueMethod => _genericTypeDefinition.GetMethod("TryGetValue");
        #endregion

        /// <summary>
        /// Wraps a generic dictionary as a dictionary of objects.
        /// </summary>
        /// <param name="objects">The source dictionary to wrap.</param>
        /// <exception cref="ArgumentException"></exception>
        public DynamicDictionaryWrapper(IEnumerable objects)
            : base(objects)
        {
            _genericTypeDefinition = objects.GetType().GetGenericType(typeof(IDictionary<,>));

            if (_genericTypeDefinition == null)
                throw new ArgumentException("The source enumerable must be a generic dictionary.", nameof(objects));

            var genericArguments = _genericTypeDefinition.GetGenericArguments();
            KeyType = genericArguments[0];
            ValueType = genericArguments[1];
        }

        #region Overrides
        /// <summary>
        /// Adds a new entry with the default key and value types.
        /// </summary>
        /// <returns>The new default added entry.</returns>
        public override object AddDefaultValue()
        {
            var entry = KeyValuePair.Create(KeyType.GetDefaultValue(), ValueType.GetDefaultValue()).CastTo(KeyType, ValueType);
            Add(entry);
            return entry;
        }
        #endregion

        #region IDictionary<object, object> implementation
        public object this[object key]
        {
            get => IndexerGet.Invoke(Source, new object[] { key });
            set => IndexerSet.Invoke(Source, new object[] { key, value });
        }

        public ICollection<object> Keys
        {
            get
            {
                var e = GetKeys.Invoke(Source, null) as IEnumerable;
                return e != null ? e.Cast<object>().ToList() : null;
            }
        }
        public ICollection<object> Values
        {
            get
            {
                var e = GetValues.Invoke(Source, null) as IEnumerable;
                return e != null ? e.Cast<object>().ToList() : null;
            }
        }

        public void Add(object key, object value) => AddMethod.Invoke(Source, new object[] { key, value });
        public void Add(KeyValuePair<object, object> entry) => base.Add(entry.CastTo(KeyType, ValueType));
        public bool Contains(KeyValuePair<object, object> entry) => base.Contains(entry.CastTo(KeyType, ValueType));
        public bool ContainsKey(object key) => ContainsKeyMethod.Invoke(Source, new object[] { key }) as bool? ?? false;
        public void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex)
        {
            foreach (var obj in Source)
                array[arrayIndex++] = obj.CastKeyValuePair<object, object>();
        }
        public new IEnumerator<KeyValuePair<object, object>> GetEnumerator()
        {
            foreach (var obj in Source)
                yield return obj.CastKeyValuePair<object, object>(); // obj is not necessarily a KeyValuePair<object, object>, cast it
        }
        public new bool Remove(object key) => RemoveMethod.Invoke(Source, new object[] { key }) as bool? ?? false;
        public bool Remove(KeyValuePair<object, object> item) => base.Remove(item.CastTo(KeyType, ValueType));
        public bool TryGetValue(object key, out object value) => TryGetValueMethod.Invoke(Source, new object[] { key, value = null }) as bool? ?? false;
        #endregion
    }
}
