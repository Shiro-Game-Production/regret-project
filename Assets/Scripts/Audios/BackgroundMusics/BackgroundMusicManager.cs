using System;
using System.Collections.Generic;
using Effects;
using UnityEngine;
using UnityEngine.Audio;

namespace Audios.BackgroundMusics
{
    public class BackgroundMusicManager : SingletonBaseClass<BackgroundMusicManager>
    {
        [ArrayElementTitle("listBackgroundMusic")]
        public List<BackgroundMusic> backgroundMusics;

        [SerializeField] private AudioSource bgmAudioSource;
        [SerializeField] private AudioMixerGroup bgmAudioMixer;

        private void Awake() {
            bgmAudioSource.outputAudioMixerGroup = bgmAudioMixer;
            bgmAudioSource.loop = true;
            
            Play(ListBackgroundMusic.MainMenuBGM);
        }

        /// <summary>
        /// Play by audio enum name
        /// </summary>
        /// <param name="audio">Audio enum name</param>
        public void Play(string audio){
            BackgroundMusic backgroundMusic = backgroundMusics.Find(
                s => Enum.TryParse(audio, true, out s.listBackgroundMusic));
            
            if(backgroundMusic != null)
                PlayAudioEffect(backgroundMusic);
            else
                Debug.LogError($"Audio with name: {audio} is not available");
        }

        /// <summary>
        /// Play by audio enum type
        /// </summary>
        /// <param name="audio">Audio enum type</param>
        public void Play(ListBackgroundMusic audio){
            BackgroundMusic backgroundMusic = backgroundMusics.Find(
                s => s.listBackgroundMusic == audio);

            if(backgroundMusic != null)
                PlayAudioEffect(backgroundMusic);
            else
                Debug.LogError($"Audio with name: {audio} is not available");
        }

        private void PlayAudioEffect(BackgroundMusic backgroundMusic){
            if(bgmAudioSource.isPlaying){
                StartCoroutine(
                    AudioFadingEffect.FadeOut(bgmAudioSource,
                        afterEffect: () =>
                        {
                            StartCoroutine(AudioFadingEffect.FadeIn(
                                bgmAudioSource, backgroundMusic.Volume, 
                                beforeEffect: () => bgmAudioSource.clip = backgroundMusic.Clip,
                                afterEffect: () => bgmAudioSource.Play()));
                        }
                    )
                );
            } else{
                StartCoroutine(AudioFadingEffect.FadeIn(
                    bgmAudioSource, backgroundMusic.Volume, 
                    beforeEffect: () => bgmAudioSource.clip = backgroundMusic.Clip,
                    afterEffect: () => bgmAudioSource.Play()));
            }
        }
    }
}