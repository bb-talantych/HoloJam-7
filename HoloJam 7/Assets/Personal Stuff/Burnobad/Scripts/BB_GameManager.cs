using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using BB_Scenes;

public class BB_GameManager : MonoBehaviour
{
    private static BB_GameManager instance;
    public static BB_GameManager Instance
    {
        get
        {
            if (instance != null)
                return instance;
            else
            {
                Debug.LogError("BB_GameManager instance not setup");
                return null;
            }

        }
    }

    bool gameOver = false;

    //testing
    public GameObject tempWinScreen;
    public GameObject tempLoseScreen;

    #region Events

    public static event Action Event_GameOver;
    public static event Action Event_LevelStarted;
    public static event Action Event_LevelRestart;

    #endregion

    #region On Enable/Disable
    private void OnEnable()
    {
        //Debug.Log(this.name + ": triggered OnEnable");

        if (instance == null)
            instance = this;

        BB_SceneManager.Event_LevelLoaded += OnLevelLoaded;
        BB_AssignmentManager.Event_LevelCompleted += OnLevelCompleted;
        BB_LevelTimer.Event_LevelTimerEnded += OnLevelTimerEnded;
    }

    private void OnDisable() 
    {
        //Debug.Log(this.name + ": triggered OnDisable");

        BB_SceneManager.Event_LevelLoaded -= OnLevelLoaded;
        BB_AssignmentManager.Event_LevelCompleted -= OnLevelCompleted;
        BB_LevelTimer.Event_LevelTimerEnded -= OnLevelTimerEnded;
    }

    #endregion

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) 
        {
            RestatLevel();
        }
    }

    void RestatLevel()
    {
        Event_LevelRestart?.Invoke();
        gameOver = false;
        Time.timeScale = 1;

        tempWinScreen.SetActive(false);
        tempLoseScreen.SetActive(false);
    }
    void GameOver()
    {
        Debug.Log(this.name.ToString() + ": Game Over");
        gameOver = true;

        tempWinScreen.SetActive(false);
        tempLoseScreen.SetActive(true);
        Time.timeScale = 0;

        Event_GameOver?.Invoke();
    }

    #region Event Reponses
    private void OnGameStart()
    {
        Debug.Log(this.name.ToString() + ": Game Start");
    }
    private void OnLevelLoaded(BB_GameScenes.GameScenes _scene)
    {
        if (!BB_SceneManager.Instance.IsMainMenu())
        {
            OnGameStart();
            // testing
            Event_LevelStarted?.Invoke();
        }
    }
    private void OnLevelCompleted(object _sender, EventArgs e)
    {
        Debug.Log(this.name.ToString() + ": Level Completed");
        gameOver = false;

        tempWinScreen.SetActive(true);
        tempLoseScreen.SetActive(false);
    }
    private void OnLevelTimerEnded()
    {
        GameOver();
    }

    #endregion

}
