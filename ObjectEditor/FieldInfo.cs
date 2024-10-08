using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TechnosoCommons.Configuration;
using TechnosoCommons.Extensions;

namespace TechnosoUI.Configuration
{
    public abstract class BaseFieldInfo
    {
        public string Name { get; }
        public string Description { get; }
        public bool IsReadOnly { get; }
        public Type Type { get; }
        public bool IsNullable { get; }

        public string Tip => !string.IsNullOrEmpty(Description) ? $"{Name} - {Description}" : Name;

        public BaseFieldInfo(Type type, string name, string description, bool isReadOnly)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            Name = name;
            Description = description;
            IsReadOnly = isReadOnly;
            Type = type;
            IsNullable = !type.IsValueType; // reference types are nullable

            if (Type.IsNullable())
            {
                IsNullable = true; // Nullable<T> is a value type (struct), but it's nullable.
                // nullable can't be nested in another nullable type, the underlying type is always the base type
                Type = Nullable.GetUnderlyingType(Type);
            }
        }

        /// <summary>
        /// Gets the value of the field from the source object.
        /// </summary>
        /// <param name="sourceObj">The object to get the value from.</param>
        /// <returns>The value of the field from the source object.</returns>
        public abstract object GetValue(object sourceObj);

        public override string ToString() => Name;
    }

    /// <summary>
    /// Field information for a property.
    /// </summary>
    public class PropertyFieldInfo : BaseFieldInfo
    {
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Creates a field information from a property information.
        /// </summary>
        /// <param name="propertyInfo">The property information to create the field information from.</param>
        /// <exception cref="ArgumentNullException">if the property information is null.</exception>
        public PropertyFieldInfo(PropertyInfo propertyInfo)
            : base(propertyInfo?.PropertyType,
                  propertyInfo?.GetCustomAttribute<InfoAttribute>()?.Name ?? propertyInfo?.Name,
                  propertyInfo?.GetCustomAttribute<InfoAttribute>()?.Description,
                  !(propertyInfo?.CanWrite ?? false))
        {
            if (propertyInfo == null)
                throw new ArgumentNullException(nameof(propertyInfo));

            PropertyInfo = propertyInfo;
        }

        public override object GetValue(object sourceObj)
            => PropertyInfo.GetValue(sourceObj);
    }

    /// <summary>
    /// Field information for an item in an enumerable.
    /// </summary>
    public class ItemFieldInfo : BaseFieldInfo
    {
        /// <summary>
        /// The index of the item in the enumerable.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Operations that can be performed on the item.
        /// </summary>
        public ItemAbility Abilities { get; }

        /// <summary>
        /// True if the item can be removed from the enumerable.
        /// </summary>
        public bool IsRemovable => Abilities.HasFlag(ItemAbility.Remove);

        /// <summary>
        /// Creates a field information for an item in an enumerable.
        /// </summary>
        /// <param name="type">The type of the item.</param>
        /// <param name="name">The name of the item.</param>
        /// <param name="description">The description for the item.</param>
        /// <param name="index">The index of the item in the enumerable.</param>
        /// <param name="abilities">The abilities that can be performed on the item.</param>
        public ItemFieldInfo(Type type, string name, string description, int index, ItemAbility abilities)
            : base(type, name, description, !abilities.HasFlag(ItemAbility.Edit)) // if it can't be edited, it's read-only
        {
            Index = index;
            Abilities = abilities;
        }

        public override object GetValue(object sourceObj)
        {
            if (!(sourceObj is IEnumerable e))
                throw new InvalidOperationException("The source object is not an enumerable.");
            
            var enumerator = e.GetEnumerator();
            for (int i = 0; i <= Index; i++)
                if (!enumerator.MoveNext())
                    throw new IndexOutOfRangeException("The index is out of range.");
            return enumerator.Current;
        }

        public override string ToString() => $"[{Index}]";
    }
}
