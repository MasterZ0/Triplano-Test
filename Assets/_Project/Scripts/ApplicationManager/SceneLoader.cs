using TriplanoTest.ObjectPooling;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TriplanoTest.ApplicationManager
{
    public enum GameScene
    {
        ApplicationManager,
        Intro,
        MainMenu,
        Gameplay
    }

    /// <summary>
    /// Load and unload scenes
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        private GameScene currentScene;
        private bool loading; // Safe variable

        /// <summary>
        /// Check if there is any scene loaded, if is not, load the MainMenu
        /// </summary>
        public void LoadApplication(Action onFinish)
        {
            Scene activeScene = SceneManager.GetActiveScene();

            if (activeScene == SceneManager.GetSceneByBuildIndex(0) && SceneManager.sceneCount == 1)
            {
                currentScene = GameScene.Intro;
                loading = true;
                StartCoroutine(LoadCurrentScene(onFinish));
                return;
            }

            currentScene = (GameScene)activeScene.buildIndex;
            onFinish();
        }

        public void ReloadScene(Action onFinish)
        {
            if (loading)
            {
                throw new ApplicationException("There is already a scene loading process in progress");
            }

            loading = true;
            StartCoroutine(ReloadCurrentScene(onFinish));
        }

        private IEnumerator ReloadCurrentScene(Action onFinish)
        {
            ObjectPool.ReturnAllToPool();

            string currentScene = SceneManager.GetActiveScene().name; // After unload the struct changes, use string

            // Unload current
            AsyncOperation operation = SceneManager.UnloadSceneAsync(currentScene, UnloadSceneOptions.None);
            yield return new WaitUntil(() => operation.isDone);

            operation = SceneManager.LoadSceneAsync(currentScene, LoadSceneMode.Additive);
            yield return new WaitUntil(() => operation.isDone);

            GC.Collect();

            Scene activeScene = SceneManager.GetSceneByName(currentScene);
            SceneManager.SetActiveScene(activeScene);

            onFinish();
            loading = false;
        }


        public void LoadScene(GameScene gameScene, Action onFinish)
        {
            if (loading)
            {
                throw new ApplicationException("There is already a scene loading process in progress");
            }

            loading = true;
            StartCoroutine(LoadNextScene(gameScene, onFinish));
        }

        #region Private Methods
        private IEnumerator LoadNextScene(GameScene gameScene, Action onFinish)
        {
            ObjectPool.ReturnAllToPool();

            // Unload current
            string activeScene = SceneManager.GetActiveScene().name; // Can be a test scene
            AsyncOperation loadSceneAsync = SceneManager.UnloadSceneAsync(activeScene);
            yield return new WaitUntil(() => loadSceneAsync.isDone);

            currentScene = gameScene;
            GC.Collect();

            // Load next
            StartCoroutine(LoadCurrentScene(onFinish));
        }

        private IEnumerator LoadCurrentScene(Action onFinish)
        {
            AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(currentScene.ToString(), LoadSceneMode.Additive);
            yield return new WaitUntil(() => loadSceneAsync.isDone);

            Scene loadedScene = SceneManager.GetSceneByName(currentScene.ToString());
            SceneManager.SetActiveScene(loadedScene);

            yield return new WaitForEndOfFrame();

            onFinish();
            loading = false;
        }
        #endregion
    }
}
