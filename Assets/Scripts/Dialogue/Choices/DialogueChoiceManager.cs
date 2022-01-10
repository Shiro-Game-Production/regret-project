using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

namespace Dialogue.Choices{
    public class DialogueChoiceManager : SingletonBaseClass<DialogueChoiceManager> {
        [SerializeField] private Transform choicesParent;
        [SerializeField] private DialogueChoice choicePrefab;
        [SerializeField] private bool choiceMode;

        private List<DialogueChoice> choicePool;
        private DialogueManager dialogueManager;

        public bool ChoiceMode => choiceMode;

        private void Awake() {
            choicePool = new List<DialogueChoice>();
            dialogueManager = DialogueManager.Instance;
        }

        /// <summary>
        /// Hide choices
        /// </summary>
        public void HideChoices()
        {
            foreach (DialogueChoice choiceManager in choicePool)
            {
                choiceManager.gameObject.SetActive(false);
            }
        }
        
        /// <summary>
        /// Display choices in the dialogue
        /// </summary>
        public void DisplayChoices()
        {
            List<Choice> currentChoices = dialogueManager.CurrentStory.currentChoices;

            if (currentChoices.Count == 0) return;

            choiceMode = true;
            dialogueManager.PushDialogueMode(DialogueMode.Pause);
            foreach (Choice choice in currentChoices)
            {
                DialogueChoice choiceManager = GetOrCreateChoiceManager();
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
        private DialogueChoice GetOrCreateChoiceManager()
        {
            DialogueChoice choiceManager = choicePool.Find(choice => !choice.gameObject.activeInHierarchy);

            if (choiceManager == null)
            {
                choiceManager = Instantiate(choicePrefab, choicesParent).GetComponent<DialogueChoice>();
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
            if(dialogueManager.dialogueState == DialogueState.FinishTyping)
            {
                choiceMode = false;
                dialogueManager.CurrentStory.ChooseChoiceIndex(index);
                
                dialogueManager.ContinueStory();
                dialogueManager.PopDialogueMode(DialogueMode.Pause);
            }
        }
    }
}