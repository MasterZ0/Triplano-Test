using TriplanoTest.Data;
using TriplanoTest.Shared;
using UnityEngine;

namespace TriplanoTest.ApplicationManager
{
    /// <summary>
    /// Control the GameManager Scene
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [Header("Game Manager")]
        [SerializeField] private SceneLoader sceneLoader;
        [SerializeField] private GameData gameData;

        public static SceneLoader SceneLoader => Instance.sceneLoader;

        protected override void AfterAwake()
        {
            Cursor.lockState = CursorLockMode.Confined;

            gameData.Initialize();
            sceneLoader.LoadApplication();
        }
    }
}
