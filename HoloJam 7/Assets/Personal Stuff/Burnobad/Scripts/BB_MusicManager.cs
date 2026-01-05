using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BB_Scenes;

public class BB_MusicManager : MonoBehaviour
{
    private static BB_MusicManager instance;
    public static BB_MusicManager Instance
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

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private AudioClip musicClip;

    #region On Enable/Disable
    private void OnEnable()
    {
        //Debug.Log(this.name + ": triggered OnEnable");

        if (instance == null)
            instance = this;
        if(musicSource == null)
            musicSource = GetComponent<AudioSource>();

        BB_SceneManager.Event_LevelLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        //Debug.Log(this.name + ": triggered OnDisable");

        BB_SceneManager.Event_LevelLoaded -= OnLevelLoaded;
    }

    #endregion

    #region Event Reponses
    void OnLevelLoaded(BB_GameScenes.GameScenes _scene)
    {
        switch(_scene)
        {
            case BB_GameScenes.GameScenes.BB_TestScene:
                TestScene();
                break;
        }
    }

    #endregion

    void TestScene()
    {
        musicSource.loop = true;
        musicSource.clip = musicClip;
        musicSource.Play();
    }
}
