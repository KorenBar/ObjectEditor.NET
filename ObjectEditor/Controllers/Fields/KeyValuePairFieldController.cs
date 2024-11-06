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
        private ValueFieldController _keyFieldController;
        private ValueFieldController _valueFieldController;

        private Type _keyType;
        private Type _valueType;

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
            _keyType = genericArguments[0];
            _valueType = genericArguments[1];

            // Create the sub-field controllers for the key and value
            _keyFieldController = new SubFieldMetadata(_keyType, "Key", fieldInfo).CreateFieldController(KeyValuePair?.Key, parentController);
            _valueFieldController = new SubFieldMetadata(_valueType, "Value", fieldInfo).CreateFieldController(KeyValuePair?.Value, parentController);

            // Add events to update the KeyValuePair when the sub-fields change
            _keyFieldController.ValueChanged += (s, e) =>
            {
                if (KeyValuePair.HasValue)
                {
                    var kvp = KeyValuePair.Value;
                    kvp = new KeyValuePair<object, object>(_keyFieldController.Value, kvp.Value);
                    SetValue(kvp, e.ByUser);
                }
            };
            _valueFieldController.ValueChanged += (s, e) =>
            {
                if (KeyValuePair.HasValue)
                {
                    var kvp = KeyValuePair.Value;
                    kvp = new KeyValuePair<object, object>(kvp.Key, _valueFieldController.Value);
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

            if (e.NewValue != null)
            { // Update the sub-field controllers when the value changes
                var kvp = e.NewValue.CastKeyValuePair<object, object>(); // will throw if not a KeyValuePair
                _keyFieldController.Value = kvp.Key;
                _valueFieldController.Value = kvp.Value;
            } // if the new value is null, the sub-fields will stay with their previous non-null values

            base.OnValueChanged(e);
        }
    }
}
