using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BB_CommonStuff;
using UnityEngine.TextCore.Text;

public class BB_NavMeshAgent : MonoBehaviour
{
    public Stats stats = default;
    public Stats TalentStats
    {
        get
        {
            return stats;
        }
    }

    public bool IsAvailable
    { get; private set; }
    private bool isMoving;
    private NavMeshAgent agent;
    private BB_Task potentialTask;

    private void OnEnable()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        IsAvailable = true;
        isMoving = false;
        potentialTask = null;

        // testing
        stats = new Stats(-3, 3);
    }

    private void FixedUpdate()
    {
        if (FinishedMovingCondition())
        {
            isMoving = false;
            Debug.Log("Finished Moving");
            
            if(potentialTask != null)
            {
                BB_AssignmentManager.Instance.AskForAssignment(this, potentialTask);
            }
        }
    }

    public void MoveToPoint(Vector3 _destination)
    {
        Debug.Log(this.name + ": moves to point");

        isMoving = true;
        potentialTask = null;
        agent.SetDestination(_destination);
    }
    public void MoveToTask(BB_Task _task)
    {
        Debug.Log(this.name + ": moves to task");

        isMoving = true;
        potentialTask = _task;
        agent.SetDestination(_task.MovePoint);
    }
    public void AssignTask(Vector3 _destination)
    {
        Debug.Log(this.name + ": assigned task");
    }

    bool FinishedMovingCondition()
    {
        if (!isMoving)
            return false;
        if (agent.pathPending)
            return false;
        if (agent.hasPath)
            return false; 
        if (agent.remainingDistance > agent.stoppingDistance)
            return false;

        return true;
    }

}
