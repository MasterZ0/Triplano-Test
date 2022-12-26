using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using Object = UnityEngine.Object;
using TriplanoTest.Shared.ExtensionMethods;
using TriplanoTest.AppEditor.Design;

namespace TriplanoTest.AppEditor
{
    public class ContentUI
    {
        public string Title { get; }
        public bool Visible { get; set; } = true;
        public Object Asset { get; private set; }
        public IconType ButtonIcon { get; private set; }
        public List<ContentUI> Children { get; } = new List<ContentUI>();

        public ContentUI(string title) => Title = title;
        internal void SetAsset(Object asset, IconType icon)
        {
            Asset = asset;
            ButtonIcon = icon;
        }

        internal void SetIcon(IconType icon)
        {
            ButtonIcon = icon;
        }
    }

    public class MenuTree
    {
        public List<ContentUI> Root { get; } = new();

        public void Add(string drawPath, Object objectToDraw, IconType iconType = IconType.None) // Todo: Icon
        {
            string[] pathSegments = drawPath.Split('/');
            AddRecursive(Root, pathSegments, 0, objectToDraw, iconType);
        }

        public void AddGameData(string drawPath, ScriptableObject mainData, IconType iconType = IconType.None)
        {
            Add(drawPath, mainData, iconType);

            FieldInfo[] fields = mainData.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (FieldInfo field in fields)
            {
                if (field.GetValue(mainData) is ScriptableObject asset)
                {
                    string segment = asset.name.GetNiceString();
                    Add(drawPath + "/" + segment, asset);
                }
            }
        }

        public void AddAllAssetsAtPath(string drawPath, string projectPath, Type searchType, IconType iconType = IconType.None, bool checkSubFolders = false)
        {
            // Use Unity's AssetDatabase class to search for all assets of the specified type in the specified project path
            string[] guids = AssetDatabase.FindAssets("t:" + searchType.Name, new[] { projectPath });
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Object asset = AssetDatabase.LoadAssetAtPath(assetPath, searchType);

                // Add the asset to the menu tree, using the specified drawing path as the tree path
                string segment = asset.name.GetNiceString();
                Add(drawPath + "/" + segment, asset, IconType.None);
            }

            if (checkSubFolders)
            {
                // Get the child folders of the specified project path
                string[] childFolderGuids = AssetDatabase.FindAssets("", new[] { projectPath });
                foreach (string childFolderGuid in childFolderGuids)
                {
                    string childFolderPath = AssetDatabase.GUIDToAssetPath(childFolderGuid);
                    if (AssetDatabase.IsValidFolder(childFolderPath))
                    {
                        // Recursively add all assets in child folders
                        AddAllAssetsAtPath(drawPath + "/" + Path.GetFileName(childFolderPath), childFolderPath, searchType, IconType.None, true);
                    }
                }
            }

            FindContentAtPath(drawPath).SetIcon(iconType);
        }

        private void AddRecursive(List<ContentUI> nodes, string[] pathSegments, int segmentIndex, Object objectToDraw, IconType iconType)
        {
            if (segmentIndex >= pathSegments.Length)
                return;

            string segment = pathSegments[segmentIndex];
            ContentUI node = nodes.FirstOrDefault(n => n.Title == segment);
            if (node == null)
            {
                node = new ContentUI(segment);
                nodes.Add(node);
            }

            if (segmentIndex == pathSegments.Length - 1)
            {
                node.SetAsset(objectToDraw, iconType);
            }
            else
            {
                AddRecursive(node.Children, pathSegments, segmentIndex + 1, objectToDraw, iconType);
            }
        }

        private ContentUI FindContentAtPath(string path, List<ContentUI> nodes = null)
        {
            nodes ??= Root;

            string[] pathSegments = path.Split('/');
            if (pathSegments.Length == 0)
            {
                return null;
            }

            ContentUI node = nodes.FirstOrDefault(n => n.Title == pathSegments[0]);
            if (node == null)
            {
                return null;
            }

            if (pathSegments.Length == 1)
            {
                return node;
            }
            else
            {
                return FindContentAtPath(string.Join("/", pathSegments, 1, pathSegments.Length - 1), node.Children);
            }
        }
    }
}