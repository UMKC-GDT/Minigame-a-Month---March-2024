using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
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

    public void Interact()
    {
        Debug.Log("interacted");

        //arrival tasks 
        //start animation 
        //when animation finishes
        //success
        //add time
        //pick a new task 
        //fails if time hits 0
        //pick a new task
        TaskManager.instance.setNewTask(TaskManager.instance.previousIndex);
    }
}




//arrival tasks 
//start animation 
//when animation finishes
//success
//add time
//pick a new task 
//fails if time hits 0
//pick a new task

//delivery tasks 
//add object sprite to player 
//set new pin location  
//if player reaches new location 
//success   
//add time
//remove object sprite from player 
//pick a new task
//fails if time hits 0
//pick a new task

//pop-up tasks 
//diable character movement 
//open popup
//run minigame 
//if player wins game 
//success
//add time 
//close popup
//enable player movement 
//pick a new task
//fails if minigame is lost
//pick a new task