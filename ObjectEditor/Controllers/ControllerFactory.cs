using ObjectEditor.Controllers.Editors;
using ObjectEditor.Controllers.Fields;
using ObjectEditor.Data;
using ObjectEditor.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.Controllers
{
    public static class ControllerFactory
    {
        /// <summary>
        /// Creates an editor controller for a given source object.
        /// </summary>
        /// <param name="sourceObject">The object to create an editor controller for.</param>
        /// <returns>The created controller.</returns>
        public static ObjectEditorController CreateEditor(object sourceObject)
        {
            switch (sourceObject)
            { // switch on type of sourceObject
                case IEnumerable sourceEnumerable:
                    var wrapper = sourceEnumerable.AsDynamic();
                    switch (wrapper)
                    { // switch on type of wrapper
                        case DynamicDictionaryWrapper dictionaryWrapper:
                            return new DictionaryEditorController(sourceEnumerable, dictionaryWrapper);
                        case DynamicCollectionWrapper collectionWrapper:
                            return new CollectionEditorController(sourceEnumerable, collectionWrapper);
                        default: // default for any other enumerable
                            return new EnumerableEditorController(sourceEnumerable, wrapper);
                    }
                default: // default for any other type
                    return new ObjectEditorController(sourceObject);
            }
        }

        /// <summary>
        /// Creates a field controller for a given field info.
        /// </summary>
        /// <param name="value">The initial value of the field.</param>
        /// <param name="fieldInfo">Information of the field.</param>
        /// <param name="parentController">The containing controller of the field.</param>
        /// <returns></returns>
        internal static ValueFieldController CreateFieldController(this FieldMetadata fieldInfo, object value, ObjectEditorController parentController)
        {
            if (fieldInfo == null) return null;

            else if (fieldInfo.Type.GetGenericType(typeof(KeyValuePair<,>)) != null)
                return new KeyValuePairFieldController(value, fieldInfo, parentController);
            else if (fieldInfo.Type.IsSimpleType()) // default field for any other simple type
                return new ValueFieldController(value, fieldInfo, parentController);
            else // it's a class type (reference)
                return new ObjectFieldController(value, fieldInfo, parentController);
        }
    }
}
