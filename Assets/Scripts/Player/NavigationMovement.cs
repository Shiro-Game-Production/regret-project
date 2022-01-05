using UnityEngine;
using UnityEngine.AI;

namespace Player{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    public class NavigationMovement : MonoBehaviour {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float angularSpeed = 1000f;
        
        private Animator animator;
        private NavMeshAgent navPlayer;
        private bool isWalking;
        private static readonly int IsWalkingParam = Animator.StringToHash("IsWalking");

        public bool IsWalking => isWalking;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            navPlayer = GetComponent<NavMeshAgent>();
        }
        
        private void Start()
        {
            navPlayer.speed = speed;
            navPlayer.angularSpeed = angularSpeed;
        }
        
        private void Update()
        {
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
    }
}