using UnityEngine;
using UnityEngine.Events;

namespace UserInterface
{
    public class PressAnyKeyManager : MonoBehaviour
    {
        public UnityEvent onClick;

        private void Update()
        {
            if (Input.anyKey)
                onClick.Invoke();
        }
    }
}
