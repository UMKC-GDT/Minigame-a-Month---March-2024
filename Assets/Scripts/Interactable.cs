using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
<<<<<<< Updated upstream
    public UnityEvent onReset;
    public GameObject player;
    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        onReset.Invoke();

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    {
        GetComponent<BoxCollider2D>().isTrigger = true;

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
                player.transform.GetChild(1).gameObject.SetActive(false);
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
                //
                //TaskManager.instance.currentTask.location = new Vector2(42.5f, 20f);
                player.transform.GetChild(1).gameObject.SetActive(true);
                TaskManager.instance.pin.transform.position = TaskManager.instance.currentTask.deliverLocation;
                TaskManager.instance.currentTask.deliverCheck = true;
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
        if(taskType != 'D')
        {
            TaskManager.instance.setNewTask();
        }
    }
}



