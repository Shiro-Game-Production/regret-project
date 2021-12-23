using UnityEngine;

namespace GameCamera.Room
{
    public class RoomTrigger: MonoBehaviour
    {
        public bool playerInRoom;
        [SerializeField] private Transform cameraTransform;
        private CameraMovement cameraMovement;

        private void Awake()
        {
            cameraMovement = CameraMovement.Instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            playerInRoom = true;
            RoomManager.Instance.detectRooms = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
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
