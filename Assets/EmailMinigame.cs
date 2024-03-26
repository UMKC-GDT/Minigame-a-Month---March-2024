using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailMinigame : MonoBehaviour
{
    [Header("Text Fields")]
    public Text goalText;
    public InputField inputField;
    public Animator animator;
    public Animator minigameAnimator;

    public Text inputFieldText;

    public Button sendButton;

    private string input;

    private void incorrectInput()
    {
        animator.Play("WrongInput", -1, 0f);
        inputFieldText.color = Color.red;
    }

    public void ReadPlayerInput(string s)
    {
        input = s;
        Debug.Log(input);

        if (input.ToLower() != goalText.text.Substring(0, input.Length).ToLower())
        {
            incorrectInput();
        } else {
            inputFieldText.color = Color.black;
        }
    }

    public void startEmail() 
    {
        minigameAnimator.Play("StartMinigame");
        input = "";
        goalText.text = "STAR";
        inputField.GetComponent<InputField>().interactable = true;
        inputFieldText.color = Color.black;
        inputField.GetComponent<InputField>().text = "";
        inputField.Select();
        inputField.ActivateInputField();
        sendButton.interactable = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = inputField.GetComponent<Animator>();
        minigameAnimator = GetComponent<Animator>();
        inputField.GetComponent<InputField>().characterLimit = goalText.text.Length;

        inputField.GetComponent<InputField>().interactable = false;
        sendButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            startEmail();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            suddenEnd();
        }
    }

    public void sendButtonClicked() {
        if (input.ToLower() != goalText.text.ToLower().Substring(0, input.Length))
        {
            incorrectInput();
        } else {

            inputFieldText.color = Color.green;
            inputField.GetComponent<InputField>().interactable = false;
            sendButton.interactable = false;
            Debug.Log("Email finished successfully!");

            //TODO: Create and run email sending animation here

            //This is where we would tell the TaskManager that the task is completed

            //Close Task
            StartCoroutine(EndMinigame());
        }

        IEnumerator EndMinigame()
        {
            // Wait for half a second
            yield return new WaitForSeconds(0.2f);

            // Then activate the closing animation
            minigameAnimator.Play("EndMinigame", -1, 0f);
            
        }
    }

    public void suddenEnd()
    {
        inputField.GetComponent<InputField>().interactable = false;
        sendButton.interactable = false;

        minigameAnimator.Play("SuddenEndMinigame");
    }
}
