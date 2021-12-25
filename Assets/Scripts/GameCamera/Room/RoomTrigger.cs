using UnityEngine;
using Cinemachine;

namespace GameCamera.Room
{
    public class RoomTrigger: MonoBehaviour
    {
        public bool playerInRoom;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        private CameraMovement cameraMovement;

        private void Awake()
        {
            cameraMovement = CameraMovement.Instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            virtualCamera.Priority = cameraMovement.HIGHER_PRIORITY;
            RoomManager.Instance.detectRooms = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            virtualCamera.Priority = cameraMovement.LOWER_PRIORITY;
            playerInRoom = false;
        }

        /// <summary>
        /// Set camera's transform
        /// </summary>
        public void SetCameraPosition()
        {
            cameraMovement.SetPosition(cameraTransform.position,
                cameraTransform.eulerAngles, updateTopDown: true);
        }

        /// <summary>
        /// Update camera's transform but doesn't move
        /// </summary>
        public void UpdateCameraPosition(){
            cameraMovement.UpdateTopDown(cameraTransform.position,
                cameraTransform.eulerAngles);
        }
    }
}
