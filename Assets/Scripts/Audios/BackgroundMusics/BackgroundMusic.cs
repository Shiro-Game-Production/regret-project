using System;
using UnityEngine;

namespace Audios.BackgroundMusics{
    public enum ListBackgroundMusic{
        EndingBGM,
        MainMenuBGM,
        NeutralBGM,
        SadBGM,
        TenseBGM,
    }
    
    [Serializable]
    public class BackgroundMusic: Sound {
        public ListBackgroundMusic listBackgroundMusic;
    }
}