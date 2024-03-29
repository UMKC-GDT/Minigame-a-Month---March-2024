using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index = 0;

    public float wordSpeed;
    public bool playerIsClose;
    private bool interacted;


    void Start()
    {
        interacted = false;
        dialogueText.text = "";
    }

    void Update()
    {
        if (playerIsClose)
        {
            //Stop the player and npc when they collide
            MovingObstacle.instance.ActivateMovement();
            PlayerController.instance.pauseControls();
            //Dont bring up the textbox again if you've exhausted the dialogue
            if (interacted == true)
            {
                RemoveText();
            }
            {
                //For first time dialogue
                if (!dialoguePanel.activeInHierarchy)
                {
                    dialoguePanel.SetActive(true);
                    StartCoroutine(Typing());
                }
                //M1 to skip to next dialogue
                if (Input.GetMouseButtonDown(0))
                {
                    if (dialogueText.text == dialogue[index])
                    {
                        NextLine();
                    }
                    else
                    {
                        StopAllCoroutines();
                        dialogueText.text = dialogue[index];
                    }
                }

            }
            //Checks if you've exhausted the dialogue. Index is at 2 because i have only 3 test lines right now
            if (index >= 3 && dialoguePanel.activeInHierarchy)
            {
                interacted = true;
                RemoveText();
            }
            //Starts back player and NPC movement
            if(!playerIsClose)
            {
                PlayerController.instance.playControls();
            }
        }
    }

    public void RemoveText()
    {
        dialogueText.text = "";
        PlayerController.instance.playControls();
        MovingObstacle.instance.DeactivateMovement();
        dialoguePanel.SetActive(false);

    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        if (index < dialogue.Length)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            RemoveText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            RemoveText();
        }
    }
}