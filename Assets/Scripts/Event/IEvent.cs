namespace Event
{
    public interface IEvent
    {
        public void OnEventStart();
        public void OnEventActive();
        public void OnEventFinish();
    }
}