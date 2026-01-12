using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BB_OcclusionPlane : MonoBehaviour
{
    [SerializeField]
    private List<BB_NavMeshAgent> agents;

    private void Start()
    {
        agents.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<BB_NavMeshAgent>(out BB_NavMeshAgent agent))
        {
            agents.Add(agent);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<BB_NavMeshAgent>(out BB_NavMeshAgent agent))
        {
            agents.Remove(agent);
        }
    }

    public bool CheckAgent(BB_NavMeshAgent _agent)
    {
        return agents.Contains(_agent);
    }
}
