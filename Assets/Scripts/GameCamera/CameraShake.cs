using System.Collections;
using UnityEngine;

namespace GameCamera
{
    public class CameraShake : SingletonBaseClass<CameraShake>
    {
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float shakeDuration = 1f;
        
        public IEnumerator ShakingEffect()
        {
            Vector3 startPosition = transform.position;
            float elapsedTime = 0;

            while (elapsedTime < shakeDuration)
            {
                elapsedTime += Time.deltaTime;
                float strength = curve.Evaluate(elapsedTime / shakeDuration);
                transform.position = startPosition + Random.insideUnitSphere * strength;
                yield return null;
            }

            transform.position = startPosition;
        }
    }
}