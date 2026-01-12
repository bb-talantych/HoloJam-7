using UnityEngine;

public class BB_ExitButton : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Game is exiting");
        Application.Quit();
    }
}
