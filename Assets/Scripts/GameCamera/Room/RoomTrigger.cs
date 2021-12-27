using UnityEngine;
using Cinemachine;

namespace GameCamera.Room
{
    public class RoomTrigger: MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        private CameraMovement cameraMovement;

        private void Awake()
        {
            cameraMovement = CameraMovement.Instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            cameraMovement.SetVirtualCameraPriority(virtualCamera,
                cameraMovement.ROOM_HIGHER_PRIORITY);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            cameraMovement.SetVirtualCameraPriority(virtualCamera,
                cameraMovement.LOWER_PRIORITY);
        }
    }
}
