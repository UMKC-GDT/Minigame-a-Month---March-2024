using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        taskAssignment();

        previousIndex = UnityEngine.Random.Range(0, tasks.Count);
        currentTask = tasks[previousIndex];
        
        currentTask.Assign();
    }
    public void taskAssignment()
    {
        Dictionary<string, (float taskLimit,
                            char type,
                            Vector2 initialLocation,
                            Vector2 deliveryLocation,
                            int sprite,
                            string word)> taskParams =
            new Dictionary<string, (float, char, Vector2, Vector2, int, string)>
        {
            {"Deliver Coffee",          (10f, 'D', new Vector2(38f, 40f), new Vector2(15.5f, 0f), 1, "") },
            {"Write Email",             (7f,'P', new Vector2(5.5f, 20f), Vector2.zero, 0, "GAME") },
            {"Pick up the Phone",       (5f, 'A', new Vector2(54f, 41.5f), Vector2.zero, 0, "") },
            {"Take Notes",              (7f,'P', new Vector2(-25f, -1f), Vector2.zero,0,"NOTE") },
            {"Change Printer Paper",    (10f, 'D', new Vector2(12.5f, 38.5f), new Vector2(-23f, -19.5f), 2, "") },
            {"Deliver Papers",          (10f, 'D', new Vector2(66f, -20f), new Vector2(-17.5f, -5.5f), 2, "")},
            {"Drink Water",             (5f, 'A', new Vector2(-25f, 37.5f), Vector2.zero, 0, "")},
            {"Fix Clock",               (5f, 'A', new Vector2(-4.5f, 43f), Vector2.zero, 0, "")}
        };

        foreach (var entry in taskParams)
        {
            Task task = new Task(
                entry.Key,
                entry.Value.taskLimit,
                entry.Value.type,
                entry.Value.initialLocation,
                entry.Value.deliveryLocation,
                entry.Value.sprite,
                entry.Value.word
                );
            tasks.Add(task);
        }
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
            {
                StopAllCoroutines();
            }
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
