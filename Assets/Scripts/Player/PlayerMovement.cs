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
        [SerializeField] private float speed = 5f;
        [SerializeField] private float angularSpeed = 1000f;
        
        private Animator animator;
        private Camera mainCamera;
        private NavMeshAgent navPlayer;
        public bool canMove;
        private bool isWalking;
        private static readonly int IsWalkingParam = Animator.StringToHash("IsWalking");

        public bool IsWalking => isWalking;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            mainCamera = Camera.main;
            navPlayer = GetComponent<NavMeshAgent>();
        }
        
        private void Start()
        {
            canMove = true;
            navPlayer.speed = speed;
            navPlayer.angularSpeed = angularSpeed;
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUiObject() && canMove)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out var hit))
                {
                    Move(hit.point);
                }
            }

            // Set animation
            // Uncomment this if want to apply animation
            if(animator.runtimeAnimatorController)
            {
                isWalking = !(navPlayer.remainingDistance <= navPlayer.stoppingDistance);
                animator.SetBool(IsWalkingParam, isWalking);
            }
        }
        
        /// <summary>
        /// Move to destination
        /// </summary>
        /// <param name="destination"></param>
        public void Move(Vector3 destination)
        {
            navPlayer.SetDestination(destination);
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