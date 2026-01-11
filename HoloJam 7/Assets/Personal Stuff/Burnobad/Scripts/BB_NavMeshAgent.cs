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
    public event EventHandler Event_AgentFinishedMoving;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AudioSource sfxSource;

    //testimg
    public event EventHandler Event_AgentStartedMoving;


    private void OnEnable()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        if(sfxSource == null)
            sfxSource = GetComponent<AudioSource>();

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
            animator.SetTrigger("Stopped");
        }
    }
    public void MoveTo(Vector3 _destination)
    {
        isMoving = true;
        agent.SetDestination(_destination);

        animator.SetTrigger("Moving");

        Event_AgentStartedMoving?.Invoke(this, EventArgs.Empty);
    }
    public void StopMoving()
    {
        //testing
        MoveTo(transform.position);
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
