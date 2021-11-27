using UnityEngine;

namespace GameCamera
{
    public class RoomManager: MonoBehaviour
    {
        [SerializeField] private bool playerInRoom;
        [SerializeField] private Vector3 cameraPosition;

        public Vector3 CameraPosition => cameraPosition;
        public bool PlayerInRoom => playerInRoom;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            playerInRoom = !playerInRoom;
        }
    }
}
