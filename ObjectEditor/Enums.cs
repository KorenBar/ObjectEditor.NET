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
    /// <summary>
    /// Abilities the collection has on its items.
    /// </summary>
    public enum ItemAbility
    {
        /// <summary>
        /// The item is read-only.
        /// </summary>
        ReadOnly = 0,
        /// <summary>
        /// The item can be set to a new value.
        /// </summary>
        Edit = 1 << 0,
        /// <summary>
        /// The collection can have items added to it.
        /// </summary>
        Add = 1 << 1,
        /// <summary>
        /// The collection can have items removed from it.
        /// The item can be removed from the collection.
        /// </summary>
        Remove = 1 << 2,
    }
}
