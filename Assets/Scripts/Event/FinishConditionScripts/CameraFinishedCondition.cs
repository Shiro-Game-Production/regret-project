using System.Collections;
using Event.CameraEvent;
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
            yield return new WaitForSeconds(cameraEventData.Duration);
            Debug.Log("Camera finished");
            OnEndingCondition();
        }
    }
}