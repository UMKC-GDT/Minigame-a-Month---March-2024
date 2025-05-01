using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject credits;

    public void PlayGame(){
        SceneManager.LoadScene(1);
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
