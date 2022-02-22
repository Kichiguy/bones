using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject OpenState;
    public GameObject ClosedState;

    private void Start() 
    {
        OpenState.SetActive(false);
        ClosedState.SetActive(true);
    }
    public void OpenDoor()
    {
        OpenState.SetActive(true);
        ClosedState.SetActive(false);
    }

    public void CloseDoor()
    {
        OpenState.SetActive(false);
        ClosedState.SetActive(true);
    }

}
