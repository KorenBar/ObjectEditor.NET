using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TechnosoCommons.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets the default value for a type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>A new default value if it's a value type or class type with a default constructor, otherwise null.</returns>
        public static object GetDefaultValue(this Type type)
        {
            if (type == null) return null;

            // custom default value for some types
            if (type.Equals(typeof(string)))
                return string.Empty;

            // check if the type has a default constructor
            if (type.IsClass && type.GetConstructor(Type.EmptyTypes) == null)
                return null; // class type without parameterless constructor

            if (!type.IsValueType && !type.IsClass)
                return null; // not a value type or class - cannot create an instance

            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// Gets the generic collection type of a type.
        /// </summary>
        /// <param name="type">A type that implements ICollection&lt;T&gt;.</param>
        /// <returns> The generic type T of the collection, or null if the type is not a collection.</returns>
        public static Type GetGenericCollectionType(this Type type)
            => type.GetGenericType(typeof(ICollection<>));

        /// <summary>
        /// Gets the generic type of a type that implements it with a specific generic type definition.
        /// </summary>
        /// <param name="type">A type that implements the given generic type definition.</param>
        /// <param name="genericTypeDefinition">The generic type definition to find.</param>
        /// <returns></returns>
        public static Type GetGenericType(this Type type, Type genericTypeDefinition)
        {
            if (type == null) return null;
            return type.GetInterfaces().FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == genericTypeDefinition);
        }

        /// <summary>
        /// Gets the inheritance chain of a type.
        /// </summary>
        /// <param name="type">The type to get the inheritance chain of.</param>
        /// <returns>The inheritance chain of the type, starting from the type itself and ending with the base type.</returns>
        public static IEnumerable<Type> GetInheritanceChain(this Type type)
        {
            if (type == null) yield break;
            for (Type t = type; t != null; t = t.BaseType)
                yield return t;
        }

        /// <summary>
        /// Get the maximum value of a type.
        /// </summary>
        /// <param name="type">The type to get the maximum value of.</param>
        /// <returns>The maximum value of the type, or 0 if the type is not a numeric type.</returns>
        public static decimal MaxValue(this Type type)
        {
            if (type.IsNullable())
                return MaxValue(Nullable.GetUnderlyingType(type));

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte: return Byte.MaxValue;
                case TypeCode.SByte: return SByte.MaxValue;
                case TypeCode.UInt16: return UInt16.MaxValue;
                case TypeCode.UInt32: return UInt32.MaxValue;
                case TypeCode.UInt64: return UInt64.MaxValue;
                case TypeCode.Int16: return Int16.MaxValue;
                case TypeCode.Int32: return Int32.MaxValue;
                case TypeCode.Int64: return Int64.MaxValue;
                case TypeCode.Decimal: return Decimal.MaxValue;
                case TypeCode.Double: return Decimal.MaxValue; // Double is bigger than Decimal
                case TypeCode.Single: return Decimal.MaxValue; // Single is bigger than Decimal
                default: return 0M;
            }
        }

        /// <summary>
        /// Get the minimum value of a type.
        /// </summary>
        /// <param name="type">The type to get the minimum value of.</param>
        /// <returns>The minimum value of the type, or 0 if the type is not a numeric type.</returns>
        public static decimal MinValue(this Type type)
        {
            if (type.IsNullable())
                return MaxValue(Nullable.GetUnderlyingType(type));

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte: return Byte.MinValue;
                case TypeCode.SByte: return SByte.MinValue;
                case TypeCode.UInt16: return UInt16.MinValue;
                case TypeCode.UInt32: return UInt32.MinValue;
                case TypeCode.UInt64: return UInt64.MinValue;
                case TypeCode.Int16: return Int16.MinValue;
                case TypeCode.Int32: return Int32.MinValue;
                case TypeCode.Int64: return Int64.MinValue;
                case TypeCode.Decimal: return Decimal.MinValue;
                case TypeCode.Double: return Decimal.MinValue; // Double is bigger than Decimal
                case TypeCode.Single: return Decimal.MinValue; // Single is bigger than Decimal
                default: return 0M;
            }
        }

        /// <summary>
        /// Checks if a property is an indexer.
        /// </summary>
        /// <param name="property">The property to check.</param>
        /// <returns>True if the property is an indexer, otherwise false.</returns>
        public static bool IsIndexer(this PropertyInfo property) => property.GetIndexParameters().Length != 0;

        /// <summary>
        /// Checks if a type is a numeric type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is an integer or decimal type, otherwise false.</returns>
        public static bool IsNumeric(this Type type) => type.IsInteger() || type.IsDecimal();

        /// <summary>
        /// Checks if a type is a decimal type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is a decimal type, otherwise false.</returns>
        public static bool IsDecimal(this Type type)
        {
            if (type.IsNullable())
                return IsDecimal(Nullable.GetUnderlyingType(type));

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks if a type is an integer type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is an integer type, otherwise false.</returns>
        public static bool IsInteger(this Type type)
        {
            if (type.IsNullable())
                return IsInteger(Nullable.GetUnderlyingType(type));

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks if a type is a simple type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is a simple type, otherwise false.</returns>
        public static bool IsSimpleType(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsNullable())
                // nullable type, check if the nested type is simple.
                return IsSimpleType(typeInfo.GetGenericArguments()[0]);

            return typeInfo.IsValueType
              || typeInfo.IsPrimitive
              || typeInfo.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal))
              || type.Equals(typeof(DateTime))
              || type.Equals(typeof(DateTimeOffset))
              || type.Equals(typeof(TimeSpan))
              || type.Equals(typeof(Guid));
        }

        /// <summary>
        /// Checks if a type is a nullable type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>True if the type is a nullable type, otherwise false.</returns>
        public static bool IsNullable(this Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        /// <summary>
        /// Returns an object of the specified type and whose value is equivalent to the specified object.
        /// </summary>
        /// <param name="value">An object that implements the IConvertible interface.</param>
        /// <param name="conversionType">The type of object to return.</param>
        /// <returns>An object whose type is conversionType (or conversionType's underlying type if conversionType is Nullable&lt;&gt;) and whose value is equivalent to value.</returns>
        public static object ChangeType(this object value, Type conversionType)
        { // this wrapper method allows to do "value?.ChangeType(toType)" and get null if value is null, even when the toType is a value type.
            if (conversionType == null)
                throw new ArgumentNullException(nameof(conversionType));

            if (value == null)
            {
                if (conversionType.IsValueType)
                    return Activator.CreateInstance(conversionType); // return default value
                return null; // reference type
            }

            if (conversionType.IsNullable())
                conversionType = Nullable.GetUnderlyingType(conversionType);

            if (value.GetType().IsAssignableTo(conversionType))
                return value; // no need to convert

            // custom conversion from string
            if (value is string strValue)
            {
                switch (Type.GetTypeCode(conversionType))
                {
                    case TypeCode.Boolean: return bool.Parse(strValue);
                    case TypeCode.Byte: return byte.Parse(strValue);
                    case TypeCode.SByte: return sbyte.Parse(strValue);
                    case TypeCode.Int16: return short.Parse(strValue);
                    case TypeCode.UInt16: return ushort.Parse(strValue);
                    case TypeCode.Int32: return int.Parse(strValue);
                    case TypeCode.UInt32: return uint.Parse(strValue);
                    case TypeCode.Int64: return long.Parse(strValue);
                    case TypeCode.UInt64: return ulong.Parse(strValue);
                    case TypeCode.Single: return float.Parse(strValue);
                    case TypeCode.Double: return double.Parse(strValue);
                    case TypeCode.Decimal: return decimal.Parse(strValue);
                    case TypeCode.DateTime: return DateTime.Parse(strValue);
                    case TypeCode.String: return strValue;
                }
                if (conversionType.IsEnum)
                    return Enum.Parse(conversionType, strValue);
                if (conversionType.Equals(typeof(TimeSpan)))
                    return TimeSpan.Parse(strValue);
            }

            return Convert.ChangeType(value, conversionType);
        }
    }
}
