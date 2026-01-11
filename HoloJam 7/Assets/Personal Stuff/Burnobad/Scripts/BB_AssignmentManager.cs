using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
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

    //tracks the completion of tasks
    private Dictionary<BB_NavMeshAgent, IF_Lock> agent_lock_dic =
       new Dictionary<BB_NavMeshAgent, IF_Lock>();

    public static event EventHandler Event_LevelCompleted;
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
                if (SelectionCondition(hit, out BB_NavMeshAgent agent))
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
                // dump way for occlussion plane to work
                if (hit.collider.tag == "NoGo")
                {
                    if(hit.collider.TryGetComponent<BB_OcclusionPlane>(out BB_OcclusionPlane plane))
                    {
                        if (!plane.CheckAgent(SelectedAgent))
                            return;
                    }
                }

                if (AssignTaskCondition(hit, out BB_Task selectedTask))
                {
                    SelectedAgent.MoveTo(selectedTask.MovePoint);
                    OverwriteDic(SelectedAgent, selectedTask);
                    selectedTask.TaskSelected();
                }
                else if(IF_LockCondition(hit, out IF_Lock if_lock))
                {
                    SelectedAgent.MoveTo(if_lock.MovePoint);
                    OverwriteLockDic(SelectedAgent, if_lock);
                }
                else
                {
                    SelectedAgent.MoveTo(hit.point);
                    OverwriteDic(SelectedAgent, null);
                    OverwriteLockDic(SelectedAgent, null);
                }
            }
        }
    }

    void SelectAgent(BB_NavMeshAgent _agent)
    {
        SelectedAgent = _agent;
        Event_AgentSelected?.Invoke(SelectedAgent);
        if (_agent != null)
            _agent.AgentSelected();
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
    bool AssignTaskCondition(RaycastHit _hit, out BB_Task selectedTask)
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
        if (_task != task)
        {
            return false;
        }
        if (task_agent_dic[_task] == _agent)
        {
            return false;
        }

        return true;
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

    bool IF_LockCondition(RaycastHit _hit, out IF_Lock if_lock)
    {
        if_lock = null;
        if (_hit.collider.gameObject.tag != "Lock")
            return false;

        IF_Lock selectedLock = _hit.collider.gameObject.GetComponent<IF_Lock>();
        if (selectedLock == null)
            return false;
        if (!selectedLock.IsAvailable)
            return false;

        if_lock = selectedLock;
        return true;
    }

    #endregion
    #region Event Responses
    private void OnAgentFinishedMoving(object _sender, EventArgs e)
    {
        BB_NavMeshAgent agent = (BB_NavMeshAgent)_sender;

        BB_Task task = null;
        if (agent_task_dic.ContainsKey(agent))
        {
            task = agent_task_dic[agent];
        }
        if (task != null)
        {
            //testing
            if (SelectedAgent == agent)
                SelectAgent(null);

            //Debug.Log(this.name + ": " + agent.name + " starts " + currentTask);
            OverwriteDic(task, agent);

            task.StartTask(agent.TalentStats);
            agent.StartTask();
        }

        IF_Lock if_lock = null;
        if (agent_lock_dic.ContainsKey(agent))
        {
            if_lock = agent_lock_dic[agent];
        }
        if (if_lock != null)
        {
            //testing
            if (SelectedAgent == agent)
                SelectAgent(null);

            if_lock.OnEnter(agent);
        }
    }
    private void OnTaskAssigned(object _sender, EventArgs e)
    {
        //Debug.Log(this.name + ": " + task.name + " is assigned");

        BB_Task task = (BB_Task)_sender;
        foreach (BB_NavMeshAgent agent in sceneAgents)
        {
            if (StoppingCondition(task, agent))
            {
                Debug.Log(this.name + ": " + agent.name + " will stop moving");
                agent.StopMoving();
                OverwriteDic(agent, null);
            }
        }
    }
    private void OnTaskFinished(object _sender, EventArgs e)
    {
        //Debug.Log(this.name + ": " + task.name + " is finished");

        BB_Task task = (BB_Task)_sender;
        BB_NavMeshAgent agent = task_agent_dic[task];
        OverwriteDic(agent, null);
        OverwriteDic(task, true);

        task.FinishTask();
        agent.FinishTask();
        Event_AgentFinishedTask?.Invoke(agent);

        IF_EmergencyTask emergency = task.GetComponent<IF_EmergencyTask>();
        if (emergency != null)
        {
            emergency.Complete();
            Debug.Log("emergency task complete");
        }

        if (LevelCompleteCondition())
        {
            Debug.Log(this.name + ": Level Complete");
            Event_LevelCompleted?.Invoke(this, EventArgs.Empty);
        }
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
    private void OverwriteLockDic(BB_NavMeshAgent _agent, IF_Lock _lock)
    {
        if (agent_lock_dic.ContainsKey(_agent))
        {
            agent_lock_dic.Remove(_agent);
        }
        agent_lock_dic.Add(_agent, _lock);
    }

    #endregion

}
