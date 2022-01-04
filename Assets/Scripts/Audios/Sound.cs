﻿using UnityEngine;
using UnityEngine.Audio;

 namespace Audios
{
    [System.Serializable]
    public class Sound
    {
        [SerializeField] private AudioClip clip;
        
        [Range(0, 1)]
        [SerializeField] private float volume = 1;
        [Range(-3, 3)]
        [SerializeField] private float pitch = 1;
        
        [SerializeField] private bool loop;

        #region Setter and Getter

        public AudioClip Clip => clip;
        public float Volume => volume;
        public float Pitch => pitch;
        public bool Loop => loop;

        #endregion
    }
}