using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class DialogueManager : SingletonBaseClass<DialogueManager>
    {
        [Header("Parameters")]
        [SerializeField] private float typingSpeed = 0.04f;

        [Header("Dialogue UI")]
        [SerializeField] private GameObject dialogueHolder; 
        [SerializeField] private Text dialogueText;
        [SerializeField] private Text speakerName;
        [SerializeField] private Transform choicesParent;
        [SerializeField] private Transform portraitsParent;

        [Header("Dialogue Choices")]
        [SerializeField] private ChoiceManager choicePrefab;
        private readonly List<ChoiceManager> choicePool = new List<ChoiceManager>();

        private Story currentStory;
        public bool DialogueIsPlaying { get; private set; }

        private bool canContinueToNextLine = false;

        private Coroutine displayLineCoroutine;

        private const string SPEAKER_TAG = "speaker";
        private const string PORTRAIT_TAG = "portrait";

        private void Start()
        {
            DialogueIsPlaying = false;
            dialogueHolder.SetActive(false);
            
            SetDialogue(Resources.Load<TextAsset>("Dialogue/chapter1"));
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
        }
        
        /// <summary>
        /// Set dialogue by using dialogue inky file
        /// </summary>
        /// <param name="dialogueInk">Dialogue JSON file from Inky</param>
        public void SetDialogue(TextAsset dialogueInk)
        {
            currentStory = new Story(dialogueInk.text);
            DialogueIsPlaying = true;
            dialogueHolder.SetActive(true);

            ContinueStory();
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
                StartCoroutine(FinishDialogue());
            }
        }

        /// <summary>
        /// Actions when dialogue is finished
        /// </summary>
        /// <returns>Wait for 0.2 seconds</returns>
        private IEnumerator FinishDialogue()
        {
            yield return new WaitForSeconds(0.2f);
            
            DialogueIsPlaying = false;
            dialogueHolder.SetActive(false);
            dialogueText.text = "";
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

            foreach (char letter in sentence)
            {
                // If there is right mouse click, finish the sentence right away
                if (Input.GetMouseButtonDown(1))
                {
                    dialogueText.text = sentence;
                    break;
                }

                // Type sentence by letter
                dialogueText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            DisplayChoices();
            canContinueToNextLine = true;
        }
        
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

                switch (tagKey)
                {
                    case SPEAKER_TAG:
                        speakerName.text = tagValue;
                        break;
                    case PORTRAIT_TAG:
                        Debug.Log(tagValue);
                        break;
                    default:
                        Debug.LogError("Tag is not in the list: " + tag);
                        break;
                }
            }
        }
    }
}
