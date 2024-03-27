using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{

    private Animator animator;

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

        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y)) setTime((float)random.NextDouble() * 100);
    }

    
    public void setTime(float time)
    {
        //animator.StopPlayback();
        

        if (time < 10) timerText.text = "0" + Math.Round(time, 2).ToString("F2");
        else timerText.text = Math.Round(time, 2).ToString("F2");

        animator.Play("Ticking", -1, 0f);
    }
}
