using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

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

    #region Events

    public static event Action Event_GameStart;
    public static event Action Event_GameOver;
    public static event Action Event_LevelRestart;

    #endregion

    #region On Enable/Disable
    private void OnEnable()
    {
        //Debug.Log(this.name + ": triggered OnEnable");

        if (instance == null)
            instance = this;

        BB_SceneManager.Event_LevelLoaded += OnLevelLoaded;
    }

    private void OnDisable() 
    {
        //Debug.Log(this.name + ": triggered OnDisable");
        BB_SceneManager.Event_LevelLoaded -= OnLevelLoaded;
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
    }

    #region Event Reponses
    private void OnGameStart()
    {
        Debug.Log(this.name.ToString() + ": Game Start");
    }

    private void OnGameOver()
    {
        Debug.Log(this.name.ToString() + ": Game Over");
        gameOver = true;
    }

    private void OnLevelLoaded()
    {
        if (!BB_SceneManager.Instance.IsMainMenu())
        {
            OnGameStart();
        }
    }

    #endregion

}
