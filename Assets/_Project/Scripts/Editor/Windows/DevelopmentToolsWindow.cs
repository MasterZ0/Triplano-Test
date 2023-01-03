using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using TriplanoTest.Shared;
using TriplanoTest.UIBuilder.Editor;

namespace TriplanoTest.AppEditor
{
    public class DevelopmentToolsWindow : EditorWindow
    {
        // This is located in the cs file inside the project
        [SerializeField] private VisualTreeAsset visualTree;

        private const string DevelopmentTools = "Development Tools";

        [MenuItem(ProjectPath.ApplicationName + "/" + DevelopmentTools)]
        public static void OpenWindow()
        {
            GetWindow<DevelopmentToolsWindow>(DevelopmentTools).Show();
        }

        public void CreateGUI()
        {
            visualTree.CloneTree(rootVisualElement);
            rootVisualElement.CreateFields<LevelDesignTools>();
        }
    }
}