using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance;

    public List<Task> tasks = new List<Task>();

    public Task currentTask;
    public int previousIndex;

    [SerializeField]
    public GameObject pin;

    void Awake()
    {
        instance = this;
    }



    // Start is called before the first frame update
    void Start()
    {

        //theres probabably a better way to do this
        Task task1 = new Task("Deliver Coffee", 8f, 'D', new Vector2(38f, 40f), new Vector2(15.5f, 0f), 1);
        tasks.Add(task1);

        Task task2 = new Task("Write Email", 15f,'P', new Vector2(-12.5f, 15f), Vector2.zero,0,"GAME");
        tasks.Add(task2);

        
        Task task3 = new Task("Pick up the Phone", 5f, 'A',new Vector2(54f, 41.5f), Vector2.zero);
        tasks.Add(task3);
        
        Task task4 = new Task("Type a Document", 15f,'P',new Vector2(-25f, -1f), Vector2.zero,0,"FIRE");
        tasks.Add(task4);

        Task task5 = new Task("Change Printer Paper", 8f, 'D', new Vector2(12.5f, 38.5f), new Vector2(-23f, -19.5f), 2);
        tasks.Add(task5);

        Task task6 = new Task("Deliver Papers", 10f, 'D', new Vector2(66f, -20f), new Vector2(-17.5f, -5.5f), 2);
        tasks.Add(task6);

        Task task7 = new Task("Drink Water", 5f, 'A', new Vector2(-25f, 37.5f), Vector2.zero);
        tasks.Add(task7);

        previousIndex = UnityEngine.Random.Range(0, tasks.Count);
        currentTask = tasks[previousIndex];
        currentTask = tasks[0];
        currentTask.Assign();
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(currentTask.taskName);
        if (!currentTask.isRuningTimer){
            //end the minigame
            EmailMinigame.instance.suddenEnd();

            setNewTask();
            currentTask.deliverCheck = false;
            setNewTask();

            StrikesDisplay.instance.addStrike();
            
            if (StrikesDisplay.instance.activeStrikes == 3)
                StopAllCoroutines();
            
        }

            //pick a random task from the list 

            //assign task

            //when completed, pick a new task
        
    }

    public void setNewTask()
    {
        int randomIndex = UnityEngine.Random.Range(0, tasks.Count);
        while(randomIndex == previousIndex)
        {
            randomIndex = UnityEngine.Random.Range(0, tasks.Count);
        }

        
        //Debug.Log("previous: "+previousIndex);
        //Debug.Log("curret: "+randomIndex);

        currentTask = tasks[randomIndex];
        previousIndex = randomIndex;


        //Debug.Log("strikes: "+StrikesDisplay.instance.activeStrikes);

        if (StrikesDisplay.instance.activeStrikes < 3)
        {
            StopAllCoroutines();
            currentTask.Assign();
        }
        

    }

    
}
