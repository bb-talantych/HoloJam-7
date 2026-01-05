using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BB_LevelTimer : MonoBehaviour
{
    [SerializeField]
    private float levelTime = 20;

    private float elapsedTime;

    public static Action Event_LevelTimerEnded;

    private Coroutine timerCoroutine;

    void OnEnable()
    {
        BB_AssignmentManager.Event_LevelCompleted += OnLevelCompleted;
    }
    private void OnDisable()
    {
        BB_AssignmentManager.Event_LevelCompleted -= OnLevelCompleted;
    }

    //testing
    private void Start()
    {
        StartLevelTimer();
    }
    void StartLevelTimer()
    {
        timerCoroutine = StartCoroutine(ITimer());
    }

    IEnumerator ITimer()
    {
        elapsedTime = 0;
        while (elapsedTime < levelTime) 
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        //Debug.Log(this.name + ": timer finished");
        Event_LevelTimerEnded?.Invoke();
        yield return null;
    }

    void OnLevelCompleted()
    {
        if(timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }
}
