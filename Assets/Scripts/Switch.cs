using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    public Sprite UnpressedSprite;
    public Sprite PressedSprite;
    public Door door;

    private SpriteRenderer spriteRenderer;
    private bool opened = false;
    private int colliding = 0;
 
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update() 
    {
        if(!opened && colliding > 0) OpenTheDoor();
        else if(opened && colliding <= 0) CloseTheDoor();
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        colliding -= 1;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player" || other.tag == "Heavy")
        {
            colliding += 1;
        }
    }

    void OpenTheDoor()
    {
        opened = true;
        spriteRenderer.sprite = PressedSprite;
        door.OpenDoor();
    }

    void CloseTheDoor()
    {
        opened = false;
        spriteRenderer.sprite = UnpressedSprite;
        door.CloseDoor();
    }
}
