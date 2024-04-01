using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSpriteController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite doorOpen, doorClosed;

    public void OpenDoor()
    {
        if(spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = doorOpen;
    }
    public void CloseDoor()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = doorClosed;
        Debug.Log("Closing door");
    }
}
