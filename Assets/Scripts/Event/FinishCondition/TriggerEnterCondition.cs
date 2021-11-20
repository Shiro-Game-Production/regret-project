using UnityEngine;

namespace Event.FinishCondition
{
    [RequireComponent(typeof(BoxCollider))]
    public class TriggerEnterCondition : MonoBehaviour
    {
        [Header("Event Data")]
        public EventData eventData;

        [Header("Trigger Collider")]
        [SerializeField] private Vector3 triggerSize = new Vector3(2, 2, 2);
        private BoxCollider boxCollider;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
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