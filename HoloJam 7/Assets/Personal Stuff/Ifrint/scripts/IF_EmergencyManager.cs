using System.Collections.Generic;
using UnityEngine;

public class IF_EmergencyManager : MonoBehaviour
{
    [SerializeField] private List<IF_EmergencyTask> emergencies;
    [SerializeField] private float[] triggerTimes;

    private bool[] triggered;

    private void Start()
    {
        triggered = new bool[triggerTimes.Length];
    }

    // Update is called once per frame
    public void UpdateCountdown(float timeRemaining)
    {
        for (int i=0; i< triggerTimes.Length; i++)
        {
            if(!triggered[i] && timeRemaining <= triggerTimes[i])
            {
                emergencies[i].Activate();
                triggered[i] = true;
            }
        }
    }
}
