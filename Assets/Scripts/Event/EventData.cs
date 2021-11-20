using Actors;
using Event.FinishCondition;
using UnityEngine;

namespace Event
{
    [CreateAssetMenu(fileName = "NewEventData", menuName = "Dialogue/Event Data", order = 0)]
    public class EventData : ScriptableObject
    {
        public enum EventState{ NotStarted, Start, Active, Finish }
        public enum FinishCondition { OnTriggerEnter, PuzzleFinished, DialogueFinished }
        
        [Header("Event Data")]
        [SerializeField] private string eventName;
        [SerializeField] private Actor affectedActor;
        public bool isFinished = false;
        [SerializeField] private int triggerLimit = 1;
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
        public Actor AffectedActor => affectedActor;
        public int TriggerLimit => triggerLimit;
        public TextAsset WaitDialogueAsset => waitDialogueAsset;
        public TextAsset FinishDialogueAsset => finishDialogueAsset;
        public TriggerEnterCondition TriggerObject => triggerObject;
    }
}