using System;
using UnityEngine.UIElements;
using System.Reflection;
using System.Linq;
using TriplanoTest.Shared.ExtensionMethods;

namespace TriplanoTest.UIBuilder.Editor
{
    public static class ButtonBuilder
    {
        internal static void GenerateButtons(object target, VisualElement root)
        {
            ReflectionUtils.GetMethodsWith<ButtonAttribute>(target)
                .Select(m => CreateButtonFromMethod(m, target))
                .ToList()
                .ForEach(b => root.Add(b));
        }

        /// <param name="field"> Field type of <seealso cref="UnityEngine.UIElements.Button"/> </param>
        internal static Button CreateButtonFromVisualElementField(FieldInfo field, object target)
        {
            if (field.GetValue(target) is not Button button)
            {
                button = Activator.CreateInstance(typeof(Button)) as Button;
                SetButton(button, field.Name.GetNiceString());
            }

            // Bind
            field.SetValue(target, button);

            return button;
        }

        internal static Button CreateButtonFromMethod(MethodInfo method, object target)
        {
            Button button = new Button();
            ButtonAttribute attribute = method.GetCustomAttribute(typeof(ButtonAttribute), false) as ButtonAttribute;

            string label = attribute.Name != null ? attribute.Name : method.Name.GetNiceString();
            SetButton(button, label);

            button.clicked += () => method.Invoke(target, null);
            return button;
        }

        internal static void SetButton(Button button, string text)
        {
            button.text = text;
        }
    }
}