using Effects;
using UnityEngine;
using UserInterface;

namespace Items.Puzzle{
    public class PuzzleItemData : ItemData {
        [SerializeField] private CanvasGroup puzzleCanvasGroup;

        public override void HandleInteraction()
        {
            StartCoroutine(FadingEffect.FadeIn(puzzleCanvasGroup));
            base.HandleInteraction();
        }
    }
}