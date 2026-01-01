using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BB_AssignmentManager : MonoBehaviour
{
    private static BB_AssignmentManager instance;
    public static BB_AssignmentManager Instance
    {
        get
        {
            if (instance != null)
                return instance;
            else
            {
                Debug.LogError("BB_AssignmentManager instance not setup");
                return null;
            }

        }
    }

    // All Events must be here
    #region On Enable/Disable
    private void OnEnable()
    {
        Debug.Log(this.name.ToString() + ": triggered OnEnable");

        if (instance == null)
            instance = this;
    }

    private void OnDisable()
    {
        Debug.Log(this.name.ToString() + ": triggered OnDisable");
    }

    #endregion

    public Camera cam;
    public BB_NavMeshAgent selectedAgent;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                SelectionCondition(hit, out selectedAgent);
            }
        }

        if (Input.GetMouseButtonDown(1) && selectedAgent != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                BB_Task selectedTask;
                if(AssignCondition(hit, out selectedTask))
                {
                    selectedAgent.MoveToTask(selectedTask);
                }
                else
                {
                    selectedAgent.MoveToPoint(hit.point);
                }
            }
        }
    }

    public void AskForAssignment(BB_NavMeshAgent _agent, BB_Task _selectedTask)
    {
        Debug.Log(_agent.name + ": ask for assignment " + _selectedTask);
        _selectedTask.Assign(_agent.TalentStats);
    }

    bool SelectionCondition(RaycastHit _hit, out BB_NavMeshAgent selectedAgent)
    {
        selectedAgent = null;
        if (_hit.collider.gameObject.layer != LayerMask.NameToLayer("characters"))
            return false;

        BB_NavMeshAgent hitAgent = _hit.collider.gameObject.GetComponent<BB_NavMeshAgent>();
        if (hitAgent == null)
            return false;
        if(!hitAgent.IsAvailable)
            return false;

        selectedAgent = hitAgent;
        return true;
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
