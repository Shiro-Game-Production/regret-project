using UnityEngine;

namespace Actors
{
    [CreateAssetMenu(fileName = "NewActor", menuName = "Actor", order = 0)]
    public class Actor : ScriptableObject
    {
        public string actorName;
        public TextAsset currentDialogue;
    }
}