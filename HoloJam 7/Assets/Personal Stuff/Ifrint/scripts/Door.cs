using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    [SerializeField] private NavMeshObstacle navObstacle;
    [SerializeField] private Collider doorCollider;

    private bool isOpen;
    
    private void Awake()
    {
        if (!navObstacle)
            navObstacle = GetComponent<NavMeshObstacle>();

        if (!doorCollider)
            doorCollider = GetComponent<Collider>();
    }

    public void DoorClose()
    {
        if (!isOpen) return;
        isOpen = false;

        doorCollider.enabled = true;
        navObstacle.enabled = true;
    }

    public void DoorOpen()
    {
        if (isOpen) return;
        isOpen = true;

        doorCollider.enabled = false;
        navObstacle.enabled = false;
    }
    

}
