﻿using System;
using Actors;
using Event.FinishConditionScripts;
using UnityEngine;

namespace Event
{
    public class EventData : MonoBehaviour
    {
        [Header("Event Data")]
        [SerializeField] private string eventName;
        [SerializeField] private ActorManager affectedActor;
        public bool isFinished;
        public EventState eventState = EventState.NotStarted;

        [Header("Dialogue Asset")]
        [SerializeField] private TextAsset waitDialogueAsset;
        [SerializeField] private TextAsset finishDialogueAsset;
        [SerializeField] private TextAsset nextEventDialogueAsset;
        [SerializeField] private TextAsset defaultDialogueAsset;
        
        [Header("Finish Condition")]
        public FinishCondition finishCondition;
        [SerializeField] private FinishConditionManager triggerObject;
        
        // OnTriggerEnter finish condition
        [DrawIf("finishCondition", FinishCondition.OnTriggerEnter)]
        private MeshRenderer eventMeshRenderer;
        [DrawIf("finishCondition", FinishCondition.OnTriggerEnter)]
        private Collider eventCollider;
        
        // Dialogue finished condition
        
        #region Setter and Getter
        
        public string EventName => eventName;
        public ActorManager AffectedActor => affectedActor;
        public TextAsset WaitDialogueAsset => waitDialogueAsset;
        public TextAsset NextEventDialogueAsset => nextEventDialogueAsset;
        public TextAsset DefaultDialogueAsset => defaultDialogueAsset;
        public TextAsset FinishDialogueAsset => finishDialogueAsset;
        public FinishConditionManager TriggerObject => triggerObject;
        
        #endregion
        
        private void Awake()
        {
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
        /// Deactivate renderer and collider
        /// </summary>
        public void DeactivateRenderer()
        {
            eventCollider.enabled = false;
            eventMeshRenderer.enabled = false;
        }
    }
    
    public enum EventState{ NotStarted, Start, Active, Finish }
    public enum FinishCondition { OnTriggerEnter, PuzzleFinished, DialogueFinished }
}