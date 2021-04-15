using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorScript : MonoBehaviour
{
    [SerializeField]
    private Transform DoorPosition;
    

    private bool isOpen;

    [SerializeField] private Transform closed;
    [SerializeField] private Transform open;

    [ContextMenu("Open the Door.")]
    public void OpenDoor()
    {
        if (isOpen)
        {
            return;
        }
        DoorPosition.position = open.position;
        DoorPosition.rotation = open.rotation;
        isOpen = true;
    }

    [ContextMenu("Close the door.")]
    public void CloseDoor()
    {
        if (!isOpen)
        {
            return;
        }
        DoorPosition.position = closed.position;
        DoorPosition.rotation = closed.rotation;
        isOpen = false;
    }
}