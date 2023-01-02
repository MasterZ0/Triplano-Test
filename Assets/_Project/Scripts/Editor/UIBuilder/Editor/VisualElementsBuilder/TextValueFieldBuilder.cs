using System;
using System.Reflection;
using UnityEditor.UIElements;
using TriplanoTest.Shared.ExtensionMethods;

namespace TriplanoTest.UIBuilder.Editor
{
    internal static class TextValueFieldBuilder
    {
        internal static IntegerField CreateIntegerFieldFromVisualElement(FieldInfo field, object target)
        {
            if (field.GetValue(target) is not IntegerField integerField)
            {
                integerField = Activator.CreateInstance(typeof(IntegerField)) as IntegerField;
                SetIntegerField(integerField, field.Name.GetNiceString());
            }

            // Bind
            field.SetValue(target, integerField);

            return integerField;
        }

        internal static void SetIntegerField(IntegerField integerField, string label, int value = 0)
        {
            integerField.label = label;
            integerField.value = value;
        }
    }
}