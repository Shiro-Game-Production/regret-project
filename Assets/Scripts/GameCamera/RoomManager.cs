using System;
using UnityEngine;

namespace GameCamera
{
    public class RoomManager: MonoBehaviour
    {
        [SerializeField] private bool playerInRoom;
        [SerializeField] private Transform cameraTransform;

        public Transform CameraTransform => cameraTransform;
        public bool PlayerInRoom => playerInRoom;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            playerInRoom = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            playerInRoom = false;
        }
    }
}
