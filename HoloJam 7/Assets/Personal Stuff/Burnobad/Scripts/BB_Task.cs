using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BB_CommonLevelStuff;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(AudioSource))]
public class BB_Task : MonoBehaviour
{
    [SerializeField]
    private GameObject movePointHolder;

    public Vector3 MovePoint
    { 
        get 
        { 
            if(movePointHolder != null)
                return movePointHolder.transform.position;
            else
            {
                Debug.LogError(this.name + ": not movePointHolder");
                return Vector3.zero;
            }
        }
    }
    public bool IsAvailable
    {
        get;
        private set;
    }

    public float timeToComplete = 10f;

    [SerializeField]
    private Stats statRequirements = default;

    [SerializeField]
    private Canvas sliderCanvas;
    [SerializeField]
    private Slider progressSlider;

    [SerializeField]
    private AudioSource sfxSource;


    public event EventHandler Event_TaskAssigned;
    public event EventHandler Event_TaskFinished;

    private void OnEnable()
    {
        IsAvailable = true;

        progressSlider.value = 0;

        //testing
        statRequirements = new Stats(-3, 3);

        sliderCanvas.enabled = false;
        if (sliderCanvas != null)
            sliderCanvas.transform.rotation = Quaternion.Euler(0, 0, 0);

    }

    public void TaskSelected()
    {
        BB_CommonDataManager.instance.PlayClip(sfxSource, BB_CommonDataManager.instance.taskSelectedClips);
    }
    public void StartTask(Stats _talentStats)
    {
        Debug.Log(this.name + ": agent assigned");

        IsAvailable = false;
        sliderCanvas.enabled = true;
        Event_TaskAssigned?.Invoke(this, EventArgs.Empty);

        float taskTime = GetTaskTime(_talentStats);
        //Debug.Log("taskTime: " + taskTime);
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
            if (progressSlider != null)
                progressSlider.value = timeElapsed/_timeToComplete;
            yield return new WaitForEndOfFrame();
        }
        //Debug.Log(this.name + ": task coroutine completed");

        Event_TaskFinished?.Invoke(this, EventArgs.Empty);
        BB_CommonDataManager.instance.PlayClip(sfxSource, BB_CommonDataManager.instance.taskCompleteClips);
    }
}
