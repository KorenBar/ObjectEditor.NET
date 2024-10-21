using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.Extensions
{
    public static class CastingExtensions
    {
        /// <summary>
        /// Casts a KeyValuePair&lt;TKey, TValue&gt; object to a KeyValuePair&lt;targetKeyType, targetValueType&gt; as an object.
        /// </summary>
        /// <typeparam name="TKey">The type of the key in the source KeyValuePair&lt;TKey, TValue&gt; object.</typeparam>
        /// <typeparam name="TValue">The type of the value in the source KeyValuePair&lt;TKey, TValue&gt; object.</typeparam>
        /// <param name="sourcePair">The source KeyValuePair&lt;TKey, TValue&gt; object to cast from.</param>
        /// <param name="targetKeyType">The type of the key in the target KeyValuePair&lt;targetKeyType, targetValueType&gt; object.</param>
        /// <param name="targetValueType">The type of the value in the target KeyValuePair&lt;targetKeyType, targetValueType&gt; object.</param>
        /// <returns>A KeyValuePair&lt;targetKeyType, targetValueType&gt; as an object.</returns>
        public static object CastTo<TKey, TValue>(this KeyValuePair<TKey, TValue> sourcePair, Type targetKeyType, Type targetValueType)
        {
            var kvpType = typeof(KeyValuePair<,>).MakeGenericType(targetKeyType, targetValueType);
            var kvpCtor = kvpType.GetConstructor(new Type[] { targetKeyType, targetValueType });
            return kvpCtor.Invoke(new object[] { sourcePair.Key, sourcePair.Value });
        }

        /// <summary>
        /// Tries to cast a KeyValuePair&lt;TKey, TValue&gt; object to a KeyValuePair&lt;targetKeyType, targetValueType&gt; as an object.
        /// </summary>
        /// <typeparam name="TKey">The type of the key in the source KeyValuePair&lt;TKey, TValue&gt; object.</typeparam>
        /// <typeparam name="TValue">The type of the value in the source KeyValuePair&lt;TKey, TValue&gt; object.</typeparam>
        /// <param name="sourcePair">The source KeyValuePair&lt;TKey, TValue&gt; object to cast from.</param>
        /// <param name="targetKeyType">The type of the key in the target KeyValuePair&lt;targetKeyType, targetValueType&gt; object.</param>
        /// <param name="targetValueType">The type of the value in the target KeyValuePair&lt;targetKeyType, targetValueType&gt; object.</param>
        /// <param name="result">The resulting KeyValuePair&lt;targetKeyType, targetValueType&gt; object.</param>
        /// <returns>true if the cast was successful; otherwise, false.</returns>
        public static bool TryCastTo<TKey, TValue>(this KeyValuePair<TKey, TValue> sourcePair, Type targetKeyType, Type targetValueType, out object result)
        {
            try
            {
                result = sourcePair.CastTo(targetKeyType, targetValueType);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Casts any KeyValuePair&lt;,&gt; object to a KeyValuePair&lt;TKey, TValue&gt; object.
        /// </summary>
        /// <typeparam name="TKey">The type of the key in the target KeyValuePair&lt;TKey, TValue&gt; object.</typeparam>
        /// <typeparam name="TValue">The type of the value in the target KeyValuePair&lt;TKey, TValue&gt; object.</typeparam>
        /// <param name="sourcePair">The source KeyValuePair&lt;,&gt; object to cast from.</param>
        /// <returns>A KeyValuePair&lt;TKey, TValue&gt; object.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">When the source object is not a KeyValuePair&lt;,&gt; object.</exception>
        public static KeyValuePair<TKey, TValue> CastKeyValuePair<TKey, TValue>(this object sourcePair)
        {
            if (sourcePair == null)
                throw new ArgumentNullException(nameof(sourcePair));
            if (sourcePair.GetType().GetGenericTypeDefinition() != typeof(KeyValuePair<,>))
                throw new ArgumentException("The source object is not a KeyValuePair<,> object.", nameof(sourcePair));

            var key = (TKey)sourcePair.GetType().GetProperty(nameof(KeyValuePair<TKey, TValue>.Key)).GetValue(sourcePair);
            var value = (TValue)sourcePair.GetType().GetProperty(nameof(KeyValuePair<TKey, TValue>.Value)).GetValue(sourcePair);
            return new KeyValuePair<TKey, TValue>(key, value);
        }
    }
}
