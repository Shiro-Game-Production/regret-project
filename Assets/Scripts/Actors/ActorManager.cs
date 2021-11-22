using UnityEngine;

namespace Actors
{
    public class ActorManager : MonoBehaviour
    {
        [SerializeField] private string actorName;
        public TextAsset currentDialogue;
        
        public string ActorName => actorName;
    }
}