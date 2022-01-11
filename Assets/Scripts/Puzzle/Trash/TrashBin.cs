using System.Collections.Generic;
using Audios.SoundEffects;
using Event.FinishConditionScripts;
using UnityEngine;

namespace Puzzle.Trash{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TrashBin : MonoBehaviour {
        [SerializeField] private bool isClosed;
        [SerializeField] private List<Trash> trashList;
        [SerializeField] private PuzzleFinishedCondition puzzleFinishedCondition;

        [Header("Audio")]
        [SerializeField] private List<AudioClip> plasticThrowAudios;
        
        private Dictionary<Trash, bool> trashDict;

        private void Awake() {
            trashDict = new Dictionary<Trash, bool>();

            foreach(Trash trash in trashList){
                trashDict.Add(trash, false);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            Trash trash = other.GetComponent<Trash>();
            TrashCanCap trashCanCap = other.GetComponent<TrashCanCap>();

            if(trashCanCap) isClosed = true;

            if(!trash || isClosed) return;

            // Play audio
            int randomAudioIndex = Random.Range(0, plasticThrowAudios.Count);
            SoundEffectManager.Instance.Play(plasticThrowAudios[randomAudioIndex]);

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

        private void OnTriggerExit2D(Collider2D other) {
            TrashCanCap trashCanCap = other.GetComponent<TrashCanCap>();
            if(trashCanCap) isClosed = false;
        }
    }
}