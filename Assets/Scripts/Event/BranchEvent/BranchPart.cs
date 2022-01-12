using System;
using System.Collections.Generic;
using Event.DialogueEvent;
using UnityEngine;

namespace Event.BranchEvent{
    [Serializable]
    public class BranchPart{
        [SerializeField] private string name;
        public BranchState branchPartState = BranchState.NotStarted;
        [SerializeField] private DialogueEventData eventToFinish;
        [SerializeField] private List<DialogueEventData> eventDatas;

        public List<DialogueEventData> EventDatas => eventDatas;
        public DialogueEventData EventToFinish => eventToFinish;
    }
}