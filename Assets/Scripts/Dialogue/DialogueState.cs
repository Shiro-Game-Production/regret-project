namespace Dialogue{
    public enum DialogueState{
        Running,
        Stop,
    }
    
    public enum DialogueTypingState{
        Typing,
        FinishTyping,
        SkipSentence,
    }

    public enum DialogueMode
    {
        Normal,
        AutoTyping,
        HideMode,
        ViewLog,
        Pause,
    }
}