using System;
using System.Collections.Generic;
using Event.DialogueEvent;
using Items;
using UnityEngine;

namespace Event.BranchEvent{
    [Serializable]
    public class BranchEventData{
        [SerializeField] private string name;
        public BranchEventState branchEventState = BranchEventState.NotStarted;
        [SerializeField] private List<DialogueEventData> sameBranchEventDatas;

        #region Setter and Getter

        public List<DialogueEventData> SameBranchEventDatas => sameBranchEventDatas;

        #endregion
    }

    public enum BranchEventState{
        NotStarted,
        Active,
        Finish
    }
}