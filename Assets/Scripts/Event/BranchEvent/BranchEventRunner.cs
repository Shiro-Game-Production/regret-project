using Event.DialogueEvent;
using UnityEngine;

namespace Event.BranchEvent{
    public class BranchEventRunner : MonoBehaviour {
        [SerializeField] private BranchEventData branchEventData;

        public BranchEventData BranchEventData => branchEventData;

        /// <summary>
        /// Update branch event state
        /// If finish update all item in event data to normal mode, 
        /// so it doesn't trigger the dialogue
        /// </summary>
        /// <param name="newState"></param>
        public void UpdateBranchEventState(BranchEventState newState){
            branchEventData.branchEventState = newState;

            switch(newState){
                case BranchEventState.Active:
                    // Make the remaining event datas in the same branch to normal mode
                    // if there is active event
                    foreach(DialogueEventData eventData in branchEventData.SameBranchEventDatas){
                        if(eventData.eventState == EventState.Active) continue;

                        if(eventData.ItemData){
                            eventData.ItemData.itemMode = Items.ItemData.ItemMode.NormalMode;
                        }
                        else{
                            Debug.LogError($"{eventData.EventId} doesn't have item data");
                        }
                    }
                    break;
                case BranchEventState.Finish:
                    // Make the remaining event datas in the same branch to normal mode
                    // if there is one event is finished already
                    foreach(DialogueEventData eventData in branchEventData.SameBranchEventDatas){
                        if(eventData.isFinished) continue;

                        if(eventData.ItemData){
                            eventData.ItemData.itemMode = Items.ItemData.ItemMode.NormalMode;
                        }
                        else{
                            Debug.LogError($"{eventData.EventId} doesn't have item data");
                        }
                    }

                    Destroy(gameObject);
                    break;
            }
        }
    }
}