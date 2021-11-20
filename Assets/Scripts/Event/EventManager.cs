using System.Collections.Generic;
using UnityEngine;

namespace Event
{
    public class EventManager : SingletonBaseClass<EventManager>
    {
        [SerializeField] private EventRunner eventRunnerPrefab;
        
        private readonly List<EventRunner> eventRunnerPool = new List<EventRunner>();

        /// <summary>
        /// Set event data to run the event
        /// </summary>
        /// <param name="eventData">Event data</param>
        public void SetEventData(EventData eventData)
        {
            EventRunner eventRunner = GetOrCreateEventRunner();
            eventRunner.eventData = eventData;
            eventRunner.canStartEvent = true;
        }
        
        /// <summary>
        /// Event runner object pooling
        /// </summary>
        /// <returns>Return inactive or new event runner</returns>
        private EventRunner GetOrCreateEventRunner()
        {
            EventRunner eventRunner = eventRunnerPool.Find(runner =>
                runner.eventData.eventState == EventState.Finish &&
                !runner.gameObject.activeInHierarchy);

            if (eventRunner == null)
            {
                eventRunner = Instantiate(eventRunnerPrefab, transform).GetComponent<EventRunner>();
                
                eventRunnerPool.Add(eventRunner);
            }
            
            eventRunner.gameObject.SetActive(true);
            return eventRunner;
        }
    }
}