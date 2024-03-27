using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup myUIGroup;
    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;
    public bool fadeFinished = true;
    
    public DialogueManager dialogueManager;

    public void ShowUI() {
        fadeIn = true;
        fadeFinished = false;
    }

    public void HideUI() {
        fadeOut = true;
        fadeFinished = false;
    }

    private void Update() 
    {
        if (fadeIn)
        {
            if(myUIGroup.alpha < 1)
            {
                myUIGroup.alpha += Time.deltaTime;
                if(myUIGroup.alpha >= 1)
                {
                    fadeIn = false;
                    fadeFinished = true;
                    Debug.Log("Fade finished. Beginning conversation!");
                    dialogueManager.DisplayMessage();
                }
            }
        }

        if (fadeOut)
        {
            if(myUIGroup.alpha >= 0)
            {
                myUIGroup.alpha -= Time.deltaTime;
                if(myUIGroup.alpha == 0)
                {
                    fadeOut = false;
                    fadeFinished = true;
                }
            }
        }
    }
}
