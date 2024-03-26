using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onReset;
    public GameObject player;
    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        onReset.Invoke();
    }

    private void Start()
    {
        player = GameObject.Find("Player");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // if the obstacle collides with the player
        if (other.CompareTag("Player"))
        {
            if(TaskManager.instance.currentTask.deliverCheck == true)
            {
                //code for finalizing the delivery
                //TaskManager.instance.currentTask.location = new Vector2(42.5f, 35f);
                TaskManager.instance.currentTask.deliverCheck = false;

                int spriteIndex = TaskManager.instance.currentTask.dilverySprite;
                if (spriteIndex > 0)
                    player.transform.GetChild(spriteIndex).gameObject.SetActive(false);
               
                TaskManager.instance.setNewTask();

            }
            else
            {
                other.GetComponent<PlayerController>().OpenInteractableIcon();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // if the obstacle collides with the player
        if (other.CompareTag("Player"))
        {

            other.GetComponent<PlayerController>().CloseInteractableIcon();

        }

    }

    public void Interact(char taskType)
    {
        player.transform.GetChild(1).gameObject.SetActive(false);
        player.transform.GetChild(2).gameObject.SetActive(false);
        player.transform.GetChild(3).gameObject.SetActive(false);

        switch (taskType)
        {
            case 'A': //arrival tasks 

                Debug.Log("Task- arrive");

                TaskManager.instance.setNewTask();

                //start animation 
                //when animation finishes
                //success
                //add time
                break;
            case 'D'://delivery tasks 

                Debug.Log("Task- deliver");

                //add object sprite to player 
                //set new pin location
                //
                //TaskManager.instance.currentTask.location = new Vector2(42.5f, 20f);

                int spriteIndex = TaskManager.instance.currentTask.dilverySprite;
                if (spriteIndex > 0)
                    player.transform.GetChild(spriteIndex).gameObject.SetActive(true);
                TaskManager.instance.pin.transform.position = TaskManager.instance.currentTask.deliverLocation;
                TaskManager.instance.currentTask.deliverCheck = true;
                //if player reaches new location 
                //success   
                //add time
                //remove object sprite from player
                break;
            case 'P'://pop-up tasks 

                Debug.Log("Task- popup");

                PlayerController.instance.pauseControls();
                EmailMinigame.instance.startEmail(TaskManager.instance.currentTask.miniGameWord);


                //disable character movement 
                //open popup
                //run minigame 
                //if player wins game 
                //success
                //add time 
                //close popup
                //enable player movement 
                break;
            default:
                break;
        }

        //pick a new task
        //TaskManager.instance.setNewTask(TaskManager.instance.previousIndex);
        
    }
}



