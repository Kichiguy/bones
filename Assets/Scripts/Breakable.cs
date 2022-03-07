using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameObject Contents;
    
    private void OnDestroy() {
        Debug.Log("BREAK");
        if (Contents != null)
        {
            Instantiate(Contents,transform.position,transform.rotation);
        }
    }
}
