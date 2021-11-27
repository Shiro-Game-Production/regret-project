using UnityEngine;

namespace GameCamera
{
    public class RoomManager: MonoBehaviour
    {
        [SerializeField] private bool playerInRoom;
        [SerializeField] private Vector3 cameraPosition;

        public Vector3 CameraPosition => cameraPosition;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            playerInRoom = !playerInRoom;
        }
    }
}
