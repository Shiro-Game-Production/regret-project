using UnityEngine;

namespace Event.PuzzleEvent{
    public class PuzzleEventData: EventData{
        [SerializeField] private CanvasGroup puzzleCanvasGroup;

        #region Setter and Getter

        public CanvasGroup PuzzleCanvasGroup => puzzleCanvasGroup;

        #endregion
    }
}