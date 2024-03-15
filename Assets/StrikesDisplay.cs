using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikesDisplay : MonoBehaviour
{
    [Header("Strike Markers")]
    public Strike strikeOne;
    public Strike strikeTwo;
    public Strike strikeThree;
    public GameObject strikeoutLine;
    private Strike[] strikeMarkers;

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
            }
            else
            {
                strikeMarkers[i].deactivate();
            }
        }

        if (activeStrikes == 3)
        {
            StartCoroutine(ActivateStrikeoutLine());
        }

        IEnumerator ActivateStrikeoutLine()
        {
            // Wait for half a second
            yield return new WaitForSeconds(0.5f);

            // Then activate the strikeoutLine
            strikeoutLine.SetActive(true);
        }
    }

    public void addStrike()
    {
        activeStrikes++;
    }

    public void aemoveStrike()
    {
        activeStrikes++;
    }
}
