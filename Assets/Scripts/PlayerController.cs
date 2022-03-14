using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidBody;
    SpriteRenderer sprite;
    BoxCollider2D collider;

    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;
    float dashTimer;
    bool dragging = false;
    Transform draggedObject;
    DashState dashState = DashState.Ready;
    bool draggingHorizontal;
    Facing facing = Facing.Left;
    float spawnDistance = 0.4f;
    public GameObject arm;
    public float moveSpeed = 5.0f;
    public float dashSpeed = 10.0f;
    public float maxDash = 0.5f;
    public float dragRange = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
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
            Drag();
        }
        if(Input.GetButtonUp("Drag"))
        {
            if(draggedObject != null) 
            {
                draggedObject.parent = null;
                draggedObject = null;
            }
            dragging = false;
        }

        //Checks for attack
        if(Input.GetButtonDown("Fire1") && !dragging)
        {
            Attack();
        }

        //Check for dash
        Dash();
    }

    private void FixedUpdate() 
    {
        //if(dragging && !dashing) DragMove();
        Move();
    }

    private bool CheckForFlip()
    {
        if(horizontal < 0 && sprite.flipX == true) return true;
        else if(horizontal > 0 && sprite.flipX == false) return true;
        else return false;
    }

    private void Dash()
    {
        switch(dashState)
        {
            case DashState.Ready:
                if(dragging) break;
                if(Input.GetButtonDown("Dash"))
                {
                    dashState = DashState.Dashing;
                    Physics2D.IgnoreLayerCollision(0,6,true);
                }
                break;
            case DashState.Dashing:
                dashTimer += Time.deltaTime;
                if (dashTimer >= maxDash)
                {
                    dashTimer = maxDash;
                    dashState = DashState.Cooldown;
                    Physics2D.IgnoreLayerCollision(0,6,false);
                }
                break;
            case DashState.Cooldown:
                dashTimer -= Time.deltaTime;
                if(dashTimer <= 0)
                {
                    dashTimer = 0;
                    dashState = DashState.Ready;
                }
                break;
        }
    }
    
    private void Drag()
    {
        Vector2 direction = new Vector2(0,0);
        switch(facing)
        {
            case Facing.Left:
                direction = Vector2.left;
                break;
            case Facing.Right:
                direction = Vector2.right;
                break;
            case Facing.Up:
                direction = Vector2.up;
                break;
            case Facing.Down:
                direction = Vector2.down;
                break;
        }
        
        RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, dragRange, LayerMask.GetMask("Draggable"));
        if(ray.collider != null)
        {
            dragging = true;
            draggedObject = ray.collider.transform;
            draggedObject.parent = transform;
        }
    }

    private void Attack()
    {
        var newArm = Instantiate(arm, this.transform, false);
        Arm armComponent = newArm.GetComponent<Arm>();
        switch(facing)
        {
            case Facing.Left:
                newArm.transform.position = new Vector3(transform.position.x - spawnDistance,transform.position.y,0);
                newArm.transform.Rotate(0,0,260);
                armComponent.Swing();
                break;
            case Facing.Right:
                newArm.transform.position = new Vector3(transform.position.x + spawnDistance,transform.position.y,0);
                newArm.transform.Rotate(0,0,80);
                armComponent.Swing();
                break;
            case Facing.Up:
                newArm.transform.position = new Vector3(transform.position.x,transform.position.y + spawnDistance,0);
                newArm.transform.Rotate(0,0,170);
                armComponent.Swing();
                break;
            case Facing.Down:
                newArm.transform.position = new Vector3(transform.position.x,transform.position.y - spawnDistance,0);
                newArm.transform.Rotate(0,0,10);
                armComponent.Swing();
                break;
            default:
                break;
        }
    }

    private void Move()
    {
        //Check for diagonal movement
        if(horizontal !=0 && vertical !=0)
        {
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }
        //Apply movement
        if(dashState == DashState.Dashing)
        {
            DashMove();
        }
        else
        {        
            //Set sprite direction
            if(CheckForFlip()) sprite.flipX = !sprite.flipX;
            if(horizontal > 0) facing = Facing.Right;
            else if(horizontal < 0) facing = Facing.Left;
            else if (vertical > 0) facing = Facing.Up;
            else if (vertical < 0) facing = Facing.Down;

            rigidBody.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
        }

    }

    private void DashMove()
    {
        switch(facing)
        {
            case Facing.Left:
                rigidBody.velocity = new Vector2(-dashSpeed,0);
                break;
            case Facing.Right:
                rigidBody.velocity = new Vector2(dashSpeed,0);
                break;
            case Facing.Up:
                rigidBody.velocity = new Vector2(0,dashSpeed);
                break;
            case Facing.Down:
                rigidBody.velocity = new Vector2(0,-dashSpeed);
                break;
        }
    }

    private void DragMove()
    {

    }

    private enum DashState
    {
        Ready,
        Dashing,
        Cooldown
    }

    private enum Facing
    {
        Left,
        Right,
        Up,
        Down
    }
}
