using UnityEngine;

namespace Audios.BackgroundMusics{
    public enum ListBackgroundMusic{
        EndingBGM,
        MainMenuBGM,
        NeutralBGM,
        SadBGM,
        TenseBGM,
    }

    public class BackgroundMusic: Sound {
        public ListBackgroundMusic listBackgroundMusic;
    }
}