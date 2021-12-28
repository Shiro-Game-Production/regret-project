using System.Collections;
using Dialogue;
using Event.CameraEvent;
using GameCamera;
using Player;
using UnityEngine;

namespace Event.FinishConditionScripts{
    public class CameraFinishedCondition : FinishConditionManager {
        private CameraMovement cameraMovement;

        private void Awake() {
            cameraMovement = CameraMovement.Instance;
            EventData = GetComponent<CameraEventData>();
        }

        public override void SetEndingCondition()
        {
            StartCoroutine(WaitForCamera());
        }

        /// <summary>
        /// Wait until camera's duration is up
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForCamera(){
            Debug.Log("Wait for camera finished");
            CameraEventData cameraEventData = EventData as CameraEventData;
            // Wait until player don't move
            yield return new WaitUntil(() => !PlayerMovement.Instance.IsWalking);

            // Run the camera
            // Move the camera to target object
            cameraMovement.SetVirtualCameraPriority(cameraEventData.TargetVirtualCamera,
                cameraMovement.CAMERA_HIGHER_PRIORITY);
            cameraEventData.CutsceneTimeline.Play();

            // Wait for camera duration
            yield return new WaitForSeconds((float) cameraEventData.CutsceneTimeline.duration + 2f);

            // Camera finish
            Debug.Log("Camera finished");
            cameraMovement.SetVirtualCameraPriority(cameraEventData.TargetVirtualCamera,
                cameraMovement.LOWER_PRIORITY);
            DialogueManager.Instance.ResumeStory();
            OnEndingCondition();
        }
    }
}