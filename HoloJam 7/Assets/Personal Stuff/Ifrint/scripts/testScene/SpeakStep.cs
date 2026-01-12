using UnityEngine;

[CreateAssetMenu(menuName = "Scene/Steps/Speak")]
public class SpeakStep : SceneStep
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [TextArea] 
    public string chatText;
    public override bool WaitsForCompletion => true;
    public override void Execute()
    {
        Debug.Log("SpeakStep executed: ");
        CutsceneEvents.OnSpeakRequested?.Invoke(chatText);
    }
}
