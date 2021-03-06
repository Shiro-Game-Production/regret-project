using Event.FinishConditionScripts;
using UnityEngine;

namespace Event
{
    public abstract class EventData : MonoBehaviour
    {
        [Header("Event Data")]
        [SerializeField] private string eventId;
        [TextArea(3,5)]
        [SerializeField] private string eventDescription;
        public bool isFinished;
        public EventState eventState = EventState.NotStarted;
        
        [Header("Finish Condition")]
        public FinishCondition finishCondition;
        [SerializeField] private FinishConditionManager triggerObject;
        public bool canBeInteracted;
        [SerializeField] private bool keepObjectAfterFinish;

        #region Setter and Getter

        public string EventId => eventId;
        public bool KeepObjectAfterFinish => keepObjectAfterFinish;
        public FinishConditionManager TriggerObject => triggerObject;
        
        #endregion

        public virtual void OnEventFinish(){}
    }
}