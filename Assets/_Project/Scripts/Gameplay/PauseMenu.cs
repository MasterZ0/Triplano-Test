using System;
using System.Collections;
using TriplanoTest.Gameplay.Level;
using TriplanoTest.Inputs;
using TriplanoTest.Shared;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TriplanoTest.Gameplay
{
    public class PauseMenu : MonoBehaviour
    {
        [Header("Main Menu")]
        [SerializeField] private Button mainMenuBtn;
        [SerializeField] private GameObject menu;
        [SerializeField] private GameEvent<bool> onPauseGame;

        public static event Action<bool> OnPause = delegate { };

        private UIInputs uiInputs;
        private bool paused;

        private void Awake()
        {
            uiInputs = new UIInputs();
            uiInputs.OnPause += OnPressPause;
        }

        private void OnDestroy()
        {
            uiInputs.Dispose();
        }

        public void OnReload() => LevelManager.Reload();

        public void OnResume()
        {
            if (!paused) // The keyboard can call you twice at the same time, due to Esc and Cancel allow you to pause the game
                return;

            EventSystem.current.SetSelectedGameObject(null);
            PauseGame(false);
        }

        public void OnExit()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        private void OnPressPause()
        {
            if (paused)
            {
                OnResume();
                return;
            }

            PauseGame(true);
        }

        private void PauseGame(bool pause)
        {
            paused = pause;

            if (paused)
            {
                StartCoroutine(SelectWithDelay());
            }

            menu.SetActive(pause);
            Time.timeScale = pause ? 0f : 1f;
            onPauseGame.Invoke(pause);
            OnPause(pause);
        }

        private IEnumerator SelectWithDelay()
        {
            yield return new WaitForEndOfFrame();
            mainMenuBtn.Select();
        }
    }
}