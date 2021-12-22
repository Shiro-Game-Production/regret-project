﻿using System;
using Event.FinishConditionScripts;
using Items;
using UnityEngine;

namespace Event
{
    public class EventData : MonoBehaviour
    {
        [Header("Event Data")]
        [SerializeField] private string eventName;
        [SerializeField] private ItemData affectedItem;
        public bool isFinished;
        public EventState eventState = EventState.NotStarted;

        [Header("Dialogue Asset")]
        [SerializeField] private EventDialogue eventDialogue;
        
        [Header("Finish Condition")]
        public FinishCondition finishCondition;
        public bool canBeInteracted;
        [SerializeField] private bool keepObjectAfterFinish;
        [SerializeField] private FinishConditionManager triggerObject;
        
        // OnTriggerEnter finish condition
        private MeshRenderer eventMeshRenderer;
        private Collider eventCollider;
        
        #region Setter and Getter

        public bool KeepObjectAfterFinish => keepObjectAfterFinish;
        public string EventName => eventName;
        public ItemData AffectedItem => affectedItem;
        public ItemData ItemData { get; private set; }
        public EventDialogue EventDialogue => eventDialogue;
        public FinishConditionManager TriggerObject => triggerObject;
        
        #endregion
        
        private void Awake()
        {
            ItemData = GetComponent<ItemData>();
            
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
        public void OnEventFinish()
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