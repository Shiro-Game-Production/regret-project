using System.Collections;
using Dialogue;
using Event.DialogueEvent;
using Items;
using UnityEngine;

namespace Event.FinishConditionScripts
{
    public class DialogueFinishedCondition: FinishConditionManager
    {
        private DialogueManager dialogueManager;
        private ItemData itemData;
        
        private void Awake()
        {
            itemData = GetComponent<ItemData>();
            dialogueManager = DialogueManager.Instance;
            EventData = GetComponent<DialogueEventData>();
        }

        public override void SetEndingCondition()
        {
            StartCoroutine(WaitUntilDialogueFinished());
        }
        
        /// <summary>
        /// Wait until dialogue finished or is not playing anymore
        /// The moment dialogue is not playing anymore, call on ending condition
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitUntilDialogueFinished()
        {
            Debug.Log("Wait for dialogue finished");
            yield return new WaitUntil(() => 
                !dialogueManager.DialogueIsPlaying && 
                dialogueManager.CurrentDialogueAsset == itemData.currentDialogue);
            Debug.Log("Dialogue finished");
            OnEndingCondition();
        }
    }
}