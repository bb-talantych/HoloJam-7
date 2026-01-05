using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using BB_Scenes;

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

    private BB_GameScenes.GameScenes currentScene = BB_GameScenes.GameScenes.NoScene;

    public static event Action<BB_GameScenes.GameScenes> Event_LevelLoaded;

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
        LoadScene(BB_GameScenes.GameScenes.MainMenu);
    }

    void LoadScene(BB_GameScenes.GameScenes _sceneToLoad)
    {
        StartCoroutine(ILoadScene(_sceneToLoad));
    }

    IEnumerator ILoadScene(BB_GameScenes.GameScenes _sceneToLoad)
    {
        if(_sceneToLoad == BB_GameScenes.GameScenes.NoScene)
        {
            Debug.LogError(this.name + ": _sceneToLoad was NoScene");
            Debug.LogError(this.name + ": _sceneToLoad changed to BB_TestScene");
            _sceneToLoad = BB_GameScenes.GameScenes.BB_TestScene;
        }

        // Start Unloading previous Scene
        if (currentScene != BB_GameScenes.GameScenes.NoScene)
        {
            string currentSceneName = BB_GameScenes.GetScene(currentScene);
            AsyncOperation unloadSceneAsync =
                SceneManager.UnloadSceneAsync(currentSceneName);

            while (!unloadSceneAsync.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        // Start Loading New Scene
        string sceneToLoadName = BB_GameScenes.GetScene(_sceneToLoad);
        AsyncOperation loadSceneAsync =
            SceneManager.LoadSceneAsync(sceneToLoadName, LoadSceneMode.Additive);

        if (loadSceneAsync == null)
        {
            Debug.LogError(this.name.ToString() + ": loadSceneAsync == null");
        }

        while (!loadSceneAsync.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        currentScene = _sceneToLoad;
        Event_LevelLoaded?.Invoke(currentScene);
    }

    #region Public Methodes
    public void LoadLevel(BB_GameScenes.GameScenes _scene, string _callerName)
    {
        Debug.Log(this.name + ": " + _callerName + " called LoadLevel");

        LoadScene(_scene);
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
