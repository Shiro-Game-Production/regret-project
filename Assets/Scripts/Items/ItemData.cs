using System.Collections;
using System.Collections.Generic;
using Dialogue;
using Event;
using Event.CameraEvent;
using Event.DialogueEvent;
using UnityEngine;

namespace Items
{
    public class ItemData: MonoBehaviour
    {
        public enum ItemMode { DialogueMode, NormalMode }
        public enum InteractionText { Interaksi, Buka, Bicara }

        public InteractionText interactionText = InteractionText.Interaksi;
        [SerializeField] private List<EventData> events;
        public ItemMode itemMode = ItemMode.NormalMode;
        public TextAsset currentDialogue;

        public virtual void HandleInteraction()
        {
            if(events.Count == 0) return;
            StartCoroutine(StartEvent());
        }

        private IEnumerator StartEvent(){
            foreach (EventData eventData in events)
            {
                if(!eventData.isFinished){
                    // Run the event
                    switch(eventData){
                        case DialogueEventData _:
                            Debug.Log("Convert to dialogue");
                            HandleDialogue();
                            break;
                        case CameraEventData cameraEventData:
                            Debug.Log("Convert to camera");
                            CameraEventManager.Instance.SetEventData(cameraEventData);
                            break;
                    }
                    
                    // Wait event until finished
                    yield return new WaitUntil(() => eventData.isFinished);
                }
            }
        }

        protected virtual void HandleDialogue()
        {
            DialogueManager.Instance.SetDialogue(currentDialogue);
        }
    }
}