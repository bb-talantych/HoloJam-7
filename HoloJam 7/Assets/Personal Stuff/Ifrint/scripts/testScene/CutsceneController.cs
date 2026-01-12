using UnityEngine;
using UnityEngine.Rendering;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private SceneData scenedata;
    private int stepIndex;
    
    private void OnEnable()
    {
        CutsceneEvents.OnNextRequested += NextStep;

    }

    private void OnDisable()
    {
        CutsceneEvents.OnNextRequested -= NextStep;
    }

    private void Start()
    {
        StartStep(0);
    }

    private void StartStep(int index)
    {
        if(scenedata == null || index >= scenedata.steps.Count)
        {
            return;
        }

        stepIndex = index;
        CutsceneEvents.OnSceneStepStarted?.Invoke(stepIndex);
        scenedata.steps[stepIndex].Execute();

        if (!scenedata.steps[stepIndex].WaitsForCompletion)
        {
            NextStep();
        }

    }

    private void NextStep()
    {
        StartStep(stepIndex + 1);
    }

}