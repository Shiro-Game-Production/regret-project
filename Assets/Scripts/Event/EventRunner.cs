using UnityEngine;

namespace Event
{
    public class EventRunner : MonoBehaviour, IEvent
    {
        public EventData eventData;
        public bool canStartEvent;

        [Header("Event Data Origin State")]
        private EventData.EventState originEventState;
        private EventData.FinishCondition originFinishCondition;
        private bool originIsFinished;

        private void Awake()
        {
            // Get the origin event state in the beginning
            originEventState = eventData.eventState;
            originFinishCondition = eventData.finishCondition;
            originIsFinished = eventData.isFinished;
            
            canStartEvent = false;
        }

        private void Update()
        {
            switch (eventData.eventState)
            {
                case EventData.EventState.NotStarted:
                    if(canStartEvent)
                        OnEventStart();
                    break;
                
                case EventData.EventState.Start:
                    OnEventActive();
                    break;
                
                case EventData.EventState.Active:
                    switch (eventData.finishCondition)
                    {
                        case EventData.FinishCondition.OnTriggerEnter:
                            eventData.TriggerObject.SetBoxCollider(true);
                            break;
                        case EventData.FinishCondition.PuzzleFinished:
                            break;
                        case EventData.FinishCondition.DialogueFinished:
                            break;
                    }
                    
                    break;
            }

            if (eventData.isFinished)
            {
                OnEventFinish();
            }
        }

        private void OnApplicationQuit()
        {
            // Return the origin event state to scriptable object file
            eventData.eventState = originEventState;
            eventData.finishCondition = originFinishCondition;
            eventData.isFinished = originIsFinished;
        }

        public void OnEventStart()
        {
            // Set actor's dialogue to dialogue manager
            // Wait dialogue
            eventData.AffectedActor.currentDialogue = eventData.WaitDialogueAsset;
            
            // Start the event
            // Set event state
            eventData.eventState = EventData.EventState.Start;
        }

        public void OnEventActive()
        {
            // Add ending condition to object
            // Call on event finish when ending condition is met
            
            // Set event state
            eventData.eventState = EventData.EventState.Active;
        }

        public void OnEventFinish()
        {
            // Set actor's dialogue to dialogue manager
            // Finish dialogue
            eventData.AffectedActor.currentDialogue = eventData.FinishDialogueAsset;
            
            // Set event state
            eventData.eventState = EventData.EventState.Finish;
            
            gameObject.SetActive(false);
        }
    }
}