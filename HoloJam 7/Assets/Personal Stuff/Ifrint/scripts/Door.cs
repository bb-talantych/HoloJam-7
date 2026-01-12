using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour
{
    [SerializeField] List<GameObject> obstructionPlanes;
    private void OnEnable()
    {
        SetPlanes(true);
    }
    public void DoorClose()
    {
        Debug.Log("door closed");

        SetPlanes(true);
    }

    public void DoorOpen()
    {
        Debug.Log("door open");

        SetPlanes(false);
    }
    
    void SetPlanes(bool _state)
    {
        if (obstructionPlanes.Count == 0)
            Debug.LogError("Ifiprint add obstruction plane prefab to door and make this prefab be just above the ground to not obstruct characters");

        foreach (var plane in obstructionPlanes) 
        {
            plane.SetActive(_state);
        }
    }

}
