using Dialogue;
using Effects;
using UnityEngine;

namespace Event.FinishConditionScripts{
    public class PuzzleFinishedCondition : FinishConditionManager {
        [SerializeField] private CanvasGroup puzzleCanvasGroup;
        [SerializeField] private GameObject sameBranchEvent;
        [SerializeField] private TextAsset finishPuzzleDialogue;

        private void Awake() {
            EventData = GetComponent<EventData>();
        }

        public override void OnEndingCondition()
        {
            base.OnEndingCondition();
            StartCoroutine(FadingEffect.FadeOut(puzzleCanvasGroup, 
                afterEffect: () => {
                    DialogueManager.Instance.SetDialogue(finishPuzzleDialogue);
                })
            );
            Destroy(sameBranchEvent);
        }
    }
}