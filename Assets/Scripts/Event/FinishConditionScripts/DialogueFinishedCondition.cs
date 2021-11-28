using System.Collections;
using Dialogue;
using UnityEngine;

namespace Event.FinishConditionScripts
{
    public class DialogueFinishedCondition: FinishConditionManager
    {
        private void Awake()
        {
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
            yield return new WaitUntil(() => !DialogueManager.Instance.DialogueIsPlaying);
            OnEndingCondition();
        }
    }
}