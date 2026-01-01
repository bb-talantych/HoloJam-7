using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BB_Task : MonoBehaviour
{
    public bool isAssigned
    {
        get;
        private set;
    }

    private void OnEnable()
    {
        isAssigned = false;
    }

    public void Assign()
    {
        Debug.Log(this.name + " is assigned");
        isAssigned = true;
    }

}
