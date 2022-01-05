using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using Player;

namespace Event.CameraEvent {
    public class CameraEventData : EventData {
        [SerializeField] private CinemachineVirtualCamera targetVirtualCamera;
        [SerializeField] private PlayableDirector cutsceneTimeline;

        [Header("Target")]
        [SerializeField] private bool useTarget;
        [DrawIf("useTarget", true)]
        [SerializeField] private NavigationMovement targetCharacter;
        [DrawIf("useTarget", true)]
        [SerializeField] private Transform targetPosition;
        [DrawIf("useTarget", true)]
        [SerializeField] private Transform lookAtTarget;

        #region Setter and Getter

        public CinemachineVirtualCamera TargetVirtualCamera => targetVirtualCamera;
        public PlayableDirector CutsceneTimeline => cutsceneTimeline;
        public NavigationMovement TargetCharacter => targetCharacter;
        public Transform TargetPosition => targetPosition;
        public Transform LookAtTarget => lookAtTarget;
        public bool UseTarget => useTarget;

        #endregion
    }
}