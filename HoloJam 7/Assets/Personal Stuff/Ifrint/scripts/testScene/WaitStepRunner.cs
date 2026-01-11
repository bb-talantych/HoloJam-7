using UnityEngine;
using System.Collections;

public class WaitStepRunner : MonoBehaviour
{
    private void OnEnable()
    {
        CutsceneEvents.OnWaitRequested += HandleWait;
    }
    private void OnDisable()
    {
        CutsceneEvents.OnWaitRequested -= HandleWait;
    }

    private void HandleWait(float time)
    {
        StartCoroutine(WaitRoutine(time));
    }

    private IEnumerator WaitRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        CutsceneEvents.OnNextRequested?.Invoke();
    }
}