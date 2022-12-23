using TriplanoTest.Data;
using TriplanoTest.Shared;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace TriplanoTest.AppEditor 
{
    /// <summary>
    /// Initialize the GameValuesSO and Load the GameManagerScene
    /// </summary>
    [InitializeOnLoad]
    public static class Startup {

        static Startup() 
        {
            EditorSceneManager.sceneOpened += LoadGameManager;
            EditorApplication.delayCall += GameValuesValidation;
        }

        private static void GameValuesValidation() 
        {
            AssetDatabase.LoadAssetAtPath<GameData>(ProjectPath.GameDataAsset).Initialize();
        }

        private static void LoadGameManager(Scene scene, OpenSceneMode mode) 
        {
            if (mode == OpenSceneMode.Single && scene.buildIndex != 0) 
            {
                OpenSceneMode openSceneMode = scene.buildIndex == -1 ? OpenSceneMode.AdditiveWithoutLoading : OpenSceneMode.Additive;
                EditorSceneManager.OpenScene(ProjectPath.ApplicationManagerScene, openSceneMode);
            }
        }
    }
}