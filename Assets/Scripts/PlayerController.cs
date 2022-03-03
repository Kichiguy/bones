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
    bool dragging = false;
    bool draggingHorizontal;
    string facing = "left";
    float spawnDistance = 0.5f;

    public GameObject arm;
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

        //Checks to see if the player is dragging something
        if(Input.GetButton("Drag"))
        {
            dragging = true;
            Drag();
        }
        else dragging = false;

        //Checks for attack
        if(Input.GetButtonDown("Fire1") && !dragging)
        {
            Attack();
        }
    }

    private void FixedUpdate() 
    {
        if(dragging) DragMove();
        else NormalMove();
    }

    private bool CheckForFlip()
    {
        if(horizontal < 0 && sprite.flipX == true) return true;
        else if(horizontal > 0 && sprite.flipX == false) return true;
        else return false;
    }

    
    private void Drag()
    {
        dragging = true;
    }

    private void Attack()
    {
        var newArm = Instantiate(arm, this.transform, false);
        switch(facing)
        {
            case "left":
                newArm.transform.position = new Vector3(transform.position.x - spawnDistance,transform.position.y,0);
                break;
            case "right":
                newArm.transform.position = new Vector3(transform.position.x + spawnDistance,transform.position.y,0);
                break;
            case "up":
                newArm.transform.position = new Vector3(transform.position.x,transform.position.y + spawnDistance,0);
                break;
            case "down":
                newArm.transform.position = new Vector3(transform.position.x,transform.position.y - spawnDistance,0);
                break;
            default:
                break;
        }
    }

    private void NormalMove()
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
        if(horizontal > 0) facing = "right";
        else if(horizontal < 0) facing = "left";
        else if (vertical > 0) facing = "up";
        else if (vertical < 0) facing = "down";

    }

    private void DragMove()
    {

    }
}
