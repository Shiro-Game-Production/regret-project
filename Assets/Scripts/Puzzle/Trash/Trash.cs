using UnityEngine;
using UnityEngine.EventSystems;

namespace Puzzle.Trash {
    public class Trash: MonoBehaviour, 
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Canvas canvas;

        private Camera mainCamera;
        private RectTransform rectTransform;

        private void Awake() {
            mainCamera = Camera.main;
            rectTransform = GetComponent<RectTransform>();
            Debug.Log($"Scale factor: {canvas.scaleFactor}");
            Debug.Log($"Screen width: {Screen.width}");
            Debug.Log($"Screen height: {Screen.height}");
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("Begin drag");
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("End drag");
        }
    }
}