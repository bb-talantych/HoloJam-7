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

    #region GameManager Events

    public static event Action Event_GameStart;
    public static event Action Event_GameOver;

    #endregion

    // All Events must be here
    #region On Enable/Disable
    private void OnEnable()
    {
        Debug.Log(this.name.ToString() + " triggered OnEnable");

        Event_GameStart += OnGameStart;
        Event_GameOver += OnGameOver;
    }

    private void OnDisable() 
    {
        Debug.Log(this.name.ToString() + " triggered OnDisable");

        Event_GameStart -= OnGameStart;
        Event_GameOver -= OnGameOver;
    }

    #endregion

    // Awake only to initialize stuff once
    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    #region All Event functions
    private void OnGameStart()
    {
        Debug.Log("Game Start");
    }

    private void OnGameOver()
    {
        Debug.Log("Game Over");
    }

    #endregion
}
