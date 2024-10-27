using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ObjectEditor.UI.Controls;
using ObjectEditor.UI.Forms;
using ObjectEditor.Extensions;
using ObjectEditor.Data;

namespace ObjectEditor.UI
{
    public static class ObjectEditorFactory
    {
        /// <summary>
        /// Creates a form for a given object.
        /// </summary>
        /// <param name="sourceObject">The object to create a form for.</param>
        /// <returns>The created form.</returns>
        public static ObjectEditorForm CreateForm(object sourceObject) => CreateForm(sourceObject, null);
        internal static ObjectEditorForm CreateForm(object sourceObject, ObjectEditorForm parentForm)
        {
            switch (sourceObject)
            { // switch on type of sourceObject
                case IEnumerable sourceEnumerable:
                    var wrapper = sourceEnumerable.AsDynamic();
                    switch (wrapper)
                    { // switch on type of wrapper
                        case DynamicDictionaryWrapper dictionaryWrapper:
                            return new DictionaryEditorForm(sourceEnumerable, dictionaryWrapper, parentForm);
                        case DynamicCollectionWrapper collectionWrapper:
                            return new CollectionEditorForm(sourceEnumerable, collectionWrapper, parentForm);
                        default: // default form for any other enumerable
                            return new EnumerableEditorForm(sourceEnumerable, wrapper, parentForm);
                    }
                default: // default form for any other type
                    return new ObjectEditorForm(sourceObject, parentForm);
            }
        }

        /// <summary>
        /// Creates a field control for a given field.
        /// </summary>
        /// <param name="value">The initial value of the field.</param>
        /// <param name="propertyInfo">Property information of the field.</param>
        /// <param name="parentForm">The containing form of the field.</param>
        /// <returns></returns>
        internal static BaseFieldControl CreateFieldControl(this BaseFieldInfo fieldInfo, object value, ObjectEditorForm parentForm)
        {
            if (fieldInfo == null) return null;

            if (fieldInfo.Type.IsEnum) // enum field
                return new EnumFieldControl(value, fieldInfo, parentForm);
            else if (fieldInfo.Type.IsNumeric()) // numeric field
                return new NumericFieldControl(value, fieldInfo, parentForm);
            else if (fieldInfo.Type == typeof(bool)) // boolean field
                return new BooleanFieldControl(value, fieldInfo, parentForm);
            else if (fieldInfo.Type.IsSimpleType()) // default field for any other simple type
                return new TextFieldControl(value, fieldInfo, parentForm);
            else // it's a class type (reference)
                return new ObjectFieldControl(value, fieldInfo, parentForm);
        }
    }
}
