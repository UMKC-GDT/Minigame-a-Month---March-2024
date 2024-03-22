using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onReset;
    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        onReset.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if the obstacle collides with the player
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().OpenInteractableIcon();

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
        switch (taskType)
        {
            case 'A': //arrival tasks 

                Debug.Log("Task- arrive");

                //start animation 
                //when animation finishes
                    //success
                    //add time
                break;
            case 'D'://delivery tasks 

                Debug.Log("Task- deliver");

                //add object sprite to player 
                //set new pin location  
                //if player reaches new location 
                    //success   
                    //add time
                //remove object sprite from player
                break;
            case 'P'://pop-up tasks 

                Debug.Log("Task- popup");


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
        
        TaskManager.instance.setNewTask();
    }
}



