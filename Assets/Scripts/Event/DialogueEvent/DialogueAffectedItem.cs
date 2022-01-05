using System;
using Items;
using UnityEngine;

namespace Event.DialogueEvent{
    [Serializable]
    public struct DialogueAffectedItem{
        [SerializeField] private string name;
        [SerializeField] private ItemData affectedItem;
        [SerializeField] private EventDialogue eventDialogue;

        #region Setter and Getter

        public ItemData AffectedItem => affectedItem;
        public EventDialogue EventDialogue => eventDialogue;

        #endregion
    }
}