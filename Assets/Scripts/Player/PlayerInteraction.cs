using Dialogue;
using Event;
using Event.FinishConditionScripts;
using Items;
using Items.Door;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerInteraction : SingletonBaseClass<PlayerInteraction>
    {
        [Header("Interaction Button UI")]
        [SerializeField] private Button interactionButton;
        [SerializeField] private Text interactionBtnText;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform interactionButtonTransform;
        [SerializeField] private RectTransform interactionButtonParent;

        private bool playerInRange;
        private bool hasInteracted;
        private DialogueManager dialogueManager;

        private void Awake()
        {
            dialogueManager = DialogueManager.Instance;
            playerInRange = false;
        }

        private void Update()
        {
            if (playerInRange && !dialogueManager.DialogueIsPlaying && !hasInteracted)
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
            if (other.GetComponent<TriggerEnterCondition>()) return;

            ItemData itemData = other.GetComponent<ItemData>();
            if(itemData)
            {
                hasInteracted = false;
                playerInRange = true;
                HandleInteractionButton(itemData);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            // if collide with event data, ...
            EventData eventData = other.GetComponent<EventData>();
            ItemData itemData = other.GetComponent<ItemData>();
            if (!eventData || !itemData) return;
            
            // Special case for door only
            DoorManager doorManager = other.GetComponent<DoorManager>();
            // If door manager is animating, set hasInteracted to true
            // Wait until door manager isn't animating
            if(doorManager){
                if(doorManager.IsAnimating){
                    hasInteracted = true;
                    return;
                }
            }

            // If event data can't be interacted (not finished yet) and item mode is dialogue mode, ...
            if (!eventData.canBeInteracted && itemData.itemMode == ItemData.ItemMode.DialogueMode ||
                !eventData && !itemData)
            {
                playerInRange = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<TriggerEnterCondition>()) return;
            
            ItemData itemData = other.GetComponent<ItemData>();
            if(itemData)
            {
                hasInteracted = true;
                playerInRange = false;
            }
        }
        
        /// <summary>
        /// Handle interaction button for item
        /// </summary>
        /// <param name="itemData"></param>
        private void HandleInteractionButton(ItemData itemData)
        {
            HandleInteractionButtonPosition(itemData.transform.position);
            interactionBtnText.text = itemData.interactionText.ToString();
            // Set button actions 
            interactionButton.onClick.RemoveAllListeners();
            interactionButton.onClick.AddListener(() => {
                hasInteracted = true;
                itemData.HandleInteraction();
            });
        }
        
        /// <summary>
        /// Handle interaction button position
        /// Convert world space position to canvas position 
        /// </summary>
        /// <param name="targetPosition">World space position</param>
        private void HandleInteractionButtonPosition(Vector3 targetPosition)
        {
            Vector2 actorScreenPosition = mainCamera.WorldToScreenPoint(targetPosition);
            // Make it to the right a bit, so it doesn't cover the actor
            actorScreenPosition.x += 50;
            
            // Set anchored position for interaction button
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                interactionButtonParent, actorScreenPosition,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
                out var anchoredPosition);

            interactionButtonTransform.anchoredPosition = anchoredPosition;
        }
    }
}