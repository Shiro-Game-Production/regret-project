﻿ namespace Audios
{
    public class AudioManager: SingletonBaseClass<AudioManager>
    { 
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
        }
    }
}