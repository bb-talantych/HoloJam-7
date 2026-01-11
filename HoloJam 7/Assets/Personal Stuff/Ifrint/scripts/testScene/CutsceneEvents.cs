using System;
using UnityEngine;

public static class CutsceneEvents
{
    public static Action OnNextRequested;
    public static Action<string> OnSpeakRequested;
    public static Action OnTextFinished;
    public static Action<int> OnSceneStepStarted;
    public static Action<string> OnSpeakerChanged;
    public static Action<float> OnWaitRequested;
}
