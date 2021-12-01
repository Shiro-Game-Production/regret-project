using Actors;
using Dialogue;
using Event;
using Event.FinishConditionScripts;
using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction Button UI")]
        [SerializeField] private Button interactionButton;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform interactionButtonTransform;
        [SerializeField] private RectTransform interactionButtonParent;

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
            ItemData itemData = other.GetComponent<ItemData>();
            if (other.GetComponent<TriggerEnterCondition>() || 
                !itemData) return;

            playerInRange = true;
            HandleInteractionButton(itemData);

            // switch (other.tag)
            // {
            //     case NPC_TAG:
            //         playerInRange = true;
            //         HandleInteractionButton(other);
            //         break;
            //     case ITEM_TAG:
            //         playerInRange = true;
            //         HandleInteractionButton(other);
            //         break;
            // }
        }

        private void OnTriggerStay(Collider other)
        {
            // if collide with event data, ...
            EventData eventData = other.GetComponent<EventData>();
            if (!eventData) return;
            // Return if the item can't be interacted
            if (!eventData.canBeInteracted)
            {
                playerInRange = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            ItemData itemData = other.GetComponent<ItemData>();
            if (other.GetComponent<TriggerEnterCondition>() || 
                !itemData) return;
            
            playerInRange = false;
            
            // switch (other.tag)
            // {
            //     case NPC_TAG:
            //         playerInRange = false;
            //         break;
            //     case ITEM_TAG:
            //         playerInRange = false;
            //         break;
            // }
        }

        /// <summary>
        /// Handle interaction buton
        /// Set dialogue to dialogue manager in interaction button
        /// </summary>
        /// <param name="objectInteraction"></param>
        private void HandleInteractionButton(Collider objectInteraction)
        {
            // Get dialogue trigger
            ActorManager dialogueTrigger = objectInteraction.GetComponent<ActorManager>();
            
            
            // Set button actions 
            interactionButton.onClick.RemoveAllListeners();
            interactionButton.onClick.AddListener(() =>
            {
                dialogueManager.SetDialogue(dialogueTrigger.currentDialogue);
            });
        }

        private void HandleInteractionButton(ItemData itemData)
        {
            HandleInteractionButtonPosition(itemData);
            // Set button actions 
            interactionButton.onClick.RemoveAllListeners();
            interactionButton.onClick.AddListener(itemData.HandleInteraction);
        }

        private void HandleInteractionButtonPosition(ItemData itemData)
        {
            Vector2 actorScreenPosition = mainCamera.WorldToScreenPoint(itemData.transform.position);
            // Make it to the right a bit, so it doesn't cover the actor
            actorScreenPosition.x += 100;
            
            // Set anchored position for interaction button
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                interactionButtonParent, actorScreenPosition,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
                out var anchoredPosition);

            interactionButtonTransform.anchoredPosition = anchoredPosition;
        }
    }
}