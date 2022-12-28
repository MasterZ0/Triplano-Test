using Cinemachine;
using UnityEngine;

namespace TriplanoTest.Gameplay
{
    /// <summary>
    /// Changes current camera when player is inside trigger. 
    /// Remember to set correcly the camera priority
    /// </summary>
    public class CameraTriggerActivator : MonoBehaviour
    {
        [SerializeField] protected CinemachineVirtualCamera virtualCamera;

        private void Reset()
        {
            if (virtualCamera == null)
            {
                virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            }
        }

        private void Start()
        {
            virtualCamera.LookAt = MainCamera.PlayerTarget;
            virtualCamera.Follow = MainCamera.PlayerTarget;
        }

        private void OnTriggerEnter(Collider other)
        {
            virtualCamera.gameObject.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            virtualCamera.gameObject.SetActive(false);
        }
    }
}