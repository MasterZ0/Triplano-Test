using UnityEngine;

namespace TriplanoTest.ApplicationManager
{
    /// <summary>
    /// Useful to receive Unity events
    /// </summary>
    public class SceneChanger : MonoBehaviour
    {
        [Header("Scene Changer")]
        [SerializeField] private GameScene scene;

        public void OnChanceScene()
        {
            GameManager.SceneLoader.LoadScene(scene);
        }

        public void OnReloadScene()
        {
            GameManager.SceneLoader.RequestReloadScene();
        }
    }
}
