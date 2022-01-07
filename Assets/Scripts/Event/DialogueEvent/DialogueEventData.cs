using System;
using System.Collections.Generic;
using Event.BranchEvent;
using Event.FinishConditionScripts;
using Items;
using UnityEngine;

namespace Event.DialogueEvent{
    public class DialogueEventData : EventData {
        [Header("Dialogue Asset")]
        [SerializeField] private List<DialogueAffectedItem> dialogueAffectedItems;

        [Header("Branching")]
        [SerializeField] private bool useBranchEvent;
        [DrawIf("useBranchEvent", true)]
        [SerializeField] private BranchEventRunner branchRunner;

        // OnTriggerEnter finish condition
        private MeshRenderer eventMeshRenderer;
        private Collider eventCollider;

        #region Setter and Getter

        public bool UseBranchEvent => useBranchEvent;
        public BranchEventRunner BranchRunner => branchRunner;
        public ItemData ItemData { get; private set; }
        public List<DialogueAffectedItem> DialogueAffectedItems => dialogueAffectedItems;

        #endregion

        private void Awake()
        {
            ItemData = GetComponent<ItemData>();
            if(!ItemData){
                ItemData = GetComponentInParent<ItemData>();
            }
            
            switch (finishCondition)
            {
                case FinishCondition.OnTriggerEnter:
                    eventCollider = GetComponent<Collider>();
                    eventMeshRenderer = GetComponent<MeshRenderer>();
                    break;
                case FinishCondition.PuzzleFinished:
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