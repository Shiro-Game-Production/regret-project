using Cinemachine;
using UnityEngine;

namespace GameCamera
{
    public class CameraShake : SingletonBaseClass<CameraShake>
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        private float shakeTimer, startingIntensity, shakeTimerTotal;
        
        private void Update() {
            if(shakeTimer > 0){
                shakeTimer -= Time.deltaTime;

                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 
                    Mathf.Lerp(startingIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
            }
        }

        public void ShakeCamera(float intensity = 5f, float time = 0.8f){
            Debug.Log("Shake baru");
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

            startingIntensity = intensity;
            shakeTimerTotal = time;
            shakeTimer = time;
        }
    }
}