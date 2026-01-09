using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool DoorOpen = false;
    [SerializeField] private int requiredSwitchToOpen = 1;
    //[SerializeField] private SlideDoorManager slideDoorManager;
    private List<IF_Lock> currentSwitchesOpen = new();


    public void AddSwitch(IF_Lock curSwitch)
    {
        //if (!currentSwitchesOpen.Contains curSwitch)
        //{
        //    currentSwitchesOpen.Add curSwitch;
        //}
        TryOpen();
    }
        public void removeSwitch(IF_Lock curSwitch)
    {
        //if (!currentSwitchesOpen.Contains curSwitch)
        //{
        //    currentSwitchesOpen.Remove curSwitch;
        //}
        TryOpen();
    }

    private void TryOpen()
    {
        if(currentSwitchesOpen.Count == requiredSwitchToOpen)
        {
            OpenDoor();
        } else if (currentSwitchesOpen.Count < requiredSwitchToOpen)
        {
            CloseDoor();
        }
    }

    private void CloseDoor()
    {
        DoorOpen = false;
    }

    private void OpenDoor()
    {
        DoorOpen = true;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
