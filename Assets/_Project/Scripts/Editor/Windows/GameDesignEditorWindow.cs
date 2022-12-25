using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using TriplanoTest.Shared;
using TriplanoTest.Data;
using System.Reflection;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using System.Xml.Linq;

namespace TriplanoTest.AppEditor
{
    public class GameDesignEditorWindow : EditorWindow
    {
        private List<ScriptableObject> dataList;

        private GameData gameData;
        private Object selection;
        private Editor selectionEditor;

        private Rect windowMenu;

        private Vector2 sideScrolll;
        private Vector2 contentScrool;

        private bool draggingLeft;
        private bool draggingRight;

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

        private void OnEnable()
        {
            gameData = AssetDatabase.LoadAssetAtPath<GameData>(ProjectPath.GameDataAsset);
            windowMenu.width = 330;

            selection = gameData;
            selectionEditor = Editor.CreateEditor(selection);

            dataList = new List<ScriptableObject>();
            FieldInfo[] fields = gameData.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (FieldInfo field in fields)
            {
                if (field.GetValue(gameData) is ScriptableObject scriptableObject)
                {
                    dataList.Add(scriptableObject);
                }
            }
        }

        private void DrawSideBarContent()
        {
            DrawBtn(gameData);

            foreach (ScriptableObject scriptableObject in dataList)
            {
                DrawBtn(scriptableObject);
            }
        }

        private void DrawBtn(ScriptableObject asset)
        {
            string name = ObjectNames.NicifyVariableName(asset.name);
            GUI.enabled = asset != selection;
            if (EditorComponents.DrawButton(name, ButtonStyle.FlatButtonStyle, TextAnchor.MiddleLeft))
            {
                selection = asset;
                selectionEditor = Editor.CreateEditor(asset);
            }
            GUI.enabled = true;
        }

        private void DrawMainContent()
        {
            GUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            {
                GUILayout.BeginHorizontal(EditorStyles.toolbar);
                {
                    GUILayout.FlexibleSpace();
                    if (EditorComponents.DrawIconButton(IconType.Lamp, ButtonStyle.Toolbar, GUILayout.Width(30)))
                    {
                        EditorGUIUtility.PingObject(selection);
                    }
                }
                GUILayout.EndHorizontal();

                EditorComponents.DrawContentWithScroll(ref contentScrool, () => selectionEditor.DrawDefaultInspector());
            }
            GUILayout.EndVertical();
        }

        #region Editor Window Areas
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            windowMenu = new Rect(0f, 0f, windowMenu.width, position.height);

            BeginResizableArea();
            SideBar();
            EndResizableArea();
            
            DrawContent();
            EditorGUILayout.EndHorizontal();
        }

        private void SideBar()
        {
            // Side bar Start
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.MaxWidth(windowMenu.width), GUILayout.ExpandWidth(true));
            {
                // Hierarchy Start
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
                {
                    sideScrolll = EditorGUILayout.BeginScrollView(sideScrolll, GUIStyle.none, GUI.skin.verticalScrollbar);
                    DrawSideBarContent();

                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawContent()
        {
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.MaxWidth   (position.width - windowMenu.width));
            if (selection != null)
            {
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    DrawMainContent();
                }
                EditorGUILayout.EndHorizontal();

            }
            Repaint();
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region Resizable
        private void BeginResizableArea()
        {
            BeginWindows();
            windowMenu = HorizResizer(windowMenu); //right
            windowMenu = HorizResizer(windowMenu, false); //left
        }

        private void EndResizableArea()
        {
            EndWindows();
        }

        /// <summary>
        /// https://forum.unity.com/threads/is-there-a-resize-equivalent-to-gui-dragwindow.10144/
        /// </summary>
        private Rect HorizResizer(Rect window, bool right = true, float detectionRange = 8f)
        {
            detectionRange *= 0.5f;
            Rect resizer = window;

            if (right)
            {
                resizer.xMin = resizer.xMax - detectionRange;
                resizer.xMax += detectionRange;
            }
            else
            {
                resizer.xMax = resizer.xMin + detectionRange;
                resizer.xMin -= detectionRange;
            }

            Event current = Event.current;
            EditorGUIUtility.AddCursorRect(resizer, MouseCursor.ResizeHorizontal);

            // if mouse is no longer dragging, stop tracking direction of drag
            if (current.type == EventType.MouseUp)
            {
                draggingLeft = false;
                draggingRight = false;
            }

            // resize window if mouse is being dragged within resizor bounds
            if (current.mousePosition.x > resizer.xMin &&
                current.mousePosition.x < resizer.xMax &&
                current.type == EventType.MouseDrag &&
                current.button == 0 ||
                draggingLeft ||
                draggingRight)
            {
                if (right == !draggingLeft)
                {
                    window.width = current.mousePosition.x + current.delta.x;
                    Repaint();
                    draggingRight = true;
                }
                else if (!right == !draggingRight)
                {
                    window.width = window.width - (current.mousePosition.x + current.delta.x);
                    Repaint();
                    draggingLeft = true;
                }

            }

            return window;
        }
        #endregion
    }

}