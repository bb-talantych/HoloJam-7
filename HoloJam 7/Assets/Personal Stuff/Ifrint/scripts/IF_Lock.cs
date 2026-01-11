using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BB_CommonLevelStuff;
using UnityEngine.UI;
using System;

public class IF_Lock : MonoBehaviour
{
    [SerializeField] private Door linkedDoor;
     [SerializeField] private GameObject movePointHolder;

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
        assignedAgent.Event_AgentStartedMoving += OnExit;

        if(linkedDoor != null)
            linkedDoor.DoorOpen();
    }

    public void OnExit(object _sender, EventArgs  e)
    {
        if ((BB_NavMeshAgent)_sender != assignedAgent)
            return;

        Debug.Log($"[SWITCH] {name} OnExit by agent");

        assignedAgent.Event_AgentStartedMoving -= OnExit;
        IsAvailable = true;
        assignedAgent = null;

        if (linkedDoor != null)
            linkedDoor.DoorClose();
    }

}
