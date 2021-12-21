using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Audios;
using Effects;
using Event;
using GameCamera;
using Ink.Runtime;
using SceneLoading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Dialogue.Portrait;

namespace Dialogue
{
    public class DialogueManager : SingletonBaseClass<DialogueManager>
    {
        [Header("Camera Manager")] 
        private CameraMovement cameraMovement;
        private CameraShake cameraShake;

        [Header("Parameters")]
        public DialogueMode dialogueMode = DialogueMode.Normal;
        public DialogueState dialogueState = DialogueState.FinishTyping;
        [SerializeField] private float typingSpeed = 0.04f;

        [Header("Dialogue UI")]
        [SerializeField] private CanvasGroup dialogueCanvasGroup;
        [SerializeField] private GameObject dialogueHolder;
        [SerializeField] private GameObject dialogueTextBox;
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Text speakerName;

        [Header("Dialogue Choices")]
        [SerializeField] private Transform choicesParent;
        [SerializeField] private ChoiceManager choicePrefab;
        private readonly List<ChoiceManager> choicePool = new List<ChoiceManager>();

        [Header("Dialogue Log")]
        [SerializeField] private CanvasGroup dialogueLogCanvasGroup;
        [SerializeField] private DialogueLogManager dialogueLogPrefab;
        [SerializeField] private Transform dialogueLogParent;
        private string dialogueTextValue, speakerNameValue;

        [Header("Dialogue Portrait")]
        private DialoguePortraitManager dialoguePortraitManager;

        [Header("Event Data")]
        [SerializeField] private TextAsset currentDialogueAsset;
        [SerializeField] private List<EventData> eventDatas;

        [Header("Ending")] 
        [SerializeField] private Image blackScreen;

        private Coroutine displayLineCoroutine;
        private Story currentStory;
        private EventManager eventManager;

        #region Setter and Getter

        public TextAsset CurrentDialogueAsset => currentDialogueAsset;
        public bool DialogueIsPlaying { get; private set; }

        #endregion

        private void Awake()
        {
            cameraMovement = CameraMovement.Instance;
            cameraShake = CameraShake.Instance;
            dialoguePortraitManager = DialoguePortraitManager.Instance;
            eventManager = EventManager.Instance;
            dialogueCanvasGroup.interactable = true;
            dialogueCanvasGroup.blocksRaycasts = false;
            dialogueHolder.SetActive(false);
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
            if (dialogueMode == DialogueMode.Normal &&
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
        }

        #region Dialogue
        
        /// <summary>
        /// Set dialogue by using dialogue inky file
        /// </summary>
        /// <param name="dialogueInk">Dialogue JSON file from Inky</param>
        public void SetDialogue(TextAsset dialogueInk)
        {
            currentDialogueAsset = dialogueInk;
            currentStory = new Story(dialogueInk.text);
            
            StartCoroutine(FadingEffect.FadeIn(dialogueCanvasGroup,
                beforeEffect: () =>
                {
                    dialogueHolder.SetActive(true);
                    cameraMovement.SetCameraToDialogueMode();
                    DialogueIsPlaying = true;
                    ContinueStory();
                })
            );
        }
        
        /// <summary>
        /// Continue story dialogue
        /// </summary>
        private void ContinueStory()
        {
            if (currentStory.canContinue)
            {
                // Set text for the current dialogue line
                if(displayLineCoroutine != null)
                    StopCoroutine(displayLineCoroutine);
                
                // Show sentence by each character
                string currentSentence = currentStory.Continue();
                dialogueTextValue = currentSentence;
                displayLineCoroutine = StartCoroutine(DisplaySentence(currentSentence));
                
                // Handle tags in story
                HandleTags(currentStory.currentTags);
                // Add dialogue log
                AddDialogueLog();
            }
            else
            {
                FinishDialogue();
            }
        }

        /// <summary>
        /// Actions when dialogue is finished
        /// </summary>
        private void FinishDialogue()
        {
            StartCoroutine(FadingEffect.FadeOut(dialogueCanvasGroup,
                beforeEffect: () =>
                {
                    cameraMovement.SetCameraToTopDownMode();
                },
                afterEffect: () =>
                {
                    dialogueHolder.SetActive(false);
                    DialogueIsPlaying = false;
                    dialogueText.text = "";
                    speakerName.text = "";
                    HideChoices();
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
            HideChoices();

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

            DisplayChoices();
            dialogueState = DialogueState.FinishTyping;
        }
        
        #endregion

        #region Dialogue Log

        /// <summary>
        /// Add dialogue log
        /// </summary>
        private void AddDialogueLog(){
            // BUG: RESET DIALOGUE LOG
            DialogueLogManager dialogueLogManager = Instantiate(dialogueLogPrefab, dialogueLogParent);
            dialogueLogManager.SetDialogueLog(speakerNameValue, dialogueTextValue);
        }

        public void ShowLog(bool showLog){
            if(showLog){
                StartCoroutine(FadingEffect.FadeIn(dialogueLogCanvasGroup,
                    beforeEffect: () => dialogueMode = DialogueMode.ViewLog));
            } else {
                StartCoroutine(FadingEffect.FadeOut(dialogueLogCanvasGroup,
                    afterEffect: () => dialogueMode = DialogueMode.Normal));
            }
        }

        #endregion

        #region Choices
        
        /// <summary>
        /// Hide choices
        /// </summary>
        private void HideChoices()
        {
            foreach (ChoiceManager choiceManager in choicePool)
            {
                choiceManager.gameObject.SetActive(false);
            }
        }
        
        /// <summary>
        /// Display choices in the dialogue
        /// </summary>
        private void DisplayChoices()
        {
            List<Choice> currentChoices = currentStory.currentChoices;

            if (currentChoices.Count == 0) return;
            
            foreach (Choice choice in currentChoices)
            {
                ChoiceManager choiceManager = GetOrCreateChoiceManager();
                choiceManager.gameObject.SetActive(true);
                // Set choice text
                choiceManager.ChoiceText.text = choice.text;
                choiceManager.choiceIndex = choice.index;
            }
        }
        
        /// <summary>
        /// Choice manager object pooling
        /// </summary>
        /// <returns>Return existing choice manager in hierarchy or create a new one</returns>
        private ChoiceManager GetOrCreateChoiceManager()
        {
            ChoiceManager choiceManager = choicePool.Find(choice => !choice.gameObject.activeInHierarchy);

            if (choiceManager == null)
            {
                choiceManager = Instantiate(choicePrefab, choicesParent).GetComponent<ChoiceManager>();
                // Add new choice manager to pool 
                choicePool.Add(choiceManager);
            }
            
            choiceManager.gameObject.SetActive(false);

            return choiceManager;
        }
        
        /// <summary>
        /// Decide from multiple choice
        /// </summary>
        /// <param name="index">Choice's index</param>
        public void Decide(int index)
        {
            if(dialogueState == DialogueState.FinishTyping )
            {
                currentStory.ChooseChoiceIndex(index);
                ContinueStory();
            }
        }
        
        #endregion

        #region Tags
        
        /// <summary>
        /// Handle tags in dialogue
        /// </summary>
        /// <param name="dialogueTags"></param>
        private void HandleTags(List<string> dialogueTags)
        {
            foreach (string dialogueTag in dialogueTags)
            {
                // Parse the tag
                string[] splitTag = dialogueTag.Split(':');

                if (splitTag.Length != 2)
                {
                    Debug.LogError("Tag could not be parsed: " + tag);
                }

                string tagKey = splitTag[0].Trim();
                string tagValue = splitTag[1].Trim();
                
                // Handle tag
                switch (tagKey)
                {
                    case DialogueTags.AUDIO_TAG:
                        HandleAudio(tagValue);
                        break;
                    
                    case DialogueTags.DIALOGUE_BOX_TAG:
                        ShowOrHideDialogueBox(tagValue);
                        break;

                    case DialogueTags.EFFECT_TAG:
                        switch (tagValue)
                        {
                            case DialogueTags.SHAKE_TAG:
                                StartCoroutine(cameraShake.ShakingEffect());
                                break;
                        }
                        break;
                    
                    case DialogueTags.ENDING_TAG:
                        HandleEndingTag(tagValue);
                        break;
                    
                    case DialogueTags.EVENT_TAG:
                        SetEventData(tagValue);
                        break;
                    
                    case DialogueTags.PORTRAIT_TAG:
                        dialoguePortraitManager.DisplayPortraits(tagValue);
                        break;
                    
                    case DialogueTags.SPEAKER_TAG:
                        speakerNameValue = tagValue == DialogueTags.BLANK_VALUE ? "" : tagValue;
                        speakerName.text = speakerNameValue;
                        break;
                    
                    default:
                        Debug.LogError("Tag is not in the list: " + tag);
                        break;
                }
            }
        }

        #region Audio
        
        private void HandleAudio(string audio)
        {
            AudioManager.Instance.Play(audio);
        }
        
        #endregion

        #region Portraits
        

        
        #endregion

        #region Dialogue Box

        /// <summary>
        /// Show or hide dialogue box depends on tag value
        /// </summary>
        /// <param name="tagValue"></param>
        private void ShowOrHideDialogueBox(string tagValue){
            switch(tagValue){
                case DialogueTags.BLANK_VALUE:
                    dialogueTextBox.SetActive(false);
                    break;
                
                case DialogueTags.SHOW_DIALOGUE_BOX:
                    dialogueTextBox.SetActive(true);
                    break;

                default:
                    Debug.LogError($"Tag: {tagValue} is not registered to handle dialogue box");
                    break;
            }
        }

        #endregion

        #region Event
        
        /// <summary>
        /// Find event data in list
        /// </summary>
        /// <param name="eventDataName">Event data name</param>
        private void SetEventData(string eventDataName)
        {
            // Find event data in list
            foreach (EventData eventData in eventDatas.Where(
                eventData => eventData.EventName == eventDataName))
            {
                eventManager.SetEventData(eventData);  
                eventData.gameObject.SetActive(true);
                break;
            }
        }
        
        #endregion

        #region Ending
        
        /// <summary>
        /// Handle ending tag
        /// </summary>
        /// <param name="tagValue"></param>
        private void HandleEndingTag(string tagValue)
        {
            if (tagValue != DialogueTags.CONFIRM_ENDING) return;
            
            StartCoroutine(FadingEffect.FadeIn(blackScreen,
                fadingSpeed: 0.02f,
                afterEffect: () => SceneLoadTrigger.Instance.LoadScene("HomeScene"))
            );
        }

        #endregion

        #endregion
    }
}
