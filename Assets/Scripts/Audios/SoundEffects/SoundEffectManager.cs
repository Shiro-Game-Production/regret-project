using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audios.SoundEffects
{
    public class SoundEffectManager : SingletonBaseClass<SoundEffectManager>
    {
        [ArrayElementTitle("listSoundEffect")]
        public List<SoundEffect> soundEffects;

        private List<AudioSource> audioSources;

        private void Awake() {
            audioSources = new List<AudioSource>();
        }

        /// <summary>
        /// Play by audio enum name
        /// </summary>
        /// <param name="audio">Audio enum name</param>
        public void Play(string audio){
            AudioSource audioSource = GetAudioSource();
            AudioClip audioClip = GetAudioClip(audio);

            audioSource.PlayOneShot(audioClip);
        }

        /// <summary>
        /// Play by audio enum type
        /// </summary>
        /// <param name="audio">Audio enum type</param>
        public void Play(SoundEffect audio){
            AudioSource audioSource = GetAudioSource();

            audioSource.PlayOneShot(audio.clip);
        }

        /// <summary>
        /// Get audio clip from audio enum name
        /// </summary>
        /// <param name="audio">Audio enum name</param>
        /// <returns>Audio clip of audio enum name</returns>
        private AudioClip GetAudioClip(string audio){
            SoundEffect soundEffect = soundEffects.Find(s => Enum.TryParse(audio, true, out s.listSoundEffect));

            return soundEffect.clip;
        }

        /// <summary>
        /// Get audio source in object pooling
        /// </summary>
        /// <returns>Available sound effect audio source</returns>
        private AudioSource GetAudioSource(){
            AudioSource audioSource = audioSources.Find(source =>
                !source.isPlaying && !source.gameObject.activeInHierarchy);
            
            if(audioSources == null){
                GameObject newAudioObject = new GameObject("Sound Effect", typeof(AudioSource));

                audioSources.Add(newAudioObject.GetComponent<AudioSource>());
            }

            audioSource.gameObject.SetActive(true);

            return audioSource;
        }
    }
}