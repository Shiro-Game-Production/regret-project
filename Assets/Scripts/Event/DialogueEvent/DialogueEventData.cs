using System;
using Event.FinishConditionScripts;
using Items;
using UnityEngine;

namespace Event.DialogueEvent{
    public class DialogueEventData : EventData {
        [Header("Dialogue Asset")]
        [SerializeField] private ItemData affectedItem;
        [SerializeField] private EventDialogue eventDialogue;
        
        // OnTriggerEnter finish condition
        private MeshRenderer eventMeshRenderer;
        private Collider eventCollider;

        #region Setter and Getter

        public ItemData AffectedItem => affectedItem;
        public ItemData ItemData { get; private set; }
        public EventDialogue EventDialogue => eventDialogue;
        
        #endregion

        private void Awake()
        {
            ItemData = GetComponentInParent<ItemData>();
            
            switch (finishCondition)
            {
                case FinishCondition.OnTriggerEnter:
                    eventCollider = GetComponent<Collider>();
                    eventMeshRenderer = GetComponent<MeshRenderer>();
                    break;
                case FinishCondition.PuzzleFinished:
                    break;
                case FinishCondition.DialogueFinished:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Actions on event finish depends on finish condition
        /// </summary>
        public override void OnEventFinish()
        {
            switch (finishCondition)
            {
                case FinishCondition.OnTriggerEnter:
                    eventCollider.enabled = false;
                    eventMeshRenderer.enabled = false;
                    break;
                case FinishCondition.PuzzleFinished:
                    break;
                case FinishCondition.DialogueFinished:
                    canBeInteracted = false;
                    ItemData.itemMode = ItemData.ItemMode.NormalMode;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}