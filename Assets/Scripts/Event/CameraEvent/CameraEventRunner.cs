using UnityEngine;

namespace Event.CameraEvent {
    public class CameraEventRunner : MonoBehaviour, IEventRunner
    {
        public CameraEventData eventData;
        public bool canStartEvent;

        private void OnEnable() {
            canStartEvent = false;
        }

        private void Update() {
            switch(eventData.eventState){
                case EventState.NotStarted:
                    if(canStartEvent)
                        OnEventStart();
                    break;

                case EventState.Start:
                    OnEventActive();
                    break;

                case EventState.Active:
                    if(eventData.isFinished)
                        OnEventFinish();
                    break;

                case EventState.Finish:
                    SetNextEvent();
                    break;
            }
        }

        public void OnEventStart()
        {
            eventData.eventState = EventState.Start;
        }
        
        public void OnEventActive()
        {
            eventData.eventState = EventState.Active;
        }

        public void OnEventFinish()
        {
            eventData.eventState = EventState.Finish;
        }

        public void SetNextEvent()
        {
            eventData.gameObject.SetActive(eventData.KeepObjectAfterFinish);
            gameObject.SetActive(false);
        }
    }
}