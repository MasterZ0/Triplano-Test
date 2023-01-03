using TriplanoTest.ObjectPooling;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using TriplanoTest.Shared;
using TriplanoTest.UIBuilder;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using TriplanoTest.Shared.ExtensionMethods;
using System.Runtime.CompilerServices;

namespace TriplanoTest.ApplicationManager
{
    public enum GameScene
    {
        Gameplay,
        Victory
    }

    public interface ISubSceneController
    {
        void Unload();
    }

    /// <summary>
    /// Load and unload scenes
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        [Header("Scene Loader")]
        [AssetReferenceUILabelRestriction(Constants.Scene)]
        [SerializeField] private AssetReference gameplay;
        [SerializeField] private AssetReference victory;

        [Header("Fade")]
        [SerializeField] private Animator transitionAnimator;
        [SerializeField] private string fadeIn = "FadeIn";
        [SerializeField] private string fadeOut = "FadeOut";

        [Header("Events")]
        [SerializeField] private GameEvent onFadeOutEnd;
        [SerializeField] private GameEvent onFadeInEnd;

        private GameScene? nextScene = null;

        private SceneInstance currentScene;
        private bool loading = true;

        /// <summary>
        /// Check if there is any scene loaded, if is not, load the Gameplay
        /// </summary>
        public void LoadApplication()
        {
            #if UNITY_EDITOR
            if (SceneManager.sceneCount > 1)
            {
                LoadFinish();
                return;
            }
            #endif

            nextScene = GameScene.Gameplay;
            StartCoroutine(LoadSceneAsync(GameScene.Gameplay));
        }

        public void RequestReloadScene() => LoadScene(null);

        /// <summary> Fade Out Start </summary>
        public void LoadScene(GameScene? scene)
        {
            if (loading)
            {
                throw new ApplicationException("There is already a scene loading process in progress");
            }

            loading = true;
            nextScene = scene;
            transitionAnimator.Play(fadeOut);
            EventSystem.current.SetSelectedGameObject(null);
        }

        public void OnFadeInEnd()
        {
            onFadeInEnd.Invoke();
        }

        public void OnFadeOutEnd()
        {
            onFadeOutEnd.Invoke();

            if (!nextScene.HasValue)
            {
                nextScene = currentScene.Scene.name.ConvertToEnum<GameScene>();
            }

            StartCoroutine(LoadNextAsyn());

            Time.timeScale = 1f;
        }

        private IEnumerator LoadNextAsyn()
        {
            #if UNITY_EDITOR
            if (currentScene.Scene.name == null)
            {
                Scene activeScene = SceneManager.GetActiveScene();
                yield return SceneManager.UnloadSceneAsync(activeScene);

                ObjectPool.ReturnAllToPool();

                yield return LoadSceneAsync(nextScene.Value);
                yield break;
            }
            #endif

            yield return UnloadSceneAsync(currentScene);

            ObjectPool.ReturnAllToPool();

            yield return LoadSceneAsync(nextScene.Value);
        }

        private IEnumerator UnloadSceneAsync(SceneInstance scene)
        {
            AsyncOperationHandle<SceneInstance> loadOperation = Addressables.UnloadSceneAsync(scene);
            yield return new WaitUntil(() => loadOperation.IsDone);
        }

        private IEnumerator LoadSceneAsync(GameScene gameScene)
        {
            // Get asset
            AssetReference newScene =  gameScene switch
            {
                GameScene.Victory => victory,
                GameScene.Gameplay => gameplay,
                _ => throw new NotImplementedException(),
            };

            // Load new scene
            AsyncOperationHandle<SceneInstance> loadOperation = Addressables.LoadSceneAsync(newScene, LoadSceneMode.Additive);
            yield return new WaitUntil(() => loadOperation.IsDone);

            // Set as active scene
            currentScene = loadOperation.Result;        
            SceneManager.SetActiveScene(currentScene.Scene);

            // Clean memory and finish
            GC.Collect();
            yield return new WaitForEndOfFrame();
            LoadFinish();
        }

        /// <summary> Start Fade In </summary>
        private void LoadFinish()
        {
            nextScene = null;
            loading = false;
            transitionAnimator.Play(fadeIn);
        }
    }
}
