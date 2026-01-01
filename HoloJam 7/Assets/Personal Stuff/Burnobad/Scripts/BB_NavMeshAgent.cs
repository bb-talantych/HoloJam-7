using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BB_NavMeshAgent : MonoBehaviour
{
    public Camera cam;   

    public NavMeshAgent agent;

    public bool selected = false;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("characters"))
                {
                    BB_NavMeshAgent hitAgent = hit.collider.gameObject.GetComponent<BB_NavMeshAgent>();
                    if(this == hitAgent)
                        hitAgent.selected = true;
                    else
                        selected = false;
                }
                else
                {
                    selected = false;
                }
            }
        }

        if (Input.GetMouseButton(1) && selected)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }

}
