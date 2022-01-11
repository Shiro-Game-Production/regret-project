using System;

namespace Audios.SoundEffects {
    public enum ListSoundEffect {
        DoorOpened,
        DoorClosed,
        Slap,
        ShelfPush,
        Locked,
        AccessDenied,
        SafeOpened
    }
    
    [Serializable]
    public class SoundEffect: Sound {
        public ListSoundEffect listSoundEffect;
    }
}