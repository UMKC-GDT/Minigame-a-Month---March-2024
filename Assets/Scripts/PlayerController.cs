using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed = 25f;
    public Rigidbody2D rb;
     Vector2 movement;

    public Animator animator;

    public GameObject selectButton;

    private Vector2 boxSize = new Vector2(1f, 1f);

    public bool isPaused = false;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;

        animator.SetFloat("Horizontal", movement.x);

        animator.SetFloat("Vertical", movement.y);

        animator.SetFloat("Speed", movement.sqrMagnitude);


        if (Input.GetKeyDown(KeyCode.Z))
        {
            CheckInteraction(TaskManager.instance.currentTask.type);

        }
        

    }

    void FixedUpdate()
    {
        if(!isPaused)
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void OpenInteractableIcon()
    {
        selectButton.SetActive(true);
    }

    public void CloseInteractableIcon()
    {
        selectButton.SetActive(false);
    }

    public void CheckInteraction(char taskType)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxSize, 0, Vector2.zero);
        

        if (hits.Length > 0)
        {
            
            foreach(RaycastHit2D rc in hits)
            {
                if (rc.transform.GetComponent<Interactable>())
                {
                    rc.transform.GetComponent<Interactable>().Interact(taskType);
                    
                }
            }
        }

    }

    public void pauseControls()
    {

        isPaused = true;
    }

    public void playControls()
    {

        isPaused = false;
    }

}
