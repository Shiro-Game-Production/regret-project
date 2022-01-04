using System.Collections.Generic;
using UnityEngine;

namespace Audios.BackgroundMusics
{
    public class BackgroundMusicManager : SingletonBaseClass<BackgroundMusicManager>
    {
        [ArrayElementTitle("listBackgroundMusic")]
        public List<BackgroundMusic> backgroundMusics;

        public void Play(string audio){

        }

        public void Play(BackgroundMusic audio){

        }

        public void Stop(){
            
        }
    }
}