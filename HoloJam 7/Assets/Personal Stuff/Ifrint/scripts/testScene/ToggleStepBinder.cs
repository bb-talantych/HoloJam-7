using UnityEngine;

public class ToggleStepBinder : MonoBehaviour
{
    public ToggleStep step;
    public GameObject target;

    private void Awake()
    {
        step.target = target;
    }
}