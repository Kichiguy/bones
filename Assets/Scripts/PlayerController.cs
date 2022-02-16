using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidBody;
    SpriteRenderer sprite;

    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;

    public float moveSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Set horizontal and vertical movement values
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate() 
    {
        //Check for diagonal movement
        if(horizontal !=0 && vertical !=0)
        {
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }
        //Apply movement
        rigidBody.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
        //Set sprite direction
        if(CheckForFlip()) sprite.flipX = !sprite.flipX;
    }

    private bool CheckForFlip()
    {
        if(horizontal < 0 && sprite.flipX == true) return true;
        else if(horizontal > 0 && sprite.flipX == false) return true;
        else return false;
    }
}
