using UnityEngine;

namespace Puzzle {
    public class DragDropPuzzle : MonoBehaviour
    {
        private Vector3 offset;

        public GameObject panel;

        private void Update() {
            PuzzleClear();
        }

        private void OnMouseDown() 
        {
            offset = transform.position - MouseWorldPosition();
        }

        private void OnMouseDrag() 
        {
            transform.position = MouseWorldPosition() + offset;
        }

        Vector3 MouseWorldPosition()
        {
            var mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
            return Camera.main.ScreenToWorldPoint(mouseScreenPos);
        }

        private void OnCollisionEnter2D(Collision2D other) 
        {
            if(other.gameObject.tag == "Trashbin")
            {
                Destroy(this.gameObject);
            }
        }

        void PuzzleClear()
        {
            if(GameObject.FindGameObjectsWithTag("Trash").Length == 1)
            {
                Debug.Log("Puzzle Cleared");
                panel.SetActive(true);
                //kode ke dialog di sini
            }
        }
    }
}