using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TriplanoTest.UIBuilder.Editor
{
    using Editor = UnityEditor.Editor;

    /// <summary>
    /// BUG: https://issuetracker.unity3d.com/issues/propertydrawer-dot-createpropertygui-will-not-get-called-when-using-a-custompropertydrawer-with-a-generic-struct
    /// </summary>
    [CustomEditor(typeof(Object), true)]
    public class ObjectEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            if (target.GetType().GetCustomAttribute<DisplayUIElementsAttribute>() == null)
                return null;

            return EditorBuilder.CreateInspector(this);
        }
    }
}