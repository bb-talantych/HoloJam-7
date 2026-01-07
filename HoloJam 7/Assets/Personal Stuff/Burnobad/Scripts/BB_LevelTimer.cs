using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BB_LevelTimer : MonoBehaviour
{
    [SerializeField]
    private float levelTime = 20;
    [SerializeField]
    private TMP_Text test;

    private float elapsedTime;

    public static Action Event_LevelTimerEnded;

    private Coroutine timerCoroutine;

    void OnEnable()
    {
        BB_GameManager.Event_LevelStarted += StartLevelTimer;
        BB_AssignmentManager.Event_LevelCompleted += OnLevelCompleted;
    }
    private void OnDisable()
    {
        BB_GameManager.Event_LevelStarted -= StartLevelTimer;
        BB_AssignmentManager.Event_LevelCompleted -= OnLevelCompleted;
    }

    public void StartLevelTimer()
    {
        timerCoroutine = StartCoroutine(ITimer());

    }

    IEnumerator ITimer()
    {
        elapsedTime = 0;
        while (elapsedTime < levelTime) 
        {
            elapsedTime += Time.deltaTime;

            float time = levelTime - elapsedTime;
            test.text = Mathf.FloorToInt(time).ToString();

            yield return new WaitForEndOfFrame();
        }
        //Debug.Log(this.name + ": timer finished");
        Event_LevelTimerEnded?.Invoke();
        yield return null;
    }

    void OnLevelCompleted(object _sender, EventArgs e)
    {
        if(timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
    }
}
