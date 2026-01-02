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

    public Camera cam;
    public BB_NavMeshAgent selectedAgent;

    public List<BB_NavMeshAgent> sceneAgents;
    public List<BB_Task> sceneTasks;
    private Dictionary<BB_NavMeshAgent, BB_Task> agent_task_dic =
    new Dictionary<BB_NavMeshAgent, BB_Task>();
    private Dictionary<BB_Task, BB_NavMeshAgent> task_agent_dic = 
        new Dictionary<BB_Task, BB_NavMeshAgent>();

    // All Events must be here
    #region On Enable/Disable
    private void OnEnable()
    {
        Debug.Log(this.name.ToString() + ": triggered OnEnable");

        if (instance == null)
            instance = this;

        foreach (BB_NavMeshAgent agent in sceneAgents)
        {
            agent.Event_AgentFinishedMoving += OnAgentFinishedMoving;
            agent_task_dic.Add(agent, null);
        }
        foreach (BB_Task task in sceneTasks)
        {
            task.Event_TaskFinished += OnTaskFinished;
            task_agent_dic.Add(task, null);
        }

    }

    private void OnDisable()
    {
        Debug.Log(this.name.ToString() + ": triggered OnDisable");

        foreach (BB_NavMeshAgent agent in sceneAgents)
            agent.Event_AgentFinishedMoving -= OnAgentFinishedMoving;
        foreach (BB_Task task in sceneTasks)
            task.Event_TaskFinished -= OnTaskFinished;
    }

    #endregion

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
                    AddToDic(selectedAgent, selectedTask);  
                }
                else
                {
                    selectedAgent.MoveToPoint(hit.point);
                    AddToDic(selectedAgent, null);
                }
            }
        }
    }

    #region Conditions
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

    #endregion

    #region Event Responses
    private void OnAgentFinishedMoving(BB_NavMeshAgent _agent)
    {
        BB_Task task = agent_task_dic[_agent];
        if(task != null)
        {
            //testing
            if (selectedAgent == _agent)
                selectedAgent = null;

            //Debug.Log(this.name + ": " + _agent.name + " starts " + currentTask);
            AddToDic(task, _agent);

            task.StartTask(_agent.TalentStats);
            _agent.StartTask();
        }
    }
    private void OnTaskFinished(BB_Task _task)
    {
        //Debug.Log(this.name + ": " + _task.name + " is finished");

        BB_NavMeshAgent currentAgent = task_agent_dic[_task];
        AddToDic(currentAgent, null);

        _task.FinishTask();
        currentAgent.FinishTask();
    }

    #endregion

    private void AddToDic(BB_Task _task, BB_NavMeshAgent _agent)
    {
        task_agent_dic.Remove(_task);
        task_agent_dic.Add(_task, _agent);
    }
    private void AddToDic(BB_NavMeshAgent _agent, BB_Task _task)
    {
        agent_task_dic.Remove(_agent);
        agent_task_dic.Add(_agent, _task);
    }
}
