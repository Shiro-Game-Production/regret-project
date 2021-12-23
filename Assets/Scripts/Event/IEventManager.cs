namespace Event
{
    public interface IEventManager
    {
        /// <summary>
        /// Set event data to run the event
        /// </summary>
        /// <param name="eventData">Event data</param>
        public void SetEventData(EventData eventData);
    }
}