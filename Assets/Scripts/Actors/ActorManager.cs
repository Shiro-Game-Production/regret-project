using UnityEngine;

namespace Actors
{
    public class ActorManager : MonoBehaviour
    {
        [SerializeField] private Actor actor;
        
        public Actor ActorData => actor;
    }
}