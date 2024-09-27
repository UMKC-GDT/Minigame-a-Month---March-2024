using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance;

    public List<Task> tasksList = new List<Task>();
   
    ///<summary>
    ///Parameter specifying what task the player is currently attempting to complete
    ///</summary>
    public Task currentTask;
    public int previousIndex;

    [SerializeField]
    public GameObject pin;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        TaskAssignment();

        previousIndex = UnityEngine.Random.Range(0, tasksList.Count);


        currentTask = tasksList[previousIndex];

        currentTask.Assign();
    }
    /// <summary>
    /// Responsible for two behaviors:
    /// <list type="number">
    /// <item><param name="taskParams">Creating the tuple dictionary <em>taskParams</em></param></item>
    /// <item><param name="taskList">Transcribing the tuple dictionary into a list <em>tasksList</em></param></item>
    /// </list>
    /// </summary>
    /// <returns>A task <strong>list</strong> with a valid index</returns>
    public void TaskAssignment()
    {
        var taskParams = new (string name, float taskLimit, char type, Vector2 initialLocation, Vector2 deliveryLocation, int sprite, string word)[]
        {
            ("Deliver Coffee",          10f, 'D', new Vector2(38f, 40f), new Vector2(15.5f, 0f), 1, ""),
            ("Write Email",             7f,'P', new Vector2(5.5f, 20f), Vector2.zero, 0, "GAME"),
            ("Pick up the Phone",       5f, 'A', new Vector2(54f, 41.5f), Vector2.zero, 0, ""),
            ("Take Notes",              7f,'P', new Vector2(-25f, -1f), Vector2.zero,0,"NOTE"),
            ("Change Printer Paper",    10f, 'D', new Vector2(12.5f, 38.5f), new Vector2(-23f, -19.5f), 2, ""),
            ("Deliver Papers",          10f, 'D', new Vector2(66f, -20f), new Vector2(-17.5f, -5.5f), 2, ""),
            ("Drink Water",             5f, 'A', new Vector2(-25f, 37.5f), Vector2.zero, 0, ""),
            ("Fix Clock",               5f, 'A', new Vector2(-4.5f, 43f), Vector2.zero, 0, "")
        };

        Task[] tasksList = new Task[taskParams.Length];


    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(currentTask.taskName);
        if (!currentTask.isRuningTimer){
            //end the minigame
            EmailMinigame.instance.suddenEnd();

            SetNewTask();
            currentTask.deliverCheck = false;
            SetNewTask();

            StrikesDisplay.instance.addStrike();
            
            if (StrikesDisplay.instance.activeStrikes == 3)
            {
                StopAllCoroutines();
            }
        }
        //pick a random task from the list 
        //assign task
        //when completed, pick a new task
    }

    public void SetNewTask()
    {
        int randomIndex = UnityEngine.Random.Range(0, tasksList.Count);
        while(randomIndex == previousIndex)
        {
            randomIndex = UnityEngine.Random.Range(0, tasksList.Count);
        }
        
        //Debug.Log("previous: "+previousIndex);
        //Debug.Log("curret: "+randomIndex);

        currentTask = tasksList[randomIndex];
        previousIndex = randomIndex;

        //Debug.Log("strikes: "+StrikesDisplay.instance.activeStrikes);

        if (StrikesDisplay.instance.activeStrikes < 3)
        {
            StopAllCoroutines();
            currentTask.Assign();
        }
    }
}
