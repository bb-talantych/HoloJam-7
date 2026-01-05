using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BB_Scenes;

public class BB_LevelChanger : MonoBehaviour
{
    public BB_GameScenes.GameScenes scene = BB_GameScenes.GameScenes.BB_TestScene;

    public void CallLevelChange()
    {
       BB_SceneManager.Instance.LoadLevel(scene, this.name);
    }
}
