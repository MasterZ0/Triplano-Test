using UnityEngine;
using TMPro;

namespace TriplanoTest.ApplicationManager
{
    /// <summary>
    /// Display the current Application Version
    /// </summary>
    public class ProjectVersion : MonoBehaviour 
    {
        [Header("Project Version")]
        [SerializeField] private TextMeshProUGUI version;

        private void Awake() => version.text = Application.version;
    }
}