using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class IF_EmergencyTask : MonoBehaviour
{
    [SerializeField] private BB_Task task;
    [SerializeField] private float emergencyDuration = 8f;

    private float timer;
    private bool active;
    public bool IsActive => active;
    public event System.Action<IF_EmergencyTask> Event_Emergencyfailed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OEnable()
    {
        task.gameObject.SetActive(false);
    }

    public void Activate()
    {
        active = true;
        timer = emergencyDuration;
        task.gameObject.SetActive(true);
    }
    void Update()
    {
        if (!active) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Fail();
        }

    }

    private void Fail()
    {
        active = false;
        task.gameObject.SetActive(false);
        Debug.Log("EMERGENCY FAIL");

    }

    public void Complete()
    {
        if (!active) return;
        active = false;
        task.gameObject.SetActive(false);
    }
}
