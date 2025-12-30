using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BB_MainMenuManager : MonoBehaviour
{
    public static event Action Event_StartButton;

    //Testing
    public void StartButton()
    {
        Debug.Log(this.name.ToString() + ": Start Button Clicked");
        Event_StartButton?.Invoke();
    }
}
