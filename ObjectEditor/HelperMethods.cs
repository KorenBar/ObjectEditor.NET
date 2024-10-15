using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor
{
    internal class HelperMethods
    {
        /// <summary>
        /// Creates a key-value pair object dynamically.
        /// </summary>
        /// <param name="keyType">The type of the key.</param>
        /// <param name="key">The key as an object, must be of the key type.</param>
        /// <param name="valueType">The type of the value.</param>
        /// <param name="value">The value as an object, must be of the value type.</param>
        /// <returns>A key-value pair KeyValuePair&lt;keyType, valueType&gt; as an object.</returns>
        public static object CreateKeyValuePair(Type keyType, object key, Type valueType, object value)
        {
            var kvpType = typeof(KeyValuePair<,>).MakeGenericType(keyType, valueType);
            var kvpCtor = kvpType.GetConstructor(new Type[] { keyType, valueType });
            return kvpCtor.Invoke(new object[] { key, value });
        }
    }
}
