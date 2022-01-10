using Dialogue;
using Effects;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class DialogueButtonManager : SingletonBaseClass<DialogueButtonManager>
    {
        [Header("Auto Mode")]
        [SerializeField] private Image autoButtonImage;
        [SerializeField] private Sprite autoModeOn, autoModeOff;
        private bool isAuto;

        [Header("Show or Hide Dialogue")]
        [SerializeField] private CanvasGroup dialogueElementsCanvasGroup;
        [SerializeField] private GameObject settingsButton;

        private DialogueManager dialogueManager;

        private void Awake() {
            dialogueManager = DialogueManager.Instance;
        }

        private void Update() {
            // If there is left-click and current mode is hide mode, show the dialogue again
            if(Input.GetMouseButtonDown(0) && dialogueManager.CurrentDialogueMode == DialogueMode.HideMode){
                ShowDialogue();
            }
        }

        /// <summary>
        /// Hide the dialogue elements to show the portraits only
        /// </summary>
        public void HideDialogue(){
            StartCoroutine(FadingEffect.FadeOut(dialogueElementsCanvasGroup,
                beforeEffect: () => {
                    dialogueManager.PushDialogueMode(DialogueMode.HideMode);
                    settingsButton.SetActive(false);
                })
            );
        }
        
        /// <summary>
        /// Show the dialogue elements after hiding it
        /// </summary>
        private void ShowDialogue(){
            StartCoroutine(FadingEffect.FadeIn(dialogueElementsCanvasGroup,
                afterEffect: () => {
                    dialogueManager.PopDialogueMode(DialogueMode.HideMode);
                    settingsButton.SetActive(true);
                })
            );
        }

        /// <summary>
        /// Auto mode button handler
        /// </summary>
        public void AutoModeButton(){
            isAuto = !isAuto;
            AutoModeState(isAuto);
        }

        /// <summary>
        /// Auto mode state actions
        /// </summary>
        /// <param name="isAuto"></param>
        public void AutoModeState(bool isAuto){
            this.isAuto = isAuto;

            // If is in auto mode, ...
            if(isAuto){
                dialogueManager.canAutoModeContinue = true;
                dialogueManager.PushDialogueMode(DialogueMode.AutoTyping);
                // Change the sprite of auto button image
                autoButtonImage.sprite = autoModeOn;
            } else{
                dialogueManager.canAutoModeContinue = false;
                dialogueManager.PopDialogueMode(DialogueMode.AutoTyping);
                // Stop the coroutine to prevent multiple calls
                dialogueManager.StopAutoModeCoroutine();
                // Change the sprite of auto button image
                autoButtonImage.sprite = autoModeOff;
            }
        }
    }
}