using System.Collections.Generic;
using UnityEngine;

namespace Puzzle.Trash{
    public class TrashBin : MonoBehaviour {
        [SerializeField] private List<Trash> trashList;


        private void OnCollisionEnter2D(Collision2D other) {
            if(other.gameObject.GetComponent<Trash>()){
                other.gameObject.SetActive(false);
            }
        }
    }
}