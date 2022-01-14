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
using Player;
using System.Collections.Generic;
using Dialogue.Illustration;

namespace Dialogue
{
    public class DialogueManager : SingletonBaseClass<DialogueManager>
    {
        [Header("Camera Manager")] 
        [SerializeField] private CinemachineVirtualCamera dialogueVcam;
        private CameraMovement cameraMovement;

        [Header("Parameters")]
        [SerializeField] private DialogueState dialogueState = DialogueState.Stop;
        [SerializeField] private DialogueMode currentDialogueMode = DialogueMode.Normal;
        public DialogueTypingState dialogueTypingState = DialogueTypingState.FinishTyping;
        [SerializeField] private TextAsset currentDialogueAsset;
        [SerializeField] private float typingSpeed = 0.04f;
        public bool canAutoModeContinue;
        [SerializeField] private float autoModeDelay = 3f;


        [Header("Dialogue UI")]
        [SerializeField] private CanvasGroup dialogueCanvasGroup;
        [SerializeField] private GameObject dialogueHolder;
        [SerializeField] private GameObject dialogueTextBox;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Text speakerName;

        private DialogueChoiceManager dialogueChoiceManager;
        private DialogueIllustrationManager dialogueIllustrationManager;
        private DialogueLogManager dialogueLogManager;
        private DialoguePortraitManager dialoguePortraitManager;
        private DialogueTagManager dialogueTagManager;
        private PlayerMovement playerMovement;

        private Coroutine displayLineCoroutine;
        private Coroutine autoModeCoroutine;
        [SerializeField] private List<DialogueMode> dialogueModeStackList;
        private Story currentStory;

        #region Setter and Getter

        public DialogueState CurrentDialogueState => dialogueState;
        public DialogueMode CurrentDialogueMode => currentDialogueMode;
        public List<DialogueMode> DialogueModeStackList => dialogueModeStackList;
        public Story CurrentStory => currentStory;
        public Text SpeakerName => speakerName;
        public TextAsset CurrentDialogueAsset => currentDialogueAsset;
        public bool DialogueIsPlaying { get; private set; }

        #endregion

        private void Awake()
        {
            cameraMovement = CameraMovement.Instance;
            dialogueChoiceManager = DialogueChoiceManager.Instance;
            dialogueIllustrationManager = DialogueIllustrationManager.Instance;
            dialogueLogManager = DialogueLogManager.Instance;
            dialoguePortraitManager = DialoguePortraitManager.Instance;
            dialogueTagManager = DialogueTagManager.Instance;
            playerMovement = PlayerMovement.Instance;
            dialogueModeStackList = new List<DialogueMode>();

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
                dialogueTypingState == DialogueTypingState.FinishTyping &&
                currentStory.currentChoices.Count == 0 &&
                Input.GetMouseButtonUp(0))
            {
                ContinueStory();
            }
            
            // If mouse button down and is typing, make player can skip dialogue sentence
            if (Input.GetMouseButtonDown(0) &&
                currentDialogueMode == DialogueMode.Normal &&
                dialogueTypingState == DialogueTypingState.Typing){
                dialogueTypingState = DialogueTypingState.SkipSentence;
            }

            // If auto mode and can continue the auto mode, Start the auto mode
            if(canAutoModeContinue && currentDialogueMode == DialogueMode.AutoTyping){
                canAutoModeContinue = false;
                autoModeCoroutine = StartCoroutine(DialogueAutoMode());
            }
        }

        #region Dialogue
        
        /// <summary>
        /// Push dialogue mode to stack list
        /// </summary>
        /// <param name="pushElement"></param>
        public void PushDialogueMode(DialogueMode pushElement){
            // Special case
            // If push element is auto typing and there are choices, ...
            if(pushElement == DialogueMode.AutoTyping && dialogueChoiceManager.ChoiceMode){
                // Remove pause mode first
                int choiceIndex = dialogueModeStackList.IndexOf(DialogueMode.Pause);
                dialogueModeStackList.RemoveAt(choiceIndex);
                // Add auto first, then pause
                dialogueModeStackList.Add(DialogueMode.AutoTyping);
                dialogueModeStackList.Add(DialogueMode.Pause);
            } else{
                // Normal push
                dialogueModeStackList.Add(pushElement);
            }
            // Get latest element in stack list
            currentDialogueMode = dialogueModeStackList[dialogueModeStackList.Count - 1];
        }

        /// <summary>
        /// Pop dialogue mode from stack list
        /// </summary>
        /// <param name="popElement"></param>
        public void PopDialogueMode(DialogueMode popElement){
            // Check if the elements in stack list is greater than 1, ...
            if(dialogueModeStackList.Count > 1){
                dialogueModeStackList.Remove(popElement);
                // Get latest element
                currentDialogueMode = dialogueModeStackList[dialogueModeStackList.Count - 1];
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
            dialogueState = DialogueState.Running;
            PushDialogueMode(DialogueMode.Normal);

            StartCoroutine(FadingEffect.FadeIn(dialogueCanvasGroup,
                beforeEffect: () =>
                {
                    playerMovement.Movement.ChangeNavMeshQuality(
                        UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance);
                    cameraMovement.SetVirtualCameraPriority(dialogueVcam,
                        cameraMovement.DIALOGUE_HIGHER_PRIORITY);
                    
                    ContinueStory();
                }, 
                afterEffect: () => DialogueIsPlaying = true)
            );
        }

        /// <summary>
        /// Dialogue auto mode
        /// 1. Wait until finish typing
        /// 2. Delay in auto mode
        /// 3. Wait until current mode is auto typing
        /// 4. Continue the line
        /// </summary>
        /// <returns></returns>
        private IEnumerator DialogueAutoMode(){
            // Wait typing
            yield return new WaitUntil(() => dialogueTypingState == DialogueTypingState.FinishTyping);
            // Delay
            yield return new WaitForSeconds(autoModeDelay);
            // Wait auto typing mode
            yield return new WaitUntil(() => currentDialogueMode == DialogueMode.AutoTyping);
            // Continue
            canAutoModeContinue = true;
            ContinueStory();
        }

        /// <summary>
        /// Stop auto mode coroutine to prevent multiple calls
        /// </summary>
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
        public void PauseStoryForEvent(){
            // Hide dialogue
            StartCoroutine(FadingEffect.FadeOut(dialogueCanvasGroup,
                blocksRaycasts: true,
                fadingSpeed: 1f,
                beforeEffect: () => PushDialogueMode(DialogueMode.Pause),
                afterEffect: () => DialogueIsPlaying = false)
            );
        }

        /// <summary>
        /// Resume the story after pausing the story
        /// </summary>
        public void ResumeStoryForEvent(){
            // Show dialogue
            StartCoroutine(FadingEffect.FadeIn(dialogueCanvasGroup,
                beforeEffect: () => DialogueIsPlaying = true,
                afterEffect: () => PopDialogueMode(DialogueMode.Pause))
            );
        }

        /// <summary>
        /// Actions when dialogue is finished
        /// </summary>
        private void FinishDialogue()
        {
            dialogueState = DialogueState.Stop;
            StartCoroutine(FadingEffect.FadeOut(dialogueCanvasGroup,
                beforeEffect: () =>
                {
                    cameraMovement.SetVirtualCameraPriority(dialogueVcam,
                        cameraMovement.LOWER_PRIORITY);
                },
                afterEffect: () =>
                {
                    // Auto mode
                    dialogueModeStackList.RemoveRange(0, dialogueModeStackList.Count);
                    canAutoModeContinue = false;
                    DialogueButtonManager.Instance.AutoModeState(false);
                    StopAutoModeCoroutine();

                    // Player
                    playerMovement.Movement.ChangeNavMeshQuality(
                        UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance);

                    // Dialogue UI
                    dialogueTextBox.SetActive(true);
                    DialogueIsPlaying = false;
                    dialogueText.text = "";
                    speakerName.text = "";
                    dialogueChoiceManager.HideChoices();
                    dialogueIllustrationManager.HideIllustrtations();
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

            dialogueTypingState = DialogueTypingState.Typing;
            bool isAddingRichTextTag = false;

            foreach (char letter in sentence)
            {
                // If there is right mouse click, finish the sentence right away
                if (dialogueTypingState == DialogueTypingState.SkipSentence)
                {
                    dialogueText.text = sentence;
                    // Wait until skip mode finish
                    // Skip mode is trigger with mouse down
                    yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
                    // When mouse up, then change dialogue state to finish typing
                    dialogueTypingState = DialogueTypingState.FinishTyping;
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
            dialogueTypingState = DialogueTypingState.FinishTyping;
        }

        /// <summary>
        /// Pause dialogue when opening setting
        /// </summary>
        public void PauseMode(CanvasGroup canvasGroup)
        {
            //Fade in
            ButtonClickManager.Instance.ShowPrompts(canvasGroup, () =>
            {
                PushDialogueMode(DialogueMode.Pause);
            });
        }

        /// <summary>
        /// Resume dialogue when closing setting
        /// </summary>
        public void ResumeMode(CanvasGroup canvasGroup)
        {
            //Fade out
            ButtonClickManager.Instance.HidePrompts(canvasGroup, () => 
            {
                //continue
                PopDialogueMode(DialogueMode.Pause);
            });
            
        }
        
        #endregion
    }
}
