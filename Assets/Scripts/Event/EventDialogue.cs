using System;
using UnityEngine;

namespace Event{
    [Serializable]
    public class EventDialogue{
        [SerializeField] private TextAsset waitDialogueAsset;
        [SerializeField] private TextAsset finishDialogueAsset;
        [SerializeField] private TextAsset nextEventDialogueAsset;
        [SerializeField] private TextAsset defaultDialogueAsset;

        public TextAsset WaitDialogueAsset => waitDialogueAsset;
        public TextAsset NextEventDialogueAsset => nextEventDialogueAsset;
        public TextAsset DefaultDialogueAsset => defaultDialogueAsset;
        public TextAsset FinishDialogueAsset => finishDialogueAsset;
    }
}