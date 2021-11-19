using Actors;
using UnityEngine;

namespace Event
{
    [CreateAssetMenu(fileName = "NewEventData", menuName = "Dialogue/Event Data", order = 0)]
    public class EventData : ScriptableObject
    {
        public enum EventState{ NotStarted, Start, Active, Finish }

        public string eventName;
        public EventState eventState = EventState.NotStarted;
        public int triggerLimit = 1;
        public Actor affectedActor;
    }
}