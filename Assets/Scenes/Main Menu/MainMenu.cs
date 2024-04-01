using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject credits;

    public void PlayGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void OpenCredits(){
        credits.SetActive(true);
    }

    public void CloseCredits(){
        credits.SetActive(false);
    }

    public void QuitGame(){
        Debug.Log("Exits Game");
        Application.Quit();
    }
   
}
