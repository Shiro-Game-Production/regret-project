using UnityEngine;
using UnityEngine.AI;

namespace Items.Door
{
    [RequireComponent(typeof(NavMeshObstacle))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(HingeJoint))]
    public class DoorRequirements : MonoBehaviour
    {
        [Header("Hinge Joint properties")] 
        private HingeJoint doorHingeJoint;
        private const bool USE_SPRING = true;
        private const float SPRING = 15;
        private const float DAMPER = 2;
        private const bool USE_LIMITS = true;
        private const float MIN_ANGLES = -90;
        private const float MAX_ANGLES = 90;

        [Header("NavMeshObstacle")]
        private NavMeshObstacle navMeshObstacle;

        private void Awake()
        {
            doorHingeJoint = GetComponent<HingeJoint>();
            navMeshObstacle = GetComponent<NavMeshObstacle>();
            
            navMeshObstacle.carving = true;
            SetHingeJointProperties();
        }
        
        /// <summary>
        /// Set hinge joint properties
        /// </summary>
        private void SetHingeJointProperties()
        {
            doorHingeJoint.useSpring = USE_SPRING;
            doorHingeJoint.useLimits = USE_LIMITS;
            
            // Set joint spring
            if(doorHingeJoint.useSpring)
            {
                JointSpring jointSpring = doorHingeJoint.spring;
                jointSpring.spring = SPRING;
                jointSpring.damper = DAMPER;
                doorHingeJoint.spring = jointSpring;
            }
            
            // Set joint limits
            if(doorHingeJoint.useLimits)
            {
                JointLimits jointLimits = doorHingeJoint.limits;
                jointLimits.min = MIN_ANGLES;
                jointLimits.max = MAX_ANGLES;
                doorHingeJoint.limits = jointLimits;
            }
        }
    }
}
