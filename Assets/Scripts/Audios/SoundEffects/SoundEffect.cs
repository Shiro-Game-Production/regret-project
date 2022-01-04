using System;

namespace Audios.SoundEffects {
    public enum ListSoundEffect {
        DoorOpened,
        DoorClosed,
        Slap
    }
    
    [Serializable]
    public class SoundEffect: Sound {
        public ListSoundEffect listSoundEffect;
    }
}