using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BB_SceneManager : MonoBehaviour
{
    private static BB_SceneManager instance;
    public static BB_SceneManager Instance
    {
        get
        {
            if (instance != null)
                return instance;
            else
            {
                Debug.LogError("BB_SceneManager instance not setup");
                return null;
            }

        }
    }

    [SerializeField]
    private string mainMenuScene;

    private string currentScene;

    public static event Action Event_LevelLoaded;

    #region On Enable/Disable
    private void OnEnable()
    {
        //Debug.Log(this.name + ": triggered OnEnable");

        if (instance == null)
            instance = this;

        BB_GameManager.Event_LevelRestart += OnLevelRestart;
    }

    private void OnDisable()
    {
        //Debug.Log(this.name + ": triggered OnDisable");
        BB_GameManager.Event_LevelRestart -= OnLevelRestart;
    }

    #endregion

    private void Start()
    {
        LoadScene(mainMenuScene);
    }

    void LoadScene(string _sceneToLoad)
    {
        if (_sceneToLoad != null)
            StartCoroutine(ILoadScene(_sceneToLoad));
        else
            Debug.LogError(this.name.ToString() + ": LoadScene, no _sceneToLoad");
    }

    IEnumerator ILoadScene(string _sceneToLoad)
    {
        // Start Unloading previous Scene
        if (currentScene != null)
        {
            AsyncOperation unloadSceneAsync =
                SceneManager.UnloadSceneAsync(currentScene);

            while (!unloadSceneAsync.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        // Start Loading New Scene
        AsyncOperation loadSceneAsync =
            SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Additive);

        if (loadSceneAsync == null)
        {
            Debug.LogError(this.name.ToString() + ": loadSceneAsync == null");
        }

        while (!loadSceneAsync.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        currentScene = _sceneToLoad;
        Event_LevelLoaded?.Invoke();
    }

    #region Public Methodes
    public void LoadLevel(string _levelName, string _callerName)
    {
        Debug.Log(this.name + ": " + _callerName + " called LoadLevel");

        LoadScene(_levelName);
    }

    public bool IsMainMenu()
    {
        return currentScene == mainMenuScene;
    }

    #endregion
    #region Event Reponses

    private void OnLevelRestart()
    {
        Debug.Log(this.name + ": on game restart");
        LoadScene(currentScene);
    }

    #endregion
}
