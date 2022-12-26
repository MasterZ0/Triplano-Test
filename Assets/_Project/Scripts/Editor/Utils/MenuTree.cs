using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using Object = UnityEngine.Object;
using TriplanoTest.Shared.ExtensionMethods;
using TriplanoTest.Shared.Design;
using TriplanoTest.AppEditor.Design;

namespace TriplanoTest.AppEditor
{
    public class ContentUI
    {
        public string Title { get; }
        public bool Visible { get; set; } = true;
        public Object Asset { get; private set; }
        public Texture ButtonIcon { get; private set; }
        public List<ContentUI> Children { get; } = new List<ContentUI>();

        public ContentUI(string title) => Title = title;

        internal void SetAsset(Object asset)
        {
            Asset = asset;

            if (asset is IHasCustomIcon customIconInterface)
            {
                ButtonIcon = customIconInterface.Icon;
            }
            if (asset is IHasIcon iconInterface)
            {
                ButtonIcon = EditorIcons.GetTexture(iconInterface.IconType);
            }
        }

        internal void SetIcon(Texture icon)
        {
            ButtonIcon = icon;
        }
    }

    public class MenuTree
    {
        public List<ContentUI> Root { get; } = new();

        public void Add(string drawPath, Object objectToDraw) // Todo: Icon
        {
            string[] pathSegments = drawPath.Split('/');
            AddRecursive(Root, pathSegments, 0, objectToDraw);
        }

        public void AddGameData(string drawPath, ScriptableObject mainData)
        {
            Add(drawPath, mainData);

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

        public void AddAllAssetsAtPath(string drawPath, string projectPath, Type searchType, bool checkSubFolders = false, IconType iconType = IconType.None) 
        { 
            Texture texture = EditorIcons.GetTexture(iconType);
            AddAllAssetsAtPath(drawPath, projectPath, searchType, texture, checkSubFolders);
        }

        public void AddAllAssetsAtPath(string drawPath, string projectPath, Type searchType, Texture iconTexture = null, bool checkSubFolders = false)
        {
            // Use Unity's AssetDatabase class to search for all assets of the specified type in the specified project path
            string[] guids = AssetDatabase.FindAssets("t:" + searchType.Name, new[] { projectPath });
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Object asset = AssetDatabase.LoadAssetAtPath(assetPath, searchType);

                // Add the asset to the menu tree, using the specified drawing path as the tree path
                string segment = asset.name.GetNiceString();
                Add(drawPath + "/" + segment, asset);
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
                        AddAllAssetsAtPath(drawPath + "/" + Path.GetFileName(childFolderPath), childFolderPath, searchType, true, IconType.None);
                    }
                }
            }

            FindContentAtPath(drawPath).SetIcon(iconTexture);
        }

        private void AddRecursive(List<ContentUI> nodes, string[] pathSegments, int segmentIndex, Object objectToDraw)
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
                node.SetAsset(objectToDraw);
            }
            else
            {
                AddRecursive(node.Children, pathSegments, segmentIndex + 1, objectToDraw);
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