using Effects;
using Event;
using UnityEngine;

namespace Items.Puzzle{
    public class PuzzleItemData : ItemData {
        [SerializeField] private CanvasGroup puzzleCanvasGroup;
        [SerializeField] private EventData eventData;

        public override void HandleInteraction()
        {
            if(!eventData.isFinished)
                StartCoroutine(FadingEffect.FadeIn(puzzleCanvasGroup));
            base.HandleInteraction();
        }
    }
}