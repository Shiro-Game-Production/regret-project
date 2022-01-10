using System.Collections;
using Effects;
using GameCamera;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Dialogue.Portrait;
using Dialogue.Choices;
using Dialogue.Logs;
using Dialogue.Tags;
using Cinemachine;
using UserInterface;
using System.Collections.Generic;

namespace Dialogue
{
    public class DialogueManager : SingletonBaseClass<DialogueManager>
    {
        [Header("Camera Manager")] 
        [SerializeField] private CinemachineVirtualCamera dialogueVcam;
        private CameraMovement cameraMovement;

        [Header("Parameters")]
        [SerializeField] private DialogueMode currentDialogueMode = DialogueMode.Normal;
        public DialogueState dialogueState = DialogueState.FinishTyping;
        [SerializeField] private TextAsset currentDialogueAsset;
        [SerializeField] private float typingSpeed = 0.04f;
        public bool canAutoModeContinue;
        [SerializeField] private float autoModeDelay = 3f;


        [Header("Dialogue UI")]
        [SerializeField] private CanvasGroup dialogueCanvasGroup;
        [SerializeField] private GameObject dialogueHolder;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Text speakerName;

        private DialogueChoiceManager dialogueChoiceManager;
        private DialogueLogManager dialogueLogManager;
        private DialoguePortraitManager dialoguePortraitManager;
        private DialogueTagManager dialogueTagManager;

        private Coroutine displayLineCoroutine;
        private Coroutine autoModeCoroutine;
        private Stack<DialogueMode> dialogueModeStack;
        private Story currentStory;

        #region Setter and Getter

        public DialogueMode CurrentDialogueMode => currentDialogueMode;
        public Story CurrentStory => currentStory;
        public Text SpeakerName => speakerName;
        public TextAsset CurrentDialogueAsset => currentDialogueAsset;
        public bool DialogueIsPlaying { get; private set; }

        #endregion

        private void Awake()
        {
            cameraMovement = CameraMovement.Instance;
            dialogueChoiceManager = DialogueChoiceManager.Instance;
            dialogueLogManager = DialogueLogManager.Instance;
            dialoguePortraitManager = DialoguePortraitManager.Instance;
            dialogueTagManager = DialogueTagManager.Instance;
            dialogueModeStack = new Stack<DialogueMode>();

            dialogueCanvasGroup.interactable = true;
            dialogueCanvasGroup.blocksRaycasts = false;
        }

        private void Start()
        {
            DialogueIsPlaying = false;
        }

        private void Update()
        {
            // Return if dialogue isn't playing
            if (!DialogueIsPlaying) return;

            /**
             * Use mouse button up to continue the story, handle dialogue log, and skip sentence
             * Handle dialogue log will trigger first if log button is clicked and will not trigger
                this if
             * Skip sentence will delay the finish typing after mouse button up, so it will not trigger
                this if
            */
            if (currentDialogueMode == DialogueMode.Normal &&
                dialogueState == DialogueState.FinishTyping &&
                currentStory.currentChoices.Count == 0 &&
                Input.GetMouseButtonUp(0))
            {
                ContinueStory();
            }
            
            // If mouse button down and is typing, make player can skip dialogue sentence
            if (Input.GetMouseButtonDown(0) &&
                dialogueState == DialogueState.Typing){
                dialogueState = DialogueState.SkipSentence;
            }

            if(canAutoModeContinue && currentDialogueMode == DialogueMode.AutoTyping){
                canAutoModeContinue = false;
                autoModeCoroutine = StartCoroutine(DialogueAutoMode());
            }
        }

        #region Dialogue
        
        public void UpdateDialogueMode(DialogueMode newDialogueMode){
            dialogueModeStack.Push(newDialogueMode);
            currentDialogueMode = dialogueModeStack.Peek();
            Debug.Log("Push: Stack: " + dialogueModeStack.Count);
        }

        public void UpdateToPreviousDialogueMode(){
            if(dialogueModeStack.Count > 1){
                dialogueModeStack.Pop();
                currentDialogueMode = dialogueModeStack.Peek();
                Debug.Log("Pop: Stack: " + dialogueModeStack.Count);
            }
        }

        /// <summary>
        /// Set dialogue by using dialogue inky file
        /// </summary>
        /// <param name="dialogueInk">Dialogue JSON file from Inky</param>
        public void SetDialogue(TextAsset dialogueInk)
        {
            currentDialogueAsset = dialogueInk;
            currentStory = new Story(dialogueInk.text);
            dialogueModeStack.Push(DialogueMode.Normal);

            StartCoroutine(FadingEffect.FadeIn(dialogueCanvasGroup,
                beforeEffect: () =>
                {
                    cameraMovement.SetVirtualCameraPriority(dialogueVcam,
                        cameraMovement.DIALOGUE_HIGHER_PRIORITY);
                    DialogueIsPlaying = true;
                    ContinueStory();
                })
            );
        }

        private IEnumerator DialogueAutoMode(){
            Debug.Log("1. Wait typing");
            yield return new WaitUntil(() => dialogueState == DialogueState.FinishTyping);
            Debug.Log("2. Delay");
            yield return new WaitForSeconds(autoModeDelay);
            Debug.Log("3. Wait auto typing mode");
            yield return new WaitUntil(() => currentDialogueMode == DialogueMode.AutoTyping);
            Debug.Log("4. Continue");
            canAutoModeContinue = true;
            ContinueStory();
        }

        public void StopAutoModeCoroutine(){
            if(autoModeCoroutine != null){
                StopCoroutine(autoModeCoroutine);
            }
        }
        
        /// <summary>
        /// Continue story dialogue
        /// </summary>
        public void ContinueStory()
        {
            if (currentStory.canContinue)
            {
                // Set text for the current dialogue line
                if(displayLineCoroutine != null)
                    StopCoroutine(displayLineCoroutine);
                
                // Show sentence by each character
                string currentSentence = currentStory.Continue();
                displayLineCoroutine = StartCoroutine(DisplaySentence(currentSentence));
                
                // Handle tags in story
                dialogueTagManager.HandleTags(currentStory.currentTags);
                // Add dialogue log
                dialogueLogManager.AddDialogueLog(speakerName.text, currentSentence);
            }
            else
            {
                FinishDialogue();
            }
        }

        /// <summary>
        /// Pause the story and play other thing
        /// </summary>
        public void PauseStory(){
            // Hide dialogue
            StartCoroutine(FadingEffect.FadeOut(dialogueCanvasGroup,
                blocksRaycasts: true,
                beforeEffect: () => UpdateDialogueMode(DialogueMode.Pause),
                afterEffect: () => DialogueIsPlaying = false)
            );
        }

        /// <summary>
        /// Resume the story after pausing the story
        /// </summary>
        public void ResumeStory(){
            // Show dialogue
            StartCoroutine(FadingEffect.FadeIn(dialogueCanvasGroup,
                beforeEffect: () => DialogueIsPlaying = true,
                afterEffect: () => UpdateToPreviousDialogueMode())
            );
        }

        /// <summary>
        /// Actions when dialogue is finished
        /// </summary>
        private void FinishDialogue()
        {
            StartCoroutine(FadingEffect.FadeOut(dialogueCanvasGroup,
                beforeEffect: () =>
                {
                    cameraMovement.SetVirtualCameraPriority(dialogueVcam,
                        cameraMovement.LOWER_PRIORITY);
                },
                afterEffect: () =>
                {
                    // Auto mode
                    dialogueModeStack.Clear();
                    canAutoModeContinue = false;
                    DialogueButtonManager.Instance.AutoModeState(false);
                    StopAutoModeCoroutine();

                    DialogueIsPlaying = false;
                    dialogueText.text = "";
                    speakerName.text = "";
                    dialogueChoiceManager.HideChoices();
                    dialoguePortraitManager.HidePortraits();
                })
            );
        }
        
        /// <summary>
        /// Display dialogue sentence letter by letter
        /// </summary>
        /// <param name="sentence">Current dialogue sentence</param>
        /// <returns></returns>
        private IEnumerator DisplaySentence(string sentence)
        {
            dialogueText.text = ""; // Empty the dialogue text
            // Hide items while typing
            dialogueChoiceManager.HideChoices();

            dialogueState = DialogueState.Typing;
            bool isAddingRichTextTag = false;

            foreach (char letter in sentence)
            {
                // If there is right mouse click, finish the sentence right away
                if (dialogueState == DialogueState.SkipSentence)
                {
                    dialogueText.text = sentence;
                    // Wait until skip mode finish
                    // Skip mode is trigger with mouse down
                    yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
                    // When mouse up, then change dialogue state to finish typing
                    dialogueState = DialogueState.FinishTyping;
                    break;
                }

                // If found rich text tag, add it without waiting
                if (letter == '<' || isAddingRichTextTag)
                {
                    isAddingRichTextTag = true;
                    dialogueText.text += letter;

                    if (letter == '>')
                        isAddingRichTextTag = false;
                } 
                // If not rich text, add letter and wait
                else
                {
                    // Type sentence by letter
                    dialogueText.text += letter;
                    yield return new WaitForSeconds(typingSpeed);
                }
            }

            dialogueChoiceManager.DisplayChoices();
            dialogueState = DialogueState.FinishTyping;
        }
        
        #endregion
    }
}
