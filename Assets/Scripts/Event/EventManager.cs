using System.Collections;
using System.Collections.Generic;
using Dialogue;
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
            StartCoroutine(StartEvent(eventRunner));
        }
        
        /// <summary>
        /// Start event when dialogue is finished
        /// </summary>
        /// <param name="eventRunner"></param>
        /// <returns></returns>
        private static IEnumerator StartEvent(EventRunner eventRunner)
        {
            yield return new WaitUntil(() => !DialogueManager.Instance.DialogueIsPlaying);
            yield return new WaitForSeconds(1f);
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