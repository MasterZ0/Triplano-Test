using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using System.Linq;
using TriplanoTest.Shared;
using System.Collections;
using UnityEngine.ResourceManagement.AsyncOperations;
using TriplanoTest.Shared.ExtensionMethods;
using TriplanoTest.ApplicationManager;
using System;
using TriplanoTest.Persistence;

namespace TriplanoTest.Gameplay.Level
{
    // It so easier with UniTask library -> https://github.com/Cysharp/UniTask
    public class LevelManager : Singleton<LevelManager>
    {
        [Header("Settings")]
        [AssetReferenceUILabelRestriction(Constants.Scene)]
        [SerializeField] private AssetReference startLevel;
        [SerializeField] private string startConnection;

        [Header("Components")]
        [SerializeField] private Transform playerController;
        [SerializeField] private Animator fadeAnimator;

        [Header("Animation States")]
        [SerializeField] private string fadeIn = "FadeIn";
        [SerializeField] private string fadeOut = "FadeOut";

        public static event Action onLoadStart;
        public static event Action onLoadFinish;
        public static event Action onUnloadFinish;

        private Room currentRoom;
        private SceneInstance currentScene;
        private bool loading;

        public static event Action<Room, RoomConnection> onLoadRoom;

        private void Start()
        {
            Time.timeScale = 0f;
            LoadNewArea(startLevel, startConnection);
        }

        /// <summary> Load new level </summary>
        public static void LoadNewArea(AssetReference newScene, string spawnPoint)
        {
            if (Instance.loading)
                return;

            Instance.StartCoroutine(Instance.LoadNewAreaAsync(newScene, spawnPoint));
        }

        /// <summary> Unload all levels and load victory scene </summary>
        public static void PlayerVictory()
        {
            Instance.StartCoroutine(Instance.ChangeScene(GameScene.Victory));
        }

        public static void Reload()
        {
            Instance.StartCoroutine(Instance.ChangeScene(GameScene.Gameplay));
        }

        private IEnumerator LoadNewAreaAsync(AssetReference newScene, string spawnPoint)
        {
            // Start fade out and disable player
            loading = true;

            // Unload current scene
            if (currentRoom != null)
            {
                yield return fadeAnimator.PlayCoroutineRealtime(fadeOut);

                // Pause Game
                Time.timeScale = 0f;

                yield return UnloadCurrentScene();

                onUnloadFinish?.Invoke();
            }

            // Load new scene
            AsyncOperationHandle<SceneInstance> loadOperation = Addressables.LoadSceneAsync(newScene, LoadSceneMode.Additive);
            yield return new WaitUntil(() => loadOperation.IsDone);

            // Get Result and Room Component
            currentScene = loadOperation.Result;
            currentScene.Scene.GetRootGameObjects().First(go => go.TryGetComponent(out currentRoom));

            // Get current connection and set player position
            RoomConnection connection = currentRoom.GetConnection(spawnPoint);
            onLoadRoom?.Invoke(currentRoom, connection);

            Time.timeScale = 1f;
            yield return new WaitForEndOfFrame();

            // Resume Game
            yield return fadeAnimator.PlayCoroutineRealtime(fadeIn);

            // Active player
            loading = false;
            onLoadFinish?.Invoke();
        }
        private IEnumerator UnloadCurrentScene()
        {
            AsyncOperationHandle<SceneInstance> unloadOperation = Addressables.UnloadSceneAsync(currentScene);
            yield return new WaitUntil(() => unloadOperation.IsDone);
        }

        private IEnumerator ChangeScene(GameScene scene)
        {
            yield return fadeAnimator.PlayCoroutineRealtime(fadeOut);

            yield return UnloadCurrentScene();

            PersistenceManager.ClearData();
            GameManager.SceneLoader.LoadScene(scene);
        }
    }
}