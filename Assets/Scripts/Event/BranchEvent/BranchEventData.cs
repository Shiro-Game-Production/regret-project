using System;
using System.Collections.Generic;
using Event.DialogueEvent;
using Items;
using UnityEngine;

namespace Event.BranchEvent{
    [Serializable]
    public class BranchEventData{
        [SerializeField] private string name;
        [SerializeField] private List<BranchPart> branchParts;

        public List<BranchPart> BranchParts => branchParts;
    }
}