using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Audios.SoundEffects
{
    public class SoundEffectManager : SingletonBaseClass<SoundEffectManager>
    {
        [SerializeField] private AudioMixerGroup sfxAudioMixer;
        
        [ArrayElementTitle("listSoundEffect")]
        public List<SoundEffect> soundEffects;

        private List<AudioSource> audioSourcePool;

        private void Awake() {
            audioSourcePool = new List<AudioSource>();
        }

        /// <summary>
        /// Play by audio enum name
        /// </summary>
        /// <param name="audio">Audio enum name</param>
        public void Play(AudioClip audio){
            AudioSource audioSource = GetAudioSource();

            audioSource.PlayOneShot(audio);
            StartCoroutine(StopAudio(audioSource));
        }

        /// <summary>
        /// Play by audio enum name
        /// </summary>
        /// <param name="audio">Audio enum name</param>
        public void Play(string audio){
            AudioSource audioSource = GetAudioSource();
            AudioClip audioClip = GetAudioClip(audio);

            audioSource.PlayOneShot(audioClip);
            StartCoroutine(StopAudio(audioSource));
        }

        /// <summary>
        /// Play by audio enum type
        /// </summary>
        /// <param name="audio">Audio enum type</param>
        public void Play(ListSoundEffect audio){
            AudioSource audioSource = GetAudioSource();
            AudioClip audioClip = GetAudioClip(audio);

            audioSource.PlayOneShot(audioClip);
            StartCoroutine(StopAudio(audioSource));
        }

        /// <summary>
        /// Deactivate audio's game object after not playing anymore
        /// </summary>
        /// <param name="audioSource">Current audio source</param>
        /// <returns></returns>
        private IEnumerator StopAudio(AudioSource audioSource){
            yield return new WaitUntil(() => !audioSource.isPlaying);
            audioSource.gameObject.SetActive(false);
        }

        /// <summary>
        /// Get audio clip from audio enum name
        /// </summary>
        /// <param name="audio">Audio enum name</param>
        /// <returns>Audio clip of audio enum name</returns>
        private AudioClip GetAudioClip(string audio){
            SoundEffect soundEffect = soundEffects.Find(
                s => {
                    if(s.listSoundEffect.ToString().ToLower() == audio.ToLower()){
                        return true;
                    }
                    return false;
                }
            );
            if(soundEffect != null) return soundEffect.Clip;

            Debug.LogError($"Audio clip: {audio} not available");
            return null;
        }

        /// <summary>
        /// Get audio clip from audio enum type
        /// </summary>
        /// <param name="audio">Audio enum type</param>
        /// <returns>Audio clip of audio enum name</returns>
        private AudioClip GetAudioClip(ListSoundEffect audio){
            SoundEffect soundEffect = soundEffects.Find(s => s.listSoundEffect == audio);
            if(soundEffect != null) return soundEffect.Clip;

            Debug.LogError($"Audio clip: {audio} not available");
            return null;
        }

        /// <summary>
        /// Get audio source in object pooling
        /// </summary>
        /// <returns>Available sound effect audio source</returns>
        private AudioSource GetAudioSource(){
            AudioSource audioSource = audioSourcePool.Find(source =>
                !source.isPlaying && !source.gameObject.activeInHierarchy);
            
            if(audioSource == null){
                GameObject newAudioObject = new GameObject("Sound Effect", typeof(AudioSource));
                newAudioObject.transform.parent = transform; // Set parent

                // Set mixer
                audioSource = newAudioObject.GetComponent<AudioSource>();
                audioSource.outputAudioMixerGroup = sfxAudioMixer;

                // Add to pool
                audioSourcePool.Add(audioSource);
            }

            audioSource.gameObject.SetActive(true);

            return audioSource;
        }
    }
}