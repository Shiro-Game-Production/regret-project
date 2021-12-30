using System.Collections.Generic;
using Event.FinishConditionScripts;
using UnityEngine;

namespace Puzzle.Trash{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TrashBin : MonoBehaviour {
        [SerializeField] private List<Trash> trashList;
        private PuzzleFinishedCondition puzzleFinishedCondition;
        private Dictionary<Trash, bool> trashDict;

        private void Awake() {
            puzzleFinishedCondition = GetComponent<PuzzleFinishedCondition>();
            trashDict = new Dictionary<Trash, bool>();

            foreach(Trash trash in trashList){
                trashDict.Add(trash, false);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            Trash trash = other.GetComponent<Trash>();
            if(!trash) return;

            // Set trash's condition
            trashDict[trash] = true;
            other.gameObject.SetActive(false);

            // Check finish condition
            foreach(bool value in trashDict.Values){
                if(!value) return;
            }

            Debug.Log("Puzzle done");
            puzzleFinishedCondition.OnEndingCondition();
        }
    }
}