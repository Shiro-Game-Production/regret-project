using System;
using System.Collections.Generic;
using Effects;
using UnityEngine;

namespace Audios.BackgroundMusics
{
    public class BackgroundMusicManager : SingletonBaseClass<BackgroundMusicManager>
    {
        [ArrayElementTitle("listBackgroundMusic")]
        public List<BackgroundMusic> backgroundMusics;

        [SerializeField] private AudioSource bgmAudioSource;

        /// <summary>
        /// Play by audio enum name
        /// </summary>
        /// <param name="audio">Audio enum name</param>
        public void Play(string audio){
            BackgroundMusic backgroundMusic = backgroundMusics.Find(
                s => Enum.TryParse(audio, true, out s.listBackgroundMusic));

            Play(backgroundMusic);
        }

        /// <summary>
        /// Play by audio enum type
        /// </summary>
        /// <param name="audio">Audio enm type</param>
        public void Play(BackgroundMusic audio){
            if(bgmAudioSource.isPlaying){
                StartCoroutine(
                    AudioFadingEffect.FadeOut(bgmAudioSource,
                        afterEffect: () =>
                        {
                            StartCoroutine(AudioFadingEffect.FadeIn(
                                bgmAudioSource, audio.volume, 
                                beforeEffect: () => bgmAudioSource.clip = audio.clip));
                        }
                    )
                );
            } else{
                StartCoroutine(AudioFadingEffect.FadeIn(
                    bgmAudioSource, audio.volume, 
                    beforeEffect: () => bgmAudioSource.clip = audio.clip));
            }
        }
    }
}