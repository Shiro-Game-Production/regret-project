using System.Collections.Generic;
using Dialogue;
using Effects;
using Puzzle;
using UnityEngine;

namespace Event.FinishConditionScripts{
    public class PuzzleFinishedCondition : FinishConditionManager {
        [SerializeField] private CanvasGroup puzzleCanvasGroup;
        [SerializeField] private PuzzleManager puzzleManager;
        [SerializeField] private List<GameObject> sameBranchEvents;
        [SerializeField] private TextAsset finishPuzzleDialogue;

        private void Awake() {
            EventData = GetComponent<EventData>();
        }

        public override void SetEndingCondition()
        {
            base.SetEndingCondition();
            puzzleManager.PuzzleFinishedCondition = this;
        }

        public override void OnEndingCondition()
        {
            base.OnEndingCondition();
            StartCoroutine(FadingEffect.FadeOut(puzzleCanvasGroup, 
                afterEffect: () => {
                    DialogueManager.Instance.SetDialogue(finishPuzzleDialogue);
                })
            );
            
            sameBranchEvents.ForEach(sameBranchEvent => Destroy(sameBranchEvent));
        }
    }
}