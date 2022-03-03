using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    private void Awake() 
    {
        Swing();
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "Breakable") Destroy(other.gameObject);
    }

    private void Swing()
    {

    }
}
