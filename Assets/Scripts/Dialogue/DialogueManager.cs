using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Audios;
using Effects;
using Event;
using GameCamera;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class DialogueManager : SingletonBaseClass<DialogueManager>
    {
        [Header("Camera Manager")] 
        private CameraMovement cameraMovement;
        
        [Header("Parameters")]
        [SerializeField] private float typingSpeed = 0.04f;

        [Header("Dialogue UI")]
        [SerializeField] private CanvasGroup dialogueCanvasGroup;
        [SerializeField] private GameObject dialogueHolder; 
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Text speakerName;

        [Header("Dialogue Choices")]
        [SerializeField] private Transform choicesParent;
        [SerializeField] private ChoiceManager choicePrefab;
        private readonly List<ChoiceManager> choicePool = new List<ChoiceManager>();
        
        [Header("Dialogue Portrait")]
        [SerializeField] private Transform portraitsParent;
        [SerializeField] private PortraitManager portraitPrefab;
        private readonly List<PortraitManager> portraitPool = new List<PortraitManager>();

        [Header("Event Data")]
        [SerializeField] private TextAsset currentDialogueAsset;
        [SerializeField] private List<EventData> eventDatas;
        
        private Coroutine displayLineCoroutine;
        private Story currentStory;
        private EventManager eventManager;
        private bool canContinueToNextLine;
        private bool canSkipSentence;

        #region Setter and Getter
        
        public TextAsset CurrentDialogueAsset => currentDialogueAsset;
        public bool DialogueIsPlaying { get; private set; }
        
        #endregion

        private void Awake()
        {
            cameraMovement = CameraMovement.Instance;
            eventManager = EventManager.Instance;
            dialogueCanvasGroup.interactable = true;
            dialogueCanvasGroup.blocksRaycasts = false;
        }

        private void Start()
        {
            DialogueIsPlaying = false;
            dialogueHolder.SetActive(false);
        }

        private void Update()
        {
            // Return if dialogue isn't playing
            if (!DialogueIsPlaying) return;

            if (canContinueToNextLine &&
                currentStory.currentChoices.Count == 0 &&
                Input.GetMouseButtonDown(0))
            {
                ContinueStory();
            }
            
            // If right click and is typing, make player can skip dialogue sentence
            if (Input.GetMouseButtonDown(1) && !canContinueToNextLine)
                canSkipSentence = true;
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
                displayLineCoroutine = StartCoroutine(DisplaySentence(currentSentence));
                
                // Handle tags in story
                HandleTags(currentStory.currentTags);
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
                    HidePortraits();
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

            canContinueToNextLine = false;
            bool isAddingRichTextTag = false;

            foreach (char letter in sentence)
            {
                // If there is right mouse click, finish the sentence right away
                if (canSkipSentence)
                {
                    dialogueText.text = sentence;
                    canSkipSentence = false;
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
            canContinueToNextLine = true;
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
            if(canContinueToNextLine)
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
                    
                    case DialogueTags.EFFECT_TAG:
                        switch (tagValue)
                        {
                            case DialogueTags.SHAKE_TAG:
                                cameraMovement.ShakingEffect();
                                break;
                        }
                        break;
                    
                    case DialogueTags.EVENT_TAG:
                        SetEventData(tagValue);
                        break;
                    
                    case DialogueTags.PORTRAIT_TAG:
                        DisplayPortraits(tagValue);
                        break;
                    
                    case DialogueTags.SPEAKER_TAG:
                        speakerName.text = 
                            tagValue == DialogueTags.BLANK_VALUE ? "" : tagValue;
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
        
        /// <summary>
        /// Display portraits
        /// </summary>
        /// <param name="filenames"></param>
        private void DisplayPortraits(string filenames)
        {
            string[] files = filenames.Split(',');
            
            // If there are no portraits or none, hide portrait and return right away
            if (files.Length <= 0 || filenames == DialogueTags.BLANK_VALUE)
            {
                HidePortraits();
                return;
            }
            
            HidePortraits(); // Hide previous portrait

            foreach (string filename in files)
            {
                Sprite portrait = Resources.Load<Sprite>($"Portraits/{filename}");
                PortraitManager portraitManager = GetOrCreatePortraitManager();
                
                portraitManager.gameObject.SetActive(true);
                portraitManager.PortraitImage.sprite = portrait;
            }
        }
        
        /// <summary>
        /// Hide portraits if there are no portraits
        /// </summary>
        private void HidePortraits()
        {
            foreach (PortraitManager portrait in portraitPool)
            {
                portrait.gameObject.SetActive(false);
            }
        }
        
        /// <summary>
        /// Portrait manager object pooling
        /// </summary>
        /// <returns>Return existing portrait manager in hierarchy or create a new one</returns>
        private PortraitManager GetOrCreatePortraitManager()
        {
            PortraitManager portraitManager = portraitPool.Find(portrait => !portrait.gameObject.activeInHierarchy);

            if (portraitManager == null)
            {
                portraitManager = Instantiate(portraitPrefab, portraitsParent).GetComponent<PortraitManager>();
                // Add new choice manager to pool 
                portraitPool.Add(portraitManager);
            }
            
            portraitManager.gameObject.SetActive(false);

            return portraitManager;
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
        
        #endregion
    }
}
