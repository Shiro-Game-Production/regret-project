using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

namespace Player{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    public class NavigationMovement : MonoBehaviour {
        [SerializeField] private float speed = 5f;
        
        private Animator animator;
        private NavMeshAgent navPlayer;
        private ThirdPersonCharacter character;
        private bool isWalking, isWalkingExist;

        private const string IS_WALKING_PARAMS = "IsWalking";

        public bool IsWalking => isWalking;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            character = GetComponent<ThirdPersonCharacter>();
            navPlayer = GetComponent<NavMeshAgent>();
        }
        
        private void Start()
        {
            navPlayer.updateRotation = false;
            navPlayer.speed = speed;
            
            // Check is walking parameter
            // For rin original animator
            foreach(AnimatorControllerParameter param in animator.parameters){
                if(IS_WALKING_PARAMS == param.name){
                    isWalkingExist = true;
                } else{
                    isWalkingExist = false;
                }
            }
        }
        
        private void Update()
        {
            // Set animation
            // Uncomment this if want to apply animation
            if(animator.runtimeAnimatorController)
            {
                isWalking = !(navPlayer.remainingDistance <= navPlayer.stoppingDistance);
                
                if(isWalkingExist){
                    Debug.Log(gameObject.name);
                    animator.SetBool("IsWalking", isWalking);
                }

                if(isWalking){
                    character.Move(navPlayer.desiredVelocity, false, false);
                } else{
                    character.Move(Vector3.zero, false, false);
                }
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
        /// Change nav mesh quality
        /// </summary>
        /// <param name="navMeshQuality"></param>
        public void ChangeNavMeshQuality(ObstacleAvoidanceType navMeshQuality){
            navPlayer.obstacleAvoidanceType = navMeshQuality;
        }
    }
}