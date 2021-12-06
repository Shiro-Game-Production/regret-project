using UnityEngine;

namespace GameCamera.Room
{
    public class RoomTrigger: MonoBehaviour
    {
        [SerializeField] public bool playerInRoom;
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

        public void SetCameraPosition()
        {
            cameraMovement.SetPosition(cameraTransform.position,
                cameraTransform.eulerAngles);
        }
    }
}
