using GameCamera;
using UnityEngine;

namespace Dialogue
{
    public class DialogueCameraManager: SingletonBaseClass<DialogueCameraManager>
    {
        [Header("Camera Transition")] 
        private CameraShake cameraShake;
        private DialogueCameraMovement dialogueCameraMovement;
        
        [Header("Top Down Mode")]
        [SerializeField] private Vector3 topDownModePosition;
        [SerializeField] private Vector3 topDownModeAngle;
        
        [Header("Dialogue Mode")]
        [SerializeField] private Vector3 dialogueModePosition;
        [SerializeField] private Vector3 dialogueModeAngle;

        private void Awake()
        {
            dialogueCameraMovement = DialogueCameraMovement.Instance;
            cameraShake = CameraShake.Instance;
        }

        private void Start()
        {
            SetCameraToTopDownMode();
        }
        
        /// <summary>
        /// Set camera to top down mode
        /// </summary>
        public void SetCameraToTopDownMode()
        {
            dialogueCameraMovement.SetPosition(topDownModePosition, topDownModeAngle, false);
        }
        
        /// <summary>
        /// Set camera to dialogue mode
        /// </summary>
        public void SetCameraToDialogueMode()
        {
            dialogueCameraMovement.SetPosition(dialogueModePosition, dialogueModeAngle, true);
        }
        
        /// <summary>
        /// Call camera shake effect
        /// </summary>
        public void ShakingEffect()
        {
            StartCoroutine(cameraShake.ShakingEffect());
        }
    }
}