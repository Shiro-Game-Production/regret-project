using UnityEngine;

namespace Event.FinishConditionScripts
{
    [RequireComponent(typeof(BoxCollider))]
    public class TriggerEnterCondition : FinishConditionManager
    {
        [Header("Trigger Collider")]
        [SerializeField] private Vector3 triggerSize = new Vector3(2, 2, 2);
        private BoxCollider boxCollider;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
            EventData = GetComponent<EventData>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && EventData.eventState == EventState.Active)
            {
                // Set is finished in event data to true
                OnEndingCondition();
            }
        }

        /// <summary>
        /// Set ending condition
        /// Ending condition is when player enter the trigger
        /// </summary>
        public override void SetEndingCondition()
        {
            // Set box collider
            boxCollider.isTrigger = true;
            boxCollider.size = triggerSize;
        }
    }
}