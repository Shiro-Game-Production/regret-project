using UnityEngine;

namespace Event.PuzzleEvent{
    public class PuzzleEventManager : SingletonBaseClass<PuzzleEventManager>, IEventManager
    {
        [SerializeField] private PuzzleEventRunner eventRunnerPrefab;

        public void SetEventData(EventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}