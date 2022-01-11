using UnityEngine;

namespace Event.FinishConditionScripts
{
    public class FinishConditionManager: MonoBehaviour
    {
        protected EventData EventData;
        
        public virtual void SetEndingCondition(){}

        public virtual void OnEndingCondition()
        {
            Debug.Log(EventData == null);
            EventData.isFinished = true;
        }
    }
}