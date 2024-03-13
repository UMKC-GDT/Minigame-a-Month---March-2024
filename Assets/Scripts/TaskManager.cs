using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance;

    public List<Task> tasks = new List<Task>();

    void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        //theres probabably a better way to do this
        Task firstTask = new Task("Deliver Coffee", 15f, new Vector2(42.5f, 32.5f));
        tasks.Add(firstTask);


        firstTask.OnAssigned();

        /*
        Task secondTask = new Task("Pick up the Phone", 10f);
        tasks.Add(secondTask);
        
        Task thirdTask = new Task("Type a Document", 20f);
        tasks.Add(thirdTask);
        */

    }

    // Update is called once per frame
    void Update()
    {

        //pick a random task from the list 

        //assign task

        //when completed, pick a new task
       


    }
}
