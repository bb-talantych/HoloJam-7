using UnityEngine;

[CreateAssetMenu(menuName = "Scene/Steps/Wait")]
public class WaitStep : SceneStep
{
    public float waitTime = 1f;
    public override bool WaitsForCompletion => true;

    public override void Execute()
    {
        CutsceneEvents.OnWaitRequested?.Invoke(waitTime);
    }
}