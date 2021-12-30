﻿using System;
using UnityEngine;
 using UnityEngine.SceneManagement;

 namespace Audios
{
    public class AudioManager: SingletonBaseClass<AudioManager>
    {
        [ArrayElementTitle("listSound")]
        public Sound[] sounds;

        private bool donePlayBgm;
        
        /// <summary>
        /// Set instance and don't destroy on load
        /// </summary>
        private void SetInstance()
        {
            if (instance ==  null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject);
        }
        
        private void Awake()
        {
            SetInstance();
            
            foreach (Sound sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.outputAudioMixerGroup = sound.audioMixer;

                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
            }
        }

        private void Update()
        {
            PlayBgm();
        }
        
        /// <summary>
        /// Play BGM but only in home scene
        /// </summary>
        private void PlayBgm()
        {
            if (SceneManager.GetActiveScene().name == "HomeScene")
            {
                if (donePlayBgm) return;
                
                donePlayBgm = true;
                Play(ListSound.MainMenuBGM);
            }
            else
            {
                donePlayBgm = false;
                Stop(ListSound.MainMenuBGM);
            }
        }

        /// <summary>
        /// Play audio
        /// To call method in scripts
        /// </summary>
        /// <param name="listSound"></param>
        public void Play(ListSound listSound)
        {
            AudioSource audioSource = GetAudioSource(listSound);
            audioSource.PlayOneShot(audioSource.clip);
        }

        /// <summary>
        /// Play audio
        /// To call method in scripts
        /// </summary>
        /// <param name="audioFileName"></param>
        public void Play(string audioFileName)
        {
            AudioSource audioSource = GetAudioSource(audioFileName);
            audioSource.PlayOneShot(audioSource.clip);
        }
        
        /// <summary>
        /// Stop specific sound that is playing
        /// </summary>
        /// <param name="listSound"></param>
        private void Stop(ListSound listSound)
        {
            AudioSource audioSource = GetAudioSource(listSound);
            if(audioSource.isPlaying)
                audioSource.Stop();
        }

        /// <summary>
        /// Get audio source for enum
        /// </summary>
        /// <param name="listSound"></param>
        /// <returns></returns>
        private AudioSource GetAudioSource(ListSound listSound)
        {
            Sound s = Array.Find(sounds, sound => sound.listSound == listSound);

            if (s != null) return s.source;
            Debug.LogError($"Sound: {listSound} not found!");
            return null;
        }
        
        /// <summary>
        /// Get audio clip by file name
        /// </summary>
        /// <param name="audioFileName"></param>
        /// <returns></returns>
        private AudioSource GetAudioSource(string audioFileName)
        {
            Sound s = Array.Find(sounds, sound => 
                Enum.TryParse(audioFileName, true, out sound.listSound));

            if (s != null) return s.source;
            Debug.LogError($"Sound: {audioFileName} not found!");
            return null;
        }
    }
}