using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BB_CommonStuff;

public class BB_NavMeshAgent : MonoBehaviour
{
    public Stats stats = default;

    private NavMeshAgent agent;

    private void OnEnable()
    {
        if(agent == null) 
            agent = GetComponent<NavMeshAgent>();

        // testing
        stats.strenght = UnityEngine.Random.Range(-3, 3);
        stats.dexterity = UnityEngine.Random.Range(-3, 3);
        stats.intellect = UnityEngine.Random.Range(-3, 3);
        stats.charisma = UnityEngine.Random.Range(-3, 3);
    }



    public void MoveTo(Vector3 _destination)
    {
        Debug.Log(this.name + ": moves to");
        agent.SetDestination(_destination);
    }
    public void AssignTask(Vector3 _destination)
    {
        Debug.Log(this.name + ": assigned task");
        MoveTo(_destination);
    }
}
