using UnityEngine;

namespace GameCamera
{
    public class RoomManager: MonoBehaviour
    {
        [SerializeField] private bool playerInRoom;
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
            
            cameraMovement.SetPosition(cameraTransform.position, 
                cameraTransform.eulerAngles);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            playerInRoom = false;
        }
    }
}
