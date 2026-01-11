using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BB_CommonLevelStuff;
using UnityEngine.UI;
using System;

public class IF_Lock : MonoBehaviour
{
    [SerializeField] private Transform movePoint;
    [SerializeField] private Door linkedDoor;
     [SerializeField] private GameObject movePointHolder;
    private bool occupied = false;

    public Vector3 MovePoint
    {
        get
        {
            if (movePointHolder != null)
                return movePointHolder.transform.position;
            else
            {
                Debug.LogError(this.name + ": not movePointHolder");
                return Vector3.zero;
            }
        }
    }
    private BB_NavMeshAgent assignedAgent;


    public bool IsAvailable { get; private set; } = true;

   public void OnEnter(BB_NavMeshAgent _agent)
    {
        Debug.Log($"[SWITCH] {name} OnEnter by agent");

        IsAvailable = false;

        assignedAgent = _agent;
        linkedDoor.DoorOpen();
    }

    public void OnExit()
    {
        Debug.Log($"[SWITCH] {name} OnExit by agent");

        IsAvailable = true;
        linkedDoor.DoorClose();
    }
}
