using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BB_Task : MonoBehaviour
{
    [SerializeField]
    private GameObject movePointHolder;
    public bool isAssigned
    {
        get;
        private set;
    }

    private float taskSpeed = 1f;

    private void OnEnable()
    {
        isAssigned = false;
    }

    public void Assign(out Vector3 movePoint)
    {
        Debug.Log(this.name + " is assigned");
        isAssigned = true;
        movePoint = movePointHolder.transform.position;
    }

}
