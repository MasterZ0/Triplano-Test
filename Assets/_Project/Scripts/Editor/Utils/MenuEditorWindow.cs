using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using TriplanoTest.AppEditor.Design;

namespace TriplanoTest.AppEditor
{
    public abstract class MenuEditorWindow : EditorWindow, IHasCustomMenu
    {
        protected ContentUI SelectedContent { get; private set; }
        private Editor selectionEditor;

        private Rect windowMenu;

        private Vector2 sideScrolll;
        private Vector2 contentScrool;

        private bool draggingLeft;
        private bool draggingRight;
        protected MenuTree Tree { get; private set; }

        private void OnEnable()
        {
            windowMenu.width = 330;
            ForceRebuildTree();
        }

        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Force Rebuild Tree"), false, ForceRebuildTree);
        }

        protected void ForceRebuildTree()
        {
            Tree = new MenuTree();
            BuildMenuTree(Tree);

            if (Tree.Root.Count > 0)
            {
                SelectActiveContent(Tree.Root[0]);
            }
        }

        protected virtual void BuildMenuTree(MenuTree tree) { }

        protected virtual void ToolBar() { }

        #region GUI + Editor Window Areas
        private void OnGUI()
        {
            // Left side = Menu tree. Right Side = ToolBar And content
            EditorGUILayout.BeginHorizontal();
            {
                windowMenu = new Rect(0f, 0f, windowMenu.width, position.height);

                BeginResizableArea(ref windowMenu);
                {
                    DrawMenuTree();
                    GUI.enabled = true;
                }
                EndResizableArea();

                DrawToolBarAndContent();
            }
            EditorGUILayout.EndHorizontal();
            Repaint();
        }

        /// <summary> Left Side </summary>
        private void DrawMenuTree()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.MaxWidth(windowMenu.width), GUILayout.ExpandWidth(true));
            {
                sideScrolll = EditorGUILayout.BeginScrollView(sideScrolll, GUIStyle.none, GUI.skin.verticalScrollbar);

                DrawRecursive(Tree.Root);

                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary> Right Side </summary>
        private void DrawToolBarAndContent()
        {
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.MaxWidth(position.width - windowMenu.width));
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                {
                    ToolBar();
                }
                EditorGUILayout.EndHorizontal();

                if (SelectedContent.Asset != null)
                {
                    EditorComponents.DrawContentWithScroll(ref contentScrool, () => selectionEditor.DrawDefaultInspector());
                }
            }
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region Menu Tree
        private void DrawRecursive(List<ContentUI> content)
        {
            // Vertical section to keep left paddings
            EditorGUILayout.BeginVertical();

            for (int i = 0; i < content.Count; i++)
            {
                // Draw Foldout and Content
                EditorGUILayout.BeginHorizontal(); 
                DrawHierarchyBtn(content[i]);
                EditorGUILayout.EndHorizontal();

                // Vertical Space between buttons
                GUILayout.Space(1);

                // Add left padding and draw Children Content 
                if (content[i].Children.Count > 0 && content[i].Visible)
                {
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Space(32);
                    DrawRecursive(content[i].Children);

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawHierarchyBtn(ContentUI content)
        {
            Rect rect = EditorGUILayout.BeginHorizontal(GUILayout.Height(32), GUILayout.Width(22));

            GUILayout.Space(0);
            if (content.Children.Count > 0)  
            {
                rect.x += 4;
                rect.y += 4;

                content.Visible = EditorGUI.Foldout(rect, content.Visible, string.Empty);
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            // Check if is the current select object
            GUI.enabled = SelectedContent != content;
            if (EditorComponents.ButtonWithIconAndLabel(content.Title, content.ButtonIcon))
            {
                SelectActiveContent(content);
                GUI.FocusControl(null);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void SelectActiveContent(ContentUI content)
        {
            SelectedContent = content;
            selectionEditor = Editor.CreateEditor(content.Asset);
        }
        #endregion

        #region Resizable
        internal void BeginResizableArea(ref Rect windowMenu)
        {
            BeginWindows();
            windowMenu = HorizResizer(windowMenu); //right
            windowMenu = HorizResizer(windowMenu, false); //left
        }

        private void EndResizableArea() => EndWindows();

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