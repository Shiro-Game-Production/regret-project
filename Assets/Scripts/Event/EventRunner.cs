using Dialogue;
using UnityEngine;

namespace Event
{
    public class EventRunner : MonoBehaviour, IEvent
    {
        public EventData eventData;
        public bool canStartEvent;

        private DialogueManager dialogueManager;
        private bool hasSetFinishCondition;

        private void Awake()
        {
            dialogueManager = DialogueManager.Instance;
        }

        private void OnEnable()
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
                    // Add ending condition to object
                    // Call on event finish when ending condition is met
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
                                eventData.TriggerObject.SetEndingCondition();
                                hasSetFinishCondition = true;
                                break;
                        }
                    }
                    
                    if (eventData.isFinished)
                    {
                        OnEventFinish();
                    }
                    break;
                case EventState.Finish:
                    if(dialogueManager.DialogueIsPlaying)
                        SetNextEvent();
                    break;
            }
        }

        public void OnEventStart()
        {
            if(eventData.WaitDialogueAsset)
            {
                // Set actor's dialogue to dialogue manager
                // Wait dialogue
                eventData.AffectedActor.currentDialogue = eventData.WaitDialogueAsset;
            }
            
            // Start the event
            // Set event state
            eventData.eventState = EventState.Start;
        }

        public void OnEventActive()
        {
            // Set event state
            eventData.eventState = EventState.Active;
            eventData.canBeInteracted = true;
        }

        public void OnEventFinish()
        {
            if(eventData.FinishDialogueAsset)
            {
                // Set actor's dialogue to dialogue manager
                // Finish dialogue
                eventData.AffectedActor.currentDialogue = eventData.FinishDialogueAsset;
            }
            
            // Set event state
            eventData.eventState = EventState.Finish;
            // Deactivate event data renderer
            eventData.OnEventFinish();
        }

        public void SetNextEvent()
        {
            if(eventData.NextEventDialogueAsset || eventData.DefaultDialogueAsset)
            {
                // Set next dialogue to affected actor
                eventData.AffectedActor.currentDialogue =
                    eventData.NextEventDialogueAsset != null
                        ? eventData.NextEventDialogueAsset
                        : eventData.DefaultDialogueAsset;
            }
            
            // Deactivate game object
            eventData.gameObject.SetActive(eventData.KeepObjectAfterFinish);
            gameObject.SetActive(false);
        }
    }
}