using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BB_CommonStuff;
using UnityEngine.UI;
using System;

public class BB_Task : MonoBehaviour
{
    [SerializeField]
    private GameObject movePointHolder;

    public Vector3 MovePoint
    { get { return movePointHolder.transform.position; } }
    public bool isAssigned
    {
        get;
        private set;
    }

    public float timeToComplete = 10f;

    [SerializeField]
    private Stats statRequirements = default;

    [SerializeField]
    private Slider progressSlider;

    public event Action<BB_Task> Event_TaskAssigned;
    public event Action<BB_Task> Event_TaskFinished;

    private void OnEnable()
    {
        isAssigned = false;

        progressSlider.value = 0;

        //testing
        statRequirements = new Stats(-3, 3);
    }

    public void StartTask(Stats _talentStats)
    {
        Debug.Log(this.name + ": agent assigned");
        isAssigned = true;
        Event_TaskAssigned?.Invoke(this);

        float taskTime = GetTaskTime(_talentStats);
        Debug.Log("taskTime: " + taskTime);
        StartCoroutine(ITask(taskTime));
    }
    private float GetTaskTime(Stats _talentStats)
    {
        float speedModifier = 1f;
        speedModifier += _talentStats.strenght >= statRequirements.strenght ? -0.1f : 0.1f;
        speedModifier += _talentStats.dexterity >= statRequirements.dexterity ? -0.1f : 0.1f;
        speedModifier += _talentStats.intellect >= statRequirements.intellect ? -0.1f : 0.1f;
        speedModifier += _talentStats.charisma >= statRequirements.charisma ? -0.1f : 0.1f;

        float finalTime = timeToComplete * speedModifier;
        return finalTime;
    }
    public void FinishTask()
    {
        Debug.Log(this.name + ": task finished");
    }

    IEnumerator ITask(float _timeToComplete)
    {
        //Debug.Log(this.name + ": task coroutine started");
        float timeElapsed = 0;
        while(timeElapsed < _timeToComplete)
        {
            timeElapsed += Time.deltaTime;  
            progressSlider.value = timeElapsed/_timeToComplete;
            yield return new WaitForEndOfFrame();
        }
        //Debug.Log(this.name + ": task coroutine completed");

        Event_TaskFinished?.Invoke(this);
    }
}
