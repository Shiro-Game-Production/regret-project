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
            if(Input.GetMouseButtonDown(0) && dialogueManager.CurrentDialogueMode == DialogueMode.HideMode){
                ShowDialogue();
            }
        }

        public void HideDialogue(){
            StartCoroutine(FadingEffect.FadeOut(dialogueElementsCanvasGroup,
                beforeEffect: () => {
                    dialogueManager.UpdateDialogueMode(DialogueMode.HideMode);
                    settingsButton.SetActive(false);
                })
            );
        }

        private void ShowDialogue(){
            StartCoroutine(FadingEffect.FadeIn(dialogueElementsCanvasGroup,
                afterEffect: () => {
                    dialogueManager.UpdateToPreviousDialogueMode();
                    settingsButton.SetActive(true);
                })
            );
        }

        public void AutoModeButton(){
            isAuto = !isAuto;
            AutoModeState(isAuto);
        }

        public void AutoModeState(bool isAuto){
            this.isAuto = isAuto;

            if(isAuto){
                dialogueManager.canAutoModeContinue = true;
                dialogueManager.UpdateDialogueMode(DialogueMode.AutoTyping);
                autoButtonImage.sprite = autoModeOn;
            } else{
                dialogueManager.canAutoModeContinue = false;
                dialogueManager.UpdateDialogueMode(DialogueMode.Normal);
                StopCoroutine(dialogueManager.AutoModeCoroutine);
                autoButtonImage.sprite = autoModeOff;
            }
        }
    }
}