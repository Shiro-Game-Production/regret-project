﻿using UnityEngine;

namespace Event.FinishConditionScripts
{
    [RequireComponent(typeof(BoxCollider))]
    public class TriggerEnterCondition : MonoBehaviour
    {
        [Header("Event Data")]
        private EventData eventData;

        [Header("Trigger Collider")]
        [SerializeField] private Vector3 triggerSize = new Vector3(2, 2, 2);
        private BoxCollider boxCollider;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
            eventData = GetComponent<EventData>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && eventData.eventState == EventState.Active)
            {
                // Set is finished in event data to true
                eventData.isFinished = true;
            }
        }
        
        /// <summary>
        /// Set box collider's size and trigger for the event
        /// </summary>
        /// <param name="isTrigger"></param>
        public void SetBoxCollider(bool isTrigger)
        {
            // Set box collider
            boxCollider.isTrigger = isTrigger;
            boxCollider.size = triggerSize;
        }
    }
}