using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikesDisplay : MonoBehaviour
{
    
    //I added this to access in my Task Class -Cameron
    public static StrikesDisplay instance;

    void Awake()
    {
        instance = this;
    }


    [Header("Strike Markers")]
    public Strike strikeOne;
    public Strike strikeTwo;
    public Strike strikeThree;
    public GameObject strikeoutLine;
    public bool struckout = false;
    private Strike[] strikeMarkers;
    private Animator animator;

    public int activeStrikes;


    // Start is called before the first frame update
    void Start()
    {
        strikeMarkers = new Strike[] {strikeOne, strikeTwo, strikeThree};
        activeStrikes = 0;
        strikeoutLine.SetActive(false);
        Array.ForEach(strikeMarkers, strike => {strike.deactivate();});

        for (int i = 0; i < strikeMarkers.Length; i++)
        {
            strikeMarkers[i].deactivate();
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            addStrike();
        for (int i = 0; i < strikeMarkers.Length; i++)
        {
            if (i < activeStrikes)
            {
                strikeMarkers[i].activate();
                //Debug.Log("trying to start the animation!");
                strikeMarkers[i].GetComponent<Animator>().Play("AddStrike");
            }
            else
            {
                strikeMarkers[i].deactivate();
            }
        }
        
        
        if (activeStrikes == 3 && struckout == true)
        {
            // Debug.Log("StrikesDisplay is trying to call the Active Strikout Line again...");
        }
        else if (activeStrikes == 3)
        {
            StartCoroutine(ActivateStrikeoutLine());
            struckout = true;
        }

        IEnumerator ActivateStrikeoutLine()
        {
            // Wait for half a second
            yield return new WaitForSeconds(0.5f);

            // Then activate the strikeoutLine
            strikeoutLine.SetActive(true);
            strikeoutLine.GetComponent<Animator>().Play("AddStrikeLine");
            
        }
    }

    public void addStrike()
    {
        if (activeStrikes < 3) {
            activeStrikes++;
        }
    }

    public void aemoveStrike()
    {
        activeStrikes++;
    }
}
