using System.Collections.Generic;
using UnityEngine;

namespace Puzzle.Trash{
    [RequireComponent(typeof(BoxCollider2D))]
    public class TrashBin : MonoBehaviour {
        [SerializeField] private List<Trash> trashList;

        private void OnTriggerEnter2D(Collider2D other) {
            if(other.GetComponent<Trash>()){
                Debug.Log("Trash");
                other.gameObject.SetActive(false);
            }
        }
    }
}