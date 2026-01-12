using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using BB_Scenes;
using UnityEngine.UI;

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
    public GameObject levelCompeleScreen;
    public Button nextLevelButton;
    public GameObject tempLoseScreen;

    [SerializeField]
    private AudioSource sfxSource;

    private BB_AssignmentManager levelManager;

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

        levelCompeleScreen.SetActive(false);
        tempLoseScreen.SetActive(false);

        BB_SceneManager.Event_ScreenDimmed += OnScreenDimmed;
        BB_SceneManager.Event_LevelLoaded += OnLevelLoaded;
        BB_AssignmentManager.Event_LevelCompleted += OnLevelCompleted;
        BB_LevelTimer.Event_LevelTimerEnded += OnLevelTimerEnded;
    }

    private void OnDisable() 
    {
        //Debug.Log(this.name + ": triggered OnDisable");

        BB_SceneManager.Event_ScreenDimmed -= OnScreenDimmed;
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

        levelCompeleScreen.SetActive(false);
        tempLoseScreen.SetActive(false);
    }
    void GameOver()
    {
        Debug.Log(this.name.ToString() + ": Game Over");
        gameOver = true;

        levelCompeleScreen.SetActive(false);
        tempLoseScreen.SetActive(true);
        Time.timeScale = 0;

        Event_GameOver?.Invoke();
        BB_CommonDataManager.Instance.PlayClip(sfxSource, BB_CommonDataManager.Instance.levelFailClips);
    }

    #region Event Reponses
    private void OnGameStart()
    {
        Debug.Log(this.name.ToString() + ": Game Start");

        levelCompeleScreen.SetActive(false);
        tempLoseScreen.SetActive(false);
    }
    private void OnLevelLoaded(BB_GameScenes.GameScenes _scene, bool _isReload)
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

        levelManager = (BB_AssignmentManager)_sender;
        levelCompeleScreen.SetActive(true);
        nextLevelButton.onClick.AddListener(OnButtonClick);
        tempLoseScreen.SetActive(false);

        BB_CommonDataManager.Instance.PlayClip(sfxSource, BB_CommonDataManager.Instance.levelCompleteClips);
    }
    private void OnLevelTimerEnded()
    {
        GameOver();
    }

    private void OnScreenDimmed()
    {
        levelCompeleScreen.SetActive(false);
        tempLoseScreen.SetActive(false);
    }
    private void OnButtonClick()
    {
        BB_SceneManager.Instance.LoadLevel(levelManager.NextLevel, this.name);
    }


    #endregion

}
