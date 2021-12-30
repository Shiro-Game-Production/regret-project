using Dialogue;
using Event.FinishConditionScripts;
using Items;
using UnityEngine;

namespace Event.DialogueEvent {
    public class DialogueEventRunner : MonoBehaviour, IEventRunner {
        public DialogueEventData eventData;
        private DialogueManager dialogueManager;
        private bool hasSetFinishCondition;
        public bool canStartEvent;

        private void Awake()
        {
            dialogueManager = DialogueManager.Instance;
        }

        private void OnEnable() {
            canStartEvent = false;
            hasSetFinishCondition = false;
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
                            case FinishCondition.DialogueFinished:
                                eventData.TriggerObject.SetEndingCondition();
                                hasSetFinishCondition = true;
                                break;
                            case FinishCondition.PuzzleFinished:
                                eventData.TriggerObject.SetEndingCondition();
                                hasSetFinishCondition = true;
                                break;
                            case FinishCondition.CameraDurationFinished:
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
            if(eventData.EventDialogue.WaitDialogueAsset)
            {
                // Set actor's dialogue to dialogue manager
                // Wait dialogue
                eventData.AffectedItem.currentDialogue = eventData.EventDialogue.WaitDialogueAsset;
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
            eventData.ItemData.itemMode = ItemData.ItemMode.DialogueMode;
        }

        public void OnEventFinish()
        {
            if(eventData.EventDialogue.FinishDialogueAsset)
            {
                // Set actor's dialogue to dialogue manager
                // Finish dialogue
                eventData.AffectedItem.currentDialogue = eventData.EventDialogue.FinishDialogueAsset;
            }
            
            // Set event state
            eventData.eventState = EventState.Finish;
            // Deactivate event data renderer
            eventData.OnEventFinish();
        }

        public void SetNextEvent()
        {
            if(eventData.EventDialogue.NextEventDialogueAsset || eventData.EventDialogue.DefaultDialogueAsset)
            {
                // Set next dialogue to affected actor
                eventData.AffectedItem.currentDialogue =
                    eventData.EventDialogue.NextEventDialogueAsset != null
                        ? eventData.EventDialogue.NextEventDialogueAsset
                        : eventData.EventDialogue.DefaultDialogueAsset;
            }
            
            // Deactivate game object
            eventData.gameObject.SetActive(eventData.KeepObjectAfterFinish);
            gameObject.SetActive(false);
        }
    }
}