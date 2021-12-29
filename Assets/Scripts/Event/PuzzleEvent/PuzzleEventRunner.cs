using UnityEngine;

namespace Event.PuzzleEvent{
    public class PuzzleEventRunner : MonoBehaviour, IEventRunner
    {
        public PuzzleEventData eventData;
        private bool hasSetFinishCondition;
        public bool canStartEvent;

        public void OnEventActive()
        {
            throw new System.NotImplementedException();
        }

        public void OnEventFinish()
        {
            throw new System.NotImplementedException();
        }

        public void OnEventStart()
        {
            throw new System.NotImplementedException();
        }

        public void SetNextEvent()
        {
            throw new System.NotImplementedException();
        }
    }
}