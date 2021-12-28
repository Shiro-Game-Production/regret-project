using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

namespace Event.CameraEvent {
    public class CameraEventData : EventData {
        [SerializeField] private CinemachineVirtualCamera targetVirtualCamera;
        [SerializeField] private PlayableDirector cutsceneTimeline;
        [SerializeField] private float duration = 5f;

        #region Setter and Getter

        public CinemachineVirtualCamera TargetVirtualCamera => targetVirtualCamera;
        public PlayableDirector CutsceneTimeline => cutsceneTimeline;
        public float Duration => duration;
        
        #endregion
    }
}