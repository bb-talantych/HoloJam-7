using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BB_CommonLevelStuff;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(AudioSource))]
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
    private IF_IInteractablehold currentInteractableHold;
    public event EventHandler Event_AgentFinishedMoving;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AudioSource sfxSource;



    private void OnEnable()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        IsAvailable = true;
        isMoving = false;
        Event_AgentFinishedMoving += HandleArrived;

        // testing
        stats = new Stats(-3, 3);
    }

    private void HandleArrived(object sender, EventArgs e)
    {
        Debug.Log($"[AGENT] Arrived. Hold = {(currentInteractableHold == null ? "NULL" : currentInteractableHold.ToString())}");
        if (currentInteractableHold != null)
        {
            currentInteractableHold.OnEnter(TalentStats);
        }
    }

    public void MoveToHoldInteraction(IF_IInteractablehold hold)
    {
        currentInteractableHold = hold;
        MoveToPoint(hold.MovePoint);
    }

    private void FixedUpdate()
    {
        if (FinishedMovingCondition())
        {
            isMoving = false;
            //Debug.Log(this.name + ": finished moving");

            Event_AgentFinishedMoving?.Invoke(this, EventArgs.Empty);
            animator.SetTrigger("Stopped");
        }
    }


    public void MoveToPoint(Vector3 _destination)
    {
        //Debug.Log(this.name + ": moves to point");

        isMoving = true;
        agent.SetDestination(_destination);

        animator.SetTrigger("Moving");
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
    public void AgentSelected()
    {
        BB_CommonDataManager.Instance.PlayClip(sfxSource, BB_CommonDataManager.Instance.characterSelectedClips);
    }

    public void LeaveHold()
    {
        if (currentInteractableHold != null)
        {
            currentInteractableHold.OnExit();
            currentInteractableHold = null;
        }

    }

    private void Osable()
    {
        Event_AgentFinishedMoving -= HandleArrived;
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
