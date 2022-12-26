using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using Object = UnityEngine.Object;
using TriplanoTest.Shared.ExtensionMethods;

namespace TriplanoTest.AppEditor
{
    public class ContentUI
    {
        public string Title { get; }
        public bool Visible { get; set; } = true;
        public Object Asset { get; private set; }
        public List<ContentUI> Children { get; } = new List<ContentUI>();

        public ContentUI(string title) => Title = title;
        public void SetAsset(Object asset) => Asset = asset;
    }

    public class MenuTree
    {
        public List<ContentUI> Root { get; } = new();
        public void Add(string drawPath, Object objectToDraw) // Todo: Icon
        {
            string[] pathSegments = drawPath.Split('/');
            AddRecursive(Root, pathSegments, 0, objectToDraw);
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

        public void AddAllAssetsAtPath(string drawPath, string projectPath, Type searchType, bool checkSubFolders = false)
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
                        AddAllAssetsAtPath(drawPath + "/" + Path.GetFileName(childFolderPath), childFolderPath, searchType, true);
                    }
                }
            }
        }
    }
}