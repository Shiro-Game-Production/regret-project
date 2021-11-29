using UnityEngine;

namespace GameCamera
{
    public class DialogueCameraMovement : SingletonBaseClass<DialogueCameraMovement>
    {
        [Range(0, 10)]
        [SerializeField] private float transitionSpeed;

        private bool canMove;
        
        private Camera mainCamera;
        private Transform playerTransform;
        private Vector3 targetPosition, targetAngle;

        private void Awake()
        {
            canMove = false;
            mainCamera = GetComponent<Camera>();
            playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            
            if(playerTransform == null)
                Debug.LogError("Object with \"Player\" tag not found");
        }

        private void LateUpdate()
        {
            if (!canMove) return;
            
            var mainCameraTransform = mainCamera.transform;
            Vector3 currentPosition = mainCameraTransform.localPosition;
            Vector3 currentAngles = mainCameraTransform.localEulerAngles;
            
            Vector3 smoothPositionTransition = Vector3.Lerp(
                currentPosition, targetPosition, transitionSpeed * Time.deltaTime);

            Vector3 smoothRotationTransition = Vector3.Lerp(
                currentAngles, targetAngle, transitionSpeed * Time.deltaTime);

            mainCameraTransform.localPosition = smoothPositionTransition;
            mainCameraTransform.localEulerAngles = smoothRotationTransition;

            if (mainCameraTransform.localPosition == targetPosition &&
                mainCameraTransform.localEulerAngles == targetAngle)
            {
                canMove = false;
            }
        }

        /// <summary>
        /// Make camera position to target position using lerp
        /// </summary>
        /// <param name="targetPosition">Camera target position</param>
        /// <param name="targetAngle">Camera target angle</param>
        /// <param name="setToPlayer">Set camera's parent to player</param>
        public void SetPosition(Vector3 targetPosition, Vector3 targetAngle, bool setToPlayer)
        {
            canMove = true;
            mainCamera.transform.SetParent(setToPlayer ? playerTransform : null);
            this.targetPosition = targetPosition;
            this.targetAngle = targetAngle;
        }
    }
}