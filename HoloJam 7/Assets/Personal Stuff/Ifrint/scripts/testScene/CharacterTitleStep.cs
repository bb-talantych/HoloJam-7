using UnityEngine;

[CreateAssetMenu(menuName = "Scene/Steps/SetSpeaker")]
public class SetSpeaker : SceneStep
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [TextArea] 
    public string speakerName;
    public override void Execute()
    {
        CutsceneEvents.OnSpeakerChanged?.Invoke(speakerName);
    }
}
