using UnityEngine;
using Cinemachine;

namespace GameCamera
{
    public class CameraMovement : SingletonBaseClass<CameraMovement>
    {
        [Header("Top Down Mode")]
        public readonly int LOWER_PRIORITY = 10;
        public readonly int ROOM_HIGHER_PRIORITY = 15;

        [Header("Dialogue Mode")]
        public readonly int DIALOGUE_HIGHER_PRIORITY = 16;

        [Header("Camera Event")]
        public readonly int CAMERA_HIGHER_PRIORITY = 17;

        /// <summary>
        /// Set virtual camera's priority
        /// </summary>
        /// <param name="virtualCamera"></param>
        public void SetVirtualCameraPriority(CinemachineVirtualCamera virtualCamera, int priority){
            virtualCamera.Priority = priority;
        }
    }
}