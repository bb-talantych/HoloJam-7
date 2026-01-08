using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace BB_CommonStuff
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
            LevelOne
        }

        public const string nonScene = "";
        public const string mainMenuScene = "BB_MainMenu";
        public const string bb_testingScene = "BB_TestScene";
        public const string IF_MainMenu1 = "IF_MainMenu1.1";
        public const string LevelOne = "IF_level1";


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
                case GameScenes.LevelOne:
                    scene = LevelOne;
                    break;
            }

            return scene;
        }
    }
}

