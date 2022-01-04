namespace Audios.SoundEffects {
    public enum ListSoundEffect {
        DoorOpened,
        DoorClosed,
        Slap
    }

    public class SoundEffect: Sound {
        public ListSoundEffect listSoundEffect;
    }
}