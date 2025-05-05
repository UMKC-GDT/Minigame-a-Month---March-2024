using System.Collections.Generic;
using System.Diagnostics;
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
        // Initialize tasks
        InitializeTasks();

        // Select a random task
        previousIndex = UnityEngine.Random.Range(0, tasks.Count);
        currentTask = tasks[previousIndex];

        // Start timing the task assignment
        TimeTaskAssignment();
    }

    private void InitializeTasks()
    {
        tasks.Add(new Task("Deliver Coffee", 10f, 'D', new Vector2(38f, 40f), new Vector2(15.5f, 0f), 1));
        tasks.Add(new Task("Write Email", 7f, 'P', new Vector2(5.5f, 20f), Vector2.zero, 0, "GAME"));
        tasks.Add(new Task("Pick up the Phone", 5f, 'A', new Vector2(54f, 41.5f), Vector2.zero));
        tasks.Add(new Task("Take Notes", 7f, 'P', new Vector2(-25f, -1f), Vector2.zero, 0, "NOTE"));
        tasks.Add(new Task("Change Printer Paper", 10f, 'D', new Vector2(12.5f, 38.5f), new Vector2(-23f, -19.5f), 2));
        tasks.Add(new Task("Deliver Papers", 10f, 'D', new Vector2(66f, -20f), new Vector2(-17.5f, -5.5f), 2));
        tasks.Add(new Task("Drink Water", 5f, 'A', new Vector2(-25f, 37.5f), Vector2.zero));
        tasks.Add(new Task("Fix Clock", 5f, 'A', new Vector2(-4.5f, 43f), Vector2.zero));
    }

    // Method to time the task assignment
    private void TimeTaskAssignment()
    {
        // Start the stopwatch
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Assign the current task
        currentTask.Assign();

        // Stop the stopwatch
        stopwatch.Stop();

        // Convert elapsed ticks to nanoseconds
        long elapsedTicks = stopwatch.ElapsedTicks;
        double elapsedNanoseconds = (elapsedTicks / (double)Stopwatch.Frequency) * 1e9;

        // Log the elapsed time in nanoseconds
        UnityEngine.Debug.Log($"Task assignment took {elapsedNanoseconds:0.##} ns.");
    }

    void Update()
    {
        if (StrikesDisplay.instance.activeStrikes == 3)
        {
            EmailMinigame.instance.suddenEnd();
        }

        if (!currentTask.isRuningTimer)
        {
            // End the minigame
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
    }

    public void setNewTask()
    {
        int randomIndex = UnityEngine.Random.Range(0, tasks.Count);
        while (randomIndex == previousIndex)
        {
            randomIndex = UnityEngine.Random.Range(0, tasks.Count);
        }

        currentTask = tasks[randomIndex];
        previousIndex = randomIndex;

        if (StrikesDisplay.instance.activeStrikes < 3)
        {
            StopAllCoroutines();
            currentTask.Assign();
        }
    }
}
