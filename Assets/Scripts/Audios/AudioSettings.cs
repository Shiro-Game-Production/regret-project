using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Audios
{
    public class AudioSettings : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;
        private const string BGM_VOLUME = "bgmVolume";
        private const string SFX_VOLUME = "sfxVolume";
        
        private void Start()
        {
            VolumeSetup(bgmSlider, BGM_VOLUME);
            VolumeSetup(sfxSlider, SFX_VOLUME);
        }

        /// <summary>
        /// Set BGM Volume
        /// </summary>
        /// <param name="sliderValue"></param>
        public void SetBgmVolume(float sliderValue)
        {
            SetVolume(sliderValue, BGM_VOLUME);
        }
        
        /// <summary>
        /// Set SFX volume
        /// </summary>
        /// <param name="sliderValue"></param>
        public void SetSfxVolume(float sliderValue)
        {
            SetVolume(sliderValue, SFX_VOLUME);
        }
        
        /// <summary>
        /// Set volume in audio mixer in player prefs
        /// </summary>
        /// <param name="sliderValue">Slider value</param>
        /// <param name="volumeType">Volume type</param>
        private void SetVolume(float sliderValue, string volumeType)
        {
            float volume = ConvertSliderValueToMixerValue(sliderValue);
            audioMixer.SetFloat(volumeType, volume);
            PlayerPrefs.SetFloat(volumeType, sliderValue);
        }
        
        /// <summary>
        /// Conver slider value (0-1) to mixer value
        /// Use logaritmic to conver 0-1 to 
        /// </summary>
        /// <param name="sliderValue">Slider value</param>
        /// <returns>log(sliderValue) * 10</returns>
        private float ConvertSliderValueToMixerValue(float sliderValue)
        {
            return Mathf.Log(sliderValue) * 10;
        }

        /// <summary>
        /// Set volume of volume type
        /// </summary>
        /// <param name="slider"></param>
        /// <param name="volumeType">Volume Type</param>
        private void VolumeSetup(Slider slider, string volumeType)
        {
            float sliderValue = 1;
            
            // Check Player prefs
            // If volume type is not in player prefs, ...  
            if (!PlayerPrefs.HasKey(volumeType))
            {
                Debug.Log($"Set {volumeType}: {sliderValue}");
                // Set new player prefs
                PlayerPrefs.SetFloat(volumeType, sliderValue);
            }
            // If there is volume type prefs
            else
            {
                // Get volume value
                sliderValue = PlayerPrefs.GetFloat(volumeType, 1);
                Debug.Log($"Get {volumeType}: {sliderValue}");
            }
            
            // Set volume in audio mixer
            audioMixer.SetFloat(volumeType, ConvertSliderValueToMixerValue(sliderValue));
            slider.value = sliderValue;
        }
    }
}
