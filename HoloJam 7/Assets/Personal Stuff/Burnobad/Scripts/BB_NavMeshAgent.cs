using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BB_NavMeshAgent : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Awake()
    {
        if(agent == null) 
            agent = GetComponent<NavMeshAgent>();
    }



    public void MoveTo(Vector3 _destination)
    {
        agent.SetDestination(_destination);
    }
    public void AssignTask(Vector3 _destination)
    {
        Debug.Log(this.name + ": assigned task");
        MoveTo(_destination);
    }
}
