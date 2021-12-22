namespace Event
{
    public interface IEventRunner
    {
        public void OnEventStart();
        public void OnEventActive();
        public void OnEventFinish();
        public void SetNextEvent();
    }
}