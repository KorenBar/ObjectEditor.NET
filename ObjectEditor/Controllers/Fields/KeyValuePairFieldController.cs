using ObjectEditor.Controllers.Editors;
using ObjectEditor.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectEditor.Controllers.Fields
{
    /// <summary>
    /// A controller for a field in an object editor.
    /// </summary>
    public class KeyValuePairFieldController : ValueFieldController
    {
        // Sub-field controllers for the key and value of the pair
        public ValueFieldController KeyFieldController { get; }
        public ValueFieldController ValueFieldController { get; }

        /// <summary>
        /// Get the pair currently represented by the controller.
        /// </summary>
        public KeyValuePair<object, object>? KeyValuePair => Value?.CastKeyValuePair<object, object>();

        /// <summary>
        /// Get the initial pair of the source object.
        /// </summary>
        public KeyValuePair<object, object>? SourceKeyValuePair { get; private set; }


        /// <summary>
        /// Creates a controller for a KeyValuePair field.
        /// </summary>
        /// <param name="value">The initial value of the field.</param>
        /// <param name="fieldInfo">The information of the field.</param>
        /// <param name="parentController">The controller that contains this field.</param>
        internal KeyValuePairFieldController(object value, FieldMetadata fieldInfo, ObjectEditorController parentController)
            : base(value, fieldInfo, parentController)
        {
            var genericType = fieldInfo.Type.GetGenericType(typeof(KeyValuePair<,>));
            if (genericType == null)
                throw new ArgumentException("The field type is not a KeyValuePair.");

            SourceKeyValuePair = value?.CastKeyValuePair<object, object>();

            // Initialize the key and value types
            var genericArguments = genericType.GetGenericArguments();
            Type keyType = genericArguments[0];
            Type valueType = genericArguments[1];

            // Create the sub-field controllers for the key and value
            KeyFieldController = new SubFieldMetadata(keyType, "Key", fieldInfo).CreateFieldController(KeyValuePair?.Key, parentController);
            ValueFieldController = new SubFieldMetadata(valueType, "Value", fieldInfo).CreateFieldController(KeyValuePair?.Value, parentController);

            // Add events to update the KeyValuePair when the sub-fields change
            KeyFieldController.ValueChanged += (s, e) =>
            {
                if (KeyValuePair.HasValue)
                {
                    var kvp = KeyValuePair.Value;
                    kvp = new KeyValuePair<object, object>(KeyFieldController.Value, kvp.Value);
                    SetValue(kvp, e.ByUser);
                }
            };
            ValueFieldController.ValueChanged += (s, e) =>
            {
                if (KeyValuePair.HasValue)
                {
                    var kvp = KeyValuePair.Value;
                    kvp = new KeyValuePair<object, object>(kvp.Key, ValueFieldController.Value);
                    SetValue(kvp, e.ByUser);
                }
            };
        }

        public override void Apply()
        {
            // The KeyValuePair was applied to the source object.
            SourceKeyValuePair = KeyValuePair;
            base.Apply();
        }

        protected override void OnValueChanged(FieldValueChangedEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            if (KeyFieldController == null || ValueFieldController == null)
                return; // not initialized yet (called from the base constructor)

            // TODO: consider that condition, is it necessary?
            //if (e.NewValue != null)
            { // Update the sub-field controllers when the value changes
                var kvp = KeyValuePair; // will throw if the value is not a KeyValuePair
                KeyFieldController.Value = kvp?.Key;
                ValueFieldController.Value = kvp?.Value;
            } // if the new value is null, the sub-fields will stay with their previous non-null values

            base.OnValueChanged(e);
        }
    }
}
