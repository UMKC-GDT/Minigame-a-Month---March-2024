using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Image actorImage1;
    public Image actorImage2;
    public Image actorImage3;
    public Image actorImage4;
    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;
    public RectTransform backgroundBox;
    public FadeScript fadeScript;
    public ActorManager actorManager;
    public CanvasGroup DialogueParent;
    [SerializeField] private float textSpeed = 0.01f;

    Message[] currentMessages;
    Actor[] currentActors;
    int activeMessage = 0;
    int currentLetter = 0;
    private bool dialogueDone = false;
    private bool skip = false;
    public static bool isActive = false;

    public void OpenDialogue(Message[] messages, Actor[] actors) {
        currentMessages = messages;
        currentActors = actors;
        activeMessage = 0;
        isActive = true;
        skip = false;
        dialogueDone = false;
        actorName.text = "";
        messageText.text = "";
        actorManager.setActive(69);

        //TODO:Prepopulate every active actor's sprite above the dialogue box
        actorManager.loadActorImages(actors);
        fadeScript.ShowUI();

        Debug.Log("Loaded messages: " + messages.Length);
    }

    public void DisplayMessage() {
        Message messageToDisplay = currentMessages[activeMessage];
        StartCoroutine(AnimateText(messageToDisplay));

        //TODO:Each message's id should be a number from 1 to 4, denoting the actor sprite's position above the dialogue box
        //TODO: Based on that id, change the actor name, and set that actor to be the active one
        //TODO: Possible version: ActorManager.setActive([int from 1 to 4])
        Actor actorToDisplay = currentActors[messageToDisplay.speakerId];
        actorName.text = actorToDisplay.name;
        actorManager.setActive(messageToDisplay.speakerId);
    }

    IEnumerator AnimateText(Message message) {
        Debug.Log("Setting dialogueDone to " + dialogueDone);
        dialogueDone = false;

        for (int i = 0; i < message.message.Length + 1; i++)
        {
            currentLetter = i;
            if (skip) {
                Debug.Log("Skip is true");
                messageText.text = message.message;  // Set the full message
                dialogueDone = true;
                skip = false;
                Debug.Log("Space has been pressed early! Setting dialogueDone to " + dialogueDone);
                yield break;
            }

            Debug.Log("Skip wasn't true");


            messageText.text = message.message.Substring(0, i);
            yield return new WaitForSeconds(message.textSpeed); // Is this line of code even doing anything?
        }

        dialogueDone = true;
        Debug.Log("The dialogue has finished naturally! Setting dialogueDone to " + dialogueDone);
    }

    public void NextMessage() {
        activeMessage++;

        if (activeMessage < currentMessages.Length) {
            DisplayMessage();
        } else {
            Debug.Log("The conversation has ended!");
            isActive = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DialogueParent.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueDone && Input.GetKeyDown(KeyCode.Space) && isActive) {
            dialogueDone = false;
            Debug.Log("Space has been pressed while the dialogue was finished. Skipping to the next message, and setting dialogueDone to " + dialogueDone + "!");
            NextMessage();

        } else if (!dialogueDone && currentLetter > 1 && Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Setting skip to true!");
            skip = true;
        }
        

        if (!isActive) {
            fadeScript.HideUI();
        }
        
    }
}
