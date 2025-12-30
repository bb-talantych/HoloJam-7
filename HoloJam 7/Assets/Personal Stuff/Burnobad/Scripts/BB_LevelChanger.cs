using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BB_LevelChanger : MonoBehaviour
{
    [SerializeField]
    private string levelName;

    public void CallLevelChange()
    {
        BB_SceneManager.Instance.LoadLevel(levelName, this.name);
    }
}
