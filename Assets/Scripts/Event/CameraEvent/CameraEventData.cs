using UnityEngine;

namespace Event.CameraEvent {
    public class CameraEventData : EventData {
        [SerializeField] private Transform targetObject;
        [SerializeField] private float duration = 5f;
    }
}