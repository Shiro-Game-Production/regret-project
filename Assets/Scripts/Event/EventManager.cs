namespace Event
{
    public abstract class EventManager : SingletonBaseClass<EventManager>
    {
        /// <summary>
        /// Set event data to run the event
        /// </summary>
        /// <param name="eventData">Event data</param>
        public virtual void SetEventData(EventData eventData){}
    }
}