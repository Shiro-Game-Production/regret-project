using System.Collections;
using Event.DialogueEvent;
using UnityEngine;

namespace Event.BranchEvent{
    public class BranchEventRunner : MonoBehaviour {
        [SerializeField] private BranchEventData branchEventData;

        private BranchPart activeBranchPart;
        private bool canObserveActive, observeInUpdate;

        public BranchEventData BranchEventData => branchEventData;

        private void Start() {
            StartCoroutine(ObserveActive());
            StartCoroutine(ObserveFinish());
        }

        private void Update() {
            if(observeInUpdate){
                for (int i = 0; i < branchEventData.BranchParts.Count; i++){
                    if(branchEventData.BranchParts[i].branchPartState == BranchState.Active) continue;

                    branchEventData.BranchParts[i].EventDatas.ForEach(eventData =>
                    {
                        eventData.ItemData.itemMode = Items.ItemData.ItemMode.NormalMode;
                    });
                }

                observeInUpdate = false;
            }
        }

        /// <summary>
        /// Update branch event state
        /// If finish update all item in event data to normal mode, 
        /// so it doesn't trigger the dialogue
        /// </summary>
        /// <param name="newState"></param>
        public void UpdateBranchPartState(DialogueEventData dialogueEventData, BranchState newState){
            foreach (BranchPart branchPart in branchEventData.BranchParts)
            {
                foreach (DialogueEventData eventData in branchPart.EventDatas){
                    // Check which dialogue event data
                    if(eventData == dialogueEventData){
                        branchPart.branchPartState = newState;

                        // Look at new state
                        if(newState == BranchState.Active){
                            activeBranchPart = branchPart;
                            canObserveActive = true;
                        }

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Observe active branch after there is active branch part and 0.5s
        /// </summary>
        /// <returns></returns>
        private IEnumerator ObserveActive(){
            yield return new WaitUntil(() => canObserveActive);
            yield return new WaitForSeconds(0.5f);
            observeInUpdate = true;
        }

        /// <summary>
        /// Observe finish branch after event to finish is finished
        /// </summary>
        /// <returns></returns>
        private IEnumerator ObserveFinish(){
            yield return new WaitUntil(() => activeBranchPart != null);
            yield return new WaitUntil(() => activeBranchPart.EventToFinish.isFinished);

            activeBranchPart.EventDatas.ForEach(eventData =>{
                eventData.ItemData.itemMode = Items.ItemData.ItemMode.NormalMode;
                eventData.canBeInteracted = false;
            });

            Destroy(gameObject);
        }
    }
}