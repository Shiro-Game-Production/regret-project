using System.Collections;
using Event.CameraEvent;
using GameCamera;
using UnityEngine;

namespace Event.FinishConditionScripts{
    public class CameraFinishedCondition : FinishConditionManager {
        
        private void Awake() {
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

            // Run the camera
            // Move the camera to target object
            CameraMovement.Instance.SetPosition(
                cameraEventData.TargetObject.position,
                cameraEventData.TargetObject.eulerAngles);
                
            // Wait for camera duration
            yield return new WaitForSeconds(cameraEventData.Duration);
            Debug.Log("Camera finished");
            OnEndingCondition();
        }
    }
}