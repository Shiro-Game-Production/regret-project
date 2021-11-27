using UnityEngine;

namespace GameCamera
{
    public class CameraManager : SingletonBaseClass<CameraManager>
    {
        [SerializeField] private float transitionSpeed;
        [SerializeField] private Vector3 currentRoomPosition;

        private void LateUpdate()
        {
            // Lerp position
            transform.position = Vector3.Lerp(transform.position, currentRoomPosition, Time.deltaTime * transitionSpeed);
        }
        
        public void MoveCamera(Vector3 roomPosition)
        {
            currentRoomPosition = roomPosition;
        }
    }
}
