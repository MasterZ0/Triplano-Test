using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using TriplanoTest.Shared;
using TriplanoTest.Data;
using TriplanoTest.AppEditor.Design;
using TriplanoTest.Shared.Design;

namespace TriplanoTest.AppEditor
{
    public class GameDesignEditorWindow : MenuEditorWindow
    {
        private const string GameDesign = "Game Design";

        [MenuItem(ProjectPath.ApplicationName + "/" + GameDesign)]
        public static void ShowWindow()
        {
            GetWindow<GameDesignEditorWindow>(GameDesign);
        }

        [OnOpenAsset]
        public static bool OpenEditor(int instanceId, int line)
        {
            GameData asset = EditorUtility.InstanceIDToObject(instanceId) as GameData;

            if (!asset)
                return false;

            ShowWindow();
            return true;
        }

        protected override void ToolBar()
        {
            GUILayout.FlexibleSpace();
            if (EditorComponents.DrawIconButton(IconType.Lamp, ButtonStyle.Toolbar, GUILayout.Width(30)))
            {
                EditorGUIUtility.PingObject(SelectedContent.Asset);
            }
        }

        protected override void BuildMenuTree(MenuTree tree)
        {
            GameData gameData = AssetDatabase.LoadAssetAtPath<GameData>(ProjectPath.GameDataAsset);

            tree.AddGameData("Game Data", gameData);

            tree.AddAllAssetsAtPath($"Game Data/Enemies", $"{ProjectPath.Settings}/Enemies", typeof(ScriptableObject), true, IconType.Eye);
            tree.AddAllAssetsAtPath($"Game Data/Coins", $"{ProjectPath.Settings}/Coins", typeof(ScriptableObject), true, IconType.Gamepad);
        }
    }
}