using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BB_CommonStuff;
using UnityEngine.UI;
using System;

public class IF_Lock : MonoBehaviour
{
    [SerializeField] private Door curDoor;
    [SerializeField] private Animator animator;
    private GameObject movePointHolder;

    public Vector3 MovePoint
    { 
        get 
        { 
            if(movePointHolder != null)
                return movePointHolder.transform.position;
            else
            {
                Debug.LogError(this.name + ": not movePointHolder");
                return Vector3.zero;
            }
        }
    }
    public bool IsAvailable
    {
        get;
        private set;
    }

    public float timeToComplete = 10f;

 
    private AudioClip sfxClip;

    public event EventHandler Event_TaskAssigned;


    private void OnEnable()
    {
        IsAvailable = true;
        curDoor.AddSwitch(this);
        

    }

    



    public void FinishTask()
    {
        Debug.Log(this.name + ": task finished");
    }

   
}
