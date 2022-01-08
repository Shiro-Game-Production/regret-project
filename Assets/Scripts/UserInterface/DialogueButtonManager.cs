using Dialogue;
using Effects;
using UnityEngine;

namespace UserInterface
{
    public class DialogueButtonManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup dialogueElementsCanvasGroup;
        [SerializeField] private GameObject settingsButton;

        private DialogueManager dialogueManager;

        private void Awake() {
            dialogueManager = DialogueManager.Instance;
        }

        private void Update() {
            if(Input.GetMouseButtonDown(0) && dialogueManager.dialogueMode == DialogueMode.HideMode){
                ShowDialogue();
            }
        }

        public void HideDialogue(){
            StartCoroutine(FadingEffect.FadeOut(dialogueElementsCanvasGroup,
                beforeEffect: () => {
                    dialogueManager.dialogueMode = DialogueMode.HideMode;
                    settingsButton.SetActive(false);
                })
            );
        }

        private void ShowDialogue(){
            StartCoroutine(FadingEffect.FadeIn(dialogueElementsCanvasGroup,
                afterEffect: () => {
                    dialogueManager.dialogueMode = DialogueMode.Normal;
                    settingsButton.SetActive(true);
                })
            );
        }
    }
}