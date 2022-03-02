using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameObject Contents;
    
    public void Break()
    {
        if (Contents != null)
        {
            Instantiate(Contents);
        }
        Destroy(gameObject);
    }
}
