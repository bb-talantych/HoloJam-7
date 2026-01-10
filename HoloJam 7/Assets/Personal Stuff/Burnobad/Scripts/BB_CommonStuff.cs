using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;



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
            LevelTutorial,
            LevelOne,
            LevelTwo,
            LevelThree
        }

        public const string nonScene = "";
        public const string mainMenuScene = "BB_MainMenu";
        public const string bb_testingScene = "BB_TestScene";
        public const string IF_MainMenu1 = "IF_MainMenu1.1";
        public const string LevelTutorial = "IF_levelTutorial";
        public const string LevelOne = "IF_level1";
        public const string LevelTwo= "IF_level2";
        public const string LevelThree = "IF_level3";


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
                case GameScenes.LevelTutorial:
                    scene = LevelTutorial;
                    break;
                case GameScenes.LevelOne:
                    scene = LevelOne;
                    break;
                case GameScenes.LevelTwo:
                    scene = LevelTwo;
                    break;
                case GameScenes.LevelThree:
                    scene = LevelThree;
                    break;
            }

            return scene;
        }
    }
}

