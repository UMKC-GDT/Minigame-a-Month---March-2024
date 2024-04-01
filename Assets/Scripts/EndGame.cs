using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    

     int counter = 0;
    void Start()
    {
        
        int counter = 0;
    }

    void FixedUpdate()
    {
        if(counter < 50){
            counter++;
        }
        else{
            SceneManager.LoadScene(3);
        }
        
    }
   
}
