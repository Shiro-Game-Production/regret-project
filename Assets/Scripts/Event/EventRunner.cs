using UnityEngine;

namespace Event
{
    public class EventRunner : MonoBehaviour, IEvent
    {
        public EventData eventData;
        public bool canStartEvent;

        private void Awake()
        {
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
                    switch (eventData.finishCondition)
                    {
                        case FinishCondition.OnTriggerEnter:
                            eventData.TriggerObject.SetBoxCollider(true);
                            break;
                        case FinishCondition.PuzzleFinished:
                            break;
                        case FinishCondition.DialogueFinished:
                            break;
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