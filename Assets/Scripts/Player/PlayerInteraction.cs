﻿using Dialogue;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private Button interactionButton;
        [SerializeField] private Text interactionButtonText;

        private bool playerInRange;
        private DialogueManager dialogueManager;
        
        private const string ITEM_TAG = "Item";
        private const string NPC_TAG = "NPC";

        private void Awake()
        {
            dialogueManager = DialogueManager.Instance;
            playerInRange = false;
        }

        private void Update()
        {
            if (playerInRange && !dialogueManager.DialogueIsPlaying)
            {
                interactionButton.gameObject.SetActive(true);
            }
            else
            {
                interactionButton.gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (other.tag)
            {
                case NPC_TAG:
                    playerInRange = true;
                    HandleInteractionButton(other,"Talk");
                    break;
                case ITEM_TAG:
                    playerInRange = true;
                    HandleInteractionButton(other,"Interact");
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            switch (other.tag)
            {
                case NPC_TAG:
                    playerInRange = false;
                    break;
                case ITEM_TAG:
                    playerInRange = false;
                    break;
            }
        }

        /// <summary>
        /// Handle interaction buton
        /// Set dialogue to dialogue manager in interaction button
        /// </summary>
        /// <param name="objectInteraction"></param>
        /// <param name="buttonText"></param>
        private void HandleInteractionButton(Collider objectInteraction, string buttonText)
        {
            // Get dialogue trigger
            DialogueTrigger dialogueTrigger = objectInteraction.GetComponent<DialogueTrigger>();
            
            // Set button text
            interactionButtonText.text = buttonText;
            // Set button actions 
            interactionButton.onClick.RemoveAllListeners();
            interactionButton.onClick.AddListener(() =>
            {
                dialogueManager.SetDialogue(dialogueTrigger.DialogueJson);
            });
        }
    }
}