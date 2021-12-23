using Event.FinishConditionScripts;
using UnityEngine;

namespace Event.CameraEvent {
    public class CameraEventRunner : MonoBehaviour, IEventRunner
    {
        public CameraEventData eventData;
        private bool hasSetFinishCondition;
        public bool canStartEvent;

        private void OnEnable() {
            canStartEvent = false;
            hasSetFinishCondition = false;
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
                    if(!hasSetFinishCondition)
                    {
                        switch (eventData.finishCondition)
                        {
                            case FinishCondition.CameraDurationFinished:
                                eventData.TriggerObject.SetEndingCondition();
                                hasSetFinishCondition = true;
                                break;
                            case FinishCondition.DialogueFinished:
                            case FinishCondition.PuzzleFinished:
                            case FinishCondition.OnTriggerEnter:
                                hasSetFinishCondition = true;
                                break;
                        }
                    }
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
            eventData.canBeInteracted = true;
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