using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    private Vector3 rotateArc;
    private float attackSpeed = 0.1f;

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "Breakable") 
        {
            Destroy(other.gameObject);
        }
    }

    public void Swing()
    {
        rotateArc = new Vector3(0,0,transform.rotation.z - 160);
        StartCoroutine(RotateArm());
    }

    IEnumerator RotateArm()
    {
        //Rotate arm
        var startRotate = transform.rotation;
        var endRotate = Quaternion.Euler(transform.eulerAngles + rotateArc);
        for(var i = 0f; i < 1; i += Time.deltaTime / attackSpeed)
        {
            transform.rotation = Quaternion.Slerp(startRotate, endRotate, i);
            yield return null;
        }

        Destroy(gameObject);
    }
}
