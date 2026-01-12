using UnityEngine;

[CreateAssetMenu(menuName = "Scene/Steps/Toggle")]
public class ToggleStep : SceneStep
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [HideInInspector] public GameObject target;
    public bool active;
    public override void Execute()
    {
        if (target != null)
        {
            Debug.Log("Toggle executed: ");
            target.SetActive(active);

        }
    }
}
