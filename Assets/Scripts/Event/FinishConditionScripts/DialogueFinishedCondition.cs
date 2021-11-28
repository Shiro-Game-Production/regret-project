using System.Collections;
using Actors;
using Dialogue;
using UnityEngine;

namespace Event.FinishConditionScripts
{
    public class DialogueFinishedCondition: FinishConditionManager
    {
        private DialogueManager dialogueManager;
        private ActorManager actorManager;
        
        private void Awake()
        {
            actorManager = GetComponent<ActorManager>();
            dialogueManager = DialogueManager.Instance;
            EventData = GetComponent<EventData>();
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
                dialogueManager.CurrentDialogueAsset == actorManager.currentDialogue);
            Debug.Log("Dialogue finished");
            OnEndingCondition();
        }
    }
}