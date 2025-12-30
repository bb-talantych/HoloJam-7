using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BB_LevelLoader : MonoBehaviour
{
    [SerializeField]
    private string mainMenuScene;


    private string currentScene;
    private int currentSceneIndex;
    [SerializeField]
    private List<string> levelList;

    #region On Enable/Disable
    private void OnEnable()
    {
        Debug.Log(this.name.ToString() + ": triggered OnEnable");

        BB_MainMenuManager.Event_StartButton += StartButton;
    }

    private void OnDisable()
    {
        Debug.Log(this.name.ToString() + ": triggered OnDisable");

        BB_MainMenuManager.Event_StartButton -= StartButton;
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
    void ReloadScene()
    {
        LoadScene(currentScene);
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
    }

    //Testing
    void StartButton()
    {
        LoadScene(levelList[0]);
    }
}
