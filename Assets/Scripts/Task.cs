using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Task 
{
    //variables 
    public string taskName;
    public float timeLimit;

    public Vector2 location;


    //constructor
    public Task(string n, float t, Vector2 l)
    {
        taskName = n;
        timeLimit = t;
        location = l;
    }


    //Coroutine for time
    public bool isRuningTimer { get { return runningTimer != null; } }
    Coroutine runningTimer = null;

    //functions
    public void Assign()
    {

        TaskManager.instance.pin.transform.position = location;
        //update ui
        //start time 
        //start corroutine to time out 

        StopRunningTimer();
        runningTimer = TaskManager.instance.StartCoroutine(RunTimer(timeLimit));
        

    }

    void StopRunningTimer()
    {
        if (isRuningTimer)
            TaskManager.instance.StopCoroutine(runningTimer);
        
        runningTimer = null;
    }

    IEnumerator RunTimer(float limit)
    {
        float timer = limit;
        while (timer > 0)
        {
            //waiting 1 second in real time and increasing the timer value
            yield return new WaitForSecondsRealtime(1);
            timer--;
            TimerDisplay.instance.setTime(timer);
            TaskDescription.instance.taskName.text = taskName;


        }
        StopRunningTimer();
    }


    public void OnCompleted()
    {
        // return true
        //determine if its successs/fail ???
    }
}

