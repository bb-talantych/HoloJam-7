using UnityEngine;
using BB_Scenes;

[CreateAssetMenu(menuName = "Scene/Steps/FinishCutscene")]
public class FinishCutscene : SceneStep
{
    public BB_GameScenes.GameScenes scene = BB_GameScenes.GameScenes.BB_TestScene;

    public override void Execute()
    {
        Debug.Log("Add finish logic here");

        BB_SceneManager.Instance.LoadLevel(scene, this.name);
    }
}