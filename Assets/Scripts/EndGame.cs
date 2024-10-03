using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    
    public int time = 50;
    public int scene = 3;
    private int counter = 0;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if(counter < time){
            counter++;
        }
        else{
            SceneManager.LoadScene(scene);
        }
        
    }
   
}
