using UnityEngine.UIElements;

namespace TriplanoTest.UIBuilder.Editor
{
    public static class UIBuilderExtensions
    {
        public static T CreateFields<T>(this VisualElement root) where T : class
        {
            return EditorBuilder.CreateFields<T>(root);
        }

        public static T CreateFields<T>(this VisualElement root, T instance) where T : class
        {
            return EditorBuilder.CreateFields(root, instance);
        }

        public static void BindFields<T>(this VisualElement root, T instance) where T : class
        {
            EditorBuilder.BindFields(root, instance);
        }
    }
}