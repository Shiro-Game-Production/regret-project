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
        
        [Header("Finish Condition")]
        public FinishCondition finishCondition;

        [DrawIf("finishCondition", FinishCondition.OnTriggerEnter)]
        [SerializeField] private TriggerEnterCondition triggerObject;

        [DrawIf("finishCondition", FinishCondition.PuzzleFinished)]
        [SerializeField] private GameObject puzzleObject;
        
        [DrawIf("finishCondition", FinishCondition.DialogueFinished)]
        [SerializeField] private TextAsset dialogue;

        public string EventName => eventName;
        public ActorManager AffectedActor => affectedActor;
        public TextAsset WaitDialogueAsset => waitDialogueAsset;
        public TextAsset FinishDialogueAsset => finishDialogueAsset;
        public TriggerEnterCondition TriggerObject => triggerObject;
    }
    
    public enum EventState{ NotStarted, Start, Active, Finish }
    public enum FinishCondition { OnTriggerEnter, PuzzleFinished, DialogueFinished }
}