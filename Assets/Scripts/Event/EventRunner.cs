using UnityEngine;

namespace Event
{
    public class EventRunner : MonoBehaviour, IEvent
    {
        public EventData eventData;
        public bool canStartEvent;
        private bool hasSetFinishCondition;

        private void Awake()
        {
            hasSetFinishCondition = false;
            canStartEvent = false;
        }

        private void Update()
        {
            switch (eventData.eventState)
            {
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
                            case FinishCondition.OnTriggerEnter:
                                eventData.TriggerObject.SetEndingCondition();
                                hasSetFinishCondition = true;
                                break;
                            case FinishCondition.PuzzleFinished:
                                hasSetFinishCondition = true;
                                break;
                            case FinishCondition.DialogueFinished:
                                hasSetFinishCondition = true;
                                break;
                        }
                    }
                    
                    break;
            }

            if (eventData.isFinished)
            {
                OnEventFinish();
            }
        }

        public void OnEventStart()
        {
            // Set actor's dialogue to dialogue manager
            // Wait dialogue
            eventData.AffectedActor.currentDialogue = eventData.WaitDialogueAsset;
            
            // Start the event
            // Set event state
            eventData.eventState = EventState.Start;
        }

        public void OnEventActive()
        {
            // Add ending condition to object
            // Call on event finish when ending condition is met
            
            // Set event state
            eventData.eventState = EventState.Active;
        }

        public void OnEventFinish()
        {
            // Set actor's dialogue to dialogue manager
            // Finish dialogue
            eventData.AffectedActor.currentDialogue = eventData.FinishDialogueAsset;
            
            // Set event state
            eventData.eventState = EventState.Finish;
            
            gameObject.SetActive(false);
            eventData.gameObject.SetActive(false);
        }
    }
}