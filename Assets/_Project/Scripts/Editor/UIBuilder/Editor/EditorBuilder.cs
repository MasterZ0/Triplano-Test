using UnityEngine;
using System;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Data;
using UnityEditor.UIElements;

namespace TriplanoTest.UIBuilder.Editor
{
    /// <summary>
    /// This is a helper class for creating and binding VisualElements in Unity's UI system. It contains several methods for creating fields of various types, including buttons, sliders, and text input fields. It also has a method for binding fields to VisualElements in a root element, and a method for generating VisualElements for all fields in an object.
    /// </summary>
    public static class EditorBuilder
    {
        /// <summary>
        /// Creates fields for the specified object using the provided root element.
        /// </summary>
        public static T CreateFields<T>(VisualElement root) where T : class
        {
            T instance = Activator.CreateInstance<T>();

            return CreateFields(root, instance);
        }

        /// <summary>
        /// Creates fields for the specified object using the provided root element and target object.
        /// </summary>
        public static T CreateFields<T>(VisualElement root, T target) where T : class
        {
            // Find declared fields
            GenerateVisualElements(root, target);

            // Add buttons
            ButtonBuilder.GenerateButtons(target, root);

            return target;
        }

        /// <summary>
        /// Binds the fields of the specified target object to VisualElements in the provided root element.
        /// </summary>
        public static void BindFields<T>(VisualElement root, T target) where T : class
        {
            IEnumerable<FieldInfo> fields = GetAllFields(target).Where(f => typeof(VisualElement).IsAssignableFrom(f.FieldType));

            foreach (FieldInfo field in fields)
            {
                VisualElement element = root.Q(field.Name);
                if (element != null && element.GetType() == field.FieldType)
                {
                    field.SetValue(target, element);
                }
            }
        }

        /// <summary>
        /// Generates VisualElements for all fields in the specified target object, and adds them to the provided root element.
        /// </summary>
        /// <remarks>
        /// Ex: int, float, string, bool, enum, objects, UnityEngine.Object, LayerMask, buttons, etc...
        /// </remarks>
        private static void GenerateVisualElements(VisualElement root, object target)
        {
            foreach (FieldInfo field in GetAllFields(target))
            {
                VisualElement newElement;

                // Instantiate field
                newElement = field.FieldType switch
                {
                    Type t when t == typeof(IntegerField) => TextValueFieldBuilder.CreateIntegerFieldFromVisualElement(field, target),
                    Type t when t == typeof(SliderInt) => SliderIntBuilder.CreateSliderIntFromVisualElement(field, target),
                    Type t when t == typeof(Button) => ButtonBuilder.CreateButtonFromVisualElementField(field, target),
                    _ => DefaultCase()
                };

                VisualElement DefaultCase()
                {
                    Debug.Log($"NotImplementedException\nField Name: {field.Name}, Type: {field.FieldType}");

                    if (field.FieldType == typeof(VisualElement))
                    {
                        return Activator.CreateInstance(field.FieldType) as VisualElement;
                    }
                    return null;
                }

                // Add root new instance
                root.Add(newElement);
            }
        }

        private static IEnumerable<FieldInfo> GetAllFields(object target)
        {
            return target.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        internal static VisualElement CreateInspector(UnityEditor.Editor editor)
        {
            VisualElement container = new();
            InspectorElement.FillDefaultInspector(container, editor.serializedObject, editor);
            ButtonBuilder.GenerateButtons(editor.target, container);
            return container;
        }
    }
}