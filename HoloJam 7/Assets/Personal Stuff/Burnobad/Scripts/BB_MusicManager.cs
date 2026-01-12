using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BB_Scenes;
using System;

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

    [Range(0, 1), SerializeField]
    private float levelCompleteMusicVolume = 0.25f;

    [SerializeField]
    private List<AudioClip> mainMenuClip;
    [SerializeField]
    private List<AudioClip> ezLevelClips;
    [SerializeField]
    private List<AudioClip> medLevelClips;
    [SerializeField]
    private List<AudioClip> hardLevelClips;

    [SerializeField]
    private List<AudioClip> gameWinClip;
    [SerializeField]
    private List<AudioClip> cutsceneNeutral;
    [SerializeField]
    private List<AudioClip> cutsceneWhacky;
    [SerializeField]
    private List<AudioClip> theTruth;


    private IEnumerator musicCor = null;

    #region On Enable/Disable
    private void OnEnable()
    {
        //Debug.Log(this.name + ": triggered OnEnable");

        if (instance == null)
            instance = this;
        if(musicSource == null)
            musicSource = GetComponent<AudioSource>();

        BB_SceneManager.Event_LevelLoaded += OnLevelLoaded;
        BB_AssignmentManager.Event_LevelCompleted += OnLevelCompleted;
    }

    private void OnDisable()
    {
        //Debug.Log(this.name + ": triggered OnDisable");

        BB_SceneManager.Event_LevelLoaded -= OnLevelLoaded;
        BB_AssignmentManager.Event_LevelCompleted -= OnLevelCompleted;
    }

    #endregion

    #region Event Reponses
    void OnLevelLoaded(BB_GameScenes.GameScenes _scene, bool _isReload)
    {
        switch(_scene)
        {
            case BB_GameScenes.GameScenes.IF_MainMenu1:
                PlayMusic(mainMenuClip, _isReload);
                break;
            case BB_GameScenes.GameScenes.BB_TestScene:
                PlayMusic(ezLevelClips, _isReload);
                break;
            case BB_GameScenes.GameScenes.LevelTutorial:
                PlayMusic(mainMenuClip, _isReload);
                break;
            case BB_GameScenes.GameScenes.LevelOne:
                PlayMusic(ezLevelClips, _isReload);
                break;
            case BB_GameScenes.GameScenes.LevelTwo:
                PlayMusic(medLevelClips, _isReload);
                break;
            case BB_GameScenes.GameScenes.LevelThree:
                PlayMusic(hardLevelClips, _isReload);
                break;

            case BB_GameScenes.GameScenes.Cutscene1:
                PlayMusic(cutsceneNeutral, _isReload);
                break;
            case BB_GameScenes.GameScenes.Cutscene2:
                PlayMusic(theTruth, _isReload);
                break;
            case BB_GameScenes.GameScenes.Cutscene3:
                PlayMusic(cutsceneNeutral, _isReload);
                break;
            case BB_GameScenes.GameScenes.Cutscene4:
                PlayMusic(theTruth, _isReload);
                break;
            case BB_GameScenes.GameScenes.Cutscene5:
                PlayMusic(cutsceneWhacky, _isReload);
                break;
            case BB_GameScenes.GameScenes.Cutscene6:
                PlayMusic(theTruth, _isReload);
                break;
            case BB_GameScenes.GameScenes.Cutscene7:
                PlayMusic(cutsceneNeutral, _isReload);
                break;
            case BB_GameScenes.GameScenes.Cutscene8:
                PlayMusic(cutsceneWhacky, _isReload);
                break;

            case BB_GameScenes.GameScenes.VictoryScrean:
                PlayMusic(gameWinClip, _isReload);
                break;

            default:
                musicSource.Stop();
                break;
        }
    }

    void OnLevelCompleted(object sender, EventArgs e)
    {
        musicSource.volume = levelCompleteMusicVolume;
    }

    #endregion
    void PlayMusic(List<AudioClip> _clips, bool _isReload)
    {
        musicSource.volume = 1;
        if (_isReload)
            return;

        if(musicCor != null)
            StopCoroutine(musicCor);

        musicCor = PlayMusicCor(_clips);
        StartCoroutine(musicCor);
    }
    IEnumerator PlayMusicCor(List<AudioClip> _clips)
    {
        if(_clips.Count > 1)
        {
            //intro
            musicSource.loop = false;
            musicSource.clip = _clips[0];
            musicSource.Play();

            yield return new WaitForSeconds(_clips[0].length);
        }

        // main loop
        musicSource.loop = true;
        musicSource.clip = _clips[_clips.Count - 1];
        musicSource.Play();

    }
}
