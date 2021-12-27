using UnityEngine;
using Cinemachine;

namespace Event.CameraEvent {
    public class CameraEventData : EventData {
        [SerializeField] private CinemachineVirtualCamera targetVirtualCamera;
        [SerializeField] private float duration = 5f;

        #region Setter and Getter

        public CinemachineVirtualCamera TargetVirtualCamera => targetVirtualCamera;
        public float Duration => duration;
        
        #endregion
    }
}