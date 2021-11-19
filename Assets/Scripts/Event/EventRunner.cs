using UnityEngine;

namespace Event
{
    public class EventRunner : MonoBehaviour, IEvent
    {
        public EventData eventData;
        public bool canStartEvent;

        private EventData.EventState originEventState;

        private void Awake()
        {
            // Get the origin event state in the beginning
            originEventState = eventData.eventState;
            canStartEvent = false;
        }

        private void Update()
        {
            if (canStartEvent &&
                eventData.eventState == EventData.EventState.NotStarted)
            {
                OnEventStart();
            }

            if (eventData.eventState == EventData.EventState.Start)
            {
                OnEventActive();
            }
        }

        private void OnApplicationQuit()
        {
            // Return the origin event state to scriptable object file
            eventData.eventState = originEventState;
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