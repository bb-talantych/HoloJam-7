using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public List<BB_NavMeshAgent> sceneAgents =
    new List<BB_NavMeshAgent>();
    public List<BB_Task> sceneTasks =
        new List<BB_Task>();

    public Camera cam;
    public BB_NavMeshAgent SelectedAgent
    { get; private set; }

    //tracks which task the agent is currently assigned to
    private Dictionary<BB_NavMeshAgent, BB_Task> agent_task_dic =
        new Dictionary<BB_NavMeshAgent, BB_Task>();
    //tracks which agent started working on the task
    private Dictionary<BB_Task, BB_NavMeshAgent> task_agent_dic =
        new Dictionary<BB_Task, BB_NavMeshAgent>();
    //tracks the completion of tasks
    private Dictionary<BB_Task, bool> task_completion_dic =
       new Dictionary<BB_Task, bool>();

    public static event Action Event_LevelCompleted;
    public event Action<BB_NavMeshAgent> Event_AgentSelected;
    public event Action<BB_NavMeshAgent> Event_AgentFinishedTask;

    #region On Enable/Disable
    private void OnEnable()
    {
        instance = this;

        foreach (BB_NavMeshAgent agent in sceneAgents)
        {
            agent.Event_AgentFinishedMoving += OnAgentFinishedMoving;
        }
        foreach (BB_Task task in sceneTasks)
        {
            task.Event_TaskAssigned += OnTaskAssigned;
            task.Event_TaskFinished += OnTaskFinished;
        }
    }

    private void OnDisable()
    {
        //Debug.Log(this.name.ToString() + ": triggered OnDisable");

        foreach (BB_NavMeshAgent agent in sceneAgents)
        {
            agent.Event_AgentFinishedMoving -= OnAgentFinishedMoving;
        }
        foreach (BB_Task task in sceneTasks)
        {
            task.Event_TaskAssigned -= OnTaskAssigned;
            task.Event_TaskFinished -= OnTaskFinished;
        }
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
                if(SelectionCondition(hit, out BB_NavMeshAgent agent))
                {
                    SelectAgent(agent);
                }
                else
                {
                    SelectAgent(null);
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && SelectedAgent != null)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                BB_Task selectedTask;
                if (AssignCondition(hit, out selectedTask))
                {
                    SelectedAgent.MoveToTask(selectedTask);
                    OverwriteDic(SelectedAgent, selectedTask);
                }
                else
                {
                    SelectedAgent.MoveToPoint(hit.point);
                    OverwriteDic(SelectedAgent, null);
                }
            }
        }
    }

    void SelectAgent(BB_NavMeshAgent _agent)
    {
        SelectedAgent = _agent;
        Event_AgentSelected?.Invoke(SelectedAgent);
    }
    void CheckLevelComplete()
    {
        if (LevelCompleteCondition())
        {
            Debug.Log(this.name + ": Level Complete");
            Event_LevelCompleted?.Invoke();
        }
    }

    #region Conditions
    bool SelectionCondition(RaycastHit _hit, out BB_NavMeshAgent agent)
    {
        agent = null;
        if (_hit.collider.gameObject.layer != LayerMask.NameToLayer("characters"))
            return false;

        BB_NavMeshAgent hitAgent = _hit.collider.gameObject.GetComponent<BB_NavMeshAgent>();
        if (hitAgent == null)
            return false;
        if (!hitAgent.IsAvailable)
            return false;

        agent = hitAgent;
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
        if (!task.IsAvailable)
            return false;

        selectedTask = task;
        return true;
    }
    bool StoppingCondition(BB_Task _task, BB_NavMeshAgent _agent)
    {
        if (!agent_task_dic.ContainsKey(_agent))
        {
            return false;
        }
        BB_Task task = agent_task_dic[_agent];
        if (task_agent_dic[_task] != _agent && _task == task)
        {
            return true;
        }

        return false;
    }
    bool LevelCompleteCondition()
    {
        if (task_completion_dic.Count != sceneTasks.Count)
            return false;

        foreach (BB_Task task in sceneTasks)
        {
            if (task_completion_dic[task] == false)
            {
                return false;
            }
        }

        return true;
    }

    #endregion
    #region Event Responses

    private void OnAgentFinishedMoving(BB_NavMeshAgent _agent)
    {
        BB_Task task = null;

        if(agent_task_dic.ContainsKey(_agent))
        {
            task = agent_task_dic[_agent];
        }
        if (task != null)
        {
            //testing
            if (SelectedAgent == _agent)
                SelectAgent(null);

            //Debug.Log(this.name + ": " + _agent.name + " starts " + currentTask);
            OverwriteDic(task, _agent);

            task.StartTask(_agent.TalentStats);
            _agent.StartTask();
        }
    }
    private void OnTaskAssigned(BB_Task _task)
    {
        //Debug.Log(this.name + ": " + _task.name + " is assigned");
        foreach (BB_NavMeshAgent agent in sceneAgents)
        {
            if (StoppingCondition(_task, agent))
            {
                Debug.Log(this.name + ": " + agent.name + " will stop moving");
                agent.StopMoving();
                OverwriteDic(agent, null);
            }
        }
    }
    private void OnTaskFinished(BB_Task _task)
    {
        //Debug.Log(this.name + ": " + _task.name + " is finished");

        BB_NavMeshAgent agent = task_agent_dic[_task];
        OverwriteDic(agent, null);
        OverwriteDic(_task, true);

        _task.FinishTask();
        agent.FinishTask();
        Event_AgentFinishedTask?.Invoke(agent);

        CheckLevelComplete();
    }

    #endregion
    #region Dictionary Stuff
    private void OverwriteDic(BB_Task _task, BB_NavMeshAgent _agent)
    {
        if (task_agent_dic.ContainsKey(_task))
        {
            task_agent_dic.Remove(_task);
        }
        task_agent_dic.Add(_task, _agent);
    }
    private void OverwriteDic(BB_NavMeshAgent _agent, BB_Task _task)
    {
        if (agent_task_dic.ContainsKey(_agent))
        {
            agent_task_dic.Remove(_agent);
        }
        agent_task_dic.Add(_agent, _task);
    }
    private void OverwriteDic(BB_Task _task, bool _completed)
    {
        if (task_agent_dic.ContainsKey(_task))
        {
            task_completion_dic.Remove(_task);
        }
        task_completion_dic.Add(_task, _completed);
    }

    #endregion
}
