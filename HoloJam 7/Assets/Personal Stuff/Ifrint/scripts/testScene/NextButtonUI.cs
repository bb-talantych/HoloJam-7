using UnityEngine;

public class NextButtonUI : MonoBehaviour
{
    public void Click()
    {
        CutsceneEvents.OnNextRequested?.Invoke();
    }
}