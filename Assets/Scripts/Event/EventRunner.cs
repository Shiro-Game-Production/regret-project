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
            // Set actor's dialogue to dialogue manager
            // Wait dialogue
            eventData.AffectedActor.currentDialogue = eventData.WaitDialogueAsset;
            
            // Start the event
            // Set event state
            eventData.eventState = EventState.Start;
        }

        public void OnEventActive()
        {
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
            // Deactivate event data renderer
            eventData.DeactivateRenderer();
        }

        public void SetNextEvent()
        {
            // Set next dialogue to affected actor
            eventData.AffectedActor.currentDialogue = 
                eventData.NextEventDialogueAsset != null 
                    ? eventData.NextEventDialogueAsset 
                    : eventData.DefaultDialogueAsset;
            
            // Deactivate game object
            gameObject.SetActive(false);
            eventData.gameObject.SetActive(false);
        }
    }
}