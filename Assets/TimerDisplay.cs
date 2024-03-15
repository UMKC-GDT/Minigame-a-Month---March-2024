using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{

    //I added this to access in my Task Class -Cameron
    public static TimerDisplay instance;

    void Awake()
    {
        instance = this;
    }


    public Text timerText;
    System.Random random = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        timerText.text = "99.99";
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y)) setTime((float)random.NextDouble() * 100);
    }

    
    public void setTime(float time)
    {
        if (time < 10) timerText.text = "0" + Math.Round(time, 2).ToString("F2");
        else timerText.text = Math.Round(time, 2).ToString("F2");
    }
}
