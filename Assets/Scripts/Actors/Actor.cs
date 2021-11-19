using UnityEngine;

namespace Actors
{
    [CreateAssetMenu(fileName = "NewActor", menuName = "Dialogue/Actor", order = 0)]
    public class Actor : ScriptableObject
    {
        [SerializeField] private string actorName;
        public TextAsset currentDialogue;

        public string ActorName => actorName;
    }
}