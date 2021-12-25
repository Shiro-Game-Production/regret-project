using UnityEngine;

namespace GameCamera
{
    public class CameraMovement : SingletonBaseClass<CameraMovement>
    {
        [Header("Camera Transition")]
        [Range(0, 10)]
        [SerializeField] private float transitionSpeed;
        [SerializeField] private bool canMove;
        private Vector3 targetPosition, targetAngle;

        public readonly int HIGHER_PRIORITY = 15;
        public readonly int LOWER_PRIORITY = 13;

        [Header("Top Down Mode")]
        [SerializeField] private Transform topDownMode;
        
        [Header("Dialogue Mode")]
        [SerializeField] private Transform dialogueMode;
        private Transform playerTransform;

        private void Awake()
        {
            canMove = false;
            playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            
            if(playerTransform == null)
                Debug.LogError("Object with \"Player\" tag not found");
        }

        /// <summary>
        /// Set camera to top down mode
        /// </summary>
        public void SetCameraToTopDownMode()
        {
            SetPosition(topDownMode.localPosition, topDownMode.localEulerAngles, updateTopDown: true);
        }
        
        /// <summary>
        /// Set camera to dialogue mode
        /// </summary>
        public void SetCameraToDialogueMode()
        {
            SetPosition(dialogueMode.localPosition, dialogueMode.localEulerAngles, true);
        }

        /// <summary>
        /// Make camera position to target position using lerp
        /// </summary>
        /// <param name="targetPosition">Camera target position</param>
        /// <param name="targetAngle">Camera target angle</param>
        /// <param name="setToPlayer">Set camera's parent to player</param>
        /// <param name="updateTopDown">Update top down transform</param>
        public void SetPosition(Vector3 targetPosition, Vector3 targetAngle, 
            bool setToPlayer = false, bool updateTopDown = false)
        {
            if (updateTopDown)
            {
                UpdateTopDown(targetPosition, targetAngle);
            }
            
            canMove = true;
            transform.SetParent(setToPlayer ? playerTransform : null);
            this.targetPosition = targetPosition;
            this.targetAngle = targetAngle;
        }

        /// <summary>
        /// Update top down position but doesn't move
        /// </summary>
        /// <param name="targetPosition">Top down target position</param>
        /// <param name="targetAngle">Top down target angle</param>
        public void UpdateTopDown(Vector3 targetPosition, Vector3 targetAngle){
            topDownMode.localPosition = targetPosition;
            topDownMode.localEulerAngles = targetAngle;
        }
    }
}