using UnityEngine;

namespace Actors
{
    public class ActorManager : MonoBehaviour
    {
        [SerializeField] private Actor actor;
        
        public Actor ActorData => actor;

        private TextAsset originTextAsset;

        private void Awake()
        {
            originTextAsset = actor.currentDialogue;
        }

        private void OnApplicationQuit()
        {
            actor.currentDialogue = originTextAsset;
        }
    }
}