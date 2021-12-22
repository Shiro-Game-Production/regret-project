using System.Collections;
using System.Collections.Generic;
using Dialogue;
using UnityEngine;

namespace Event.DialogueEvent{
    public class DialogueEventManager : EventManager {
        [SerializeField] private DialogueEventRunner eventRunnerPrefab;

        private List<DialogueEventRunner> eventRunnerPool;

        private void Awake() {
            eventRunnerPool = new List<DialogueEventRunner>(); 
        }

        /// <summary>
        /// Set event data to run the event
        /// </summary>
        /// <param name="eventData">Event data</param>
        public override void SetEventData(EventData eventData)
        {
            DialogueEventRunner eventRunner = GetOrCreateEventRunner();
            eventRunner.eventData = eventData as DialogueEventData;
            StartCoroutine(StartEvent(eventRunner));
        }
        
        /// <summary>
        /// Start event when dialogue is finished
        /// </summary>
        /// <param name="eventRunner"></param>
        /// <returns></returns>
        private static IEnumerator StartEvent(DialogueEventRunner eventRunner)
        {
            yield return new WaitUntil(() => !DialogueManager.Instance.DialogueIsPlaying);
            yield return new WaitForSeconds(1f);
            eventRunner.canStartEvent = true;
        }
        
        /// <summary>
        /// Event runner object pooling
        /// </summary>
        /// <returns>Return inactive or new event runner</returns>
        private DialogueEventRunner GetOrCreateEventRunner()
        {
            DialogueEventRunner eventRunner = eventRunnerPool.Find(runner =>
                runner.eventData.eventState == EventState.Finish &&
                !runner.gameObject.activeInHierarchy);

            if (eventRunner == null)
            {
                eventRunner = Instantiate(eventRunnerPrefab, transform).GetComponent<DialogueEventRunner>();
                
                eventRunnerPool.Add(eventRunner);
            }
            
            eventRunner.gameObject.SetActive(true);
            return eventRunner;
        }
    }
}