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

    public event EventHandler Event_AgentFinishedMoving;

    private void OnEnable()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        IsAvailable = true;
        isMoving = false;

        // testing
        stats = new Stats(-3, 3);
    }

    private void FixedUpdate()
    {
        if (FinishedMovingCondition())
        {
            isMoving = false;
            //Debug.Log(this.name + ": finished moving");

            Event_AgentFinishedMoving?.Invoke(this, EventArgs.Empty);
        }
    }

    public void MoveToPoint(Vector3 _destination)
    {
        //Debug.Log(this.name + ": moves to point");

        isMoving = true;
        agent.SetDestination(_destination);
    }
    public void MoveToTask(BB_Task _task)
    {
        Debug.Log(this.name + ": moves to task");
        MoveToPoint(_task.MovePoint);
    }
    public void StopMoving()
    {
        //testing
        MoveToPoint(transform.position);
    }
    public void StartTask()
    {
        Debug.Log(this.name + ": started task");

        IsAvailable = false;
    }
    public void FinishTask()
    {
        Debug.Log(this.name + ": finished task");

        IsAvailable = true;
    }

    #region Conditions
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

    #endregion

}
