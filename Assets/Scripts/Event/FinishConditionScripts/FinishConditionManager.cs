using UnityEngine;

namespace Event.FinishConditionScripts
{
    public class FinishConditionManager: MonoBehaviour
    {
        protected EventData EventData;
        
        public virtual void SetEndingCondition(){}

        public void OnEndingCondition()
        {
            EventData.isFinished = true;
        }
    }
}