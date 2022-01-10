using Effects;
using UnityEngine;

namespace Dialogue.Logs{
    public class DialogueLogManager : SingletonBaseClass<DialogueLogManager> {
        [SerializeField] private CanvasGroup dialogueLogCanvasGroup;
        [SerializeField] private DialogueLog dialogueLogPrefab;
        [SerializeField] private Transform dialogueLogParent;

        private DialogueManager dialogueManager;

        private void Awake() {
            dialogueManager = DialogueManager.Instance;
        }

        /// <summary>
        /// Add dialogue log
        /// </summary>
        public void AddDialogueLog(string speakerNameValue, string dialogueTextValue){
            // BUG: RESET DIALOGUE LOG
            DialogueLog dialogueLogManager = Instantiate(dialogueLogPrefab, dialogueLogParent);
            dialogueLogManager.SetDialogueLog(speakerNameValue, dialogueTextValue);
        }

        /// <summary>
        /// Show log for log button
        /// </summary>
        /// <param name="showLog">If true, show log, else, hide log</param>
        public void ShowLog(bool showLog){
            if(showLog){
                StartCoroutine(FadingEffect.FadeIn(dialogueLogCanvasGroup,
                    beforeEffect: () => dialogueManager.PushDialogueMode(DialogueMode.ViewLog))
                );
            } else {
                StartCoroutine(FadingEffect.FadeOut(dialogueLogCanvasGroup,
                    afterEffect: () => dialogueManager.PopDialogueMode(DialogueMode.ViewLog))
                );
            }
        }
    }
}