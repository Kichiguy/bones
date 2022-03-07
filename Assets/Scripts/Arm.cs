using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    private Vector3 rotateArc = new Vector3(0,0,120);
    private float attackSpeed = 0.2f;
    private void Awake() 
    {
        StartCoroutine(RotateArm(attackSpeed));
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "Breakable") 
        {
            Destroy(other.gameObject);
        }
    }

    IEnumerator RotateArm(float speed)
    {
        //Rotate arm
        var startRotate = transform.rotation;
        var endRotate = Quaternion.Euler(transform.eulerAngles + rotateArc);
        for(var i = 0f; i < 1; i += Time.deltaTime / speed)
        {
            transform.rotation = Quaternion.Slerp(startRotate, endRotate, i);
            yield return null;
        }

        Destroy(gameObject);
    }
}
