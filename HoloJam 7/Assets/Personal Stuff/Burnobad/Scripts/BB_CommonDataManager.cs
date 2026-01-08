using System;
using System.Collections.Generic;
using UnityEngine;

public class BB_CommonDataManager : MonoBehaviour
{
    public static BB_CommonDataManager instance;
    public static BB_CommonDataManager Instance
    {
        get
        {
            if (instance != null)
                return instance;
            else
            {
                Debug.LogError("BB_CommonDataManager instance not setup");
                return null;
            }

        }
    }

    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }

    public List<AudioClip> characterSelectedClips;

    public List<AudioClip> taskSelectedClips;
    public List<AudioClip> taskCompleteClips;

    public void PlayClip(AudioSource _sfxSource, List<AudioClip> _clipsList)
    {
        if (_sfxSource == null)
            return;

        if (_clipsList.Count == 0)
            return;

        int max = _clipsList.Count - 1;
        AudioClip clip = _clipsList[UnityEngine.Random.Range(0, max)];

        _sfxSource.loop = false;
        _sfxSource.clip = clip;
        _sfxSource.Play();
    }

}
namespace BB_CommonLevelStuff
{
    [Serializable]
    public struct Stats
    {
        public int strenght;
        public int dexterity;
        public int intellect;
        public int charisma;

        public Stats(int min, int max)
        {
            strenght = UnityEngine.Random.Range(min, max);
            dexterity = UnityEngine.Random.Range(min, max);
            intellect = UnityEngine.Random.Range(min, max);
            charisma = UnityEngine.Random.Range(min, max);
        }
    }
}

namespace BB_Scenes
{
    public static class BB_GameScenes
    {
        public enum GameScenes
        {
            NoScene,
            MainMenu,
            BB_TestScene,
            IF_MainMenu1,
        }

        public const string nonScene = "";
        public const string mainMenuScene = "BB_MainMenu";
        public const string bb_testingScene = "BB_TestScene";
        public const string IF_MainMenu1 = "IF_MainMenu1.1";

        public static string GetScene(GameScenes _selectedScene)
        {
            string scene = bb_testingScene;

            switch (_selectedScene)
            {
                case GameScenes.NoScene:
                    scene = nonScene;
                    break;
                case GameScenes.MainMenu:
                    scene = mainMenuScene;
                    break;
                case GameScenes.BB_TestScene:
                    scene = bb_testingScene;
                    break;
                case GameScenes.IF_MainMenu1:
                    scene = IF_MainMenu1;
                    break;
            }

            return scene;
        }
    }
}
