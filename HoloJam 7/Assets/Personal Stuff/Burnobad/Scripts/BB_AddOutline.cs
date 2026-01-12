using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BB_AddOutline : MonoBehaviour
{
    [Range(0f, 1f), SerializeField]
    private float OutlineSize = 0.015f;
    [SerializeField]
    private Color hoverColor = Color.black;
    [SerializeField]
    private Color selectedColor = Color.white;

    [HideInInspector]
    public Shader outlineShader_3d;
    [HideInInspector]
    public Shader outlineShader_2d;
    private Material outlineMaterial;

    [SerializeField]
    private MeshRenderer mshRenderer;
    private SpriteRenderer sprRenderer;

    private BB_Task task;
    private BB_NavMeshAgent agent;

    private BB_AssignmentManager assignmentManager;
    private bool forceOutline;
    private bool mouseOver;

    #region On Enable/Disable
    private void OnEnable()
    {
        if(task == null)
            TryGetComponent<BB_Task>(out task);
        if(task != null)
        {
            task.Event_TaskAssigned += OnTaskAssigned;
        }

        if (agent == null)
            TryGetComponent<BB_NavMeshAgent>(out agent);

        // intialize one of the renderers
        if (!HasRenderer()) 
        {
            TryGetComponent<MeshRenderer>(out mshRenderer);
            TryGetComponent<SpriteRenderer>(out sprRenderer);
        }
        // create outline material instance
        if(HasRenderer() && HasShaders())
        {
            outlineMaterial = mshRenderer != null ? 
                new Material(outlineShader_3d) : new Material(outlineShader_2d);
        }
        // add/assign this material, depending on renderer
        if(HasRenderer() && outlineMaterial != null)
        {
            if(mshRenderer != null)
            {
                var meshMaterials = mshRenderer.materials;
                var objMats = new List<Material>(meshMaterials);
                objMats.Add(outlineMaterial);
                mshRenderer.materials = objMats.ToArray();
            }
            else
            {
                sprRenderer.material = outlineMaterial;
            }
        }

        forceOutline = false;
        mouseOver = false;
    }

    private void OnDisable()
    {
        if (task != null)
        {
            task.Event_TaskAssigned -= OnTaskAssigned;
        }
        if (agent != null)
        {
            if(assignmentManager != null)
            {
                assignmentManager.Event_AgentSelected -= OnAgentSelected;
                assignmentManager.Event_AgentFinishedTask -= OnAgentFinishedTask;
            }
        }
    }

    private void Start()
    {
        assignmentManager = BB_AssignmentManager.Instance;
        if (agent != null)
        {
            if (assignmentManager != null)
            {
                assignmentManager.Event_AgentSelected += OnAgentSelected;
                assignmentManager.Event_AgentFinishedTask += OnAgentFinishedTask;
            }
        }
    }

    #endregion
    private void OnMouseEnter()
    {
        mouseOver = true;
        if (EnableOutlinesCondition())
        {
            if(agent != null && assignmentManager.SelectedAgent != agent)
            {
                EnableOutlines(hoverColor);
            }
            else if(agent == null && task == null)
            {
                Debug.Log(this.name);
                EnableOutlines(hoverColor);
            }
            else
            {
                EnableOutlines(selectedColor);
            }
        }
    }

    private void OnMouseExit()
    {
        mouseOver = false;
        if (DisableOutlinesCondition())
            DisableOutlines();
    }

    void EnableOutlines(Color _outlineColor)
    {
        outlineMaterial.SetFloat("_Thickness", OutlineSize);
        outlineMaterial.SetColor("_OutlineColor", _outlineColor);
        outlineMaterial.EnableKeyword("OUTLINE_ON");
    }
    void DisableOutlines()
    {
        outlineMaterial.DisableKeyword("OUTLINE_ON");
    }

    #region Bools and conditions
    bool HasRenderer()
    {
        if (mshRenderer == null && sprRenderer == null)
            return false;

        return true;
    }
    bool HasShaders()
    {
        if (outlineShader_3d == null && outlineShader_2d == null)
            return false;

        return true;
    }

    bool EnableOutlinesCondition()
    {
        if (outlineMaterial == null)
            return false;
        if(forceOutline)
            return true;

        if(task != null)
        {
            if(!task.IsAvailable)
                return false;
            if (BB_AssignmentManager.Instance.SelectedAgent == null)
                return false;
        }
        
        if(agent != null)
        {
            if (!agent.IsAvailable)
                return false;
        }


        return true;
    }
    bool DisableOutlinesCondition()
    {
        if (outlineMaterial == null)
            return false;
        if (forceOutline)
            return false;

        return true;
    }

    #endregion
    #region For Events
    void OnTaskAssigned(object _sender, EventArgs e)
    {
        BB_Task assignedTask = (BB_Task)_sender;
        if (task == assignedTask) 
        {
            forceOutline = false;
            DisableOutlines();
        }
    }
    void OnAgentSelected(BB_NavMeshAgent _agent)
    {
        forceOutline = agent == _agent;
        if(forceOutline)
        {
            EnableOutlines(selectedColor);
        }
        else
        {
            DisableOutlines();
        }
    }
    void OnAgentFinishedTask(BB_NavMeshAgent _agent)
    {
        if(agent == _agent && mouseOver)
        {
            EnableOutlines(hoverColor);
        }
    }

    #endregion

}

