using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BB_LevelLoader : MonoBehaviour
{
    [SerializeField]
    private string mainMenuScene;

    private string currentScene;
    private int currentSceneIndex;

    private void Start()
    {
        StartCoroutine(LoadScene(mainMenuScene));
    }

    IEnumerator LoadScene(string _sceneName)
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
            SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);

        if (loadSceneAsync == null)
        {
            Debug.LogError(this.name + ": loadSceneAsync == null");
        }

        while (!loadSceneAsync.isDone)
        {
            yield return new WaitForEndOfFrame();
        }

        currentScene = _sceneName;
    }
}
