using UnityEngine;
using UnityEngine.EventSystems;

namespace Puzzle.Trash {
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Trash: MonoBehaviour, 
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform parentRectTransform;

        private BoxCollider2D trashCollider;
        private Rigidbody2D trashRigidbody;
        private Bounds parentBounds;
        private RectTransform rectTransform;

        private void Awake() {
            trashCollider = GetComponent<BoxCollider2D>();
            trashRigidbody = GetComponent<Rigidbody2D>();
            rectTransform = GetComponent<RectTransform>();

            trashCollider.size = rectTransform.rect.size;
            trashRigidbody.bodyType = RigidbodyType2D.Kinematic;

            parentBounds = new Bounds(
                parentRectTransform.anchoredPosition,
                parentRectTransform.rect.size);
        }

        public void OnBeginDrag(PointerEventData eventData) { }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            rectTransform.anchoredPosition = CheckPosition(rectTransform.anchoredPosition);
        }

        public void OnEndDrag(PointerEventData eventData) { }
        
        /// <summary>
        /// Make sure trash's position is in parentBounds
        /// </summary>
        /// <param name="position">Trash's anchored position</param>
        /// <returns>Trash's anchored position</returns>
        private Vector3 CheckPosition(Vector3 position){
            float xAxis = position.x;
            float yAxis = position.y;

            // Left side (min)
            if(xAxis < 0){
                if(xAxis < parentBounds.min.x){
                    position.x = parentBounds.min.x;
                }
            } else if(xAxis > 0) { // Right side (max)
                if(xAxis > parentBounds.max.x){
                    position.x = parentBounds.max.x;
                }
            }

            // Lower side (min)
            if(yAxis < 0){
                if(yAxis < parentBounds.min.y){
                    position.y = parentBounds.min.y;
                }
            } else if(yAxis > 0) { // Upper side (max)
                if(yAxis > parentBounds.max.y){
                    position.y = parentBounds.max.y;
                }
            }

            return position;
        }
    }
}