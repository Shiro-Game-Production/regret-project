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
        private MeshRenderer eventMeshRenderer;
        private Collider eventCollider;

        [Header("Dialogue Asset")]
        [SerializeField] private TextAsset waitDialogueAsset;
        [SerializeField] private TextAsset finishDialogueAsset;
        [SerializeField] private TextAsset nextEventDialogueAsset;
        [SerializeField] private TextAsset defaultDialogueAsset;
        
        [Header("Finish Condition")]
        public FinishCondition finishCondition;
        [SerializeField] private FinishConditionManager triggerObject;

        public string EventName => eventName;
        public ActorManager AffectedActor => affectedActor;
        public TextAsset WaitDialogueAsset => waitDialogueAsset;
        public TextAsset NextEventDialogueAsset => nextEventDialogueAsset;
        public TextAsset DefaultDialogueAsset => defaultDialogueAsset;
        public TextAsset FinishDialogueAsset => finishDialogueAsset;
        public FinishConditionManager TriggerObject => triggerObject;

        private void Awake()
        {
            eventCollider = GetComponent<Collider>();
            eventMeshRenderer = GetComponent<MeshRenderer>();
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