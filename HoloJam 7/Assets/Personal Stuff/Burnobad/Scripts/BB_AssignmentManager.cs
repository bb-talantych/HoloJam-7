using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BB_AssignmentManager : MonoBehaviour
{
    public Camera cam;
    public BB_NavMeshAgent selectedAgent;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("characters"))
                {
                    BB_NavMeshAgent hitAgent = hit.collider.gameObject.GetComponent<BB_NavMeshAgent>();
                    selectedAgent = hitAgent;
                }
                else
                {
                    selectedAgent = null;
                }
            }
        }

        if (Input.GetMouseButton(1) && selectedAgent != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                BB_Task selectedTask;
                if(AssignCondition(hit, out selectedTask))
                {
                    selectedAgent.AssignTask(hit.point);
                    selectedTask.Assign();
                }
                else
                {
                    selectedAgent.MoveTo(hit.point);
                }
            }
        }
    }

    bool AssignCondition(RaycastHit _hit, out BB_Task selectedTask)
    {
        selectedTask = null;
        if (_hit.collider.gameObject.tag != "Task")
            return false;

        BB_Task task = _hit.collider.gameObject.GetComponent<BB_Task>();
        if (task == null)
            return false; 
        if(task.isAssigned)
            return false;

        selectedTask = task;
        return true;
    }
}
