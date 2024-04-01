using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC : MonoBehaviour
{
    //This looks terrible im sorry

    //i fixed some of the apostraphes that were weird -cameron
    //also for some reason, adding an empty string at tthe end fixed a bug -cameron
    private string[] d1 = new string[]
{
        "Wanna hear about my cat's new trick?",
        "Oh, it's the cutest thing. I've been training Mr. Whiskers to fetch like a dog, and he's finally got it!",
        "He figured out how to bring me the treat bag.",
        "He even brings me my slippers now!",
        "Wanna see the video?",
        "Okay, have a good one!",
        ""
};
    private string[] d2 = new string[]
   {
        "Oh, the drama with the coffee machine continues.",
        "Remember how it was broken last week? Well, they fixed it, but now it only makes decaf!",
        "I know, right?! You know I can't survive without my caffeine.",
        "Yeah, I've had to start bringing my own coffee from home. My wife thinks I spend too much on coffee.",
        "So what if I spent four hundred dollars on it last month?",
        ""
   };
    private string[] d3 = new string[]
{
        "You know, I've had this idea for a novel for a while.",
        "Okay, hear me out. It's about this kid that lives in an abusive household, who finds out he's a wizard.",
        "It's gonna be so crazy! And then he goes to this even better school of wizard kids like him.",
        "Yeah, I'm thinking that the bad guy could be the one who killed his parents.",
        "I know, right? Maybe he could be famous for surviving the attack!",
        "...wait, what do you mean it already exists?",
        ""
};
    private string[] d4 = new string[]
{
        "Have you been following the stock market lately?",
        "Don't worry, I don't blame you. I didn't want to get into it at first either 'cause of my grandpa.",
        "Yeah, he would always talk about how much money he lost and all that,",
        "But this one book changed my mind. Have you heard of cryptocurrency?",
        "Oh, man, it's gonna change the world. You're missing out. Have you heard of blockchain?",
        "Jesus. They just hire anybody here, huh?",
        ""
};
    private string[] d5 = new string[]
{
        "Dawg, did you know the office plants are more than just decor?",
        "It's so crazy, bro. I've been reading about plant psychology, and apparently they can sense our emotions.",
        "Bazinga! Dude, I've even started talking to the fern next to the copier.",
        "I think it's been looking greener, but I don't know. I don't want to make the copier jealous, you know?",
        ""
};
    private string[] d6 = new string[]
{
        "Guess what happened in the elevator today?",
        "I was there with the CEO, but it got stuck for a whole 20 minutes!",
        "I know! I was so scared. I thought it would be awkward.",
        "Nope! We actually had a nice conversation about poodles. Apparently, we're both dog people.",
        "Right? Like, who would've guessed?",
        ""
};

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    private string[] dialogue;
    private int index = 0;

    public float wordSpeed;
    public bool playerIsClose;
    private bool interacted;


    void Start()
    {
        interacted = false;
        dialogueText.text = "";
        dialogue = GetDialogue();
    }

    void Update()
    {
        if (playerIsClose)
        {
            //Stop the player and npc when they collide
            /*For some reason the moving obstacle ActivateMovement is bugged(?)
              where I was getting the reverse results with Deactivate Movement where they would
              only stop after exiting the collision box instead of when it was triggered.
              But it works properly when ActivateMovement is called as in it stops when the player
              is detected on the collision box.
             */


            GetComponent<MovingObstacle>().ActivateMovement();  //this component already exists in this object -cameron
            PlayerController.instance.pauseControls();
            //Dont bring up the textbox again if you've exhausted the dialogue
            if (interacted == true)
            {
                RemoveText();
            }
            else //there wasnt an else here before -cameron
            {
                //For first time dialogue
                if (!dialoguePanel.activeInHierarchy)
                {
                    dialoguePanel.SetActive(true);
                    StartCoroutine(Typing());
                }
                //M1 to skip to next dialogue
                if (Input.GetKeyDown(KeyCode.X))
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
            //Checks if you've exhausted the dialogue. And if talked to before will not talk again.
            if (index >= dialogue.Length - 1 && dialoguePanel.activeInHierarchy)
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
        dialogue = GetDialogue();//makes it pick a new random dialogue -cameron

        PlayerController.instance.playControls();
        //MovingObstacle.instance.DeactivateMovement();
        GetComponent<MovingObstacle>().DeactivateMovement();  //this component already exists in this object -cameron
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
        if (index < dialogue.Length - 1)
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

    public string[] GetDialogue()
    {
        int randomNum = Random.Range(1, 7);//range doesnt include the last digit so i made this 7 - cameron
        string[] selectArray = null;
        switch (randomNum)
        {
            case 1:
                selectArray = d1;
                break;
            case 2:
                selectArray = d2;
                break;
            case 3:
                selectArray = d3;
                break;
            case 4:
                selectArray = d4;
                break;
            case 5:
                selectArray = d5;
                break;
            case 6:
                selectArray = d6;
                break;
        }

        return selectArray;
    }
}

