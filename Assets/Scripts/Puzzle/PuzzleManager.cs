using Event.FinishConditionScripts;
using UnityEngine;

namespace Puzzle
{
    public class PuzzleManager : MonoBehaviour
    {
        protected PuzzleFinishedCondition puzzleFinishedCondition;

        public PuzzleFinishedCondition PuzzleFinishedCondition { 
            get => puzzleFinishedCondition; 
            set => puzzleFinishedCondition = value; 
        }
    }
}