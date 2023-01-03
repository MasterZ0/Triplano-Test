using UnityEngine;
using UnityEngine.UI;

namespace TriplanoTest.Gameplay
{
    public class VictoryScreen : MonoBehaviour
    {
        [SerializeField] private Button firstBtn;

        private void Awake() => firstBtn.Select();

        public void OnExit()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}