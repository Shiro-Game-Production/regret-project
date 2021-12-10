﻿using UnityEngine;
using UnityEngine.Audio;

 namespace Audios
{
    public enum ListSound
    {
        BackgroundMusic,
        DoorOpened,
        DoorClosed,
        Slap
    }
    
    [System.Serializable]
    public class Sound
    {
        public ListSound listSound;
        public AudioClip clip;
        public AudioMixerGroup audioMixer;
        
        [Range(0, 1)]
        public float volume = 1;
        [Range(-3, 3)]
        public float pitch = 1;
        
        public bool loop;
        
        [HideInInspector]
        public AudioSource source;
    }
}