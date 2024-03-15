using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskDescription : MonoBehaviour
{

    
    //I added this to access in my Task Class -Cameron
    public static TaskDescription instance;

    void Awake()
    {
        instance = this;
    }


    public Text taskHeader;
    public Text taskName;

    public void setTask(string taskType, string taskObject, bool highRisk)
    {
        string formattedTaskObject = char.ToUpper(taskObject[0]) + taskObject.ToLower().Substring(1);

        if (taskType.ToLower() == "deliver")
        {
            taskName.text = "Deliver My " + formattedTaskObject + "!";
            if (highRisk) taskName.color = Color.red;
            else taskName.color = Color.blue;

        } else if (taskType.ToLower() == "type")
        {
            taskName.text = "Write My " + formattedTaskObject + "!";
            if (highRisk) taskName.color = Color.red;
            else taskName.color = Color.green;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            int randomNumber = UnityEngine.Random.Range(1, 6); // Generates a random number between 1 and 5

            switch (randomNumber)
            {
                case 1:
                    setTask("deliver", "mail", false);
                    break;
                case 2:
                    setTask("type", "email", true);
                    break;
                case 3:
                    setTask("deliver", "coffee", false);
                    break;
                case 4:
                    setTask("DELIVER", "food", true);
                    break;
                case 5:
                    setTask("type", "NOTES", false);
                    break;
            }
        }

        //setTask("deliver", "mail", false);
        //setTask("type", "email", true);
        //setTask("deliver", "coffee", false);
        //setTask("DELIVER", "food", true);
        //setTask("type", "NOTES", false);
    }
}
