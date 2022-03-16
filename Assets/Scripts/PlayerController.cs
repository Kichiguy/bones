using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Components
    Rigidbody2D rigidBody;
    SpriteRenderer sprite;
    BoxCollider2D collider;
    #endregion
    
    #region Private Variables
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
    #endregion

    #region Public Variables
    public GameObject arm;
    public float moveSpeed = 5.0f;
    public float dashSpeed = 10.0f;
    public float maxDash = 0.5f;
    public float dragRange = 0.5f;
    #endregion

    #region Unity Methods
    void Start()
    {
        //Set components
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
        
        //Checks to see if the player is trying to drag something
        if(Input.GetButton("Drag"))
        {
            Drag();
        }

        //Resets drag state when button is released
        if(Input.GetButtonUp("Drag"))
        {
            if(draggedObject != null) 
            {
                draggedObject.parent = null;
                draggedObject = null;
            }
            dragging = false;
        }

        //Checks for attack if the player is not dragging something
        if(Input.GetButtonDown("Fire1") && !dragging)
        {
            Attack();
        }

        //Check for dash state
        Dash();
    }

    private void FixedUpdate() 
    {
        //Applies movement
        Move();
    }
    #endregion
    
    #region Private Methods
    /// <summary>
    /// Determines if the player sprite should flip. This should be removed when full animations are in place.
    /// </summary>
    private bool CheckForFlip()
    {
        if(horizontal < 0 && sprite.flipX == true) return true;
        else if(horizontal > 0 && sprite.flipX == false) return true;
        else return false;
    }

    /// <summary>
    /// Checks the current dash state. Applies the dash action if dash is ready and player is pushing the button. Checks if the player should still be dashing if currently dashing. Ticks down the cooldown if currently in cooldown.
    /// </summary>
    private void Dash()
    {
        switch(dashState)
        {
            case DashState.Ready:
                if(dragging) break;
                if(Input.GetButtonDown("Dash"))
                {
                    dashState = DashState.Dashing;
                    Physics2D.IgnoreLayerCollision(0,6,true); //Ignores collision with anything in the Pit layer.
                }
                break;
            case DashState.Dashing:
                dashTimer += Time.deltaTime;
                if (dashTimer >= maxDash)
                {
                    dashTimer = maxDash;
                    dashState = DashState.Cooldown;
                    Physics2D.IgnoreLayerCollision(0,6,false); //Resets Pit layer collision.
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
    /// <summary>
    /// This method allows the player to push and pull Draggable objects in the game. A raycast is created in the direction that the player is currently facing and looks for the first Draggable object in range and then sets the dragging state accordingly.
    /// </summary>
    private void Drag()
    {
        //Checks and sets the direction the player is currently facing.
        Vector2 direction = new Vector2(0,0);
        switch(facing)
        {
            case Facing.Left:
                direction = Vector2.left;
                draggingHorizontal = true;
                break;
            case Facing.Right:
                direction = Vector2.right;
                draggingHorizontal = true;
                break;
            case Facing.Up:
                direction = Vector2.up;
                draggingHorizontal = false;
                break;
            case Facing.Down:
                direction = Vector2.down;
                draggingHorizontal = false;
                break;
        }
        
        //Casts a ray out to the current drag range and returns anything on the Draggable layer. The object is set as a child to the player transform.
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
        if(dragging)
        {
            if(draggingHorizontal)
            { 
                vertical = 0;
                horizontal *= 0.5f;
            }
            else 
            {
                horizontal = 0;
                vertical *= 0.5f;
            }
        }
        //Apply movement
        if(dashState == DashState.Dashing && !dragging)
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

            Vector2 movement = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);

            if(draggedObject != null)
            {
                draggedObject.GetComponent<Rigidbody2D>().velocity = movement;
            }

            rigidBody.velocity = movement;
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
    #endregion

    #region Enums
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
    #endregion
}
