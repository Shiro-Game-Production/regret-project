using Actors;
using UnityEngine;

namespace Event
{
    [CreateAssetMenu(fileName = "NewEventData", menuName = "Dialogue/Event Data", order = 0)]
    public class EventData : ScriptableObject
    {
        public enum EventState{ NotStarted, Start, Active, Finish }

        [SerializeField] private string eventName;
        [SerializeField] private Actor affectedActor;
        public EventState eventState = EventState.NotStarted;
        [SerializeField] private int triggerLimit = 1;
        
        public string EventName => eventName;
        public Actor AffectedActor => affectedActor;
        public int TriggerLimit => triggerLimit;
    }
}