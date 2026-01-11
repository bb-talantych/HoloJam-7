using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject obstructionPlane;

    private void OnEnable()
    {
        obstructionPlane.SetActive(true);
    }
    public void DoorClose()
    {
        Debug.Log("door closed");

        obstructionPlane.SetActive(true);
    }

    public void DoorOpen()
    {
        Debug.Log("door open");

        obstructionPlane.SetActive(false);
    }
    

}
