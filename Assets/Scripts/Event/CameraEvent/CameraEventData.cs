using Event.FinishConditionScripts;
using UnityEngine;

namespace Event.CameraEvent {
    public class CameraEventData : EventData {
        [SerializeField] private Transform targetObject;
        [SerializeField] private float duration = 5f;

        #region Setter and Getter

        public Transform TargetObject => targetObject;
        public float Duration => duration;
        

        #endregion
    }
}