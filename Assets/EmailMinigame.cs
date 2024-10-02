using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailMinigame : MonoBehaviour
{
    public static EmailMinigame instance;
    
    [Header("Text Fields")]
    public Text goalText;
    public InputField inputField;
    public Animator animator;
    public Animator minigameAnimator;

    public Text inputFieldText;

    public Button sendButton;

    private string input;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

    public void startEmail(string word) 
    {
        minigameAnimator.Play("StartMinigame");
        input = "";
        goalText.text = word;
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
        /*
        if (Input.GetKeyDown(KeyCode.O))
        {
            startEmail("STAR");
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            suddenEnd();
        }
        */
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

            //Close Task
            StartCoroutine(EndMinigame());

            //This is where we would tell the TaskManager that the task is completed
                //nah, im just gonna call the next task right here -cameron
            TaskManager.instance.setNewTask();
            PlayerController.instance.playControls(); //allows player to move again
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
        if (inputField.GetComponent<InputField>().interactable == true)
        {
            inputField.GetComponent<InputField>().interactable = false;
            sendButton.interactable = false;

            minigameAnimator.Play("SuddenEndMinigame");
            
            //allows player to move agian, even  if they lose -camerron
            TaskManager.instance.setNewTask();
            PlayerController.instance.playControls();
        }
        
    }
}
