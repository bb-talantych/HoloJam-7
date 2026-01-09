using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BB_CommonStuff;
using UnityEngine.UI;
using System;

public class IF_Lock : MonoBehaviour, IF_IInteractablehold
{
    [SerializeField] private Transform movePoint;
    [SerializeField] private Door linkedDoor;
     [SerializeField] private GameObject movePointHolder;
    private int occupants = 0;


    public Vector3 MovePoint
    {
        get
        {
            if (movePointHolder != null)
                return movePointHolder.transform.position;

            Debug.LogError($"{name}: no movePointHolder");
            return transform.position;
        }
    }

    public bool IsAvailable { get; private set; } = true;

   public void OnEnter(Stats stats)
    {
        Debug.Log($"[SWITCH] {name} OnEnter by agent");
        occupants++;
        if (occupants == 1)
        {
            linkedDoor.DoorOpen();
        }
    }

    public void OnExit()
    {
        Debug.Log($"[SWITCH] {name} OnExit by agent");
        occupants--;
        if (occupants <= 0)
        {
            occupants = 0;
            linkedDoor.DoorClose();
        }
    }
}
