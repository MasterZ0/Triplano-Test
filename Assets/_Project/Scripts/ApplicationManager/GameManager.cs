using TriplanoTest.Data;
using TriplanoTest.Shared;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TriplanoTest.ApplicationManager
{
    /// <summary>
    /// Control the GameManager Scene
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [Header("Game Manager")]
        [SerializeField] private Animator transitionAnimator;
        [SerializeField] private SceneLoader sceneLoader;
        [SerializeField] private GameData gameData;

        [Header("Events")]
        [SerializeField] private GameEvent onFadeOutEnd;
        [SerializeField] private GameEvent onFadeInEnd;

        private GameScene? nextScene = null;

        private const string FadeIn = "FadeIn";
        private const string FadeOut = "FadeOut";

        public static event Action<bool> OnChanceFocus;
        public static bool FocusOnGame { get; private set; }

        protected override void AfterAwake()
        {
            Cursor.lockState = CursorLockMode.Confined;

            gameData.Initialize();
            sceneLoader.LoadApplication(OnLoadFinish);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            #if UNITY_EDITOR
            if (Application.isEditor)
                return;
            #endif

            FocusOnGame = hasFocus;
            OnChanceFocus?.Invoke(FocusOnGame);
        }

        public static void RequestReloadScene() => RequestLoadScene(null);
        public static void RequestLoadScene(GameScene? scene)
        {
            Instance.LoadScene(scene);
        }

        public void OnFadeInEnd()
        {
            onFadeInEnd.Invoke();
        }

        public void OnFadeOutEnd()
        {
            onFadeOutEnd.Invoke();

            if (nextScene.HasValue)
            {
                sceneLoader.LoadScene(nextScene.Value, OnLoadFinish);
            }
            else
            {
                sceneLoader.ReloadScene(OnLoadFinish);
            }

            nextScene = null;
            Time.timeScale = 1f;
        }

        /// <summary> Fade Out Start </summary>
        private void LoadScene(GameScene? scene)
        {
            nextScene = scene;
            transitionAnimator.Play(FadeOut);
            EventSystem.current.SetSelectedGameObject(null);
        }

        /// <summary> Fade In Start </summary>
        private void OnLoadFinish()
        {
            transitionAnimator.Play(FadeIn);
        }
    }
}
