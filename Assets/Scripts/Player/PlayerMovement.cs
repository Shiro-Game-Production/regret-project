using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : SingletonBaseClass<PlayerMovement>
    {
        private NavigationMovement navigationMovement;
        private Camera mainCamera;
        public bool canMove;

        public NavigationMovement Movement => navigationMovement;

        private void Awake()
        {
            navigationMovement = GetComponent<NavigationMovement>();
            mainCamera = Camera.main;
            canMove = true;
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUiObject() && canMove)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out var hit))
                {
                    navigationMovement.Move(hit.point);
                }
            }
        }
        
        /// <summary>
        /// Check if pointer is over user interface or not
        /// </summary>
        /// <returns>Return true if pointer is over UI object, false if pointer is not over UI object</returns>
        private bool IsPointerOverUiObject()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
}